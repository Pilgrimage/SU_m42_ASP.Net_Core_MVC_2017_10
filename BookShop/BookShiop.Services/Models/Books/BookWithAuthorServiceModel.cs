namespace BookShop.Services.Models.Books
{
    using AutoMapper;
    using BookShop.Common.Mapping;
    using BookShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BookWithAuthorServiceModel : IMapFrom<Book>, IHaveCustomMapping
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Copies { get; set; }

        public int? Edition { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int? AgeRestriction { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public IEnumerable<string> Categories { get; set; }


        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Book, BookWithAuthorServiceModel>()
                .ForMember(b => b.Categories, cfg => cfg.MapFrom(b => b.Categories.Select(c => c.Category.Name)))
                .ForMember(b => b.AuthorName, cfg => cfg.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"));
        }
    }
}