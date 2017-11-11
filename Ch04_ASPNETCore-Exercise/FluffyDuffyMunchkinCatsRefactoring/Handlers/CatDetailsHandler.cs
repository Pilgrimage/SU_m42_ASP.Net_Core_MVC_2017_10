namespace FluffyDuffyMunchkinCatsRefactoring.Handlers
{
    using System;
    using FluffyDuffyMunchkinCatsRefactoring.Data;
    using FluffyDuffyMunchkinCatsRefactoring.Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class CatDetailsHandler : IHandler
    {
        public int Order => 3;


        public Func<HttpContext, bool> Condition => 
            ctx => ctx.Request.Path.Value.StartsWith("/cat") && ctx.Request.Method == HttpMethod.Get;


        public RequestDelegate RequestHandler => async (context) =>
        {
            var urlParts = context
                .Request
                .Path
                .Value
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length < 2)
            {
                context.Response.Redirect("/");
                return;
            }
            else
            {
                int catId = 0;
                int.TryParse(urlParts[urlParts.Length - 1], out catId);

                if (catId == 0)
                {
                    context.Response.Redirect("/");
                    return;
                }

                var db = context.RequestServices.GetRequiredService<CatsRefactoringDbContext>();

                using (db)
                {
                    Cat cat = await db.Cats.FindAsync(catId);
                    if (cat == null)
                    {
                        context.Response.Redirect("/");
                        return;
                    }

                    await context.Response.WriteAsync($"<h1>{cat.Name}</h1>");
                    await context.Response.WriteAsync($@"<img src=""{cat.ImageUrl}"" alt=""{cat.Name}"" width=""300""/>");
                    await context.Response.WriteAsync("<br/>");
                    await context.Response.WriteAsync($"<p><strong>Age: {cat.Age}</strong></p>");
                    await context.Response.WriteAsync($"<p><strong>Breed: {cat.Breed}</strong></p>");
                }
            }

        };
        
    }
}