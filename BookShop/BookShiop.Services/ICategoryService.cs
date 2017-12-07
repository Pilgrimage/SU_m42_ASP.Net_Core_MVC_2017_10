namespace BookShop.Services
{
    using BookShop.Services.Models.Categories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListingServiceModel>> AllAsync();

        Task<CategoryListingServiceModel> ByIdAsync(int categoryId);

        Task<int> CreateAsync(string categoryName);

        Task<int> DeleteAsync(int categoryId);

        Task<int> EditAsync(int categoryId, string categoryName);

        Task<int> GetIdOrCreateAsync(string categoryName);

        Task<bool> CategoryIdExistsAsync(int categoryId);

        Task<bool> CategoryNameExistsAsync(string categoryName);
    }
}