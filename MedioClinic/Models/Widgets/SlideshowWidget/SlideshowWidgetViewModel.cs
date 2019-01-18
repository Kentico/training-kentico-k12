using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CMS.DocumentEngine;

namespace MedioClinic.Models.Widgets
{
    public class SlideshowWidgetViewModel
    {
        public IEnumerable<DocumentAttachment> Images { get; set; }
    }
}