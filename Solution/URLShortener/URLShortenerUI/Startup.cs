using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Autofac;
using MongoDB.Driver;
using StackExchange.Redis;

namespace URLShortenerUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public ILifetimeScope AutoFacContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllersWithViews();
            services.AddSingleton((provider) =>
                new MongoClient(Configuration.GetSection("ConnectionStrings")["MongoDBConnectionString"]));
            services.AddSingleton((provider) =>
                ConnectionMultiplexer.Connect(Configuration.GetSection("ConnectionStrings")["RedisConnectionString"]));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "regEmpty",
                    pattern: "",
                    defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "regRegister",
                    pattern: "register",
                    defaults: new { controller = "Home", action = "Register" });
                endpoints.MapControllerRoute(name: "shortUrl",
                    pattern: "{*shortUrl}",
                    defaults: new { controller = "Home", action = "RedirectTo" });
            });
        }
    }
}
