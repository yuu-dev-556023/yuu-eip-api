using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Yuu.Eip.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EipDbContextFactory : IDesignTimeDbContextFactory<EipDbContext>
{
    public EipDbContext CreateDbContext(string[] args)
    {
        /* https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic */
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var env = args.FirstOrDefault();

        EipEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration(env);
        var connectionString = configuration.GetConnectionString("Default");

        if (!args.Contains("skip"))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"即將執行作業，連線字串: {connectionString}");
            Console.Write("是否正確? (Y/N): ");
            var input = Console.ReadKey();
            Console.WriteLine();

            if (input.Key == ConsoleKey.Y)
            {
                Console.WriteLine("作業開始");
            }
            else
            {
                Console.WriteLine("操作已被中斷。");
                Environment.Exit(0); // 結束當前執行緒
            }
        }

        var builder = new DbContextOptionsBuilder<EipDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new EipDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration(string? env)
    {
        var path = string.IsNullOrWhiteSpace(env) ? "appsettings.json" : ("appsettings." + env + ".json");

        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Yuu.Eip.DbMigrator/"))
            .AddJsonFile(path, optional: false);

        return builder.Build();
    }
}
