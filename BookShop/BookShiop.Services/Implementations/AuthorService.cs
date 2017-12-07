namespace BookShop.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using BookShop.Data;
    using BookShop.Models;
    using BookShop.Services.Models.Author;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorService : IAuthorService
    {
        private readonly BookShopDbContext db;

        public AuthorService(BookShopDbContext db)
        {
            this.db = db;
        }


        public async Task<AuthorDetailsServiceModel> ByIdDetailsAsync(int authorId)
        {
            if (!await this.AuthorIdExistsAsync(authorId))
            {
                return null;
            }

            return await this.db
                .Authors
                .Where(a => a.Id == authorId)
                .ProjectTo<AuthorDetailsServiceModel>()
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<BookWithCategoriesServiceModel>> ByIdBooksWithCategoriesAsync(int authorId)
        {
            if (!await this.AuthorIdExistsAsync(authorId))
            {
                return null;
            }

            return await this.db
                .Books
                .Where(b => b.AuthorId == authorId)
                .ProjectTo<BookWithCategoriesServiceModel>()
                .ToListAsync();
        }


        public async Task<int> CreateAsync(string firstName, string lastName)
        {
            if (await this.GetIdAsync(firstName, lastName) > 0)
            {
                return 0;
            }

            Author author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            this.db.Authors.Add(author);
            await this.db.SaveChangesAsync();

            return author.Id;
        }


        public async Task<int> GetIdAsync(string firstName, string lastName)
        {
            var author = await this.db.Authors.FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);

            if (author == null)
            {
                return 0;
            }

            return author.Id;
        }


        public async Task<int> GetIdOrCreateAsync(string firstName, string lastName)
        {
            var authorId = await this.GetIdAsync(firstName, lastName);

            if (authorId != 0)
            {
                return authorId;
            }

            return await this.CreateAsync(firstName, lastName);
        }


        public async Task<bool> AuthorIdExistsAsync(int authorId)
        {
            return await this.db.Authors.AnyAsync(a => a.Id == authorId);
        }

    }
}