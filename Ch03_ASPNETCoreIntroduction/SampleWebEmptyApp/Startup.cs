namespace SampleWebEmptyApp
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using SampleWebEmptyApp.Services;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add service as a Singleton. e.g. omly 1 instance (for example)
            services.AddSingleton<IMyService, MyService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.MapWhen(ctx => ctx.Request.Query.ContainsKey("search"), searchApp =>
            {
                searchApp.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("My-Custom-Header","Random Value");

                    await next();
                });

                searchApp.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Searching");
                });
            });


            app.Map("/cats", catsApp =>
            {
                // if match - execute and finalize
                catsApp.Run(async (context) =>
                {
                    await context.Response.WriteAsync("CATS AREA");
                });
            });



            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Type","text/html");

                await next();
            });


            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.StartsWith("/users"))
                {
                    await context.Response.WriteAsync("Users Area");
                }

                await next();

            });

            

            // this wil be executed on any request
            app.Run(async (context) =>
            {
                // get an instance of desired Service (from registered services)
                var service = context.RequestServices.GetService<IMyService>();

                Console.WriteLine($"{context.Request.Method} - {context.Request.Path}");
                await context.Response.WriteAsync("<h1>Hello from ASP.NET Core!</h1>");
                await context.Response.WriteAsync("<h2>ASP.NET Core is cool (may be)...</h2>");
                await context.Response.WriteAsync($"<h2>{service.Name}</h2>");
            });




        }
    }
}
