using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Business.DependencyInjection;
using Business.Repository.LandingPage;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic.Controllers
{
    public class LandingPageController : BaseController
    {
        private ILandingPageRepository LandingPageRepository { get; }

        public LandingPageController(
            IBusinessDependencies dependencies,
            ILandingPageRepository landingPageRepository
            ) : base(dependencies)
        {
            LandingPageRepository = landingPageRepository;
        }

        // GET: LandingPage
        public ActionResult Index(string pageAlias)
        {
            var landingPageDto = LandingPageRepository.GetLandingPage(pageAlias);

            if (landingPageDto == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(landingPageDto.Title);

            HttpContext.Kentico().PageBuilder().Initialize(landingPageDto.DocumentId);

            return View(model);
        }
    }
}