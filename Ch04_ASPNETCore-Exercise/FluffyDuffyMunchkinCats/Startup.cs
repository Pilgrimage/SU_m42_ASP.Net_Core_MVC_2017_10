namespace FluffyDuffyMunchkinCats
{
    using System;
    using System.Linq;
    using Data;
    using Infrastructure;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.Edm.Library.Expressions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatsDbContext>(options =>
                options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=CatsDb; Integrated Security=True;"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Set database
            app.Use((context, next) =>
            {
                context.RequestServices.GetRequiredService<CatsDbContext>().Database.Migrate();
                return next();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }


            app.UseStaticFiles();


            // Set Content-Type
            app.Use((context, next) =>
            {
                // method is not async!
                context.Response.Headers.Add("Content-Type", "text/html");
                return next();
            });


            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/" &&
                       ctx.Request.Method == HttpMethod.Get,
                home =>
                {
                    home.Run(async (context) =>
                    {
                        await context.Response.WriteAsync($"<h1>{env.ApplicationName}</h1>");

                        var db = context.RequestServices.GetRequiredService<CatsDbContext>();

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

                    });
                });


            app.MapWhen(
                ctx => ctx.Request.Path.Value == "/cat/add",
                catAdd =>
                {
                    catAdd.Run(async (context) =>
                    {
                        if (context.Request.Method == HttpMethod.Get)
                        {
                            // old brutal style
                            //await context.Response.WriteAsync("<h1>Add Cat</h1>");
                            //await context.Response.WriteAsync(@"
                            //    <form method=""POST"" >
                            //    </form>");

                            // with redirect to static page
                            context.Response.Redirect("/cats-add-form.html");

                        }
                        else if (context.Request.Method == HttpMethod.Post)
                        {

                            if (context.Request.HasFormContentType)
                            {
                                // if there is a form

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
                                    // handmade check for validity
                                    if (string.IsNullOrWhiteSpace(cat.Name) ||
                                        string.IsNullOrWhiteSpace(cat.Breed) ||
                                        string.IsNullOrWhiteSpace(cat.ImageUrl))
                                    {
                                        throw new InvalidOperationException("Invalid cat data.");
                                    }

                                    var db = context.RequestServices.GetRequiredService<CatsDbContext>();

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
                            else
                            {
                                // if there is not a form
                                context.Response.Redirect("/cat/add");
                            }


                        }

                        return;
                    });
                }
            );


            app.MapWhen(
                ctx => ctx.Request.Path.Value.StartsWith("/cat") &&
                       ctx.Request.Method == HttpMethod.Get,
                catDetails =>
                {
                    catDetails.Run(async (context) =>
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

                            if (catId==0)
                            {
                                context.Response.Redirect("/");
                                return;
                            }

                            var db = context.RequestServices.GetRequiredService<CatsDbContext>();

                            using (db)
                            {
                                Cat cat = await db.Cats.FindAsync(catId);
                                if (cat==null)
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

                        return;
                    });

                });


            app.Run(async (context) =>
            {
                // if does not match nothing !
                context.Response.StatusCode = HttpStatusCode.NotFound;
                await context.Response.WriteAsync("404 Page Was Not Found :/");
            });
        }
    }
}
