namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Models.Cars;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : Controller
    {
        private readonly ICarService cars;

        public CarsController(ICarService cars)
        {
            this.cars = cars;
        }


        [Route("cars/all", Order = 1)]
        public IActionResult All()
        {
            return this.ViewOrNotFound(this.cars.WithParts());
        }


        [Route("cars/parts", Order = 2)]

        public IActionResult Parts()
        {
            return this.ViewOrNotFound(this.cars.WithParts());
        }


        [Route("cars/{make}", Order = 3)]
        public IActionResult ByMake(string make)
        {
            var result = this.cars.ByMake(make);

            return this.View(new CarsByMakeModel
            {
                Make = make,
                Cars = result
            });
        }


        [Route("cars/{id}/parts")]
        public IActionResult Details(int id)
        {
            return this.ViewOrNotFound(this.cars.WithParts(id));
        }
    }
}