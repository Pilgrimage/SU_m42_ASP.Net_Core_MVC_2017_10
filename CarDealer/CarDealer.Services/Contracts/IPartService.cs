namespace CarDealer.Services.Contracts
{
    using CarDealer.Services.Models.Parts;
    using System.Collections.Generic;

    public interface IPartService
    {
        IEnumerable<PartListingServiceModel> AllListing(int page=1, int pageSize=25);

        IEnumerable<PartBasicServiceModel> All();

        void Create(string name, decimal price, int quantity, int supplierId);

        int Total();

        bool Delete(int id);

        PartListingServiceModel ById(int id);

        bool Edit(int id, decimal price, int quantity);

    }
}