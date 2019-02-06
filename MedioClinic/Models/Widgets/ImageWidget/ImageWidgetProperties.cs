using System;

using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Widgets
{
    /// <summary>
    /// Properties of Image widget.
    /// </summary>
    public sealed class ImageWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Guid of an image to be displayed.
        /// </summary>
        public Guid ImageGuid { get; set; }
    }
}