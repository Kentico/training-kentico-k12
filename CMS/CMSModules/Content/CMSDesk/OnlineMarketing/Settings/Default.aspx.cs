using System;
using System.Linq;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WebAnalytics.Web.UI;
using CMS.Core;

[SaveAction(0)]
[UIElement(ModuleName.CONTENT, "Analytics.Settings")]
public partial class CMSModules_Content_CMSDesk_OnlineMarketing_Settings_Default : CMSAnalyticsContentPage
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.LocalDocumentPanel = pnlDoc;

        // Non-versioned data are modified
        DocumentManager.UseDocumentHelper = false;
        DocumentManager.HandleWorkflow = false;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        EditedObject = Node;

        // Set disabled module info
        ucDisabledModule.ParentPanel = pnlDisabled;

        ucConversionSelector.SelectionMode = SelectionModeEnum.SingleTextBox;
        ucConversionSelector.IsLiveSite = false;

        // Check modify permissions
        if (!DocumentUIHelper.CheckDocumentPermissions(Node, PermissionsEnum.Modify))
        {
            DocumentManager.DocumentInfo = String.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), Node.NodeAliasPath);

            // Disable save button
            CurrentMaster.HeaderActions.Enabled = false;
        }

        if ((Node != null) && !RequestHelper.IsPostBack())
        {
            ReloadData();
        }
    }


    /// <summary>
    /// Reload data from node to controls.
    /// </summary>
    private void ReloadData()
    {
        ucConversionSelector.Value = Node.DocumentTrackConversionName;
        txtConversionValue.Value = Node.DocumentConversionValue;
    }


    protected void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        if (Node != null)
        {
            string conversionName = ValidationHelper.GetString(ucConversionSelector.Value, String.Empty).Trim();

            if (!ucConversionSelector.IsValid())
            {
                e.ErrorMessage = ucConversionSelector.ValidationError;
                e.IsValid = false;
                return;
            }

            if (!txtConversionValue.IsValid())
            {
                e.ErrorMessage = GetString("conversionvalue.error");
                e.IsValid = false;
                return;
            }

            Node.DocumentConversionValue = txtConversionValue.Value.ToString();
            Node.DocumentTrackConversionName = conversionName;
        }
    }
}