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

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$MedioClinic.Widget.Slideshow.Width$}", Order = 1)]
        public int Width { get; set; } = 400;

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$MedioClinic.Widget.Slideshow.Height$}", Order = 2)]
        public int Height { get; set; } = 300;

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$MedioClinic.Widget.Slideshow.TransitionDelay$}", Order = 3)]
        public int TransitionDelay { get; set; } = 5000;

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$MedioClinic.Widget.Slideshow.TransitionSpeed$}", Order = 4)]
        public int TransitionSpeed { get; set; } = 300;
    }
}