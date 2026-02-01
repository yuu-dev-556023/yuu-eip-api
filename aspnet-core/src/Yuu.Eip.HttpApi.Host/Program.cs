using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Yuu.AspNetCore.Hosting.Configuration;
using Yuu.Serilog.Seq.Extensions;

namespace Yuu.Eip;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            /* Microsoft Debug 相關 */
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            /* Abp Debug 相關 */
            .MinimumLevel.Override("Volo.Abp.AbpApplicationBase", LogEventLevel.Debug)
            .MinimumLevel.Override("Volo.Abp.AspNetCore.Bundling.BundlerBase", LogEventLevel.Debug)
            .MinimumLevel.Override("Volo.Abp.AspNetCore.Mvc.AspNetCoreApiDescriptionModelProvider", LogEventLevel.Debug)
            /* AspNetCore Debug 相關 */
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Debug)
            /* SqlCommand Debug 相關 */
            /* .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information) */
            /* OpenIddict Debug 相關 */
            .MinimumLevel.Override("OpenIddict.Server.OpenIddictServerDispatcher", LogEventLevel.Debug)
            .MinimumLevel.Override("OpenIddict.Validation.OpenIddictValidationDispatcher", LogEventLevel.Debug)
            .MinimumLevel.Override("OpenIddict", LogEventLevel.Debug)
            .MinimumLevel.Override("OpenIddict.Server", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Async(c => c.Console(restrictedToMinimumLevel: LogEventLevel.Debug))
            .WriteTo.Logger(c => c.Filter.ByExcluding(logEvent => logEvent.Properties.TryGetValue("SourceContext", out var sourceContext) && sourceContext.ToString().Contains("Yuu.EntityFrameworkCore.Database.Command")).UseSeqAsync())
            /* .UseSeqAsync() */
            .CreateLogger();

        try
        {
            Log.Information("Starting Yuu.Eip.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .AddAppSettingsSecretsJson()
                .AddAppSettingsEnvsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<EipHttpApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
