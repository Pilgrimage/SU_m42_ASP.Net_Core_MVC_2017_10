namespace CarDealer.Services.Models.Parts
{
    using System.ComponentModel.DataAnnotations;

    public class PartServiceModel
    {
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive number.")]
        public decimal Price { get; set; }
    }
}