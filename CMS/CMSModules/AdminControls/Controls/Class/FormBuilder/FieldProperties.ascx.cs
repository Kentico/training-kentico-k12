using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_AdminControls_Controls_Class_FormBuilder_FieldProperties : CMSUserControl
{
    #region "Constants"

    private const string TEXTBOX = "TextBoxControl";

    private const string TEXTAREA = "LargeTextArea";

    private const string CHECKBOX = "CheckBoxControl";

    private const string CALENDAR = "CalendarControl";

    #endregion


    #region "Properties"

    private string DefaultValueControlName
    {
        get
        {
            if (ViewState["defaultvaluecontrol"] == null)
            {
                ViewState["defaultvaluecontrol"] = TEXTBOX;
            }
            return ViewState["defaultvaluecontrol"].ToString();
        }
        set
        {
            ViewState["defaultvaluecontrol"] = value;
        }
    }

    #endregion


    #region "Control events"

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);

        defaultValue.FormControlName = DefaultValueControlName;
        if (defaultValue.FormControlName.EqualsCSafe(CALENDAR, StringComparison.InvariantCultureIgnoreCase))
        {
            defaultValue.SetValue("AllowMacros", true);
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        PrepareInfoLabels();

        optionsDesigner.Changed += optionsDesigner_OptionAdded;
    }


    protected void optionsDesigner_OptionAdded(object sender, EventArgs e)
    {
        // Save settings after adding new option
        ScriptHelper.RegisterStartupScript(this, GetType(), "FormBuilderSaveSettings", "FormBuilder.doPostBack(\"saveSettings\");", true);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (DefaultValueControlName.EqualsCSafe(TEXTAREA, StringComparison.InvariantCultureIgnoreCase))
        {
            // Disable Tab key for large text area
            defaultValue.SetValue("enabletabkey", false);
        }

        // Set associated control IDs
        lblLabel.AssociatedControlClientID = EditingFormControl.GetInputClientID(txtLabel.Controls);
        lblDefaultValue.AssociatedControlClientID = EditingFormControl.GetInputClientID(defaultValue.Controls);
        lblExplanationText.AssociatedControlClientID = EditingFormControl.GetInputClientID(txtExplanationText.Controls);
        lblTooltip.AssociatedControlClientID = EditingFormControl.GetInputClientID(txtTooltip.Controls);

        RegisterStartupScripts();
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Loads properties from given form field info.
    /// </summary>
    /// <param name="ffi">Form field info</param>
    public void LoadProperties(FormFieldInfo ffi)
    {
        if (ffi != null)
        {
            // Load caption
            LoadProperty(ffi, FormFieldPropertyEnum.FieldCaption, txtLabel, mphLabel);

            // Load explanation text
            LoadProperty(ffi, FormFieldPropertyEnum.ExplanationText, txtExplanationText, mphExplanationText);

            // Load tooltip
            LoadProperty(ffi, FormFieldPropertyEnum.FieldDescription, txtTooltip, mphTooltip);

            // Load default value
            LoadDefaultValue(ffi);

            // Set required checkbox
            chkRequired.Checked = !ffi.AllowEmpty;
            plcRequired.Visible = !FormHelper.IsFieldOfType(ffi, FormFieldControlTypeEnum.CheckBoxControl);

            LoadOptions(ffi);
        }
    }


    /// <summary>
    /// Saves properties to given form field info.
    /// </summary>
    /// <param name="ffi">Form field info</param>
    public void SaveProperties(FormFieldInfo ffi)
    {
        if (ffi != null)
        {
            // Save caption
            SaveProperty(ffi, FormFieldPropertyEnum.FieldCaption, txtLabel, true);

            // Save explanation text
            SaveProperty(ffi, FormFieldPropertyEnum.ExplanationText, txtExplanationText, true);

            // Save description
            SaveProperty(ffi, FormFieldPropertyEnum.FieldDescription, txtTooltip);

            // Save default value
            SaveProperty(ffi, FormFieldPropertyEnum.DefaultValue, defaultValue.EditingControl);

            // Save if the field is required
            ffi.AllowEmpty = !chkRequired.Checked;

            // Save options
            if (plcOptions.Visible)
            {
                ffi.Settings["Options"] = optionsDesigner.OptionsDefinition;
                SaveProperty(ffi, FormFieldPropertyEnum.DefaultValue, optionsDesigner);
            }
        }
    }

    #endregion


    #region "Private methods"

    private void LoadProperty(FormFieldInfo ffi, FormFieldPropertyEnum property, FormEngineUserControl control, MessagesPlaceHolder msgPlaceHolder)
    {
        if (control != null)
        {
            bool isMacro = ffi.IsMacro(property);
            control.Visible = !isMacro;
            if (!isMacro)
            {
                control.Value = ffi.GetPropertyValue(property);
            }
            else
            {
                msgPlaceHolder.ShowInformation(GetString("FormBuilder.PropertyIsMacro"));
            }
        }
    }


    private void LoadDefaultValue(FormFieldInfo ffi)
    {
        pnlDefValue.CssClass = "field-property";
        switch (ffi.DataType)
        {
            case FieldDataType.DateTime:
                {
                    SetControl(CALENDAR);
                    bool editTime = ValidationHelper.GetBoolean(ffi.Settings["EditTime"], true);
                    defaultValue.SetValue("EditTime", editTime);
                    defaultValue.SetValue("AllowMacros", true);
                }
                break;

            case FieldDataType.Boolean:
                SetControl(CHECKBOX);

                pnlDefValue.CssClass = "field-property inline-block";
                break;

            case FieldDataType.LongText:
                SetControl(TEXTAREA);
                break;

            default:
                SetControl(TEXTBOX);
                break;
        }

        if (FormHelper.HasListControl(ffi))
        {
            // For multiple choice control and for listbox allow select multiple options 
            optionsDesigner.AllowMultipleChoice = FormHelper.IsFieldOfType(ffi, FormFieldControlTypeEnum.MultipleChoiceControl)
                                                  || (FormHelper.IsFieldOfType(ffi, FormFieldControlTypeEnum.ListBoxControl) && (ValidationHelper.GetBoolean(ffi.Settings["AllowMultipleChoices"], true)));

            LoadProperty(ffi, FormFieldPropertyEnum.DefaultValue, optionsDesigner, mphDefaultValue);
        }
        else
        {
            LoadProperty(ffi, FormFieldPropertyEnum.DefaultValue, defaultValue.EditingControl, mphDefaultValue);
        }
    }


    private void SetControl(string controlName)
    {
        if (!controlName.EqualsCSafe(DefaultValueControlName, StringComparison.InvariantCultureIgnoreCase))
        {
            // Change default value control
            defaultValue.FormControlName = DefaultValueControlName = controlName;
            defaultValue.Reload();
        }
    }


    private void LoadOptions(FormFieldInfo ffi)
    {
        bool showOptions = FormHelper.HasListControl(ffi);

        plcOptions.Visible = showOptions;
        plcDefaultValue.Visible = !showOptions;

        if (showOptions)
        {
            // Query means that options contains sql select statement
            bool isSqlQuery = ffi.Settings.Contains("Query");
            bool isMacro = ffi.SettingIsMacro("Options");
            optionsDesigner.Visible = !isMacro && !isSqlQuery;

            if (isMacro)
            {
                mphOptions.ShowInformation(GetString("FormBuilder.PropertyIsMacro"));
            }
            else if (isSqlQuery)
            {
                mphOptions.ShowInformation(GetString("FormBuilder.PropertyIsNotOption"));
            }
            else
            {
                optionsDesigner.OptionsDefinition = ValidationHelper.GetString(ffi.Settings["Options"], string.Empty);
            }
        }
    }


    private void SaveProperty(FormFieldInfo ffi, FormFieldPropertyEnum property, FormEngineUserControl control, bool removeScripts = false)
    {
        if (control.Visible)
        {
            if (control is LocalizableFormEngineUserControl)
            {
                // Save translation
                ((LocalizableFormEngineUserControl)control).Save();
            }

            var value = ValidationHelper.GetString(control.Value, String.Empty);
            if (removeScripts)
            {
                // Wipe out scripts
                value = HTMLHelper.RemoveScripts(value);
            }

            ffi.SetPropertyValue(property, value);
        }
    }


    private void PrepareInfoLabels()
    {
        const string additionalCss = " InfoMessage";

        mphLabel.InfoLabel.CssClass += additionalCss;
        mphExplanationText.InfoLabel.CssClass += additionalCss;
        mphTooltip.InfoLabel.CssClass += additionalCss;
        mphDefaultValue.InfoLabel.CssClass += additionalCss;
        mphOptions.InfoLabel.CssClass += additionalCss;
    }


    private void RegisterStartupScripts()
    {
        const string script = @"
// Propertychange is IE8 and IE9 event
$cmsj('.pnl-edit #form-builder-properties .PanelProperties input, .pnl-edit #form-builder-properties .PanelProperties textarea').on('input propertychange', FormBuilder.setSaveSettingsTimeout);
$cmsj('.pnl-edit #form-builder-properties .PanelProperties input, .pnl-edit #form-builder-properties .PanelProperties textarea').on('change', FormBuilder.saveSettings);

// This is workaround for jQueryUI bug http://bugs.jqueryui.com/ticket/7941
// Clicking on sortable doesnt trigger blur event.
$cmsj('.EditingFormCategoryFields').mousedown(function () {
try {
    $cmsj('.settings-content input, .settings-content textarea').blur();
} catch (err) {}
});

// Set focus to Label property
FormBuilder.setFocusToInput();";

        ScriptHelper.RegisterStartupScript(this, GetType(), "PropertiesPanel", script, true);
    }

    #endregion
}