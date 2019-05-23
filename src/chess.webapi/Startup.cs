using System;
using chess.engine;
using chess.webapi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace chess.webapi
{
    public class Startup
    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }
//
//        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var config = services.ConfigureConfig();
            services.ConfigureLogging(config);
            services.AddSwaggerDocument();

            services.AddTransient<IChessService, ChessGameService>();
            services.AddTransient<IPerfService, PerfService>();
            services.AddChessDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IConfigurationRoot ConfigureConfig(this IServiceCollection serviceCollection)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddSingleton(config);
            return config;
        }

        public static void ConfigureLogging(this IServiceCollection serviceCollection, IConfigurationRoot config)
        {
            // Add logging
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
//                loggingBuilder.AddConsole();
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateLogger();

        }
    }
}
