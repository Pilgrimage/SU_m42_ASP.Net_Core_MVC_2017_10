namespace BookShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MinLength(BookTitleMinLength)]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; }

        [Range(0,BookMaxPrice)]
        public decimal Price { get; set; }

        [Range(0,CopiesMaxValue)]
        public int Copies { get; set; }

        [Range(0, EditionMaxValue)]
        public int? Edition { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Range(0, AgeRestrictionMaxValue)]
        public int? AgeRestriction { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public List<BookCategory> Categories { get; set; } = new List<BookCategory>();
    }
}