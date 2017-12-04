namespace CarDealer.Services.Contracts
{
    using System.Collections.Generic;
    using CarDealer.Services.Models.Cars;

    public interface ICarService
    {
        IEnumerable<CarModel> ByMake(string make);

        IEnumerable<CarWithPartsModel> WithParts(int page = 1, int pageSize = 10);

        IEnumerable<CarBasicServiceModel> CarsList();

        CarBasicServiceModel ByIdBasic(int id);

        CarWithPartsModel WithParts(int id);

        IEnumerable<MakeModel> Makers();

        void Create(string make, string model, long travelledDistance, IEnumerable<int> parts);

        int Total();
    }
}