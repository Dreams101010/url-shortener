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
using URLShortenerUI.Models.Helpers;
using URLShortenerDomainLayer.Services;
using URLShortenerDomainLayer.Interfaces;
using URLShortenerDomainLayer.Decorators.Command;
using URLShortenerDomainLayer.Decorators.Query;
using URLShortenerDataAccessLayer;
using URLShortenerDomainLayer.Models;

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
            services.AddSingleton<IHomeControllerModelHelper, HomeControllerModelHelper>();
            services.AddSingleton<URLService>();
            services.AddSingleton<ICache, RedisCache>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<AddUrlCommand>().As(typeof(ICommand<AddURLCommandParam, bool>));
            builder.RegisterType<GetUrlQuery>().As(typeof(IQuery<GetURLByShortURLQueryParam, URLDomainModel>));
            // command decorators
            builder.RegisterGenericDecorator(typeof(ErrorHandlingCommandDecorator<,>),
                typeof(ICommand<,>));
            builder.RegisterGenericDecorator(typeof(LoggingCommandDecorator<,>),
                typeof(ICommand<,>));
            builder.RegisterGenericDecorator(typeof(RetryCommandDecorator<,>),
                typeof(ICommand<,>));
            // query decorators
            builder.RegisterGenericDecorator(typeof(ErrorHandlingQueryDecorator<,>),
                typeof(IQuery<,>));
            builder.RegisterGenericDecorator(typeof(LoggingQueryDecorator<,>),
                typeof(IQuery<,>));
            builder.RegisterGenericDecorator(typeof(CachingQueryDecorator<,>),
                typeof(IQuery<,>));
            builder.RegisterGenericDecorator(typeof(RetryQueryDecorator<,>),
                typeof(IQuery<,>));
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
