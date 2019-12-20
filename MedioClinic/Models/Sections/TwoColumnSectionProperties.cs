using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace MedioClinic.Models.Sections
{
    public class TwoColumnSectionProperties : ISectionProperties
    {
        [EditingComponent(
            IntInputComponent.IDENTIFIER, 
            DefaultValue = 6, 
            Label = "{$Section.TwoColumn.Label$}", 
            ExplanationText = "{$Section.TwoColumn.ExplanationText$}",
            Order = 0)]
        public int LeftColumnWidth { get; set; }
    }
}