namespace BookShop.Api.Controllers
{
    using BookShop.Api.Infrastructure.Extensions;
    using BookShop.Api.Infrastructure.Filters;
    using BookShop.Api.Models.Books;
    using BookShop.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using static WebConstants;

    public class BooksController : BaseApiController
    {
        private readonly IBookService books;
        private readonly IAuthorService authors;

        public BooksController(IBookService books, IAuthorService authors)
        {
            this.books = books;
            this.authors = authors;
        }


        [HttpGet(WithId)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.books.ByIdBookAsync(id);
            return this.OkOrNotFound(result);
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string search="")
        {
            var result = await this.books.AllAsync(search);
            return this.OkOrNotFound(result);
        }


        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Post([FromBody]CreateBookRequestModel model)
        {
            var result = await this.books.CreateAsync(
                model.Title,
                model.Description,
                model.Price,
                model.Copies,
                model.Edition,
                model.ReleaseDate,
                model.AgeRestriction,
                model.AuthorFirstName,
                model.AuthorLastName,
                model.Categories);

            if (result == 0)
            {
                return this.BadRequest("Fault! This book cannot be created.");
            }

            return this.Ok(result);
        }


        [HttpDelete(WithId)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.books.DeleteAsync(id);
            if (result == 0)
            {
                return this.BadRequest("Fault! Such a book does not exist in database.");
            }

            return this.Ok($"Book with Id={result} was deleted.");
        }


        [HttpPut(WithId)]
        [ValidateModelState]
        public async Task<IActionResult> Put(int id, [FromBody]EditBookRequestModel model)
        {
            if (!await this.books.BookIdExistsAsync(id))
            {
                return this.BadRequest("Fault! Such 'bookId' does not exist in database.");
            }

            if (model.AuthorId>0 && !await this.authors.AuthorIdExistsAsync(model.AuthorId))
            {
                return this.BadRequest("Fault! Such 'authorId' does not exist in database.");
            }
            
            var result = await this.books.EditAsync(
                id,
                model.Title,
                model.Description,
                model.Price,
                model.Copies,
                model.Edition,
                model.ReleaseDate,
                model.AgeRestriction,
                model.AuthorId);

            if (result == 0)
            {
                return this.BadRequest("Fault! This book cannot be edited.");
            }

            return this.OkOrNotFound($"Book with Id={result} was edited.");
        }

    }
}