namespace FluffyDuffyMunchkinCatsRefactoring
{
    using FluffyDuffyMunchkinCatsRefactoring.Data;
    using FluffyDuffyMunchkinCatsRefactoring.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatsRefactoringDbContext>(options => 
                options.UseSqlServer(AppSettings.DbConnectionString));
        }

        public void Configure(IApplicationBuilder app) 
            => app
                .UseDatabaseMigration()
                . UseStaticFiles()
                .UseHtmlContentType()
                .UseRequestHandlers()
                .UseNotFoundHandler();


        // more classic way :-)
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    app.UseDatabaseMigration();
        //    app.UseStaticFiles();
        //    app.UseHtmlContentType();
        //    app.UseRequestHandlers();
        //    app.UseNotFoundHandler();
        //}
    }
}
