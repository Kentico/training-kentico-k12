using System;

using CMS.Helpers;

using System.Text;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;


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

        selectElem.AliasPath = QueryHelper.GetString("aliaspath", String.Empty);
        selectElem.CultureCode = QueryHelper.GetString("culture", LocalizationContext.PreferredCultureCode);
        selectElem.ZoneId = QueryHelper.GetString("zoneid", String.Empty);
        selectElem.ZoneType = QueryHelper.GetString("zonetype", "").ToEnum<WidgetZoneTypeEnum>();

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
