namespace BookShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public List<BookCategory> Books { get; set; } = new List<BookCategory>();
    }
}