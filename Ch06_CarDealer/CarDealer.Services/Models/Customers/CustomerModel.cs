﻿namespace CarDealer.Services.Models.Customers
{
    using System;

    public class CustomerModel
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}