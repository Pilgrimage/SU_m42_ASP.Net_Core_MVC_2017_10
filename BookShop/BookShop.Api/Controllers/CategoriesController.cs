namespace BookShop.Api.Controllers
{
    using BookShop.Api.Infrastructure.Extensions;
    using BookShop.Api.Infrastructure.Filters;
    using BookShop.Api.Models.Categories;
    using BookShop.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using static WebConstants;

    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService categories;

        public CategoriesController(ICategoryService categories)
        {
            this.categories = categories;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.categories.AllAsync();
            return this.OkOrNotFound(result);
        }


        [HttpGet(WithId)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this.categories.ByIdAsync(id);
            return this.OkOrNotFound(result);
        }

        [HttpDelete(WithId)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.categories.DeleteAsync(id);
            if (result == 0)
            {
                return this.BadRequest("Fault! Such a category does not exist in database.");
            }

            return this.Ok($"Category with Id={result} was deleted.");
        }


        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Get([FromBody]CategoryRequestModel model)
        {
            var result = await this.categories.CreateAsync(model.Name);

            if (result == 0)
            {
                return this.BadRequest("Fault! Category with this name already exist in database.");
            }

            return this.Ok(result);
        }

        [HttpPut(WithId)]
        [ValidateModelState]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryRequestModel model)
        {
            if (!await this.categories.CategoryIdExistsAsync(id))
            {
                return this.BadRequest("Fault! Such 'categoryId' does not exist in database.");
            }

            if (!await this.categories.CategoryNameExistsAsync(model.Name))
            {
                return this.BadRequest("Fault! Category with this name already exist in database.");
            }

            var result = await this.categories.EditAsync(id, model.Name);

            if (result == 0)
            {
                return this.BadRequest("Fault!");
            }

            return this.Ok($"Category with Id={result} was set to '{model.Name}'.");

        }


    }
}