using System;
using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using Business.DependencyInjection;
using MedioClinic.Controllers.PageTemplates;
using MedioClinic.Models;
using MedioClinic.Models.PageTemplates;

[assembly: RegisterPageTemplate(
    "MedioClinic.Template.BasicTemplate",
    typeof(BasicTemplateController),
    "{$PageTemplate.BasicTemplate.Name$}",
    Description = "{$PageTemplate.BasicTemplate.Description$}",
    IconClass = "icon-app-page-templates")]

namespace MedioClinic.Controllers.PageTemplates
{
    public class BasicTemplateController : PageTemplateController<BasicTemplateProperties>
    {
        IBusinessDependencies Dependencies { get; }

        public BasicTemplateController(IBusinessDependencies dependencies)
        {
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        public ActionResult Index()
        {
            var page = GetPage();
            var properties = GetProperties();

            if (page != null && properties != null)
            {
                var model = PageViewModel<BasicTemplateProperties>.GetPageViewModel(properties, page.DocumentPageTitle, Dependencies);

                return View("PageTemplates/_BasicTemplate", model);
            }

            return HttpNotFound();
        }
    }
}