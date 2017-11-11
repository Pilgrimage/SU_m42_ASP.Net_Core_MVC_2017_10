namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
    using CarDealer.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ISaleService sales;

        public SalesController(ISaleService sales)
        {
            this.sales = sales;
        }


        [Route("")]
        public IActionResult All()
        {
            return View(this.sales.All());
        }


        [Route("{id}")]
        public IActionResult Details(int id)
        {
            return this.ViewOrNotFound(this.sales.Details(id));
        }


        [Route("discounted/{percent?}")]
        public IActionResult All(double? percent)
        {
            //return this.ViewOrNotFound(this.sales.All((percent != null) ? 0 : (double)percent));
            return this.ViewOrNotFound(this.sales.All(percent ?? 0));
        }

    }
}