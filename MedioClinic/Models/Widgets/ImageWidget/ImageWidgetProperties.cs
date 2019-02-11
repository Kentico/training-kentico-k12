using System;

using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Widgets
{
    public sealed class ImageWidgetProperties : IWidgetProperties
    {
        public Guid ImageGuid { get; set; }
    }
}