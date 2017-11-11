namespace CarDealer.Services.Contracts
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;

    public interface ICustomerService
    {
        IEnumerable<CustomerModel> Ordered(OrderDirection order);

        CustomerTotalSalesModel TotalSalesById(int id);
    }
}