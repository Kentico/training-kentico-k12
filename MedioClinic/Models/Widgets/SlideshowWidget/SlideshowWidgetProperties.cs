using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Widgets
{
    public class SlideshowWidgetProperties : IWidgetProperties
    {
        public string[] ImageIds { get; set; }
    }
}