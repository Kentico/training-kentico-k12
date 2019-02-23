using System;
using System.Web.Mvc;
using System.Web.UI;

using Business.DependencyInjection;
using Business.Repository.LandingPage;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

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
        [OutputCache(Duration = 3600, VaryByParam = "nodeAlias", Location = OutputCacheLocation.Server)]
        public ActionResult Index(string nodeAlias)
        {
            var landingPageDto = LandingPageRepository.GetLandingPage(nodeAlias);

            if (landingPageDto == null)
            {
                return HttpNotFound();
            }

            Dependencies.CacheService.SetOutputCacheDependency(nodeAlias);
            var model = GetPageViewModel(landingPageDto.Title);
            HttpContext.Kentico().PageBuilder().Initialize(landingPageDto.DocumentId);

            return View(model);
        }
    }
}