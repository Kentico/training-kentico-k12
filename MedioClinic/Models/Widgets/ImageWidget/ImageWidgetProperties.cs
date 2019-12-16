using System;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Widgets
{
    public class ImageWidgetProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "{$Widget.MediaLibraryName$}", Order = 0)]
        public string MediaLibraryName { get; set; }

        public Guid ImageGuid { get; set; }
    }
}