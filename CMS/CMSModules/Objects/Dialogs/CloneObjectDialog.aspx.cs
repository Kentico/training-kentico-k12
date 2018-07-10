using System;
using System.Text;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.UIControls;

public partial class CMSModules_Objects_Dialogs_CloneObjectDialog : CMSModalPage
{
    string objectType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get query string parameters
        objectType = QueryHelper.GetString("objecttype", String.Empty);
        int objectId = QueryHelper.GetInteger("objectid", 0);

        // Get the object
        BaseInfo info = ProviderHelper.GetInfoById(objectType, objectId);

        string objTypeName = "";
        if (info != null)
        {
            objTypeName = GetString("objecttype." + TranslationHelper.GetSafeClassName(info.TypeInfo.ObjectType));
        }

        if (objTypeName.StartsWithCSafe("objecttype."))
        {
            objTypeName = "";
            SetTitle(String.Format(GetString("clonning.dialog.title"), HTMLHelper.HTMLEncode(ResHelper.LocalizeString(info.Generalized.ObjectDisplayName))));
        }
        else
        {
            SetTitle(String.Format(GetString("clonning.dialog.title"), objTypeName));
        }

        btnClone.Text = GetString("General.Clone");
        btnClone.Click += btnClone_Click;

        if (info == null)
        {
            ShowInformation(GetString("clonning.dialog.objectdoesnotexist"));
            cloneObjectElem.Visible = false;
            return;
        }

        if (cloneObjectElem.HasNoSettings())
        {
            ShowInformation(String.Format(GetString("clonning.settings.emptyinfobox"), objTypeName, HTMLHelper.HTMLEncode(ResHelper.LocalizeString(info.Generalized.ObjectDisplayName))));
        }
        else
        {
            ShowInformation(String.Format(GetString("clonning.settings.infobox"), objTypeName, HTMLHelper.HTMLEncode(ResHelper.LocalizeString(info.Generalized.ObjectDisplayName))));
        }

        // Check permissions
        if (!info.CheckPermissions(PermissionsEnum.Read, CurrentSiteName, CurrentUser))
        {
            RedirectToAccessDenied(info.TypeInfo.ModuleName, "read");
        }

        cloneObjectElem.InfoToClone = info;

        // Register refresh script to refresh wopener
        StringBuilder script = new StringBuilder();
        script.Append(@"
function RefreshContent() {
  if (wopener != null) {
    if (wopener.RefreshWOpener)
    {
        window.refreshPageOnClose = true;
        wopener.RefreshWOpener(window);
    }
    else
    {
        wopener.window.location.replace(wopener.window.location);
    }
  }
}");
        // Register script
        ScriptHelper.RegisterWOpenerScript(Page);
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "WOpenerRefresh", ScriptHelper.GetScript(script.ToString()));
    }


    protected void btnClone_Click(object sender, EventArgs e)
    {
        try
        {
            CloneResult result = cloneObjectElem.CloneObject();
            if (result != null)
            {
                if (result.Errors.Count > 0)
                {
                    ShowError(ResHelper.LocalizeString(String.Join("\n", result.Errors)));
                    SwitchToErrorMode();
                }
                else if (result.Warnings.Count > 0)
                {
                    ShowWarning(GetString("cloning.savedwithwarnings"), ResHelper.LocalizeString(String.Join("<br/>", result.Warnings)));
                    SwitchToErrorMode();
                }
                else
                {
                    ScriptHelper.RegisterStartupScript(Page, typeof(string), "CloneRefresh", cloneObjectElem.CloseScript, true);
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException(String.IsNullOrEmpty(objectType) ? "System" : objectType.ToLowerCSafe(), "CLONEOBJECT", ex);
            ShowError(ex.Message);

            if (!cloneObjectElem.UseTransaction)
            {
                SwitchToErrorMode();
            }
        }
    }


    private void SwitchToErrorMode()
    {
        plcForm.Visible = false;
        btnClone.Visible = false;
        SetCloseJavascript(cloneObjectElem.CloseScript + ";return false;");
    }
}
