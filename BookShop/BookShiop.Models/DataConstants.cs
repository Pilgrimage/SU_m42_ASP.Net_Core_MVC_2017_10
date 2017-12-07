namespace BookShop.Models
{
    public static class DataConstants
    {
        public const int BookTitleMinLength = 1;
        public const int BookTitleMaxLength = 100;

        public const int BookDescriptionMaxLength = 2000;
        public const int BookMaxPrice = 999999999;
        public const int CopiesMaxValue = 100000;
        public const int EditionMaxValue = 1000;
        public const int AgeRestrictionMaxValue = 21;
        
        public const int AuthorFirstNameMinLength = 1;
        public const int AuthorFirstNameMaxLength = 50;
        public const int AuthorLastNameMinLength = 1;
        public const int AuthorLastNameMaxLength = 50;

        public const int CategoryNameMaxLength = 50;
    }
}