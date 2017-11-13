namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Parts;
    using System.Collections.Generic;
    using System.Linq;

    public class PartService : IPartService
    {

        private readonly CarDealerDbContext db;

        public PartService(CarDealerDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<PartListingServiceModel> AllListing(int page = 1, int pageSize = 25)
        {
            page = page < 1 ? 1 : page;

            var allResults = this.db
                .Parts
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PartListingServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierName = p.Supplier.Name
                })
                .ToList();

            return allResults;
        }


        public int Total()
        {
            return this.db.Parts.Count();
        }


        public bool Delete(int id)
        {
            Part part = this.db.Parts.Find(id);

            if (part == null)
            {
                return false;
            }

            this.db.Parts.Remove(part);
            this.db.SaveChanges();
            return true;
        }


        public PartListingServiceModel ById(int id)
        {
            return this.db
                .Parts
                .Select(p => new PartListingServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierName = p.Supplier.Name
                })
                .FirstOrDefault(p => p.Id == id);
        }


        public bool Edit(int id, decimal price, int quantity)
        {
            Part part = this.db.Parts.Find(id);

            if (part == null)
            {
                return false;
            }

            part.Price = price;
            part.Quantity = quantity;

            this.db.SaveChanges();
            return true;
        }


        public IEnumerable<PartBasicServiceModel> All()
        {
            return this.db
                .Parts
                .OrderBy(p => p.Id)
                .Select(p => new PartBasicServiceModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }


        public bool Edit(int id, string name, int Quantity)
        {
            return true;
        }


        public void Create(string name, decimal price, int quantity, int supplierId)
        {
            Part part = new Part
            {
                Name = name,
                Price = price,
                Quantity = (quantity > 0 ? quantity : 1),
                SupplierId = supplierId
            };

            this.db.Parts.Add(part);
            this.db.SaveChanges();
        }
    }
}