using System;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.UIControls;

// Edited object
[EditedObject(BizFormInfo.OBJECT_TYPE, "formId")]
[Security(Resource = "CMS.Form", Permission = "ReadForm")]
[UIElement("CMS.Form", "Forms.FormBuilder")]
public partial class CMSModules_BizForms_Tools_BizForm_Edit_FormBuilder : CMSBizFormPage
{
    #region "Properties"

    protected BizFormInfo FormInfo
    {
        get
        {
            return EditedObject as BizFormInfo;
        }
    }

    #endregion


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (FormInfo == null)
        {
            return;
        }
        
        // Get form class info
        var dci = DataClassInfoProvider.GetDataClassInfo(FormInfo.FormClassID);
        if (dci != null)
        {
            if (dci.ClassIsCoupledClass)
            {
                // Setup the field editor
                FormBuilder.OnAfterDefinitionUpdate += FormBuilder_OnAfterDefinitionUpdate;
            }
            else
            {
                ShowError(GetString("EditTemplateFields.ErrorIsNotCoupled"));
            }

            FormBuilder.ClassName = DataClassInfoProvider.GetClassName(FormInfo.FormClassID);
        }

        // Prepare submit text
        string submitText = null;
        if (!String.IsNullOrEmpty(FormInfo.FormSubmitButtonText))
        {
            submitText = ResHelper.LocalizeString(FormInfo.FormSubmitButtonText);
        }

        // Init submit button
        if (!string.IsNullOrEmpty(FormInfo.FormSubmitButtonImage))
        {
            ImageButton imageButton = FormBuilder.Form.SubmitImageButton;
            // Image button
            imageButton.ImageUrl = FormInfo.FormSubmitButtonImage;

            if (submitText != null)
            {
                imageButton.AlternateText = submitText;
                imageButton.ToolTip = submitText;
            }
        }
        else
        {
            // Standard button
            if (submitText != null)
            {
                FormBuilder.Form.SubmitButton.ResourceString = submitText;
            }
        }

        ScriptHelper.HideVerticalTabs(this);
    }


    private void FormBuilder_OnAfterDefinitionUpdate(object sender, EventArgs e)
    {
        // Update form to log synchronization
        if (FormInfo != null)
        {
            BizFormInfoProvider.SetBizFormInfo(FormInfo);
        }
    }
}
