namespace CarDealer.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtensions
    {
        public static IActionResult ViewOrNotFound(this Controller controller, object viewModel)
        {
            if (viewModel==null)
            {
                return controller.NotFound();
            }

            return controller.View(viewModel);
        }
    }
}