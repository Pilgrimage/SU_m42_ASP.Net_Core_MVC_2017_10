namespace LearningSystem.Web.Areas.Admin.Models.Courses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using static LearningSystem.Data.DataConstants;

    public class AddCourseFormModel : IValidatableObject
    {
        [Required]
        [MinLength(CourseNameMinLength)]
        [MaxLength(CourseNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CourseDescriptionMaxLength)]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Trainer")]
        [Required]
        public string TrainerId { get; set; }

        public IEnumerable<SelectListItem> Trainers { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.StartDate<=DateTime.UtcNow)
            {
                yield return new ValidationResult("Start date must be in the future.");
            }

            if (this.StartDate>this.EndDate)
            {
                yield return new ValidationResult("Start date must be before end date.");
            }
        }
    }
}
