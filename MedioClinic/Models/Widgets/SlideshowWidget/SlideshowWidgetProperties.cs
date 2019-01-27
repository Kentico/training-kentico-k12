using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Widgets
{
    public class SlideshowWidgetProperties : IWidgetProperties
    {
        public string[] ImageIds { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "{$MedioClinic.Widget.Slideshow.Width$}", Order = 1)]
        public string Width { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "{$MedioClinic.Widget.Slideshow.Height$}", Order = 2)]
        public string Height { get; set; }
    }
}