using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterSection(
    "MedioClinic.Section.SingleColumn", 
    "{$Section.SingleColumn.Name$}", 
    customViewName: "Sections/_SingleColumnSection", 
    Description = "{$Section.SingleColumn.Description$}", 
    IconClass = "icon-square")]

[assembly: RegisterSection(
    "MedioClinic.Section.TwoColumn", 
    "{$Section.TwoColumn.Name$}", 
    propertiesType: typeof(MedioClinic.Models.Sections.TwoColumnSectionProperties),
    customViewName: "Sections/_TwoColumnSection",
    Description = "{$Section.TwoColumn.Description$}", 
    IconClass = "icon-l-cols-2")]

[assembly: RegisterWidget(
    "MedioClinic.Widget.Text",
    "{$Widget.Text.Name$}",
    propertiesType: typeof(MedioClinic.Models.Widgets.TextWidgetProperties),
    customViewName: "Widgets/_TextWidget",
    Description = "{$Widget.Text.Description$}",
    IconClass = "icon-l-text")]



