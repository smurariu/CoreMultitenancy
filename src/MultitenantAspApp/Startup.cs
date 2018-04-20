using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Multitenancy;
using System;

namespace MultitenantAspApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(o => { o.UseTenantRoutePrefix(); })
                    .AddJsonFormatters();

            services.AddTransient<IHelloWorldService, DefaultHelloWorldService>();

            services.Configure<ValuesControllerOptions>(o =>
            {
                o.Value1Value = 42;
                o.Value2Value = "value1_configured_by_delegate";
            });

            //Tenant configuration
            services.ConfigureTenant(ConfigureSz());
        }

        private static Action<Tenant> ConfigureSz()
        {
            return t =>
            {
                t.TenantId = "SZ";
                t.ServiceCollection.AddTransient<IHelloWorldService, SzHelloWorldService>();

                t.ServiceCollection.Configure<ValuesControllerOptions>(o =>
                {
                    o.Value1Value = 42;
                    o.Value2Value = "value1_configured_by_delegate_for_tenant_SZ";
                });
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMultitenancy();
            app.UseMvc();
        }
    }
}