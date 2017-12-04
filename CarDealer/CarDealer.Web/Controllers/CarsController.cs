namespace CarDealer.Web.Controllers
{
    using System.Collections.Generic;
    using CarDealer.Services.Contracts;
    using CarDealer.Web.Infrastructure.Extensions;
    using CarDealer.Web.Models.Cars;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    public class CarsController : Controller
    {
        private const int PageSize = 10;
        private const int PartsPageSize = 5;

        private readonly ICarService cars;
        private readonly IPartService parts;

        public CarsController(ICarService cars, IPartService parts)
        {
            this.cars = cars;
            this.parts = parts;
        }


        public IActionResult All(int page = 1)
        {
            int total = this.cars.Total();
            var totalPages = ((total / PageSize) + ((total % PageSize) == 0 ? 0 : 1));

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            return View(new CarPageListingViewModel
            {
                Cars = this.cars.WithParts(page, PageSize),
                CurrentPage = page,
                TotalPages = totalPages
            });

        }


        public IActionResult Parts(int page = 1)
        {
            int total = this.cars.Total();
            var totalPages = ((total / PartsPageSize) + ((total % PartsPageSize) == 0 ? 0 : 1));

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            return View(new CarPageListingViewModel
            {
                Cars = this.cars.WithParts(page, PartsPageSize),
                CurrentPage = page,
                TotalPages = totalPages
            });
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View(new CarFormModel
            {
                Parts= GetPartsListItems()
            });
        }


        [HttpPost]
        [Authorize]
        public IActionResult Create(CarFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                formModel.Parts = GetPartsListItems();
                return this.View(formModel);
            }

            this.cars.Create(formModel.Make, formModel.Model, formModel.TravelledDistance, formModel.PartsId);

            return RedirectToAction(nameof(All));
        }


        [Route("cars/{id}/parts")]
        public IActionResult Details(int id)
        {
            return this.ViewOrNotFound(this.cars.WithParts(id));
        }


        //[Route("byMake/{make?}")]
        public IActionResult ByMake(string make)
        {
            var makers = this.cars
                .Makers()
                .Select(m => new SelectListItem
                {
                    Text = m.Make,
                    Value = m.Make
                });

            if (string.IsNullOrWhiteSpace(make))
            {
                make = makers
                    .Select(m => m.Value)
                    .FirstOrDefault();
            }

            var cars = this.cars.ByMake(make);

            CarsByMakeChoiseModel model = new CarsByMakeChoiseModel
            {
                Make = make,
                Makers = makers,
                Cars = cars
            };

            return this.View(model);
        }


        private IEnumerable<SelectListItem> GetPartsListItems()
        {
            return this.parts
                .All()
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                })
                .ToList();
        }

    }
}