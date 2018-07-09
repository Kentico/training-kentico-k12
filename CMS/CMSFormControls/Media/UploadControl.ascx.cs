using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.IO;
using CMS.OnlineForms.Web.UI;
using CMS.SiteProvider;

public partial class CMSFormControls_Media_UploadControl : FormEngineUserControl
{
    #region "Variables"

    private string mValue;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return uploader.Enabled;
        }
        set
        {
            uploader.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets form control value.
    /// </summary>
    public override object Value
    {
        get
        {
            if (String.IsNullOrEmpty(mValue) || (mValue == Guid.Empty.ToString()))
            {
                return null;
            }
            return mValue;
        }
        set
        {
            mValue = ValidationHelper.GetString(value, String.Empty);
        }
    }


    /// <summary>
    /// Returns true if the form control provides some value to the field
    /// </summary>
    public override bool HasValue
    {
        get
        {
            return !(Form is CMSForm);
        }
    }


    /// <summary>
    /// Returns client ID of the inner upload control.
    /// </summary>
    public override string InputClientID
    {
        get
        {
            return uploader.UploadControl.ClientID;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (FieldInfo != null)
        {
            uploader.ID = FieldInfo.Name;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Form != null)
        {
            uploader.OnUploadFile += Form.RaiseOnUploadFile;
            uploader.OnDeleteFile += Form.RaiseOnDeleteFile;
        }

        // Apply styles
        if (!String.IsNullOrEmpty(ControlStyle))
        {
            uploader.Attributes.Add("style", ControlStyle);
            ControlStyle = null;
        }
        if (!String.IsNullOrEmpty(CssClass))
        {
            uploader.CssClass = CssClass;
            CssClass = null;
        }

        // Set image auto resize configuration
        if (FieldInfo != null)
        {
            int uploaderWidth;
            int uploaderHeight;
            int uploaderMaxSideSize;

            ImageHelper.GetAutoResizeDimensions(FieldInfo.Settings, SiteContext.CurrentSiteName, out uploaderWidth, out uploaderHeight, out uploaderMaxSideSize);

            uploader.ResizeToWidth = uploaderWidth;
            uploader.ResizeToHeight = uploaderHeight;
            uploader.ResizeToMaxSideSize = uploaderMaxSideSize;
        }

        CheckFieldEmptiness = false;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Hide hidden button
        hdnPostback.Style.Add("display", "none");

        // Do not process special actions if there is no form
        if (Form == null)
        {
            return;
        }

        // Uploader is not supported in CMSForm anymore
        if (Form is CMSForm)
        {
            lblError.Text = ResHelper.GetString("uploader.pagesnotsupported");
            lblError.Visible = true;
            plcUpload.Visible = false;
            return;
        }

        uploader.CurrentFileName = (Form is BizForm) ? ((BizForm)Form).GetFileNameForUploader(mValue) : FormHelper.GetOriginalFileName(mValue);
        uploader.CurrentFileUrl = "~/CMSPages/GetBizFormFile.aspx?filename=" + FormHelper.GetGuidFileName(mValue) + "&sitename=" + Form.SiteName;

        // Register post back button for update panel
        if (Form.ShowImageButton && Form.SubmitImageButton.Visible)
        {
            ControlsHelper.RegisterPostbackControl(Form.SubmitImageButton);
        }
        else if (Form.SubmitButton.Visible)
        {
            ControlsHelper.RegisterPostbackControl(Form.SubmitButton);
        }
    }


    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    public override bool IsValid()
    {
        var postedFile = uploader.PostedFile;

        // Check allow empty
        if ((FieldInfo != null) && !FieldInfo.AllowEmpty && ((Form == null) || Form.CheckFieldEmptiness))
        {
            if (String.IsNullOrEmpty(uploader.CurrentFileName) && (postedFile == null))
            {
                // Empty error
                if ((ErrorMessage != null) && !ErrorMessage.EqualsCSafe(ResHelper.GetString("BasicForm.InvalidInput"), true))
                {
                    ValidationError = ErrorMessage;
                }
                else
                {
                    ValidationError += ResHelper.GetString("BasicForm.ErrorEmptyValue");
                }
                return false;
            }
        }

        if ((postedFile != null) && (!String.IsNullOrEmpty(postedFile.FileName.Trim())))
        {
            // Test if file has allowed file-type
            string customExtension = ValidationHelper.GetString(GetValue("extensions"), String.Empty);
            string extensions = null;

            if (CMSString.Compare(customExtension, "custom", true) == 0)
            {
                extensions = ValidationHelper.GetString(GetValue("allowed_extensions"), String.Empty);
            }

            // Only extensions that are also allowed in settings can be uploaded
            extensions = UploadHelper.RestrictExtensions(extensions, SiteContext.CurrentSiteName);

            string ext = Path.GetExtension(postedFile.FileName);
            string validationError = String.Empty;

            if (extensions.EqualsCSafe(UploadHelper.NO_ALLOWED_EXTENSION))
            {
                validationError = ResHelper.GetString("uploader.noextensionallowed");
            }
            else if (!UploadHelper.IsExtensionAllowed(ext, extensions))
            {
                validationError = String.Format(ResHelper.GetString("BasicForm.ErrorWrongFileType"), HTMLHelper.HTMLEncode(ext.TrimStart('.')), extensions.Replace(";", ", "));
            }

            if (!String.IsNullOrEmpty(validationError))
            {
                ValidationError += validationError;
                return false;
            }
        }

        return true;
    }

    #endregion
}