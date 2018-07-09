using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.Reporting.Web.UI;
using CMS.WebAnalytics;
using CMS.WebAnalytics.Web.UI;


public partial class CMSModules_Content_CMSDesk_OnlineMarketing_Reports : CMSAnalyticsContentPage
{
    private IDisplayReport mUcDisplayReport;


    protected override void OnPreRender(EventArgs e)
    {
        var ui = MembershipContext.AuthenticatedUser;
        if (!ui.IsAuthorizedPerUIElement("CMS.Content", "Reports"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Reports");
        }

        // Check read for web analytics
        if (!ui.IsAuthorizedPerResource("cms.webanalytics", "Read"))
        {
            RedirectToAccessDenied(String.Format(GetString("general.permissionresource"), "Read", "Web analytics"));
        }

        // Set disabled module info
        ucDisabledModule.ParentPanel = pnlDisabled;

        mUcDisplayReport = (IDisplayReport)LoadUserControl("~/CMSModules/Reporting/Controls/DisplayReport.ascx");
        pnlContent.Controls.Add((Control)mUcDisplayReport);

        ucGraphType.ProcessChartSelectors(false);
        CurrentMaster.PanelContent.CssClass = String.Empty;
        UIHelper.AllowUpdateProgress = false;

        // General report data
        if (IsFilePageType(Node.NodeClassName))
        {
            reportTypePnl.Visible = false;
            mUcDisplayReport.ReportName = "pagereports.file";
        }
        else
        {
            mUcDisplayReport.ReportName = rbContent.Checked ? "pagereports.content" : "pagereports.Traffic";
        }
        
        mUcDisplayReport.LoadFormParameters = false;
        mUcDisplayReport.DisplayFilter = false;
        mUcDisplayReport.GraphImageWidth = 100;
        mUcDisplayReport.IgnoreWasInit = true;
        mUcDisplayReport.TableFirstColumnWidth = Unit.Percentage(30);
        mUcDisplayReport.UseExternalReload = true;
        mUcDisplayReport.UseProgressIndicator = true;

        mUcDisplayReport.SetDefaultDynamicMacros((int)ucGraphType.SelectedInterval);
        
        EditedObject = Node;

        // Resolve report macros 
        DataTable dtp = new DataTable();
        dtp.Columns.Add("FromDate", typeof(DateTime));
        dtp.Columns.Add("ToDate", typeof(DateTime));
        dtp.Columns.Add("CodeName", typeof(string));
        dtp.Columns.Add("NodeID", typeof(int));
        dtp.Columns.Add("CultureCode", typeof(string));

        object[] parameters = new object[5];
        parameters[0] = ucGraphType.From;
        parameters[1] = ucGraphType.To;
        parameters[2] = "pageviews";
        parameters[3] = Node.NodeID;
        parameters[4] = Node.DocumentCulture;

        dtp.Rows.Add(parameters);
        dtp.AcceptChanges();
        mUcDisplayReport.ReportParameters = dtp.Rows[0];
        mUcDisplayReport.SelectedInterval = HitsIntervalEnumFunctions.HitsConversionToString(ucGraphType.SelectedInterval);

        mUcDisplayReport.ReloadData(true);

		DocumentManager.RegisterSaveChangesScript = false;

        base.OnPreRender(e);
    }


    private static bool IsFilePageType(string documentTypeName)
    {
        return documentTypeName.Equals(SystemDocumentTypes.File, StringComparison.OrdinalIgnoreCase);
    }
}
