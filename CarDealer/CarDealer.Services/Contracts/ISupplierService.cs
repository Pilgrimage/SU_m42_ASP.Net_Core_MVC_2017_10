namespace CarDealer.Services.Contracts
{
    using CarDealer.Services.Models.Suppliers;
    using System.Collections.Generic;

    public interface ISupplierService
    {
        IEnumerable<SupplierListingServiceModel> AllListing(bool isImporter);

        IEnumerable<SupplierServiceModel> All();

        bool IdExist(int supplierId);

        bool Delete(int id);

        SupplierServiceModel ById(int id);
    }
}