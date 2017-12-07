namespace BookShop.Services
{
    using BookShop.Services.Models.Author;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAuthorService
    {
        Task<AuthorDetailsServiceModel> ByIdDetailsAsync(int authorId);

        Task<IEnumerable<BookWithCategoriesServiceModel>> ByIdBooksWithCategoriesAsync(int authorId);

        Task<int> CreateAsync(string firstName, string lastName);

        Task<int> GetIdAsync(string firstName, string lastName);

        Task<int> GetIdOrCreateAsync(string firstName, string lastName);

        Task<bool> AuthorIdExistsAsync(int authorId);
    }
}