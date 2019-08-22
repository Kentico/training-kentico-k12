using System;

using CMS.Helpers;

using System.Text;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;
using CMS.PortalEngine.Web.UI.Internal;


public partial class CMSModules_Widgets_LiveDialogs_WidgetSelector : CMSLiveModalPage
{
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

        var aliasPath = QueryHelper.GetString("aliaspath", String.Empty);
        var zoneId = QueryHelper.GetString("zoneid", String.Empty);
        var zoneType = QueryHelper.GetString("zonetype", "").ToEnum<WidgetZoneTypeEnum>();
        var templateId = QueryHelper.GetInteger("templateid", 0);
        var instanceGuid = QueryHelper.GetGuid("instanceguid", Guid.Empty);
        var viewMode = ViewModeCode.FromString(QueryHelper.GetString("viewmode", String.Empty));
        var hash = QueryHelper.GetString("hash", String.Empty);
        var inline = QueryHelper.GetBoolean("inline", false);

        LiveSiteWidgetsParameters dialogParameters = new LiveSiteWidgetsParameters(aliasPath, viewMode)
        {
            ZoneId = zoneId,
            ZoneType = zoneType,
            InstanceGuid = instanceGuid,
            TemplateId = templateId,
            IsInlineWidget = inline
        };
        
        if (!dialogParameters.ValidateHash(hash))
        {
            return;
        }

        selectElem.AliasPath = aliasPath;
        selectElem.CultureCode = QueryHelper.GetString("culture", LocalizationContext.PreferredCultureCode);
        selectElem.ZoneId = zoneId;
        selectElem.ZoneType = zoneType;

        bool isInline = QueryHelper.GetBoolean("inline", false);
        selectElem.IsInline = isInline;

        // Base tag is added in master page
        AddBaseTag = false;

        // Proceeds the current item selection
        StringBuilder script = new StringBuilder();
        script.Append(@"
function SelectCurrentWidget() 
{");
        if (isInline)
        {
            // Skip initial configuration for inline widgets is not supported on the live site
            script.Append(@"
    selectedSkipDialog = false;");
        }
        script.Append(@"
    SelectWidget(selectedValue, selectedSkipDialog);
}

function SelectWidget(value, skipDialog)
{
    if ((value != null) && (value != ''))
    {");
        if (isInline)
        {
            script.Append(@"
        var editor = wopener.currentEditor || wopener.CMSPlugin.currentEditor;
        if (editor) {
            editor.getCommand('InsertWidget').open(value);
        }

        CloseDialog(false);");
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
           
");

        ScriptHelper.RegisterStartupScript(this, typeof(string), "WidgetSelector", script.ToString(), true);
        selectElem.SelectFunction = "SelectWidget";
        selectElem.IsLiveSite = true;

        // Set the title and icon
        PageTitle.TitleText = GetString("widgets.selectortitle");
        // Remove default css class
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
