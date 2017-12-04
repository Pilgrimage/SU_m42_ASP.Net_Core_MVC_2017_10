namespace CarDealer.Web.Controllers
{
    using System.Linq;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Customers;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Models.Sales;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ISaleService sales;
        private readonly ICarService cars;
        private readonly ICustomerService customers;

        public SalesController(ISaleService sales, ICarService cars, ICustomerService customers)
        {
            this.sales = sales;
            this.cars = cars;
            this.customers = customers;
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



        [Authorize]
        [Route("create")]
        public IActionResult Create()
        {
            SalesFormModel createModel = new SalesFormModel();

            return this.View(this.ReloadCollections(createModel));
        }


        [Authorize]
        [HttpPost]
        [Route("create")]
        public IActionResult Create(SalesFormModel createModel)
        {
            createModel = this.ReloadCollections(createModel);

            if (!ModelState.IsValid)
            {
                return this.View(createModel);
            }


            SalesReviewViewModel finalizeModel = new SalesReviewViewModel();

            finalizeModel.CustomerId = createModel.CustomerId;
            CustomerModel customer = this.customers.ById(createModel.CustomerId);
            finalizeModel.CustomerName = customer.Name;
            finalizeModel.DiscountDriver = customer.IsYoungDriver ? 5 : 0;

            finalizeModel.DiscountCar = createModel.Discount;

            finalizeModel.CarId = createModel.CarId;
            CarBasicServiceModel car = this.cars.ByIdBasic(createModel.CarId);
            finalizeModel.CarMakeModel = car.FullModel;
            finalizeModel.Price = car.Price;

            return this.View(nameof(Finalize), finalizeModel);
        }

        [Authorize]
        [HttpPost]
        [Route("finalize")]
        public IActionResult Finalize(SalesReviewViewModel finalizeModel)
        {
            if (!this.sales.Add(finalizeModel.CarId, finalizeModel.CustomerId, (double)finalizeModel.DiscountTotal/100))
            {
                return this.NotFound();
            }
            
            return RedirectToAction(nameof(All));
        }


        private SalesFormModel ReloadCollections(SalesFormModel model)
        {
            model.Customers = this.customers
                .CustomersList()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            model.Cars = this.cars
                .CarsList()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.FullModel
                })
                .ToList();

            return model;
        }
    }
}