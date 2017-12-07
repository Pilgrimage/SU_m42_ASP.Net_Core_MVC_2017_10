namespace BookShop.Services
{
    using BookShop.Services.Models.Books;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookService
    {
        Task<BookWithAuthorServiceModel> ByIdBookAsync(int bookId);
            
        Task<IEnumerable<BookListingServiceModel>> AllAsync(string searchText);

        Task<int> DeleteAsync(int bookId);

        Task<bool> BookIdExistsAsync(int bookId);

        Task<int> CreateAsync(
            string title,
            string description,
            decimal price,
            int copies,
            int edition,
            DateTime releaseDate,
            int ageRestriction,
            string authorFirstName,
            string authorLastName,
            string categories);

        Task<int> EditAsync(
            int bookId,
            string title,
            string description,
            decimal price,
            int copies,
            int edition,
            DateTime releaseDate,
            int ageRestriction,
            int authorId);
    }
}