using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.VirtualFileSystem;

namespace Yuu.Eip;

[DependsOn(
    typeof(AbpHttpClientIdentityModelModule),
    typeof(AbpAccountHttpApiClientModule),
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpPermissionManagementHttpApiClientModule),
    typeof(AbpTenantManagementHttpApiClientModule),
    typeof(AbpFeatureManagementHttpApiClientModule),
    typeof(AbpSettingManagementHttpApiClientModule)
)]
public class EipHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(EipHttpApiClientModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EipHttpApiClientModule>();
        });

        var configuration = context.Services.GetConfiguration();

        /*
         
            Configure<AbpIdentityClientOptions>(options =>
            {
                options.IdentityClients = new IdentityClientConfigurationDictionary
                {
                    {
                        "App",
                        new IdentityClientConfiguration
                        {
                            Authority = configuration.GetValue<string>("RemoteServices:App:BaseUrl")!, //  App API AuthServer Authority
                            ClientId = "Management_Server", // 需使用 App API 方的 AuthServer 來 請求Token
                            ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                            Scope = "App",
                            GrantType = OpenIddictConstants.GrantTypes.ClientCredentials
                        }
                    }
                };
            });
         
         */

    }
}
