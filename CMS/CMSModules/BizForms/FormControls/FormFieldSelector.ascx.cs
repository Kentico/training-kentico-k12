using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.OnlineForms;
using CMS.SiteProvider;


/// <summary>
/// Form control for selecting specified form's field.
/// Returns data in "sitename;formname;fieldname" format.
/// Contains one parameter "FieldsDataType" that can be used to filter fields by data type.
/// </summary>
public partial class CMSModules_BizForms_FormControls_FormFieldSelector : FormEngineUserControl
{
    private const char SEPARATOR = ';';
    private string mSavedValue;

    /// <summary>
    /// Can be used to filter by data type.
    /// "Text" by default.
    /// </summary>
    public string FieldsDataType
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FieldsDataType"), "Text");
        }
        set
        {
            SetValue("FieldsDataType", value);
        }
    }


    /// <summary>
    /// Value in "sitename;formname;fieldname" format.
    /// </summary>
    public override object Value
    {
        get
        {
            if (string.IsNullOrEmpty(drpFields.SelectedValue))
            {
                return null;
            }

            return string.Format("{0}{3}{1}{3}{2}", SiteContext.CurrentSiteName, selectForm.Value, drpFields.SelectedValue, SEPARATOR);
        }
        set
        {
            mSavedValue = ValidationHelper.GetString(value, null);
        }
    }


    /// <summary>
    /// Value display name in "formdisplayname - fielddisplayname" format.
    /// </summary>
    public override string ValueDisplayName
    {
        get
        {
            if (string.IsNullOrEmpty(drpFields.SelectedValue))
            {
                return null;
            }

            return string.Format("{0} - {1}", selectForm.ValueDisplayName, drpFields.SelectedItem);
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        selectForm.DropDownSingleSelect.AutoPostBack = true;
        selectForm.Reload(false);

        if (!LoadExistingValue())
        {
            LoadFields();
        }

        base.OnLoad(e);
    }


    /// <summary>
    /// Tries to load existing value.
    /// Returns true if load was successful.
    /// </summary>
    private bool LoadExistingValue()
    {
        if (mSavedValue == null)
        {
            return false;
        }

        var formNameFieldName = mSavedValue.Split(SEPARATOR);
        if (formNameFieldName.Length != 3)
        {
            return false;
        }
        
        var formName = formNameFieldName[1];
        var fieldName = formNameFieldName[2];

        if (selectForm.DropDownItems.FindByValue(formName) == null)
        {
            return false;
        }

        selectForm.Value = formName;
        LoadFields();

        if (drpFields.Items.FindByValue(fieldName) == null)
        {
            return false;
        }
        
        drpFields.SelectedValue = fieldName;
        return true;
    }


    protected void UniSelector_OnSelectionChanged(object sender, EventArgs e)
    {
        LoadFields();
    }


    /// <summary>
    /// Loads all the fields from form selected in the first dropdown (selectForm) into the second dropdown (drpFields).
    /// </summary>
    private void LoadFields()
    {
        string className = ValidationHelper.GetString(selectForm.Value, null);
        if (className == null)
        {
            return;
        }

        var form = BizFormInfoProvider.GetBizFormInfo(className, SiteContext.CurrentSiteID);
        if (form == null)
        {
            return;
        }

        var classInfo = DataClassInfoProvider.GetDataClassInfo(form.FormClassID);
        if (classInfo == null)
        {
            return;
        }

        var formInfo = FormHelper.GetFormInfo(classInfo.ClassName, false);
        if (formInfo == null)
        {
            return;
        }

        drpFields.Items.Clear();
        drpFields.Enabled = false;

        IEnumerable<FormFieldInfo> fields;
        if (FieldsDataType != FieldDataType.Unknown)
        {
            fields = formInfo.GetFields(FieldsDataType).Where(x => x.Visible);
        }
        else
        {
            fields = formInfo.GetFields(true, true);
        }

        foreach (var fieldInfo in fields)
        {
            drpFields.Items.Add(new ListItem(fieldInfo.GetDisplayName(MacroResolver.GetInstance()), fieldInfo.Name));
        }

        if (drpFields.Items.Count > 0)
        {
            drpFields.Enabled = true;
        }
    }
}