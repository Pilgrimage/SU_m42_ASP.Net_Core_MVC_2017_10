namespace LearningSystem.Web.Controllers
{
    using LearningSystem.Data.Models;
    using LearningSystem.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using LearningSystem.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;

    public class UsersController : Controller
    {
        private readonly IUserService users;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService users, UserManager<User> userManager)
        {
            this.users = users;
            this.userManager = userManager;
        }


        [Authorize]
        public async Task<IActionResult> Profile(string username)
        {
            var currentUsername = this.User.Identity.Name;

            if (username != currentUsername.ToLower() && !this.User.IsInRole(WebConstants.AdministratorRole))
            {
                TempData.AddErrorMessage($"You do not have permission to view '{username}' profile. You can only watch your own.");
                username = currentUsername;
            }

            var user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                return this.NotFound();
            }

            var profile = await this.users.ProfileAsync(user.Id);

            return this.View(profile);
        }


        [Authorize]
        public async Task<IActionResult> DownloadCertificate(int id)
        {
            var userId = this.userManager.GetUserId(this.User);

            var certificateContents = await this.users.GetPdfCertificateAsync(id, userId);

            if (certificateContents == null)
            {
                return this.BadRequest();
            }

            //var studentCourseNamesInfo = await this.trainers.StudentInCourseNamesAsync(id, studentId);


            return File(certificateContents, "application/pdf", $"{this.User.Identity.Name}-Certificate.pdf");

        }
    }
}