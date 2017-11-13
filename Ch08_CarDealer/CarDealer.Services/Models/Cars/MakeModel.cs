using System.ComponentModel.DataAnnotations;

namespace CarDealer.Services.Models.Cars
{
    public class MakeModel
    {
        [Required]
        [MaxLength(50)]
        public string Make { get; set; }
    }
}