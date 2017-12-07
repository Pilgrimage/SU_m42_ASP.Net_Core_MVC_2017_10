namespace BookShop.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using BookShop.Common.Extensions;
    using BookShop.Data;
    using BookShop.Models;
    using BookShop.Services.Models.Books;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BookService : IBookService
    {
        private readonly BookShopDbContext db;
        private readonly IAuthorService authors;
        private readonly ICategoryService categories;

        public BookService(
            BookShopDbContext db,
            IAuthorService authors,
            ICategoryService categories)
        {
            this.db = db;
            this.authors = authors;
            this.categories = categories;
        }


        public async Task<BookWithAuthorServiceModel> ByIdBookAsync(int bookId)
        {
            if (!await this.BookIdExistsAsync(bookId))
            {
                return null;
            }

            return await this.db
                .Books
                .Where(b => b.Id == bookId)
                .ProjectTo<BookWithAuthorServiceModel>()
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<BookListingServiceModel>> AllAsync(string searchText)
        {
            return await this.db
                .Books
                .Where(b => b.Title.ToLower().Contains(searchText.ToLower()) 
                         || b.Description.ToLower().Contains(searchText.ToLower()))
                .OrderBy(b=>b.Title)
                .Take(10)
                .ProjectTo<BookListingServiceModel>()
                .ToListAsync();
        }


        public async Task<int> DeleteAsync(int bookId)
        {
            if (!await this.BookIdExistsAsync(bookId))
            {
                return 0;
            }

            Book book = this.db.Books.Find(bookId);

            this.db.Books.Remove(book);
            await this.db.SaveChangesAsync();

            return bookId;
        }


        public async Task<int> CreateAsync(
            string title, 
            string description, 
            decimal price, 
            int copies, 
            int edition, 
            DateTime releaseDate,
            int ageRestriction, 
            string authorFirstName, 
            string authorLastName, 
            string categories)
        {
            Book newBook = new Book
            {
                Title=title,
                Description=description,
                Price = price,
                Copies = copies,
                Edition=edition,
                ReleaseDate=releaseDate,
                AgeRestriction=ageRestriction,
            };

            newBook.AuthorId = await this.authors.GetIdOrCreateAsync(authorFirstName, authorLastName);

            var categoryNames = categories.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            if (categoryNames!=null)
            {
                foreach (string categoryName in categoryNames)
                {
                    int categoryId = await this.categories.GetIdOrCreateAsync(categoryName);
                    newBook.Categories.Add(new BookCategory{CategoryId = categoryId});
                }
            }

            this.db.Books.Add(newBook);
            await this.db.SaveChangesAsync();

            return newBook.Id;
        }


        public async Task<int> EditAsync(
            int bookId, 
            string title, 
            string description, 
            decimal price, 
            int copies, 
            int edition, 
            DateTime releaseDate,
            int ageRestriction, 
            int authorId)
        {
            Book book = this.db.Books.Find(bookId);

            if (!string.IsNullOrWhiteSpace(title))
            {
                book.Title = title;
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                book.Description = description;
            }

            if (price>0)
            {
                book.Price = price;
            }

            book.Copies = copies;

            if (edition>0)
            {
                book.Edition = edition;
            }

            book.ReleaseDate = releaseDate;

            if (ageRestriction>0)
            {
                book.AgeRestriction = ageRestriction;
            }

            if (authorId>0)
            {
                book.AuthorId = authorId;
            }
            
            await this.db.SaveChangesAsync();

            return bookId;
        }


        public async Task<bool> BookIdExistsAsync(int bookId)
        {
            return await this.db.Books.AnyAsync(b => b.Id == bookId);
        }

    }
}