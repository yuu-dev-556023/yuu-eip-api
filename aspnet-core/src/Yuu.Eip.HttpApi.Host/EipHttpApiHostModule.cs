using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Yuu.AspNetCore.Extensions;
using Yuu.AspNetCore.Mvc.Extensions;
using Yuu.ClientInfo;
using Yuu.Eip.EntityFrameworkCore;
using Yuu.Eip.MultiTenancy;
using Yuu.Environment;
using Yuu.Environment.Provider.Extensions;

namespace Yuu.Eip;

[DependsOn(
    typeof(YuuEnvironmentProviderModule),
    typeof(YuuClientInfoProviderModule),
    typeof(EipHttpApiModule),
    typeof(EipHttpApiClientModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(EipApplicationModule),
    typeof(EipEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class EipHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddServer(options =>
            {
                /* options.RequireProofKeyForCodeExchange(); */
                /* MCP伺服器 存取Resource 需要在伺服器啟動時註冊 */
                /* 以下URL 也需要在OpenDataSeeder 執行時候 一起進入 Permissions裡 */
                options.RegisterResources(
                    "https://localhost:44319/mcp",
                    "https://yuu-eip.yuu-is87.com/mcp"
                );

                /* 測試時使用，關閉Resource認證 */
                /* options.DisableResourceValidation(); */
            });

            builder.AddValidation(options =>
            {
                options.AddAudiences("Eip");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsProd())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                var certificatePath = System.Environment.GetEnvironmentVariable("OPENIDDICT_CERTIFICATE_PATH") ?? Path.Combine(Directory.GetCurrentDirectory(), "openiddict.pfx");

                serverBuilder.AddProductionEncryptionAndSigningCertificate(
                    certificatePath,
                    configuration["AuthServer:CertificatePassPhrase"]!,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
                serverBuilder.SetIssuer(new Uri(configuration["AuthServer:Authority"]!));
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);

        ConfigureClockToUtc();
        ConfigureForwardedHeaders();
        ConfigureAutoValidate();
        ConfigureExposeIntegration();
        ConfigureConventionalControllers();

        context.Services.AddMcpServer()
            .AddAuthorizationFilters()
            .WithHttpTransport()
            .WithToolsFromAssembly();

        if (!hostingEnvironment.IsProd())
        {
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = true;
            });

            Configure<OpenIddictServerAspNetCoreOptions>(options =>
            {
                options.DisableTransportSecurityRequirement = true;
            });
        }
    }

    private static void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        var serverUrl = configuration["App:SelfUrl"]!; // 例如: "https://localhost:44319/"
        var authorizationServerUrl = configuration["AuthServer:Authority"]!; // 同上或不同

        context.Services
        .AddAuthentication()
        .AddJwtBearer(options =>
        {
            options.Authority = authorizationServerUrl;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authorizationServerUrl,
                ValidAudience = "Eip",
            };

            if (hostingEnvironment.IsProd())
            {
                options.RequireHttpsMetadata = true;
            }
        })
        .AddMcp(options =>
        {
            options.ResourceMetadata = new()
            {
                Resource = new Uri($"{serverUrl.TrimEnd('/')}/mcp"),
                ResourceDocumentation = new Uri($"{serverUrl.TrimEnd('/')}/swagger"), // 可選
                AuthorizationServers = { new Uri(authorizationServerUrl.TrimEnd('/')) },
                ScopesSupported = ["Eip"]
            };
        });

        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureClockToUtc()
    {
        Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
    }

    private void ConfigureForwardedHeaders()
    {
        Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            /* 清除預設設定，使得所有來源的proxy都可以被處理 (容器內部限定) */
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });
    }

    private void ConfigureAutoValidate()
    {
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });
    }

    private void ConfigureExposeIntegration()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ExposeIntegrationServices = true;
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? []);

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsLocal())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<EipDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Yuu.Eip.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<EipDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Yuu.Eip.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<EipApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Yuu.Eip.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<EipApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Yuu.Eip.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<MvcOptions>(options =>
        {
            options.UsePluralizedSlugifiedRoutes();
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                {"Eip", "Eip API"}
            },
            options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Eip API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);

                if (hostingEnvironment.IsProd())
                {
                    options.HideAbpEndpoints();
                }

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "Dash.*.xml");

                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(xmlFile);
                }
            });
    }

    private static void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? [])
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseForwardedHeaders();

        if (env.IsProd())
        {
            app.UseHttpsRedirection();
        }

        app.UseCorrelationId();
        app.UseAbpRequestLocalization();

        if (!env.IsProd())
        {
            app.UseDeveloperExceptionPage();
        }

        if (env.IsProd())
        {
            app.UseErrorPage();
        }

        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        //app.UseMiddleware<IPWhitelistMiddleware>();

        /* Debug 用*/
        app.Use(async (context, next) =>
        {
            if (!context.Request.Path.StartsWithSegments("/mcp"))
            {
                await next();
                return;
            }

            /* 已登入就放行 */
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                await next();
                return;
            }

            var resourceMetadataUrl =
                    $"{context.Request.Scheme}://{context.Request.Host}/.well-known/oauth-protected-resource";

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            context.Response.Headers.Append(
                "WWW-Authenticate",
                $"Bearer realm=\"Eip\", resource_metadata=\"{resourceMetadataUrl}\""
            );

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                "{ \"error\": \"unauthorized\", \"error_description\": \"MCP authentication required\" }"
            );

            return;
        });
        /* Debug 用*/

        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseRequestResponseLoggingMiddleware();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eip API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("Eip");
            c.OAuthUsePkce();
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapMcp("/mcp")
            .AllowCookieRedirect()
            .DisableAntiforgery()
            .RequireAuthorization();
        });
    }
}
