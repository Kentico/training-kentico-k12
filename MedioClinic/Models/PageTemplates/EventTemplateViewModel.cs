using Business.Dto.LandingPage;

namespace MedioClinic.Models.PageTemplates
{
    public class EventTemplateViewModel : IViewModel
    {
        public EventTemplateProperties EventTemplateProperties { get; set; }

        public EventLandingPageDto EventLandingPageDto { get; set; }
    }
}