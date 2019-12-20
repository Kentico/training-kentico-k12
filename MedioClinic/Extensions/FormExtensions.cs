using System.Collections.Generic;
using System.Web.Mvc;

namespace MedioClinic.Extensions
{
    public static class FormExtensions
    {
        /// <summary>
        /// Renders a custom input HTML element.
        /// </summary>
        /// <param name="helper">The HTML helper to operate upon.</param>
        /// <param name="inputType">Type of the input element.</param>
        /// <param name="name">Name of the input element.</param>
        /// <param name="value">Value of the input element.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>The input element.</returns>
        public static MvcHtmlString CustomInput(this HtmlHelper helper, string inputType, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("type", inputType);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("value", value.ToString());
            tagBuilder.MergeAttributes(htmlAttributes);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.StartTag));
        }

        /// <summary>
        /// Renders a custom input HTML element.
        /// </summary>
        /// <param name="helper">The HTML helper to operate upon.</param>
        /// <param name="inputType">Type of the input element.</param>
        /// <param name="name">Name of the input element.</param>
        /// <param name="value">Value of the input element.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>The input element.</returns>
        public static MvcHtmlString CustomInput(this HtmlHelper helper, string inputType, string name, object value, object htmlAttributes) =>
            CustomInput(helper, inputType, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

        /// <summary>
        /// Renders a button HTML element.
        /// </summary>
        /// <param name="helper">The HTML helper ot operate upon.</param>
        /// <param name="innerHtml">The inner markup of the button.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>The button element.</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);

            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// Renders a button HTML element.
        /// </summary>
        /// <param name="helper">The HTML helper ot operate upon.</param>
        /// <param name="innerHtml">The inner markup of the button.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>The button element.</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, object htmlAttributes) =>
            Button(helper, innerHtml, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }
}