﻿namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Services.Contracts;
    using CarDealer.Services.Models;
    using CarDealer.Services.Models.Customers;
    using CarDealer.Services.Models.Sales;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomerService : ICustomerService
    {
        private readonly CarDealerDbContext db;

        public CustomerService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CustomerModel> Ordered(OrderDirection order)
        {
            var customersQuery = this.db.Customers.AsQueryable();

            switch (order)
            {
                case OrderDirection.Ascending:
                    customersQuery = customersQuery
                        .OrderBy(c => c.BirthDate)
                        .ThenBy(c => c.Name);
                    break;
                case OrderDirection.Descending:
                    customersQuery = customersQuery
                        .OrderByDescending(c => c.BirthDate)
                        .ThenBy(c => c.Name);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid order direction: {order}.");
            }

            return customersQuery
                .Select(c => new CustomerModel
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver

                })
                .ToList();
        }


        public CustomerTotalSalesModel TotalSalesById(int id)
        {
            if (this.db.Customers.Any(c => c.Id == id))
            {
                return this.db
                    .Customers
                    .Where(c => c.Id == id)
                    .Select(c => new CustomerTotalSalesModel
                    {
                        Name = c.Name,
                        IsYoungDriver = c.IsYoungDriver,
                        BoughtCars = c.Sales.Select(s=> new SaleModel
                        {
                            Price = s.Car.Parts.Sum(p=>p.Part.Price),
                            Discount = s.Discount
                        })
                    })
                    .FirstOrDefault();
            }

            return null;
        }
    }
}