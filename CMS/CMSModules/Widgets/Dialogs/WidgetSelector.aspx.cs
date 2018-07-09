using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Widgets_Dialogs_WidgetSelector : CMSModalPage, ICallbackEventHandler
{
    #region "Variables"

    private string callbackResult = string.Empty;

    #endregion


    #region "Page methods"

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Public user is not allowed for widgets
        if (!AuthenticationHelper.IsAuthenticated())
        {
            RedirectToAccessDenied(GetString("widgets.security.notallowed"));
        }

        string aliasPath = QueryHelper.GetString("aliaspath", "");
        selectElem.AliasPath = aliasPath;
        selectElem.CultureCode = QueryHelper.GetString("culture", LocalizationContext.PreferredCultureCode);
        selectElem.PageTemplateId = QueryHelper.GetInteger("templateid", 0);
        selectElem.ZoneId = QueryHelper.GetString("zoneid", "");
        selectElem.ZoneType = QueryHelper.GetString("zonetype", "").ToEnum<WidgetZoneTypeEnum>();

        // Ensure the design mode for the dialog
        if (String.IsNullOrEmpty(aliasPath))
        {
            PortalContext.SetRequestViewMode(ViewModeEnum.Design);
        }

        bool isInline = QueryHelper.GetBoolean("inline", false);
        selectElem.IsInline = isInline;

        ScriptHelper.RegisterWOpenerScript(Page);

        btnOk.OnClientClick = "SelectCurrentWidget(); return false;";

        // Base tag is added in master page
        AddBaseTag = false;

        // Proceeds the current item selection
        StringBuilder script = new StringBuilder();
        script.Append(@"
function SelectCurrentWidget() 
{                
    SelectWidget(selectedValue, selectedSkipDialog);
}
function SelectWidget(value, skipDialog)
{
    if ((value != null) && (value != ''))
    {");

        if (isInline)
        {
            script.Append(@"
        if (skipDialog) {
            AddInlineWidgetWithoutDialog(value);
        }
        else {
            var editor = wopener.currentEditor || wopener.CMSPlugin.currentEditor;
            if (editor) {
                editor.getCommand('InsertWidget').open(value);
            }

            CloseDialog(false);
        }");
        }
        else
        {
            script.Append(@"
	    if (wopener.OnSelectWidget)
        {                    
            wopener.OnSelectWidget(value, skipDialog);                      
        }

        CloseDialog();");
        }

        script.Append(@"  
	}
	else
	{
        alert(document.getElementById('", hdnMessage.ClientID, @"').value);		    
	}                
}            
// Cancel action
function Cancel()
{
    CloseDialog(false);
} ");

        ScriptHelper.RegisterStartupScript(this, typeof(string), "WidgetSelector", script.ToString(), true);
        selectElem.SelectFunction = "SelectWidget";
        selectElem.IsLiveSite = false;

        // Set the title and icon
        PageTitle.TitleText = GetString("widgets.selectortitle");
        CurrentMaster.PanelContent.RemoveCssClass("dialog-content");

        // Remove default css class
        if (CurrentMaster.PanelBody != null)
        {
            Panel pnl = CurrentMaster.PanelBody.FindControl("pnlContent") as Panel;
            if (pnl != null)
            {
                pnl.CssClass = String.Empty;
            }
        }

        // Register scripts for inline widgets with the property 'Skip initial configuration' set (insert widget without configuration dialog)
        string skipInitialConfigurationScript = @"
// Inline widgets
function AddInlineWidgetWithoutDialog(widgetId) {
    " + Page.ClientScript.GetCallbackEventReference(this, "widgetId", "OnReceiveAddInlineWidgetScript", null) + @";
    }

function OnReceiveAddInlineWidgetScript(rvalue, context) {
    if ((rvalue != null) && (rvalue != '')) {
        setTimeout(rvalue, 1);
    }
}";

        ScriptHelper.RegisterStartupScript(this, typeof(string), "inlinewidgetsscript", skipInitialConfigurationScript, true);
    }

    #endregion


    #region "Callback handling"

    /// <summary>
    /// Raises the callback event.
    /// </summary>
    public void RaiseCallbackEvent(string eventArgument)
    {
        if ((eventArgument == null))
        {
            return;
        }

        int widgetId = ValidationHelper.GetInteger(eventArgument, 0);
        if (widgetId > 0)
        {
            // Get the insert widget script
            callbackResult = AddInlineWidgetWithoutDialog(widgetId);
        }
    }


    /// <summary>
    /// Prepares the callback result.
    /// </summary>
    public string GetCallbackResult()
    {
        return callbackResult;
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Adds the inline widget without the properties dialog.
    /// </summary>
    /// <param name="widgetId">The widget id</param>
    private string AddInlineWidgetWithoutDialog(int widgetId)
    {
        string script = string.Empty;

        if (widgetId > 0)
        {
            // New widget - load widget info by id
            WidgetInfo wi = WidgetInfoProvider.GetWidgetInfo(widgetId);

            if ((wi != null) && wi.WidgetForInline)
            {
                // Test permission for user
                var currentUser = MembershipContext.AuthenticatedUser;
                if (!WidgetRoleInfoProvider.IsWidgetAllowed(widgetId, currentUser.UserID, AuthenticationHelper.IsAuthenticated()))
                {
                    return string.Empty;
                }

                // If user is editor, more properties are shown
                WidgetZoneTypeEnum zoneType = WidgetZoneTypeEnum.User;
                if (currentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Editor, SiteContext.CurrentSiteName))
                {
                    zoneType = WidgetZoneTypeEnum.Editor;
                }

                WebPartInfo wpi = WebPartInfoProvider.GetWebPartInfo(wi.WidgetWebPartID);

                // Merge the parent web part properties definition with the widget properties definition
                string widgetProperties = FormHelper.MergeFormDefinitions(wpi.WebPartProperties, wi.WidgetProperties);

                // Create the FormInfo for the current widget properties definition
                FormInfo fi = PortalFormHelper.GetWidgetFormInfo(wi.WidgetName, zoneType, widgetProperties, true);
                DataRow dr = null;

                if (fi != null)
                {
                    // Get data rows with required columns
                    dr = PortalHelper.CombineWithDefaultValues(fi, wi);

                    // Load default values for new widget
                    fi.LoadDefaultValues(dr, FormResolveTypeEnum.Visible);

                    // Override default value and set title as widget display name
                    DataHelper.SetDataRowValue(dr, "WidgetTitle", wi.WidgetDisplayName);
                }

                // Save inline widget script
                script = PortalHelper.GetAddInlineWidgetScript(wi, dr, fi.GetFields(true, true), Enumerable.Empty<string>());

                script += " CloseDialog(false);";

                if (!string.IsNullOrEmpty(script))
                {
                    // Add to recently used widgets collection
                    MembershipContext.AuthenticatedUser.UserSettings.UpdateRecentlyUsedWidget(wi.WidgetName);
                }
            }
        }

        return script;
    }

    #endregion
}
