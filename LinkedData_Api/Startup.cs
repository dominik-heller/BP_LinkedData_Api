using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using LinkedData_Api.Data;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services;
using LinkedData_Api.Services.Contracts;
using LinkedData_Api.Swagger.ExampleSchemas;
using LinkedData_Api.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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
            services.AddControllers().AddNewtonsoftJson();
       //     services.AddMvc(opt => opt.EnableEndpointRouting = false);
            services.AddSwaggerGen(c =>
            {
                c.ExampleFilters();
                // c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LinkedData_Api", Version = "v1"});
                //Generate swagger advanced documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();



            //enables JSONIGNORE
            //services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


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
           // app.UseMvc();
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