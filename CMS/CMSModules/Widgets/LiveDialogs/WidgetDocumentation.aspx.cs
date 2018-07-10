using System;
using System.Web.UI.WebControls;

using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Widgets_LiveDialogs_WidgetDocumentation : CMSWidgetPropertiesLiveModalPage
{
    /// <summary>
    /// Handles the Init event of the Page control.
    /// </summary>
    protected void Page_Init(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("WebPartDocumentDialog.Documentation");
        ucWebPartDocumentation.PageTemplateID = templateId;
        ucWebPartDocumentation.AliasPath = aliasPath;
        ucWebPartDocumentation.CultureCode = culture;
        ucWebPartDocumentation.IsInline = inline;
        ucWebPartDocumentation.ZoneID = zoneId;
        ucWebPartDocumentation.WidgetID = QueryHelper.GetInteger("widgetId", 0);
        ucWebPartDocumentation.InstanceGUID = instanceGuid;
        ucWebPartDocumentation.WebpartID = QueryHelper.GetString("webPartId", String.Empty);
        ucWebPartDocumentation.DashboardName = QueryHelper.GetString("dashboard", String.Empty);
        ucWebPartDocumentation.DashboardSiteName = QueryHelper.GetString("sitename", String.Empty);

        if (CurrentMaster.PanelBody != null)
        {
            Panel pnl = CurrentMaster.PanelContent;
            if (pnl != null)
            {
                pnl.CssClass = String.Empty;
            }
        }
    }
}