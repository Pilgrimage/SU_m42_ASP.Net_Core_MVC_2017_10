namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Cars;
    using System.Collections.Generic;
    using System.Linq;

    public class CarService : ICarService
    {
        private readonly CarDealerDbContext db;

        public CarService(CarDealerDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<CarModel> ByMake(string make)
        {
            return this.db
                .Cars
                .Where(c => c.Make.ToLower() == make.ToLower())
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new CarModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToList();
        }


        public IEnumerable<CarWithPartsModel> WithParts()
        {
            return this.db
                .Cars
                .Select(c => new CarWithPartsModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.Parts.Select(p => new PartModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                })
                .ToList();
        }

        public CarWithPartsModel WithParts(int id)
        {
            if (this.db.Cars.Any(c => c.Id==id))
            {
                return this.db
                    .Cars
                    .Where(c => c.Id == id)
                    .Select(c => new CarWithPartsModel
                    {
                        Id = c.Id,
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance,
                        Parts = c.Parts.Select(p => new PartModel
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price
                        })
                    })
                    .FirstOrDefault();
            }

            return null;
        }

    }
}