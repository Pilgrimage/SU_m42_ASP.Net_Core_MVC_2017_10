namespace FluffyDuffyMunchkinCatsRefactoring.Handlers
{
    using System;
    using Microsoft.AspNetCore.Http;

    public interface IHandler
    {
        // In what order should be executed
        int Order { get;  }

        // condition to be executed (the type of lambda, which we submit)
        Func<HttpContext, bool> Condition { get; }

        // what needs to be done (logic)
        RequestDelegate RequestHandler { get; }

    }
}