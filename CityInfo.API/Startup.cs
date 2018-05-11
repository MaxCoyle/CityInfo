using CityInfo.API.Repository;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"Data source=(local);Initial catalog=CityInfo;Integrated Security=SSPI;";
            services.AddTransient(provider => new CityRepository(connectionString, conn => new UnitOfWork(conn)));

            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));

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

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityRepository cityRepository)
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

            cityRepository.GetAllCities();

            app.UseMvc();
        }
    }
}
