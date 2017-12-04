namespace LearningSystem.Web.Controllers
{
    using LearningSystem.Data.Models;
    using LearningSystem.Services;
    using LearningSystem.Services.Models.Courses;
    using LearningSystem.Web.Infrastructure.Extensions;
    using LearningSystem.Web.Models.Trainer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using static WebConstants;

    [Authorize(Roles = TrainerRole)]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainers;
        private readonly ICourseService courses;
        private readonly UserManager<User> userManager;

        public TrainerController(
            ITrainerService trainers,
            ICourseService courses,
            UserManager<User> userManager)
        {
            this.trainers = trainers;
            this.courses = courses;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Courses()
        {
            var trainerId = this.userManager.GetUserId(this.User);

            var courses = await this.trainers.CoursesAsync(trainerId);

            return this.View(courses);
        }


        public async Task<IActionResult> Students(int id)
        {
            var userId = this.userManager.GetUserId(this.User);
            if (!await this.trainers.IsTrainerAsync(id, userId))
            {
                return this.BadRequest();
            }

            var students = await this.trainers.StudentsInCourseAsync(id);
            var course = await this.courses.ByIdAsync<CourseListingServiceModel>(id);

            if (students == null | course == null)
            {
                return this.NotFound();
            }

            return this.View(new StudentInCourseViewModel
            {
                Students = students,
                Course = course
            });
        }

        [HttpPost]
        public async Task<IActionResult> GradeStudent(int id, string studentId, Grade grade)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);
            if (!await this.trainers.IsTrainerAsync(id, userId))
            {
                return this.BadRequest();
            }

            var result = await this.trainers.SetGradeAsync(id, studentId, grade);
            if (result)
            {
                this.TempData.AddSuccessMessage("Student evaluation is successful.");
            }
            else
            {
                this.TempData.AddErrorMessage("Student evaluation failed.");
            }

            return this.RedirectToAction(nameof(this.Students), new { id });
        }


        public async Task<IActionResult> DownloadExam(int id, string studentId)
        {

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);
            if (!await this.trainers.IsTrainerAsync(id, userId))
            {
                return this.BadRequest();
            }
            
            var fullInfo = await this.trainers.GetExamFullInfoAsync(id, studentId);

            if (fullInfo == null)
            {
                var studentCourseNamesInfo = await this.trainers.StudentInCourseNamesAsync(id, studentId);
                this.TempData.AddErrorMessage($"There is no exam submission from '{studentCourseNamesInfo.Username}' for '{studentCourseNamesInfo.CourseName}' course.");
                return RedirectToAction(nameof(this.Students), new {id});
            }


            return File(fullInfo.ExamSubmission, "application/zip", $"{fullInfo.CourseName}-{fullInfo.Username}-[{fullInfo.SubmissionTime.ToString("yyyy-MM-dd_HH.mm")}]-({fullInfo.ExamFileName}).zip");
        }
    }
}