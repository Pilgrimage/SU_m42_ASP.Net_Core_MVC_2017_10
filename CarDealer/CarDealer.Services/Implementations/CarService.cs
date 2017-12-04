namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models.Cars;
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Parts;

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


        public IEnumerable<CarWithPartsModel> WithParts(int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;

            return this.db
                .Cars
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CarWithPartsModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.Parts.Select(p => new PartServiceModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                })
                .ToList();
        }


        public IEnumerable<CarBasicServiceModel> CarsList()
        {
            return this.db
                .Cars
                .OrderByDescending(c => c.Id)
                .Select(c => new CarBasicServiceModel
                {
                    Id = c.Id,
                    FullModel = $"{c.Make} {c.Model}",
                    Price = c.Parts.Sum(p=>p.Part.Price)
                })
                .ToList();
        }

        public CarBasicServiceModel ByIdBasic(int id)
        {
            return this.db
                .Cars
                .Where(c => c.Id==id)
                .Select(c => new CarBasicServiceModel
                {
                    Id = c.Id,
                    FullModel = $"{c.Make} {c.Model}",
                    Price = c.Parts.Sum(p => p.Part.Price)
                })
                .FirstOrDefault();
        }


        public CarWithPartsModel WithParts(int id)
        {
            if (this.db.Cars.Any(c => c.Id == id))
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
                        Parts = c.Parts.Select(p => new PartServiceModel
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price
                        })
                    })
                    .FirstOrDefault();
            }

            return null;
        }


        public IEnumerable<MakeModel> Makers()
        {
            return this.db
                .Cars
                .OrderBy(c => c.Make)
                .Select(c => new MakeModel
                {
                    Make = c.Make
                })
                .Distinct()
                .ToList();
        }


        public void Create(string make, string model, long travelledDistance, IEnumerable<int> parts)
        {
            Car car = new Car
            {
                Make = make,
                Model = model,
                TravelledDistance = travelledDistance,
            };

            var ExistingPartIds = this.db
                .Parts
                .Where(p => parts.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            foreach (int partId in ExistingPartIds)
            {
                car.Parts.Add(new PartCar {PartId = partId});
            }

            this.db.Cars.Add(car);
            this.db.SaveChanges();
        }


        public int Total()
        {
            return this.db.Cars.Count();
        }
    }
}