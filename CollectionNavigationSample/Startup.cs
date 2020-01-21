using System.Collections.Generic;
using CollectionNavigationSample.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CollectionNavigationSample
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
            services.AddDbContext<AppDbContext>(
                options => options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers(mvcOptions =>
                mvcOptions.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Filter().Expand().Count().OrderBy().MaxTop(100);
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });

            // Seed some data
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope
                    .ServiceProvider.GetService<AppDbContext>();
                if (!context.Department.Any() && !context.Employer.Any())
                {
                    var scienceDept = new Department { Name = "Science" };
                    var artsDept = new Department { Name = "Arts" };
                    context.Department.AddRange(scienceDept, artsDept);

                    context.Employer.Add(new Employer
                    {
                        Name = "Steven",
                        Employees = new List<Employee>()
                        {
                            new Employee { Name = "Jack", Department = scienceDept }, 
                            new Employee { Name = "Jane", Department = artsDept }
                        }
                    });

                    context.SaveChanges();
                }
            }
        }

        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Employer>("Employer");
            odataBuilder.EntitySet<Employee>("Employee");
            odataBuilder.EntitySet<Department>("Department");

            return odataBuilder.GetEdmModel();
        }
    }
}
