﻿namespace CarDealer.Web
{
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<CarDealerDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<CarDealerDbContext>()
                .AddDefaultTokenProviders();


            services.AddDomainServices();

            // Replace this (and the next IServices) :
            //services.AddTransient <ICustomerService, CustomerService> ();
            //services.AddTransient <ICarService, CarService> ();
            //services.AddTransient <ISupplierService, SupplierService> ();
            //services.AddTransient <ISaleService, SaleService> ();


            services.AddMvc(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                //// For Example
                //routes.MapRoute(
                //    name: "customers",
                //    template: "customers/all/{order}", 
                //    defaults: new {controller="Customers", action="All" });

                

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
