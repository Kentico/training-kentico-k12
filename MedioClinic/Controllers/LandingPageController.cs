using System;
using System.Web.Mvc;
using System.Web.UI;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Kentico.Web.Mvc;

using Business.DependencyInjection;
using Business.Dto.LandingPage;
using Business.Repository.LandingPage;

namespace MedioClinic.Controllers
{
    public class LandingPageController : BaseController
    {
        protected ILandingPageRepository LandingPageRepository { get; }

        public LandingPageController(
            IBusinessDependencies dependencies, ILandingPageRepository landingPageRepository) : base(dependencies)
        {
            LandingPageRepository = landingPageRepository ?? throw new ArgumentNullException(nameof(landingPageRepository));
        }

        // GET: LandingPage/[nodeAlias]
        //[OutputCache(Duration = 3600, VaryByParam = "nodeAlias", Location = OutputCacheLocation.Server)]
        public ActionResult Index(string nodeAlias)
        {
            var landingPageDto = LandingPageRepository
                .GetLandingPage<CMS.DocumentEngine.Types.MedioClinic.LandingPage, LandingPageDto>(nodeAlias);

            if (landingPageDto == null)
            {
                return HttpNotFound();
            }

            Dependencies.CacheService.SetOutputCacheDependency(nodeAlias);

            // Implementation without page templates (begin)
            /*var model = GetPageViewModel(landingPageDto.Title);
            HttpContext.Kentico().PageBuilder().Initialize(landingPageDto.DocumentId);

            return View(model);*/
            // Implementation without page templates (end)

            // Page template implementation (begin)
            return new TemplateResult(landingPageDto.DocumentId);
            // Page template implementation (end)
        }
    }
}