using System;
using System.Collections.Generic;
using System.Linq;

using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace MedioClinic.Utils
{
    public class LandingPageTemplateFilter : IPageTemplateFilter
    {
        private const string CodenameBase = "MedioClinic.Template";
        private const string LandingPageTypeCodename = CMS.DocumentEngine.Types.MedioClinic.LandingPage.CLASS_NAME;
        private const string EventLandingPageTypeCodename = CMS.DocumentEngine.Types.MedioClinic.EventLandingPage.CLASS_NAME;

        private string BasicTemplateCodename => $"{CodenameBase}.BasicTemplate";

        private string EventTemplateCodename => $"{CodenameBase}.EventTemplate";

        protected IEnumerable<string> LandingPageTemplates => new string[]
        {
            BasicTemplateCodename,
            EventTemplateCodename
        };

        protected IEnumerable<string> LandingPageTypeCodenames => new string[]
        {
            LandingPageTypeCodename,
            EventLandingPageTypeCodename
        };

        public IEnumerable<PageTemplateDefinition> Filter(IEnumerable<PageTemplateDefinition> pageTemplates, PageTemplateFilterContext context)
        {
            if (LandingPageTypeCodenames.Any(codename => context.PageType.Equals(codename, StringComparison.OrdinalIgnoreCase)))
            {
                return context.PageType.Equals(EventLandingPageTypeCodename, StringComparison.OrdinalIgnoreCase)
                    ? pageTemplates.Where(template => LandingPageTemplates.Contains(template.Identifier, StringComparer.OrdinalIgnoreCase))
                    : pageTemplates.Where(template => template.Identifier.Equals(BasicTemplateCodename, StringComparison.OrdinalIgnoreCase));
            }

            return pageTemplates.Where(template => !LandingPageTemplates.Contains(template.Identifier));
        }
    }
}