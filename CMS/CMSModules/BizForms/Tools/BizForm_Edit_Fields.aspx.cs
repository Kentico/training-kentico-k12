using System;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.UIControls;

// Edited object
[EditedObject(BizFormInfo.OBJECT_TYPE, "formId")]
[Security(Resource = "CMS.Form", Permission = "ReadForm")]
[UIElement("CMS.Form", "Forms.Fields")]
public partial class CMSModules_BizForms_Tools_BizForm_Edit_Fields : CMSBizFormPage
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


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        FieldEditor.FormType = FormTypeEnum.BizForm;
        FieldEditor.Visible = false;

        if (FormInfo != null)
        {
            // Get form class info
            DataClassInfo dci = DataClassInfoProvider.GetDataClassInfo(FormInfo.FormClassID);
            if (dci != null)
            {
                if (dci.ClassIsCoupledClass)
                {
                    // Setup the field editor
                    CurrentMaster.BodyClass += " FieldEditorBody";
                    FieldEditor.Visible = true;
                    FieldEditor.ClassName = dci.ClassName;
                    FieldEditor.Mode = FieldEditorModeEnum.BizFormDefinition;
                    FieldEditor.OnAfterDefinitionUpdate += FieldEditor_OnAfterDefinitionUpdate;
                    FieldEditor.OnFieldNameChanged += FieldEditor_OnFieldNameChanged;
                }
                else
                {
                    ShowError(GetString("EditTemplateFields.ErrorIsNotCoupled"));
                }
            }
        }

        ScriptHelper.HideVerticalTabs(this);
    }

    #endregion


    #region "Event handlers"

    protected void FieldEditor_OnAfterDefinitionUpdate(object sender, EventArgs e)
    {
        // Update form to log synchronization
        if (FormInfo != null)
        {
            BizFormInfoProvider.SetBizFormInfo(FormInfo);

            // State of unigrids may contain where/order by clauses no longer valid after definition update
            UniGrid.ResetStates(BizFormItemProvider.GetObjectType(FieldEditor.ClassName));
        }
    }


    protected void FieldEditor_OnFieldNameChanged(object sender, string oldFieldName, string newFieldName)
    {
        if (FormInfo != null)
        {
            // Rename field in layout(s)
            FormHelper.RenameFieldInFormLayout(FormInfo.FormClassID, oldFieldName, newFieldName);
        }
    }

    #endregion
}