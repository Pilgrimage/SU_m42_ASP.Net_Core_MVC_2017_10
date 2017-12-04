namespace CameraBazaar.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class HtmlHelperExtensions
    {
        // this is just for example
        public static IHtmlContent Image(this IHtmlHelper helper, string url, string alt = "")
        {
            return new HtmlString($"<img src='{url}' alt='{alt}' />");
        }
    }
}