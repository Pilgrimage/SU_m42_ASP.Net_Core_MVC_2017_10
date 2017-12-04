namespace LearningSystem.Web.Infrastructure.Filters
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;


    /// <summary>
    /// This action folter validate the model state, if :
    ///   1. The controller inherit Controller,
    ///   2. The action have parameter, which name contains "model"
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;

                if (controller==null)
                {
                    return;
                }

                var model = context
                    .ActionArguments
                    .FirstOrDefault(a => a.Key.ToLower().Contains("model"))
                    .Value;

                if (model == null)
                {
                    return;
                }

                context.Result = controller.View(model);
            }

        }
    }
}