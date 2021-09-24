using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Autofac;
using MongoDB.Driver;
using StackExchange.Redis;
using UrlShortenerUI.Models.Helpers;
using UrlShortenerDomainLayer.Services;
using UrlShortenerDomainLayer.Interfaces;
using UrlShortenerDomainLayer.Decorators.Command;
using UrlShortenerDomainLayer.Decorators.Query;
using UrlShortenerDataAccessLayer;
using UrlShortenerDomainLayer.Models;
using UrlShortenerUI.Filters;
using AspNetCore.ReCaptcha;

namespace UrlShortenerUI
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
            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));
            services.AddOptions();
            services.AddControllersWithViews((opts) =>
            {
                opts.Filters.Add(new ExceptionRedirectFilter());
            });
            services.AddSingleton((provider) =>
                new MongoClient(Configuration.GetSection("ConnectionStrings")["MongoDBConnectionString"]));
            services.AddSingleton((provider) =>
                ConnectionMultiplexer
                    .Connect(Configuration.GetSection("ConnectionStrings")["RedisConnectionString"])
                );
            services.AddSingleton<IHomeControllerModelHelper, HomeControllerModelHelper>();
            services.AddSingleton<UrlService>();
            services.AddSingleton<ICache, RedisCache>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<AddUrlCommand>().As(typeof(ICommand<AddUrlCommandParam, bool>));
            builder.RegisterType<GetUrlQuery>().As(typeof(IQuery<GetUrlByShortUrlQueryParam, UrlDomainModel>));
            builder.RegisterType<RemoveUrlByShortUrlCommand>()
                .As(typeof(ICommand<RemoveUrlByShortUrlCommandParam, bool>));
            // command decorators
            builder.RegisterGenericDecorator(typeof(RetryCommandDecorator<,>),
                typeof(ICommand<,>));
            builder.RegisterGenericDecorator(typeof(LoggingCommandDecorator<,>),
                typeof(ICommand<,>));
            builder.RegisterGenericDecorator(typeof(ErrorHandlingCommandDecorator<,>),
                typeof(ICommand<,>));
            // query decorators
            builder.RegisterGenericDecorator(typeof(CachingQueryDecorator<,>),
                typeof(IQuery<,>));
            builder.RegisterGenericDecorator(typeof(RetryQueryDecorator<,>),
                typeof(IQuery<,>));
            builder.RegisterGenericDecorator(typeof(LoggingQueryDecorator<,>),
                typeof(IQuery<,>));
            builder.RegisterGenericDecorator(typeof(ErrorHandlingQueryDecorator<,>),
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
                endpoints.MapControllerRoute(name: "error",
                    pattern: "error",
                    defaults: new { controller = "Home", action = "Error" });
            });
        }
    }
}
