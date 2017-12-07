namespace BookShop.Api.Models.Books
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static BookShop.Models.DataConstants;

    public class CreateBookRequestModel
    {
        [Required]
        [MinLength(BookTitleMinLength)]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; }

        [Range(0, BookMaxPrice)]
        public decimal Price { get; set; }

        [Range(0, CopiesMaxValue)]
        public int Copies { get; set; }

        public int Edition { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public int AgeRestriction { get; set; }

        [Required]
        [MinLength(AuthorFirstNameMinLength)]
        [MaxLength(AuthorFirstNameMaxLength)]
        public string AuthorFirstName { get; set; }

        [Required]
        [MinLength(AuthorLastNameMinLength)]
        [MaxLength(AuthorLastNameMaxLength)]
        public string AuthorLastName { get; set; }

        public string Categories { get; set; }
    }
}