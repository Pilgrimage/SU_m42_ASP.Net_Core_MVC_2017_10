namespace CarDealer.Web.Controllers
{
    using System.Collections.Generic;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Suppliers;
    using CarDealer.Web.Models.Suppliers;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : Controller
    {
        private const string SuppliersView = "Suppliers";
        private readonly ISupplierService suppliers;

        public SuppliersController(ISupplierService suppliers)
        {
            this.suppliers = suppliers;
        }


        public IActionResult Local()
        {
            var result = this.GetSuppliersModel(false);
            return this.View(SuppliersView, result);
        }


        public IActionResult Importers()
        {
            var result = this.GetSuppliersModel(true);
            return this.View(SuppliersView, result);
        }


        private SuppliersViewModel GetSuppliersModel(bool isImporter)
        {
            string type = isImporter ? "Importer" : "Local";
            var result = this.suppliers.AllListing(isImporter);

            return new SuppliersViewModel
            {
                Type = type,
                Suppliers = result 
            };
        }


        public IActionResult All()
        {
            return this.View(this.suppliers.All());
        }


        public IActionResult Delete(int id)
        {
            return this.View(id);
        }


        public IActionResult Destroy(int id)
        {
            if (!this.suppliers.Delete(id))
            {
                return this.NotFound();
            }

            return RedirectToAction(nameof(All));
        }


    }
}