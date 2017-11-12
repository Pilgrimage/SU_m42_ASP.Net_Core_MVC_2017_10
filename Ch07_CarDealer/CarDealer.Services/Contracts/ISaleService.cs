namespace CarDealer.Services.Contracts
{
    using CarDealer.Services.Models.Sales;
    using System.Collections.Generic;

    public interface ISaleService
    {
        IEnumerable<SaleListingModel> All();

        SaleDetailsModel Details(int id);

        IEnumerable<SaleListingModel> All(double discount);

    }
}