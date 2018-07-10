using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineMarketing;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_OnlineMarketing_Pages_Widgets_WidgetProperties_Variant : CMSWidgetPropertiesPage
{
    #region "Page events"

    /// <summary>
    /// Init event handler
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        LoadEditForm();

        base.OnInit(e);
    }


    /// <summary>
    /// Load event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Design", "WidgetProperties.Variant"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WidgetProperties.Variant");
        }

        // Register the OnSave event handler
        FramesManager.OnSave += Save;
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Saves webpart properties.
    /// </summary>
    private bool Save(object sender, EventArgs e)
    {
        bool result = false;
        if (variantMode == VariantModeEnum.MVT)
        {
            // Save MVT variant
            result = mvtEditElem.UIFormControl.SaveData(null);
        }
        else if (variantMode == VariantModeEnum.ContentPersonalization)
        {
            // Save Content personalization variant
            result = cpEditElem.UIFormControl.SaveData(null);
        }

        // Display info message
        if (result)
        {
            ShowChangesSaved();
        }

        return result;
    }


    /// <summary>
    /// Loads and displays either the MVT or Content personalization edit form.
    /// </summary>
    /// <param name="forceReload">if true, then reloads the edit form content</param>
    private void LoadEditForm()
    {
        // Set the EditedObject attribute for the UIForm
        if (variantMode == VariantModeEnum.MVT)
        {
            mvtEditElem.UIFormControl.EditedObject = ProviderHelper.GetInfoById(MVTVariantInfo.OBJECT_TYPE, QueryHelper.GetInteger("variantid", 0));
            mvtEditElem.UIFormControl.ReloadData();

            // Display MVT edit dialog
            mvtEditElem.Visible = true;
            mvtEditElem.UIFormControl.SubmitButton.Visible = false;
        }
        else if (variantMode == VariantModeEnum.ContentPersonalization)
        {
            cpEditElem.UIFormControl.EditedObject = ProviderHelper.GetInfoById(ContentPersonalizationVariantInfo.OBJECT_TYPE, QueryHelper.GetInteger("variantid", 0));
            cpEditElem.UIFormControl.ReloadData();

            // Display Content personalization edit dialog
            cpEditElem.Visible = true;
            cpEditElem.UIFormControl.SubmitButton.Visible = false;
        }
    }

    #endregion
}