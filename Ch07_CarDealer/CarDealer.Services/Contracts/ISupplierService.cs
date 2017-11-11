namespace CarDealer.Services.Contracts
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    public interface ISupplierService
    {
        IEnumerable<SupplierModel> All(bool isImporter);
    }
}