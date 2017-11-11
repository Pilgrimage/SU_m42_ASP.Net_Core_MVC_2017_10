namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Sales;

    public class SaleService : ISaleService
    {
        private readonly CarDealerDbContext db;

        public SaleService(CarDealerDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<SaleListModel> All()
        {
            return this.GetSalesList();
        }


        public SaleDetailsModel Details(int id)
        {
            return this.db
                .Sales
                .Where(s => s.Id == id)
                .Select(s => new SaleDetailsModel
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Car = new CarModel
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    }
                })
                .FirstOrDefault();
        }


        public IEnumerable<SaleListModel> All(double discount)
        {
            return ((discount == 0)
                ? this.GetSalesList().Where(s => s.TotalDiscount > 0).ToList()
                : this.GetSalesList().Where(s => s.TotalDiscount == (discount / 100)).ToList());
        }

        
        private IEnumerable<SaleListModel> GetSalesList()
        {
            return this.db
                .Sales
                .Select(s => new SaleListModel
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Price = s.Car.Parts.Sum(p => p.Part.Price)
                })
                .ToList();
        }
    }
}
