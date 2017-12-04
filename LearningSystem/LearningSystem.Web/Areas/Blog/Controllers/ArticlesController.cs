namespace LearningSystem.Web.Areas.Blog.Controllers
{
    using System;
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Blog;
    using LearningSystem.Services.Html;
    using LearningSystem.Web.Areas.Blog.Models.Articles;
    using LearningSystem.Web.Controllers;
    using LearningSystem.Web.Infrastructure.Extensions;
    using LearningSystem.Web.Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using static LearningSystem.Services.ServiceConstants;
    using static WebConstants;

    [Area(BlogArea)]
    [Authorize(Roles = BlogAuthorRole)]
    public class ArticlesController : Controller
    {
        private readonly IHtmlService html;
        private readonly IBlogArticleService articles;
        private readonly UserManager<User> userManager;

        public ArticlesController(
            IHtmlService html,
            IBlogArticleService articles,
            UserManager<User> userManager)
        {
            this.html = html;
            this.articles = articles;
            this.userManager = userManager;
        }


        //[AllowAnonymous]
        //public async Task<IActionResult> Index(int page = 1)
        //{

        //    int total = await this.articles.TotalAsync();

        //    if (total==0)
        //    {
        //        TempData.AddErrorMessage($"There is not articles in the blog.");
        //        return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
        //    }

        //    var totalPages = ((total / BlogArticlesPageSize) + ((total % BlogArticlesPageSize) == 0 ? 0 : 1));

        //    if (page < 1)
        //    {
        //        page = 1;
        //    }
        //    else if (page > totalPages)
        //    {
        //        page = totalPages;
        //    }

        //    return View(new ArticlePageListingViewModel
        //    {
        //        Articles = await this.articles.AllAsync(page),
        //        CurrentPage = page,
        //        TotalPages = totalPages
        //    });

        //}



        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchText, int page = 1)
        {
            searchText = searchText ?? string.Empty;

            int total = await this.articles.TotalFindAsync(searchText);

            if (total == 0)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    TempData.AddErrorMessage($"There is not articles in the blog.");
                    return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
                }
                else
                {
                    TempData.AddErrorMessage($"There is NOT articles to display for '{searchText}'. ALL Articles are shown.");
                    return RedirectToAction(nameof(Index), new { searchText = string.Empty, page = 1 });

                }
            }

            var totalPages = ((total / BlogArticlesPageSize) + ((total % BlogArticlesPageSize) == 0 ? 0 : 1));

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            return View(new ArticlePageListingViewModel
            {
                Articles = await this.articles.FindAsync(searchText, page),
                SearchText = searchText,
                CurrentPage = page,
                TotalPages = totalPages
            });

        }





        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            return this.ViewOrNotFound(await this.articles.ById(id));
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create(PublishArticleFormModel model)
        {
            model.Content = this.html.Sanitize(model.Content);

            var userId = this.userManager.GetUserId(this.User);

            await this.articles.CreateAsync(model.Title, model.Content, userId);

            TempData.AddSuccessMessage($"Article {model.Title} was successfully published.");

            //return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
            return RedirectToAction(nameof(Index));
        }
    }
}