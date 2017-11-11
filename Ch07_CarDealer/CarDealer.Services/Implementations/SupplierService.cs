namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models;

    public class SupplierService : ISupplierService
    {
        private readonly CarDealerDbContext db;

        public SupplierService(CarDealerDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<SupplierModel> All(bool isImporter)
        {
            return this.db
                .Suppliers
                .Where(s => s.IsImporter == isImporter)
                .Select(s => new SupplierModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ToralParts = s.Parts.Count
                })
                .ToList();
        }
    }
}