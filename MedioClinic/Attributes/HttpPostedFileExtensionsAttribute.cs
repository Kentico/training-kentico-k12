using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

using CMS.Helpers;

namespace MedioClinic.Attributes
{
    /// <summary>
    /// Inspired by https://stackoverflow.com/questions/8536589/asp-net-mvc-3-dataannotations-fileextensionsattribute-not-working
    /// </summary>
    public class HttpPostedFileExtensionsAttribute : DataTypeAttribute, IClientValidatable
    {
        private readonly FileExtensionsAttribute _innerAttribute =
            new FileExtensionsAttribute();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostedFileExtensionsAttribute" /> class.
        /// </summary>
        public HttpPostedFileExtensionsAttribute()
            : base(DataType.Upload)
        {
        }

        /// <summary>
        /// Gets or sets the file name extensions.
        /// </summary>
        /// <returns>
        /// The file name extensions, or the default file extensions (".png", ".jpg", ".jpeg", and ".gif") if the property is not set.
        /// </returns>
        public string Extensions
        {
            get => _innerAttribute.Extensions;
            set => _innerAttribute.Extensions = value;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "extension",
            };

            rule.ValidationParameters["extension"] = _innerAttribute.Extensions;

            yield return rule;
        }

        /// <summary>
        /// Checks that the specified file name extension or extensions is valid.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the file name extension is valid; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="value">A comma delimited list of valid file extensions.</param>
        public override bool IsValid(object value)
        {
            // Included in this class just for sake of simplicity.
            // To meet requirements of the single responsibility principle,
            // consider localizing through a System.Web.Mvc.DataAnnotationsModelValidator<TAttribute> child.
            ErrorMessage = ResHelper.GetStringFormat(ErrorMessage, Extensions);

            var file = value as HttpPostedFileBase;

            if (file != null)
            {
                return _innerAttribute.IsValid(file.FileName);
            }

            return _innerAttribute.IsValid(value);
        }
    }
}