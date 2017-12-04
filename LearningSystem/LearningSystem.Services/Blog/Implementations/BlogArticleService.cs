namespace LearningSystem.Services.Blog.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using LearningSystem.Data;
    using LearningSystem.Data.Models;
    using LearningSystem.Services.Blog.Models;
    using Microsoft.EntityFrameworkCore;
    using static ServiceConstants;

    public class BlogArticleService : IBlogArticleService
    {
        private readonly LearningSystemDbContext db;

        public BlogArticleService(LearningSystemDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<BlogArticleListingServiceModel>> AllAsync(int page = 1)
        {
            return await this.db
                .Articles
                .OrderByDescending(a => a.PublishDate)
                .Skip((page - 1) * BlogArticlesPageSize)
                .Take(BlogArticlesPageSize)
                .ProjectTo<BlogArticleListingServiceModel>()
                .ToListAsync();
        }


        public async Task<IEnumerable<BlogArticleListingServiceModel>> FindAsync(string searchText, int page = 1)
        {
            searchText = searchText ?? string.Empty;
            return await this.db
                .Articles
                .Where(a=>a.Title.ToLower().Contains(searchText.ToLower()) || a.Content.ToLower().Contains(searchText.ToLower()))
                .OrderByDescending(a => a.PublishDate)
                .Skip((page - 1) * BlogArticlesPageSize)
                .Take(BlogArticlesPageSize)
                .ProjectTo<BlogArticleListingServiceModel>()
                .ToListAsync();
        }


        public async Task<int> TotalAsync()
        {
            return await this.db.Articles.CountAsync();
        }

        public async Task<int> TotalFindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;
            return await this.db
                .Articles
                .Where(a => a.Title.ToLower().Contains(searchText.ToLower()) 
                         || a.Content.ToLower().Contains(searchText.ToLower()))
                .CountAsync();
        }


        public async Task<BlogArticleDetailsServiceModel> ById(int id)
        {
            return await this.db
                .Articles
                .Where(a => a.Id==id)
                .ProjectTo<BlogArticleDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }


        public async Task CreateAsync(string title, string content, string authorId)
        {
            Article article = new Article
            {
                Title = title,
                Content = content,
                AuthorId = authorId,
                PublishDate = DateTime.UtcNow
            };

            this.db.Articles.Add(article);
            await this.db.SaveChangesAsync();
        }
    }
}