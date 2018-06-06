using System;
using System.Text;
using System.Web.Caching;
using System.Web.UI;

using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_AdminControls_Controls_Class_FormBuilder_FormComponents : CMSUserControl
{
    private const string CACHE_KEY = "FormBuilderComponents";


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        FillPanelWithComponents();
    }


    private void FillPanelWithComponents()
    {
        string value;

        // Try to retrieve data from the cache
        if (!CacheHelper.TryGetItem(CACHE_KEY, out value))
        {
            var controls = FormUserControlInfoProvider.GetFormUserControls()
                                                      .Columns("UserControlDisplayName,UserControlCodeName,UserControlDescription,UserControlThumbnailGUID")
                                                      .WhereEquals("UserControlShowInBizForms", 1).WhereEquals("UserControlPriority", 100).OrderBy("UserControlDisplayName");

            StringBuilder content = new StringBuilder();

            foreach (var control in controls)
            {
                string iconUrl;
                if (control.UserControlThumbnailGUID == Guid.Empty)
                {
                    iconUrl = GetImageUrl("CMSModules/CMS_FormEngine/custom.png");
                }
                else
                {
                    iconUrl = ResolveUrl(MetaFileURLProvider.GetMetaFileUrl(control.UserControlThumbnailGUID, "icon"));
                }

                content.AppendFormat("<div title=\"{0}\"><div class=\"form-component component_{1}\" ondblclick=\"FormBuilder.addNewField('{1}','',-1);FormBuilder.scrollPosition=9999;FormBuilder.showSavingInfo();\"><span class=\"component-label\">{2}</span><img src=\"{3}\" alt=\"{1}\" /></div></div>",
                    ResHelper.LocalizeString(control.UserControlDescription), control.UserControlCodeName, HTMLHelper.HTMLEncode(control.UserControlDisplayName), iconUrl);
            }

            value = content.ToString();

            var dependency = CacheHelper.GetCacheDependency(FormUserControlInfo.OBJECT_TYPE + "|all");

            CacheHelper.Add(CACHE_KEY, value, dependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
        }

        pnlFormComponents.Controls.Add(new LiteralControl(value));
    }
}