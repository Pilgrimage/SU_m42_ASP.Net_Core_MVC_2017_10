namespace CarDealer.Web.Models.Sales
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Customers;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SalesFormModel
    {

        [Range(0,int.MaxValue)]
        [Display(Name = "Car:")]
        public int CarId { get; set; }

        public IEnumerable<SelectListItem> Cars { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Customer:")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        [Range(0, 100)]
        [Display(Name = "Discount:")]
        public int Discount { get; set; }

    }
}