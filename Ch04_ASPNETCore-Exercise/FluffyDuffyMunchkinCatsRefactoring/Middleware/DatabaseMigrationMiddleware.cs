namespace FluffyDuffyMunchkinCatsRefactoring.Middleware
{
    using System.Threading.Tasks;
    using FluffyDuffyMunchkinCatsRefactoring.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class DatabaseMigrationMiddleware
    {
        private readonly RequestDelegate next;

        public DatabaseMigrationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }


        public Task Invoke(HttpContext context)
        {
            context.RequestServices.GetRequiredService<CatsRefactoringDbContext>().Database.Migrate();
            
            // Call the next delegate/middleware in the pipeline
            return this.next(context);
        }
    }
}