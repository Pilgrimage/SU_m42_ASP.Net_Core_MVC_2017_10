namespace LearningSystem.Web.Areas.Admin.Controllers
{
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Admin;
    using LearningSystem.Web.Areas.Admin.Models.Courses;
    using LearningSystem.Web.Controllers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LearningSystem.Web.Infrastructure.Extensions;

    public class CoursesController : BaseAdminController
    {
        private readonly UserManager<User> userManager;
        private readonly IAdminCourseService courses;

        public CoursesController(
            UserManager<User> userManager,
            IAdminCourseService courses)
        {
            this.userManager = userManager;
            this.courses = courses;
        }


        public async Task<IActionResult> Index()
        {
            var courses = await this.courses.AllAsync();
            return View(courses);
        }


        public async Task<IActionResult> Create()
        {
            return View(new AddCourseFormModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(60),
                Trainers = await this.GetTrainers()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(AddCourseFormModel model)
        {
            
            if (!ModelState.IsValid)
            {
                model.Trainers = await this.GetTrainers();
                return this.View(model);
            }

            var endDate = model.EndDate;

            await this.courses.CreateAsync(
                model.Name,
                model.Description,
                model.TrainerId,
                model.StartDate,
                //new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59)
                model.EndDate.AddDays(1));

            TempData.AddSuccessMessage($"Course {model.Name} created successfully.");

            //return RedirectToAction(nameof(HomeController.Index), "Home", new {area=string.Empty});
            return RedirectToAction(nameof(Index));
        }



        private async Task<IEnumerable<SelectListItem>> GetTrainers()
        {
            var trainers = await this.userManager.GetUsersInRoleAsync(WebConstants.TrainerRole);
            return trainers.Select(t => new SelectListItem
            {
                Text = t.UserName,
                Value = t.Id
            })
            .ToList();
        }
    }
}