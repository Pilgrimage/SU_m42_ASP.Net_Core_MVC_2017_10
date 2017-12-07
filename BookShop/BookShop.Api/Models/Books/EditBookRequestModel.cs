namespace BookShop.Api.Models.Books
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static BookShop.Models.DataConstants;

    public class EditBookRequestModel
    {
        [MinLength(BookTitleMinLength)]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; }

        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; }

        [Range(0, BookMaxPrice)]
        public decimal Price { get; set; }

        [Range(0, CopiesMaxValue)]
        public int Copies { get; set; }

        [Range(0, EditionMaxValue)]
        public int Edition { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Range(0, AgeRestrictionMaxValue)]
        public int AgeRestriction { get; set; }

        [Range(0, int.MaxValue)]
        public int AuthorId { get; set; }

    }
}