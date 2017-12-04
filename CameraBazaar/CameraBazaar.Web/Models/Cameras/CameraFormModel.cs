namespace CameraBazaar.Web.Models.Cameras
{
    using CameraBazaar.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using CameraBazaar.Web.Infrastructure.Validations;

    public class CameraFormModel : IValidatableObject
    {
        [Display(Name = "Make:")]
        public CameraMakeType Make { get; set; }

        [Required]
        [Display(Name = "Model:")]
        [StringLength(100, ErrorMessage = "The Model must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [RegularExpression(@"^([A-Z0-9-]+)$", ErrorMessage = "The {0} must have only uppercase letters, digits and dash '-'.")]
        public string Model { get; set; }

        [Display(Name = "Price:")]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "The Price must have precision up to 2 digits after floating point.")]
        [Range(0.01, 999999999, ErrorMessage = "Price must be greater than 0.00 and smaller then one billion.")]
        public decimal Price { get; set; }

        [Range(0, 100)]
        [Display(Name = "Quantity:")]
        public int Quantity { get; set; }

        [Range(1, 30)]
        [Display(Name = "Min Shutter Speed:")]
        public int MinShutterSpeed { get; set; }

        [Range(2000, 8000)]
        [Display(Name = "Max Shutter Speed:")]
        public int MaxShutterSpeed { get; set; }

        [Display(Name = "Min ISO:")]
        public MinISO MinISO { get; set; }

        [Range(200, 409600)]
        [Display(Name = "Max ISO:")]
        [MaxIsoAcceptable(ErrorMessage = "The Max ISO must be dividable by 100.")]
        public int MaxISO { get; set; }

        [Display(Name = "Full Frame: ")]
        public bool IsFullFrame { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The Video Resolution must be max {1} characters long.")]
        [Display(Name = "Video Resolution:")]
        public string VideoResolution { get; set; }

        [Required]
        [Display(Name = "Light Metering: ")]
        public IEnumerable<LightMetering> LightMeterings { get; set; } = new List<LightMetering>();

        [Required]
        [StringLength(6000, ErrorMessage = "The Description must be max {1} characters long.")]
        [Display(Name = "Description:")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image URL:")]
        [StringLength(2000, ErrorMessage = "The Image URL must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [ImageUrlAcceptable(ErrorMessage = "The Image URL must start with 'http://' or 'https://'.")]
        public string ImageUrl { get; set; }


        // Now this is unnecessary, but but I left it for an example
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.MaxISO != ((this.MaxISO / 100) * 100))
            {
                yield return new ValidationResult("The Max ISO must be dividable by 100.");
            }
            else if (!this.ImageUrl.StartsWith("http://") && !this.ImageUrl.StartsWith("https://"))
            {
                yield return new ValidationResult("The Image URL must start with 'http://' or 'https://'.");
            }
        }
    }
}