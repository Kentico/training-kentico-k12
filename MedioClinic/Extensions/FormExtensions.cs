using System.Collections.Generic;
using System.Web.Mvc;

namespace MedioClinic.Extensions
{
    public static class FormExtensions
    {
        public static MvcHtmlString CustomInput(this HtmlHelper helper, string inputType, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("type", inputType);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("value", value.ToString());
            tagBuilder.MergeAttributes(htmlAttributes);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.StartTag));
        }

        public static MvcHtmlString CustomInput(this HtmlHelper helper, string inputType, string name, object value, object htmlAttributes) =>
            CustomInput(helper, inputType, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, object htmlAttributes) =>
            Button(helper, innerHtml, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }
}