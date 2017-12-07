namespace BookShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MinLength(AuthorFirstNameMinLength)]
        [MaxLength(AuthorFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(AuthorLastNameMinLength)]
        [MaxLength(AuthorLastNameMaxLength)]
        public string LastName { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}