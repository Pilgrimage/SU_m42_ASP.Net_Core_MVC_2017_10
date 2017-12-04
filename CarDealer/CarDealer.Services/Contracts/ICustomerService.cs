namespace CarDealer.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;

    public interface ICustomerService
    {
        IEnumerable<CustomerModel> Ordered(OrderDirection order);

        IEnumerable<CustomerBasicServiceModel> CustomersList();

        CustomerTotalSalesModel TotalSalesById(int id);

        void Create(string name, DateTime birthDate, bool isYounDriver);

        void Edit(int id, string name, DateTime birthDate, bool isYoungDriver);

        CustomerModel ById(int id);

        bool Exists(int id);
    }
}