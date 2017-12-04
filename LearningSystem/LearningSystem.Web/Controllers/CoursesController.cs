namespace LearningSystem.Web.Controllers
{
    using System;
    using System.IO;
    using LearningSystem.Data.Models;
    using LearningSystem.Services;
    using LearningSystem.Services.Models.Courses;
    using LearningSystem.Web.Infrastructure.Extensions;
    using LearningSystem.Web.Models.Courses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using static LearningSystem.Data.DataConstants;


    public class CoursesController : Controller
    {
        private readonly ICourseService courses;
        private readonly UserManager<User> userManager;

        public CoursesController(ICourseService courses, UserManager<User> userManager)
        {
            this.courses = courses;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await this.courses.ByIdAsync<CourseDetailsServiceModel>(id);

            if (course == null)
            {
                return this.NotFound();
            }

            var model = new CourseDetailsViewModel();

            model.Course = course;

            if (User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(this.User);
                model.IsUserSignedInCourse = await this.courses.IsUserSignedInCourseAsync(id, userId);
            }
            
            return this.View(model);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUp(int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var result = await this.courses.SignUpUserAsync(id, userId);

            if (!result)
            {
                TempData.AddErrorMessage("Sign In failed.");
                return this.BadRequest();
            }

            TempData.AddSuccessMessage($"Student is successfully signed to the course.");
            return RedirectToAction(nameof(Details), new {id});
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut(int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var result = await this.courses.SignOutUserAsync(id, userId);

            if (!result)
            {
                TempData.AddErrorMessage("Sign Out failed.");
                return this.BadRequest();
            }

            TempData.AddSuccessMessage($"Student successfully go out of the course.");
            return RedirectToAction(nameof(Details), new { id });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitExam(int id, IFormFile exam)
        {
            var userId = this.userManager.GetUserId(this.User);
            var result = await this.courses.IsUserSignedInCourseAsync(id, userId);

            if (!result)
            {
                TempData.AddErrorMessage("Invalid request.");
                return RedirectToAction(nameof(this.Details), new { id });
            }


            if (!exam.FileName.EndsWith(".zip")
                || exam.Length == 0
                || exam.Length > CourseExamSubmissionFileLength)
            {
                TempData.AddErrorMessage("Your submission schould be a '.zip' file with max size 2 MB (and dont be empty).");
                return RedirectToAction(nameof(this.Details), new {id});
            }

            var fileContents = await exam.ToByteArrayAsync();

            result = await this.courses.SaveExamSubmissionAsync(id, userId, fileContents, exam.FileName);
            if (!result)
            {
                TempData.AddErrorMessage($"Uploading file '{exam.FileName}' failed.");
                return RedirectToAction(nameof(this.Details), new { id });
            }

            TempData.AddSuccessMessage($"File '{exam.FileName}' successfully uploaded.");
            return RedirectToAction(nameof(this.Details), new { id });
        }
    }
}