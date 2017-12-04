namespace CarDealer.Web.Models.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomerFormModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //[DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name="Young Driver")]
        public bool IsYoungDriver { get; set; }
    }
}