using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Kentico.Forms.Web.Mvc;

using Business.Repository.Airport;
using MedioClinic.Models.FormComponents;

[assembly: RegisterFormComponent(
    AirportSelectionComponent.Identifier,
    typeof(AirportSelectionComponent),
    "{$FormComponent.AirportSelection.Name$}",
    ViewName = "FormComponents/_AirportSelection",
    Description = "{$FormComponent.AirportSelection.Description$}",
    IconClass = "icon-menu")]

namespace MedioClinic.Models.FormComponents
{
    public class AirportSelectionComponent : SelectorFormComponent<AirportSelectionProperties>
    {
        public const string Identifier = "MedioClinic.FormComponent.AirportSelection";
        protected const string CustomTableClassName = "customtable.Airports";
        
        IAirportRepository AirportRepository { get; }

        public AirportSelectionComponent()
        {
            AirportRepository = new AirportRepository();
        }

        protected override IEnumerable<SelectListItem> GetItems() =>
            AirportRepository
                .GetAirportDtos()
                .Select(dto => new SelectListItem
                {
                    Text = dto.AirportName,
                    Value = dto.AirportIataCode
                });
    }
}