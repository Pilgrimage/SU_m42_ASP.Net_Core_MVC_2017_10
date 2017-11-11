namespace CarDealer.Web.Controllers
{
    using CarDealer.Services.Contracts;
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


        private SuppliersModel GetSuppliersModel(bool isImporter)
        {
            string type = isImporter ? "Importer" : "Local";
            var result = this.suppliers.All(isImporter);

            return new SuppliersModel
            {
                Type = type,
                Suppliers = result 
            };
        }
    }
}