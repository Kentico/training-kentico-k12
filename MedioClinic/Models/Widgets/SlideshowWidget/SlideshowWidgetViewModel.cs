using System.Collections.Generic;

using CMS.DocumentEngine;

namespace MedioClinic.Models.Widgets
{
    public class SlideshowWidgetViewModel
    {
        public IEnumerable<DocumentAttachment> Images { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool EnforceDimensions { get; set; }
        public int TransitionDelay { get; set; }
        public int TransitionSpeed { get; set; }
        public bool DisplayArrowSigns { get; set; }
    }
}