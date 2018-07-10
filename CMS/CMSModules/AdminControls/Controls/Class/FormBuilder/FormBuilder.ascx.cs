using System;
using System.Web.UI;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;

public partial class CMSModules_AdminControls_Controls_Class_FormBuilder_FormBuilder : BaseFormBuilder, ICallbackEventHandler
{
    #region "Variables"

    private string mCallbackResult = string.Empty;
    private bool mReloadField;
    private bool mReloadForm;

    #endregion


    #region "Properties"

    /// <summary>
    /// Control which is used to design the form.
    /// </summary>
    public BasicForm Form
    {
        get
        {
            return formElem;
        }
    }


    /// <summary>
    /// Field name.
    /// </summary>
    private string FieldName
    {
        get
        {
            return ValidationHelper.GetString(ViewState["FieldName"], string.Empty);
        }
        set
        {
            ViewState["FieldName"] = value;
        }
    }


    /// <summary>
    /// Gets or sets Name of the macro rule category(ies) which should be displayed in Rule designer. Items should be separated by semicolon.
    /// </summary>
    public string ValidationRulesCategory
    {
        get
        {
            return ValidationHelper.GetString(UIContext["ConditionsCategory"], string.Empty);
        }
        set
        {
            UIContext["ConditionsCategory"] = value;
        }
    }


    /// <summary>
    /// Messages placeholder.
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMessagesHolder;
        }
    }

    #endregion


    #region "Control events"

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);

        if (ViewState["CurrentFormFields"] != null)
        {
            // Refresh UIContext data
            UIContext["CurrentFormFields"] = ViewState["CurrentFormFields"];
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Load form info
        FormInfo = FormHelper.GetFormInfo(ClassName, true);
        if (FormInfo != null)
        {
            ScriptHelper.RegisterJQueryUI(Page);
            ScriptHelper.RegisterScriptFile(Page, "~/CMSModules/AdminControls/Controls/Class/FormBuilder/FormBuilder.js");

            // Set up callback script
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "FormBuilder.receiveServerData", String.Empty);
            String callbackScript = "function doFieldAction(arg) {" + cbReference + "; }";
            ScriptHelper.RegisterClientScriptBlock(Page, GetType(), "FormBuilderCallback", callbackScript, true);

            // Prepare Submit button
            formElem.SubmitButton.RegisterHeaderAction = false;
            formElem.SubmitButton.OnClientClick = formElem.SubmitImageButton.OnClientClick = "return false;";

            formElem.FormInformation = FormInfo;
            formElem.Data = new DataRowContainer(FormInfo.GetDataRow());

            // Load form
            formElem.ReloadData();

            // Prepare error message label
            MessagesPlaceHolder.ErrorLabel.CssClass += " form-builder-error-hidden";
            MessagesPlaceHolder.ErrorText = GetString("FormBuilder.GeneralError");

            InitUIContext(FormInfo);
        }
        else
        {
            formElem.StopProcessing = true;
            ShowError(GetString("FormBuilder.ErrorLoadingForm"));
        }

        if (RequestHelper.IsPostBack())
        {
            ProcessAjaxPostBack();
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (FormInfo == null)
        {
            return;
        }

        // Reload selected field if required
        if (mReloadField && !mReloadForm && !String.IsNullOrEmpty(FieldName))
        {
            Form.ReloadData();
            if (Form.FieldUpdatePanels.ContainsKey(FieldName))
            {
                Form.FieldUpdatePanels[FieldName].Update();

                string script = String.Format("if (window.RecalculateFormWidth) {{RecalculateFormWidth('{0}');}}; FormBuilder.showSavedInfo();", formElem.ClientID);
                ScriptHelper.RegisterStartupScript(Page, pnlUpdateForm.GetType(), "recalculateFormWidth" + ClientID, ScriptHelper.GetScript(script));
            }
            else
            {
                Form.StopProcessing = true;
                MessagesPlaceHolder.ShowError(String.Format("{0} {1}", GetString("editedobject.notexists"), GetString("formbuilder.refresh")));

                MessagesPlaceHolder.ErrorLabel.CssClass += " form-builder-error";
                pnlUpdateForm.Update();
            }
        }

        // Reload whole form
        if (mReloadForm && !Form.StopProcessing)
        {
            formElem.ReloadData();
            pnlUpdateForm.Update();

            ScriptHelper.RegisterStartupScript(pnlUpdateForm, pnlUpdateForm.GetType(), "FormBuilderAddComponent", "FormBuilder.init(); FormBuilder.selectField('" + FieldName + "'); FormBuilder.showSavedInfo();", true);
        }

        // Display placeholder with message if form has no visible components and hide OK button
        if (FormInfo.GetFormElements(true, false).Count == 0)
        {
            ScriptHelper.RegisterStartupScript(pnlUpdateForm, pnlUpdateForm.GetType(), "FormBuilderShowPlaceholder", "FormBuilder.showEmptyFormPlaceholder();", true);
            formElem.SubmitButton.Visible = false;
        }

        // Set settings panel visibility
        pnlSettings.SetSettingsVisibility(!string.IsNullOrEmpty(FieldName));

        // Localized messages for JavaScript
        string infoScript = String.Format(@"
var msgSaving = {0};
var msgSaved = {1};",
                              ScriptHelper.GetLocalizedString("formbuilder.saving"),
                              ScriptHelper.GetLocalizedString("formbuilder.saved"));

        ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "FormBuilderLocalizedStrings", infoScript, true);
    }

    #endregion


    #region "Methods"

    private void ProcessAjaxPostBack()
    {
        if (RequestHelper.IsPostBack())
        {
            string eventArgument = Request.Params.Get("__EVENTARGUMENT");

            if (!string.IsNullOrEmpty(eventArgument))
            {
                string errorMessage;
                string[] data = eventArgument.Split(':');

                switch (data[0])
                {
                    case "loadSettings":
                        {
                            FieldName = data[1];
                            LoadSettings(FieldName);
                        }
                        break;

                    case "remove":
                        {
                            // Hide selected field from form
                            FieldName = string.Empty;
                            errorMessage = HideField(data[2]);
                            if (!String.IsNullOrEmpty(errorMessage))
                            {
                                ShowError(errorMessage);
                                return;
                            }

                            mReloadForm = true;
                            pnlSettings.Update();
                        }
                        break;

                    case "hideSettingsPanel":
                        {
                            FieldName = string.Empty;
                            pnlSettings.Update();
                        }
                        break;

                    case "saveSettings":
                        {
                            FormFieldInfo ffi = FormInfo.GetFormField(FieldName);
                            if (ffi != null)
                            {
                                FormFieldInfo originalFieldInfo = (FormFieldInfo)ffi.Clone();
                                pnlSettings.SaveSettings(ffi);

                                errorMessage = SaveFormDefinition(originalFieldInfo, ffi);
                                if (!String.IsNullOrEmpty(errorMessage))
                                {
                                    ShowError(errorMessage);
                                    return;
                                }

                                mReloadField = true;
                            }
                        }
                        break;

                    case "addField":
                        {
                            FormFieldInfo ffi = PrepareNewField(data[1]);
                            FieldName = ffi.Name;

                            errorMessage = AddField(ffi, data[2], ValidationHelper.GetInteger(data[3], -1));
                            if (!String.IsNullOrEmpty(errorMessage))
                            {
                                ShowError(errorMessage);
                                return;
                            }

                            LoadSettings(FieldName);
                            mReloadForm = true;
                        }
                        break;

                    case "clone":
                        {
                            FormFieldInfo ffi = FormInfo.GetFormField(data[2]);
                            if (ffi != null)
                            {
                                string newFieldName;
                                errorMessage = CloneField(ffi, out newFieldName);
                                if (!String.IsNullOrEmpty(errorMessage))
                                {
                                    ShowError(errorMessage);
                                    return;
                                }

                                FieldName = newFieldName;

                                LoadSettings(FieldName);
                                mReloadForm = true;
                            }
                        }
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Loads field settings to the Settings panel.
    /// </summary>
    /// <param name="fieldName">Field name</param>
    private void LoadSettings(string fieldName)
    {
        FormFieldInfo ffi = FormInfo.GetFormField(fieldName);
        if (ffi != null)
        {
            pnlSettings.LoadSettings(ffi);
            pnlSettings.Update();
        }
        else
        {
            mReloadForm = true;
        }
    }


    /// <summary>
    /// Hides field.
    /// </summary>
    /// <param name="fieldName">Name of field that should be hidden</param>
    /// <returns>Error message if an error occurred</returns>
    private string HideField(string fieldName)
    {
        if (!string.IsNullOrEmpty(fieldName))
        {
            FormFieldInfo ffiSelected = FormInfo.GetFormField(fieldName);
            if (ffiSelected == null)
            {
                return GetString("editedobject.notexists");
            }

            ffiSelected.Visible = false;
            return SaveFormDefinition();
        }
        return string.Empty;
    }


    /// <summary>
    /// Saves form definition. Updates database column if both original and changed info is passed and the change requires database update.
    /// </summary>
    /// <param name="oldFieldInfo">Form field info prior to the change</param>
    /// <param name="updatedFieldInfo">Form field info after the change has been made.</param>
    /// <returns>Error message if an error occurred</returns>
    private string SaveFormDefinition(FormFieldInfo oldFieldInfo = null, FormFieldInfo updatedFieldInfo = null)
    {
		if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm"))
		{
			if (RequestHelper.IsCallback())
			{
				return GetString("formbuilder.missingeditpermission");
			}

			RedirectToAccessDenied("cms.form", "EditForm");
		}

        string result = SaveFormDefinitionInternal(oldFieldInfo, updatedFieldInfo);
        if (String.IsNullOrEmpty(result) && (oldFieldInfo != null) && (updatedFieldInfo != null))
        {
            // Update current field name
            FieldName = updatedFieldInfo.Name;

            // Reload the whole form if field name changed
            mReloadForm = !oldFieldInfo.Name.Equals(FieldName, StringComparison.InvariantCultureIgnoreCase);
        }

        return result;
    }


    /// <summary>
    /// Adds form field info to the form to the specified position.
    /// </summary>
    /// <param name="ffi">Form field info which will be added</param>
    /// <param name="category">Category name</param>
    /// <param name="position">Field position in the category</param>
    private string AddField(FormFieldInfo ffi, string category, int position)
    {
		if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm"))
		{
			RedirectToAccessDenied("cms.form", "EditForm");
		}

        return AddFieldInternal(ffi, category, position);
    }


    /// <summary>
    /// Clones given field <paramref name="ffi"/>, adds the clone next to the original field and returns name of new field in <paramref name="newFieldName"/> out parameter.
    /// </summary>
    /// <param name="ffi">Form field to be cloned</param>
    /// <param name="newFieldName">Field name of new field</param>
    private string CloneField(FormFieldInfo ffi, out string newFieldName)
    {
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm"))
        {
            RedirectToAccessDenied("cms.form", "EditForm");
        }

        return CloneFieldInternal(ffi, out newFieldName);
    }

    #endregion


    #region "ICallbackEventHandler Members"

    /// <summary>
    /// Callback result retrieving handler.
    /// </summary>
    public string GetCallbackResult()
    {
        return mCallbackResult;
    }


    /// <summary>
    /// Raise callback method.
    /// </summary>
    public void RaiseCallbackEvent(string eventArgument)
    {
        if (!string.IsNullOrEmpty(eventArgument) && (FormInfo != null))
        {
            string[] data = eventArgument.Split(':');

            // Check that data are in proper format
            if (data.Length >= 3)
            {
                switch (data[0])
                {
                    case "move":
                        string fieldNames = data[1];
                        string errorMessage = "";

                        if (!string.IsNullOrEmpty(fieldNames))
                        {
                            string categoryName = data[2];
                            int position = ValidationHelper.GetInteger(data[3], -1);

                            errorMessage = MoveFieldsInternal(fieldNames.Split('|'), categoryName, position);

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                // Save changes
                                errorMessage = SaveFormDefinition();
                            }
                        }
                        mCallbackResult = PrepareCallbackResult(errorMessage);
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Returns <paramref name="errorMessage"/> in proper format or empty string.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    private string PrepareCallbackResult(string errorMessage)
    {
        return !String.IsNullOrEmpty(errorMessage) ? "error:" + String.Format("{0} {1}", errorMessage, GetString("formbuilder.refresh")).Replace(":", "##COLON##") : "";
    }

    #endregion
}