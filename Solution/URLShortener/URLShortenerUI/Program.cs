using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace URLShortenerUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new();
            configurationBuilder.AddJsonFile("appsettings.json", false);
            var configuration = configurationBuilder.Build();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(configuration["LogFilePath"])
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting up...");
                GetHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Application start-up failed: {ex}");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder GetHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
