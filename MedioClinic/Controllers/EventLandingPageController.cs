using System;
using System.Web.Mvc;
using System.Web.UI;

using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using Business.DependencyInjection;
using Business.Dto.LandingPage;
using Business.Repository.LandingPage;

namespace MedioClinic.Controllers
{
    public class EventLandingPageController : BaseController
    {
        protected ILandingPageRepository LandingPageRepository { get; }

        public EventLandingPageController(
            IBusinessDependencies dependencies, ILandingPageRepository landingPageRepository) : base(dependencies)
        {
            LandingPageRepository = landingPageRepository ?? throw new ArgumentNullException(nameof(landingPageRepository));
        }

        // GET: EventLandingPage/[nodeAlias]
        //[OutputCache(Duration = 3600, VaryByParam = "nodeAlias", Location = OutputCacheLocation.Server)]
        public ActionResult Index(string nodeAlias)
        {
            var landingPageDto = LandingPageRepository
                .GetLandingPage(nodeAlias, EventLandingPageDto.QueryModifier, EventLandingPageDto.Selector);

            if (landingPageDto == null)
            {
                return HttpNotFound();
            }

            Dependencies.CacheService.SetOutputCacheDependency(nodeAlias);

            return new TemplateResult(landingPageDto.DocumentId);
        }
    }
}