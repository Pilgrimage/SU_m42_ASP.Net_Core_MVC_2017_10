namespace LearningSystem.Web.Controllers
{
    using LearningSystem.Services;
    using LearningSystem.Web.Models;
    using LearningSystem.Web.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly ICourseService courses;
        private readonly IUserService users;

        public HomeController(ICourseService courses, IUserService users)
        {
            this.courses = courses;
            this.users = users;
        }


        public async Task<IActionResult> Index()
        {
            return this.View(new HomeIndexViewModel
            {
                Courses = await this.courses.UpcomingAsync()
            });
        }


        public async Task<IActionResult> Search(SearchFormModel model)
        {
            SearchViewModel viewModel = new SearchViewModel();

            if (model.SearchInCourses)
            {
                if (string.IsNullOrEmpty(model.SearchText))
                {
                      // take All Courses maybe?
                }
                else
                {
                    viewModel.Courses = await this.courses.FindAsync(model.SearchText);
                }
            }

            if (model.SearchInUsers)
            {
                if (string.IsNullOrEmpty(model.SearchText))
                {
                    // take All Users maybe?
                }
                else
                {
                    viewModel.Users = await this.users.FindAsync(model.SearchText);
                }
            }

            viewModel.SearchText = model.SearchText;

            return this.View(viewModel);
        }



        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
