using System;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using FluentValidation.AspNetCore;
using LinkedData_Api.Data;
using LinkedData_Api.Filters;
using LinkedData_Api.Model.ViewModels;
using LinkedData_Api.Services;
using LinkedData_Api.Services.Contracts;
using LinkedData_Api.Swagger.ExampleSchemas;
using LinkedData_Api.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            //FluentValidation and ValidationFilter
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddMvc(options => { options.Filters.Add<ValidationFilter>(); })
                .AddFluentValidation(options => { options.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddControllers().AddNewtonsoftJson();
            //     services.AddMvc(opt => opt.EnableEndpointRouting = false);
            services.AddSwaggerGen(c =>
            {
                c.ExampleFilters();
                // c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LinkedData API",
                    Description =
                        @"<article>REST API for extraction and manipulation with semantic data, normally available only via SPARQL endpoints.<br>
                         This project is part of bachelor thesis <em>Linked data utilization via web API</em> at Faculty of Science, The University of South Bohemia, České Budějovice, Czech Republic.<br>
                         <a href='https://github.com/dominik-heller/BP_LinkedData_Api'>GitHub repository</a></article>",
                    Contact = new OpenApiContact
                    {
                        Name = "Dominik Heller",
                        Email = "helled01@prf.jcu.cz"
                    }
                });
                //Generate swagger extented documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

            //automapper
            services.AddAutoMapper(typeof(Startup));

            //enables JSONIGNORE
            //services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            //CustomServices
            services.AddSingleton<IDataAccess, DataAccess>();
            services.AddSingleton<INamespaceFactoryService, NamespaceFactoryService>();
            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<IParametersProcessorService, ParametersProcessorService>();
            services.AddSingleton<ISparqlFactoryService, SparqlFactoryService>();
            services.AddSingleton<IResultFormatterService, ResultFormatterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //crates IDataAccessService instance at start of app, not only when it is called
            app.ApplicationServices.GetService<IDataAccess>();
            // app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinkedData_Api v1");
                c.DocExpansion(DocExpansion.None);
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // MyTests.Test();
        }
    }
}