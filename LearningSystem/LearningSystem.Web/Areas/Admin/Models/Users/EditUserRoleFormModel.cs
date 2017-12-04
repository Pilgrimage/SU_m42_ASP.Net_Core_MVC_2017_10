namespace LearningSystem.Web.Areas.Admin.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class EditUserRoleFormModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Submit { get; set; }
    }
}