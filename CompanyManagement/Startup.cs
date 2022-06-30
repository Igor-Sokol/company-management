using DataAccess.Companies;
using DataAccess.Employees;
using DataAccess.SqlServer.Companies;
using DataAccess.SqlServer.Employees;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SqlConnection>((services) => new SqlConnection(this.Configuration.GetConnectionString("CompanyManagement")));

            services.AddScoped<ICompanyDataAccess, CompanySqlServerAccess>();
            services.AddScoped<IEmployeeDataAccess, EmployeeSqlServerAccess>();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("CompanyPage", "Companies/Pages/{page:int}",
                    new { Controller = "Company", action = "Index", page = 1 });

                endpoints.MapControllerRoute("EmployeePage", "Employees/Pages/{page:int}",
                    new { Controller = "Employee", action = "Index", page = 1 });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Company}/{action=Index}");

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
