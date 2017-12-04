namespace LearningSystem.Web.Areas.Blog.Models.Articles
{
    using System.Collections.Generic;
    using LearningSystem.Services.Blog.Models;

    public class ArticlePageListingViewModel
    {
        public IEnumerable<BlogArticleListingServiceModel> Articles { get; set; }

        public string SearchText { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PreviousPage => this.CurrentPage <= 1
            ? 1
            : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage >= this.TotalPages
            ? this.TotalPages
            : this.CurrentPage + 1;

    }
}