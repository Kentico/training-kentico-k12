using System;
using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using Business.DependencyInjection;
using Business.Dto.LandingPage;
using Business.Repository.LandingPage;
using MedioClinic.Controllers.PageTemplates;
using MedioClinic.Models;
using MedioClinic.Models.PageTemplates;

[assembly: RegisterPageTemplate(
    "MedioClinic.Template.EventTemplate",
    typeof(EventTemplateController),
    "{$PageTemplate.EventTemplate.Name$}",
    Description = "{$PageTemplate.EventTemplate.Description$}",
    IconClass = "icon-app-events")]

namespace MedioClinic.Controllers.PageTemplates
{
    public class EventTemplateController : PageTemplateController<EventTemplateProperties>
    {
        protected IBusinessDependencies Dependencies { get; }

        protected ILandingPageRepository LandingPageRepository { get; }

        public EventTemplateController(IBusinessDependencies dependencies, ILandingPageRepository landingPageRepository)
        {
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            LandingPageRepository = landingPageRepository ?? throw new ArgumentNullException(nameof(landingPageRepository));
        }

        public ActionResult Index()
        {
            var page = GetPage();
            var properties = GetProperties();

            var eventLandingPageDto = !string.IsNullOrEmpty(page?.NodeAlias)
                ? LandingPageRepository.GetLandingPage(page.NodeAlias, EventLandingPageDto.QueryModifier, EventLandingPageDto.Selector)
                : null;

            if (properties != null && eventLandingPageDto != null)
            {
                var subViewModel = new EventTemplateViewModel
                {
                    EventLandingPageDto = eventLandingPageDto,
                    EventTemplateProperties = properties
                };

                var model = PageViewModel<EventTemplateViewModel>.GetPageViewModel(subViewModel, eventLandingPageDto.Title, Dependencies);

                return View("PageTemplates/_EventTemplate", model);
            }

            return HttpNotFound();
        }
    }
}