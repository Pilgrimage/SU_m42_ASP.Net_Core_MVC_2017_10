namespace BookShop.Api.Models.Autors
{
    using System.ComponentModel.DataAnnotations;
    using static BookShop.Models.DataConstants;

    public class PostAuthorRequestModel
    {
        [Required]
        [MinLength(AuthorFirstNameMinLength)]
        [MaxLength(AuthorFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(AuthorLastNameMinLength)]
        [MaxLength(AuthorLastNameMaxLength)]
        public string LastName { get; set; }
    }
}