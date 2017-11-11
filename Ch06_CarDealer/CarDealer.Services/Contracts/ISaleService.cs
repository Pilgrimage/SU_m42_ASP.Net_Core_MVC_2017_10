namespace CarDealer.Services.Contracts
{
    using CarDealer.Services.Models.Sales;
    using System.Collections.Generic;

    public interface ISaleService
    {
        IEnumerable<SaleListModel> All();

        SaleDetailsModel Details(int id);

        IEnumerable<SaleListModel> All(double discount);
    }
}