namespace BookShop.Api.Controllers
{
    using BookShop.Api.Infrastructure.Extensions;
    using BookShop.Api.Infrastructure.Filters;
    using BookShop.Api.Models.Autors;
    using BookShop.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using static WebConstants;

    public class AuthorsController : BaseApiController
    {
        private readonly IAuthorService authors;

        public AuthorsController(IAuthorService authors)
        {
            this.authors = authors;
        }


        [HttpGet(WithId)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.authors.ByIdDetailsAsync(id);
            return this.OkOrNotFound(result);
        }


        [HttpGet(WithId + "/books")]
        public async Task<IActionResult> GetBooksWithCategories(int id)
        {
            var result = await this.authors.ByIdBooksWithCategoriesAsync(id);
            return this.OkOrNotFound(result);
        }


        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Post([FromBody]PostAuthorRequestModel model)
        {
            var result = await this.authors.CreateAsync(model.FirstName, model.LastName);
            if (result == 0)
            {
                return this.BadRequest("Fault! Author with this name already exist in database.");
            }

            return this.Ok(result);
        }

    }
}