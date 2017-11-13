namespace CarDealer.Web.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToPrice(this decimal price)
        {
            return $"${price.ToString("F2")}";
        }


        public static string ToPercentage(this double koef)
        {
            return $"{(koef*100).ToString("F0")} %";
        }
    }
}