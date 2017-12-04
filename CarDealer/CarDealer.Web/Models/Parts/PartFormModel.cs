namespace CarDealer.Web.Models.Parts
{
    using CarDealer.Services.Models.Suppliers;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class PartFormModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage="Price must be positive number.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name="Supplier")]
        public int SupplierId { get; set; }

        //public IEnumerable<SupplierServiceModel> Suppliers { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
    }
}