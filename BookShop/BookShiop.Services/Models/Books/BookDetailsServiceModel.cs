namespace BookShop.Services.Models.Books
{
    using AutoMapper;
    using BookShop.Common.Mapping;
    using BookShop.Models;
    using BookShop.Services.Models.Author;
    using System.Linq;

    public class BookDetailsServiceModel : BookWithCategoriesServiceModel, IMapFrom<Book>, IHaveCustomMapping
    {

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Book, BookDetailsServiceModel>()
                .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Name)))
                .ForMember(b => b.AuthorName, cfg => cfg.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"));
        }
    }
}