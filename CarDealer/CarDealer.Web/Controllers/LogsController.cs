namespace CarDealer.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StackExchange.Redis;

    public class LogsController : Controller
    {
        public IActionResult Index(string search)
        {
            //var logs = null;

            //if (!string.IsNullOrWhiteSpace(search))
            //{
            //    logs = ....
            //}

            return View(null);
        }
    }
}