namespace CarDealer.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Parts;
    using CarDealer.Web.Models.Parts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PartsController : Controller
    {
        private const int PageSize = 10;

        private readonly IPartService parts;
        private readonly ISupplierService suppliers;

        public PartsController(IPartService parts, ISupplierService suppliers)
        {
            this.parts = parts;
            this.suppliers = suppliers;
        }


        public IActionResult All(int page = 1)
        {
            int total = this.parts.Total();
            var totalPages = ((total / PageSize) + ((total % PageSize) == 0 ? 0 : 1));
            //var totalPages = (int)Math.Ceiling(total/(double)PageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            return View(new PartPageListingViewModel
            {
                Parts = this.parts.AllListing(page, PageSize),
                CurrentPage = page,
                TotalPages = totalPages,
            });
        }


        public IActionResult Create()
        {
            return this.View(new PartFormModel
            {
                Quantity = 1,
                Suppliers = GetSupplierListItems()
            });
        }

        [HttpPost]
        public IActionResult Create(PartFormModel formModel)
        {

            if (!this.suppliers.IdExist(formModel.SupplierId))
            {
                ModelState.AddModelError(nameof(PartFormModel.SupplierId), "Invalid supplier.");
            }

            if (!ModelState.IsValid)
            {
                formModel.Suppliers = GetSupplierListItems();
                return this.View(formModel);
            }

            this.parts.Create(
                formModel.Name,
                formModel.Price,
                formModel.Quantity,
                formModel.SupplierId);

            return this.RedirectToAction(nameof(All));
        }


        public IActionResult Delete(int id)
        {
            return this.View(id);
        }


        public IActionResult Destroy(int id)
        {
            if (!this.parts.Delete(id))
            {
                return this.NotFound();
            }

            return RedirectToAction(nameof(All));
        }


        public IActionResult Edit(int id)
        {
            return this.View(this.parts.ById(id));
        }


        [HttpPost]
        public IActionResult Edit(PartListingServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!this.parts.Edit(model.Id, model.Price, model.Quantity))
            {
                return this.NotFound();
            }

            return RedirectToAction(nameof(All));
        }


        private IEnumerable<SelectListItem> GetSupplierListItems()
        {
            return this.suppliers
                .All()
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                });
        }
    }
}
