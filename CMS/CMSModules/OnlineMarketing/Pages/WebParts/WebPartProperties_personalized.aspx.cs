using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.Membership;
using CMS.OnlineMarketing;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_OnlineMarketing_Pages_WebParts_WebPartProperties_personalized : CMSWebPartPropertiesPage
{
    #region "Page events"

    /// <summary>
    /// Init event handler
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        // Set the EditedObject attribute for the UIForm
        if (variantMode == VariantModeEnum.MVT)
        {
            mvtEditElem.UIFormControl.EditedObject = ProviderHelper.GetInfoById(MVTVariantInfo.OBJECT_TYPE, QueryHelper.GetInteger("variantid", 0));
            mvtEditElem.UIFormControl.ReloadData();
        }
        else if (variantMode == VariantModeEnum.ContentPersonalization)
        {
            cpEditElem.UIFormControl.EditedObject = ProviderHelper.GetInfoById(ContentPersonalizationVariantInfo.OBJECT_TYPE, QueryHelper.GetInteger("variantid", 0));
            cpEditElem.UIFormControl.ReloadData();
        }

        base.OnInit(e);
    }


    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        var ui = MembershipContext.AuthenticatedUser;
        if (!ui.IsAuthorizedPerUIElement("CMS.Design", "WebPartProperties.Variant"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WebPartProperties.Variant");
        }

        // Check permissions (based on variant type)
        if (variantMode == VariantModeEnum.MVT)
        {
            if (!ui.IsAuthorizedPerResource("CMS.MVTest", "Read"))
            {
                // Not authorized for MV test - Read.
                RedirectToInformation(String.Format(GetString("general.permissionresource"), "Read", "CMS.MVTest"));
            }
        }
        else if (variantMode == VariantModeEnum.ContentPersonalization)
        {
            if (!ui.IsAuthorizedPerResource("CMS.ContentPersonalization", "Read"))
            {
                // Not authorized for Content personalization - Read.
                RedirectToInformation(String.Format(GetString("general.permissionresource"), "Read", "CMS.ContentPersonalization"));
            }
        }

        // Setup the control
        SetupControl();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Setups the control.
    /// </summary>
    private void SetupControl()
    {
        if (!VirtualPathHelper.UsingVirtualPathProvider)
        {
            ShowWarning(GetString("WebPartCode.ProviderNotRunning"), "", "");
        }

        // Register the OnSave event handler
        FramesManager.OnSave += OnSaveEventHandler;

        if (variantMode == VariantModeEnum.MVT)
        {
            // Display MVT edit dialog
            mvtEditElem.Visible = true;
            mvtEditElem.UIFormControl.SubmitButton.Visible = false;
        }
        else if (variantMode == VariantModeEnum.ContentPersonalization)
        {
            // Display Content personalization edit dialog
            cpEditElem.Visible = true;
            cpEditElem.UIFormControl.SubmitButton.Visible = false;
        }
    }


    /// <summary>
    /// Raised when the Save action is required.
    /// </summary>
    protected bool OnSaveEventHandler(object sender, EventArgs e)
    {
        if (variantMode == VariantModeEnum.MVT)
        {
            return mvtEditElem.UIFormControl.SaveData(null);
        }
        else if (variantMode == VariantModeEnum.ContentPersonalization)
        {
            return cpEditElem.UIFormControl.SaveData(null);
        }

        return false;
    }

    #endregion
}