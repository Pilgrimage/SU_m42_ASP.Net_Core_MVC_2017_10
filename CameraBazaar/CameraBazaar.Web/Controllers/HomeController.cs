namespace CameraBazaar.Web.Controllers
{
    using CameraBazaar.Web.Infrastructure.Filters;
    using CameraBazaar.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    [Log]
    public class HomeController : Controller
    {
        [MeasureTime]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
