using System;

using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.UIControls;


public partial class CMSModules_AdminControls_Controls_Class_FormBuilder_FieldValidation : CMSUserControl
{
    #region "Control events"
    
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        ruleDesigner.RuleAdded += ruleDesigner_RuleAdded;
    }


    protected void ruleDesigner_RuleAdded(object sender, EventArgs e)
    {
        ScriptHelper.RegisterStartupScript(this, GetType(), "FormBuilderSaveSettings", "FormBuilder.doPostBack(\"saveSettings\");", true);
    }

    #endregion


    #region "Public methods"

    public void LoadRules(FormFieldInfo ffi)
    {
        if (ffi != null)
        {
            ruleDesigner.Value = ffi.FieldMacroRules;
            ruleDesigner.DefaultErrorMessage = ffi.GetPropertyValue(FormFieldPropertyEnum.ValidationErrorMessage);
        }
    }


    public void SaveValidation(FormFieldInfo ffi)
    {
        if (ffi != null)
        {
            ffi.FieldMacroRules = ruleDesigner.Value;
        }
    }

    #endregion
}
