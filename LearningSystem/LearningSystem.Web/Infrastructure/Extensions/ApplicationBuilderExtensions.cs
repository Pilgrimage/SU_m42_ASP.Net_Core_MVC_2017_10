﻿namespace LearningSystem.Web.Infrastructure.Extensions
{
    using System;
    using LearningSystem.Data;
    using LearningSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<LearningSystemDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task.Run(async () =>
                    {
                        var roles = new[]
                        {
                            WebConstants.AdministratorRole,
                            WebConstants.BlogAuthorRole,
                            WebConstants.TrainerRole
                        };

                        foreach (string role in roles)
                        {
                            bool roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole
                                {
                                    Name = role,
                                });
                            }
                        }

                        var adminName = "Administrator";
                        var adminEmail = "admin@admin.com";

                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                Email = adminEmail,
                                UserName = adminName,
                                Name = adminName,
                                Birthdate = DateTime.UtcNow
                            };

                            await userManager.CreateAsync(adminUser, "admin12");
                            await userManager.AddToRoleAsync(adminUser, adminName);
                        }

                    })
                    .GetAwaiter()
                    .GetResult();
            }

            return app;
        }

    }
}