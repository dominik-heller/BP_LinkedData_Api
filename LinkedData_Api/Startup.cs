using System;
using LinkedData_Api.Data;
using LinkedData_Api.Services;
using LinkedData_Api.Services.Contracts;
using LinkedData_Api.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LinkedData_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
                //c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LinkedData_Api", Version = "v1"});
            });
            
            //CustomServices
            services.AddSingleton<INamespaceFactoryService, NamespaceFactoryService>();
            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<IParametersProcessorService, ParametersProcessorService>();
            services.AddSingleton<ISparqlFactoryService, SparqlFactoryService>();
            services.AddSingleton<IResultFormatterService, ResultFormatterService>();
            services.AddSingleton<IDataAccess, DataAccess>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinkedData_Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

           // MyTests.Test();
        }
    }
}