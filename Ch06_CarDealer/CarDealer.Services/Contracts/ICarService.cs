namespace CarDealer.Services.Contracts
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Cars;

    public interface ICarService
    {
        IEnumerable<CarModel> ByMake(string make);

        IEnumerable<CarWithPartsModel> WithParts();

        CarWithPartsModel WithParts(int id);

    }
}