namespace FluffyDuffyMunchkinCatsRefactoring.Handlers
{
    using System;
    using System.Linq;
    using FluffyDuffyMunchkinCatsRefactoring.Data;
    using FluffyDuffyMunchkinCatsRefactoring.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class HomeHandler : IHandler
    {

        public int Order => 1;


        public Func<HttpContext, bool> Condition =>
            ctx => ctx.Request.Path.Value == "/" && ctx.Request.Method == HttpMethod.Get;


        public RequestDelegate RequestHandler => async (context) =>
        {
            //var env = context.RequestServices.GetRequiredService<IHostingEnvironment>();
            //await context.Response.WriteAsync($"<h1>{env.ApplicationName}</h1>");

            await context.Response.WriteAsync($"<h1>{AppSettings.ApplicationName}</h1>");

            var db = context.RequestServices.GetRequiredService<CatsRefactoringDbContext>();

            using (db)
            {
                var catData = db
                    .Cats
                    .Select(c => new
                    {
                        c.Id,
                        c.Name
                    })
                    .ToList();

                await context.Response.WriteAsync("<ul>");

                foreach (var cat in catData)
                {
                    await context.Response.WriteAsync($@"<li><a href=""/cat/{cat.Id}"">{cat.Name}</a></li>");
                }

                await context.Response.WriteAsync("</ul>");
                await context.Response.WriteAsync(@"
                            <form action=""/cat/add"">
                                <input type=""submit"" value=""Add Cat"" />
                            </form>");
            }

        };
    }
}