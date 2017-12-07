namespace BookShop.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using BookShop.Data;
    using BookShop.Models;
    using BookShop.Services.Models.Categories;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly BookShopDbContext db;

        public CategoryService(BookShopDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<CategoryListingServiceModel>> AllAsync()
        {
            return await this.db
                .Categories
                .ProjectTo<CategoryListingServiceModel>()
                .ToListAsync();
        }


        public async Task<CategoryListingServiceModel> ByIdAsync(int categoryId)
        {
            return await this.db
                .Categories
                .Where(c => c.Id==categoryId)
                .ProjectTo<CategoryListingServiceModel>()
                .FirstOrDefaultAsync();
        }


        public async Task<int> CreateAsync(string categoryName)
        {
            var categoryExists = await this.db.Categories.AnyAsync(c => c.Name == categoryName);
            if (categoryExists)
            {
                return 0;
            }

            Category newCategory = new Category{Name=categoryName};

            this.db.Categories.Add(newCategory);
            await this.db.SaveChangesAsync();

            return newCategory.Id;
        }


        public async Task<int> DeleteAsync(int categoryId)
        {
            if (!await this.CategoryIdExistsAsync(categoryId))
            {
                return 0;
            }

            Category category = this.db.Categories.Find(categoryId);
            this.db.Categories.Remove(category);
            await this.db.SaveChangesAsync();

            return categoryId;
        }


        public async Task<int> EditAsync(int categoryId, string categoryName)
        {
            if (await this.CategoryNameExistsAsync(categoryName))
            {
                return 0;
            }

            Category category = this.db.Categories.Find(categoryId);
            category.Name = categoryName;
            await this.db.SaveChangesAsync();

            return categoryId;
        }


        public async Task<int> GetIdOrCreateAsync(string categoryName)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (category!=null)
            {
                return category.Id;
            }

            Category newCategory = new Category { Name = categoryName };

            this.db.Categories.Add(newCategory);
            await this.db.SaveChangesAsync();

            return newCategory.Id;
        }


        public async Task<bool> CategoryIdExistsAsync(int categoryId)
        {
            return await this.db.Categories.AnyAsync(a => a.Id == categoryId);
        }

        public async Task<bool> CategoryNameExistsAsync(string categoryName)
        {
            return await this.db.Categories.AnyAsync(a => a.Name == categoryName);
        }
    }
}