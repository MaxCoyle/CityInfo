﻿using System.IO;
using CityInfo.API.Repository;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSwaggerGen(swagger =>
                {
                    swagger.DescribeAllEnumsAsStrings();
                    swagger.SwaggerDoc("CityInfo", 
                        new Swashbuckle.AspNetCore.Swagger.Info
                        {
                            Title = "City Information"
                        });
                }
            );

            services.AddTransient<CitiesRepository>(cityInfoRepository => new CitiesRepository(Configuration));
            services.AddTransient<ICitiesRepository, CitiesRepository>();
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Information);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/CityInfo/swagger.json", "City Information");
            });

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("City Information");
            });
        }
    }
}
