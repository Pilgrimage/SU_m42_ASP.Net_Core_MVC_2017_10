namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Suppliers;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data.Models;

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
                    IsImporter = s.IsImporter
                })
                .ToList();
        }


        public bool IdExist(int supplierId)
        {
            return this.db.Suppliers.Any(s => s.Id == supplierId);
        }


        public bool Delete(int id)
        {
            Supplier supplier = this.db.Suppliers.Find(id);

            if (supplier == null)
            {
                return false;
            }

            this.db.Suppliers.Remove(supplier);
            this.db.SaveChanges();
            return true;
        }

        public SupplierServiceModel ById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}