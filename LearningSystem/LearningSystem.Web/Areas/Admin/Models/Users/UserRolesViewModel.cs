namespace LearningSystem.Web.Areas.Admin.Models.Users
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserRolesViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}