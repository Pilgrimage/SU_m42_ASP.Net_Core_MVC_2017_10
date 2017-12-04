namespace CameraBazaar.Web.Infrastructure.Validations
{
    using System.ComponentModel.DataAnnotations;

    public class MaxIsoAcceptableAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int? maxIso = value as int?;

            if (maxIso != null)
            {
                int maxIsoInt = (int)maxIso;
                if (maxIsoInt != ((maxIsoInt / 100) * 100))
                {
                    return false;
                    //ErrorMessage="The Max ISO must be dividable by 100.";
                }

            }

            return true;
        }
    }
}
