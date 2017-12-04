namespace CameraBazaar.Web.Infrastructure.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class ImageTagHelper : TagHelper
    {
        // this is just for example

        public string Source { get; set; }

        public string AlternativeText { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.SetAttribute("src", this.Source);

            if (!string.IsNullOrWhiteSpace(this.AlternativeText))
            {
                output.Attributes.SetAttribute("alt", this.AlternativeText);
            }
        }
    }
}