using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Widgets
{
    public class SlideshowWidgetProperties : IWidgetProperties
    {
        public string[] ImageIds { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$Widget.Slideshow.Width$}", Order = 1)]
        public int Width { get; set; } = 800;

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$Widget.Slideshow.Height$}", Order = 2)]
        public int Height { get; set; } = 600;

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Label = "{$Widget.Slideshow.EnforceDimensions$}", Order = 3)]
        public bool EnforceDimensions { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$Widget.Slideshow.TransitionDelay$}", Order = 4)]
        public int TransitionDelay { get; set; } = 5000;

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "{$Widget.Slideshow.TransitionSpeed$}", Order = 5)]
        public int TransitionSpeed { get; set; } = 300;

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Label = "{$Widget.Slideshow.DisplayArrowSigns$}", Order = 6)]
        public bool DisplayArrowSigns { get; set; } = true;
    }
}