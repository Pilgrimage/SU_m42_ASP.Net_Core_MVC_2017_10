namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Data.Models;
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


        public IEnumerable<SaleListingModel> All()
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


        public IEnumerable<SaleListingModel> All(double discount)
        {
            return ((discount == 0)
                ? this.GetSalesList().Where(s => s.TotalDiscount > 0).ToList()
                : this.GetSalesList().Where(s => s.TotalDiscount == (discount / 100)).ToList());
        }


        private IEnumerable<SaleListingModel> GetSalesList()
        {
            return this.db
                .Sales
                .OrderByDescending(s => s.Id)
                .Select(s => new SaleListingModel
                {
                    Id = s.Id,
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    IsYoungDriver = s.Customer.IsYoungDriver,
                    Price = s.Car.Parts.Sum(p => p.Part.Price)
                })
                .ToList();
        }



        public bool Add(int carId, int customerId, double discount)
        {
            Car car = this.db.Cars.Find(carId);
            Customer customer = this.db.Customers.Find(customerId);

            if (car == null || customer == null)
            {
                return false;
            }

            if (0 > discount || discount > 1)
            {
                return false;
            }

            this.db.Sales.Add(new Sale
            {
                CarId = carId,
                CustomerId = customerId,
                Discount = discount
            });

            this.db.SaveChanges();

            return true;
        }



    }
}
