namespace BCKFreightTMS.Web.Helpers
{
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class InvariantDecimalTagHelper : InputTagHelper
    {
        private const string ForAttributeName = "asp-for";

        private IHtmlGenerator generator;

        [HtmlAttributeName("asp-is-invariant")]
        public bool IsInvariant { get; set; }

        public InvariantDecimalTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
            this.generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (this.IsInvariant && output.TagName == "input" && this.For.Model != null && this.For.Model.GetType() == typeof(decimal))
            {
                decimal value = (decimal)this.For.Model;
                var invariantValue = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                output.Attributes.SetAttribute(new TagHelperAttribute("value", invariantValue));
            }
        }
    }
}
