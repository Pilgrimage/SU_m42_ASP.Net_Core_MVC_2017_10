namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Suppliers;
    using System.Collections.Generic;
    using System.Linq;

    public class SupplierService : ISupplierService
    {
        private readonly CarDealerDbContext db;

        public SupplierService(CarDealerDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<SupplierListingServiceModel> AllListing(bool isImporter)
        {
            return this.db
                .Suppliers
                .OrderByDescending(s => s.Id)
                .Where(s => s.IsImporter == isImporter)
                .Select(s => new SupplierListingServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ToralParts = s.Parts.Count
                })
                .ToList();
        }


        public IEnumerable<SupplierServiceModel> All()
        {
            return this.db
                .Suppliers
                .OrderBy(s => s.Name)
                .Select(s => new SupplierServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToList();
        }


        public bool IdExist(int supplierId)
        {
            return this.db.Suppliers.Any(s => s.Id == supplierId);
        }
    }
}