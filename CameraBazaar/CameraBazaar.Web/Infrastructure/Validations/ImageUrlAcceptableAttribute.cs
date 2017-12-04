namespace CameraBazaar.Web.Infrastructure.Validations
{
    using System.ComponentModel.DataAnnotations;

    public class ImageUrlAcceptableAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string imageUrl = value as string;

            if (imageUrl != null)
            {
                if (!imageUrl.StartsWith("http://") && !imageUrl.StartsWith("https://"))
                {
                    return false;
                    //ErrorMessage="The Image URL must start with 'http://' or 'https://'.";
                }

            }

            return true;
        }

    }
}