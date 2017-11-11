namespace FluffyDuffyMunchkinCatsRefactoring.Handlers
{
    using System;
    using Microsoft.AspNetCore.Http;
    using FluffyDuffyMunchkinCatsRefactoring.Infrastructure;
    using FluffyDuffyMunchkinCatsRefactoring.Data;
    using Microsoft.Extensions.DependencyInjection;

    public class AddCatHandler : IHandler
    {
        public int Order => 2;


        public Func<HttpContext, bool> Condition =>
            ctx => ctx.Request.Path.Value == "/cat/add";


        public RequestDelegate RequestHandler => async (context) =>
        {

            if (context.Request.Method == HttpMethod.Get)
            {
                context.Response.Redirect("/cats-add-form.html");
            }
            else if (context.Request.Method == HttpMethod.Post)
            {
                var formData = context.Request.Form;

                // if field "Age" is empty, or non-number, his value = 0
                int age = 0;
                int.TryParse(formData["Age"], out age);

                Cat cat = new Cat
                {
                    Name = formData["Name"],
                    Age = age,
                    Breed = formData["Breed"],
                    ImageUrl = formData["ImageUrl"]
                };
                
                try
                {
                    if (string.IsNullOrWhiteSpace(cat.Name) ||
                        string.IsNullOrWhiteSpace(cat.Breed) ||
                        string.IsNullOrWhiteSpace(cat.ImageUrl))
                    {
                        throw new InvalidOperationException("Invalid cat data.");
                    }

                    var db = context.RequestServices.GetRequiredService<CatsRefactoringDbContext>();

                    using (db)
                    {
                        db.Add(cat);
                        await db.SaveChangesAsync();
                    }

                    context.Response.Redirect("/");
                }
                catch
                {
                    // if there is problem with Db (validation or another) and our validation of the fields
                    await context.Response.WriteAsync("<h2>Invalid cat data!</h2>");
                    await context.Response.WriteAsync(@"<a href=""/cat/add"">Back to the Form</a>");
                }
            }
        };
    }
}