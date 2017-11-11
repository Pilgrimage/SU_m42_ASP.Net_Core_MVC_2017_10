namespace FluffyDuffyMunchkinCatsRefactoring.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using FluffyDuffyMunchkinCatsRefactoring.Handlers;
    using FluffyDuffyMunchkinCatsRefactoring.Middleware;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DatabaseMigrationMiddleware>();
        }


        public static IApplicationBuilder UseHtmlContentType(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HtmlContentTypeMiddleware>();
        }


        // work with handlers
        public static IApplicationBuilder UseRequestHandlers(this IApplicationBuilder builder)
        {
            // get all classes, that implement IHandler, and instance it
            var handlers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && typeof(IHandler).IsAssignableFrom(t))
                .Select(Activator.CreateInstance)
                .Cast<IHandler>()
                .OrderBy(h => h.Order);

            foreach (IHandler handler in handlers)
            {
                builder.MapWhen(handler.Condition, app =>
                {
                    app.Run(handler.RequestHandler);
                });
            }
            
            return builder;
        }


        public static void UseNotFoundHandler(this IApplicationBuilder builder)
        {
            builder.Run(async (context) =>
            {
                context.Response.StatusCode = HttpStatusCode.NotFound;
                await context.Response.WriteAsync("404 Page Was Not Found :/");
            });
        }


    }
}