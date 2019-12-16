using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using MedioClinic.Models.FormComponents;

namespace MedioClinic.Models.PageTemplates
{
    public class EventTemplateProperties : IPageTemplateProperties, IViewModel
    {
        [EditingComponent(AirportSelectionComponent.Identifier, 
            Label = "{$PageTemplate.EventTemplate.LocationAirport$}",
            Order = 0)]
        public string EventLocationAirport { get; set; }
    }
}