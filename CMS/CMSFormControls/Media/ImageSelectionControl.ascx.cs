using System;

using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;


public partial class CMSFormControls_Media_ImageSelectionControl : FormEngineUserControl
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return imageSelector.Enabled;
        }
        set
        {
            imageSelector.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets form control value.
    /// </summary>
    public override object Value
    {
        get
        {
            return imageSelector.Value;
        }
        set
        {
            imageSelector.Value = ValidationHelper.GetString(value, null);
        }
    }


    /// <summary>
    /// Indicates if control is placed on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return imageSelector.IsLiveSite;
        }
        set
        {
            imageSelector.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Gets or sets the width of preview image.
    /// </summary>
    public int PreviewImageWidth
    {
        get
        {
            return GetValue("width", 0);
        }
        set
        {
            SetValue("width", value);
        }
    }


    /// <summary>
    /// Gets or sets the height of preview image.
    /// </summary>
    public int PreviewImageHeight
    {
        get
        {
            return GetValue("height", 0);
        }
        set
        {
            SetValue("height", value);
        }
    }


    /// <summary>
    /// Gets or sets the max side size of preview image.
    /// </summary>
    public int PreviewImageMaxSideSize
    {
        get
        {
            return GetValue("maxsidesize", 0);
        }
        set
        {
            SetValue("maxsidesize", value);
        }
    }
    

    /// <summary>
    /// Gets the value indicating if form control is in mode FileSelectionControl. If FALSE then form control is in mode ImageSelectionControl.
    /// </summary>
    public bool IsFileSelection
    {
        get
        {
            return FormHelper.IsFieldOfType(FieldInfo, FormFieldControlTypeEnum.FileSelectionControl);
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing)
        {
            SetupControl();
        }
    }


    /// <summary>
    /// Setups all controls.
    /// </summary>
    private void SetupControl()
    {
        // Initialize control
        imageSelector.ImagePreviewControl.AddCssClass("image-selector-image-preview");

        // Setup control
        imageSelector.ImageWidth = PreviewImageWidth;
        imageSelector.ImageHeight = PreviewImageHeight;
        imageSelector.ImageMaxSideSize = PreviewImageMaxSideSize;

        // Apply CSS styles
        if (!String.IsNullOrEmpty(ControlStyle))
        {
            imageSelector.Attributes.Add("style", ControlStyle);
            ControlStyle = null;
        }
        if (!String.IsNullOrEmpty(CssClass))
        {
            imageSelector.CssClass = CssClass;
            CssClass = null;
        }

        // Set image selector dialog
        imageSelector.DialogConfig = GetDialogConfiguration();

        // Set properties specific to File selection control
        imageSelector.ShowImagePreview = !IsFileSelection;
        imageSelector.DialogConfig.SelectableContent = IsFileSelection ? SelectableContentEnum.AllFiles : SelectableContentEnum.OnlyImages;
    }

    #endregion
}