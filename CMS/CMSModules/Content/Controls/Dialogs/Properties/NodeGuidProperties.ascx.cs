using System;
using System.Collections;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.IO;
using CMS.UIControls;


public partial class CMSModules_Content_Controls_Dialogs_Properties_NodeGuidProperties : ItemProperties
{
    #region "Private variables"

    private const int DefaultMaxSideSize = 192;
    private bool mCurrentIsImage = false;

    #endregion


    #region "Private properties"

    /// <summary>
    /// Indicates whether the current item is image.
    /// </summary>
    private bool CurrentIsImage
    {
        get
        {
            return mCurrentIsImage;
        }
        set
        {
            mCurrentIsImage = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing)
        {
            tabImageGeneral.HeaderText = GetString("general.general");
            imagePreview.SizeToURL = false;
            imagePreview.BorderWidth = 0;
            lblEmpty.Text = NoSelectionText;

            LoadPreview();
        }
    }


    #region "Private methods"

    /// <summary>
    /// Configures the preview control.
    /// </summary>
    private void LoadPreview()
    {
        plcImagePreviewArea.Visible = CurrentIsImage;

        if (CurrentIsImage)
        {
            string url = ValidationHelper.GetString(ViewState[DialogParameters.URL_URL], "");

            imagePreview.Tooltip = GetString("viewfile.openinfull");

            // Full size
            aFullSize.HRef = UrlResolver.ResolveUrl(url);
            aFullSize.Target = "parent";

            if (!string.IsNullOrEmpty(url))
            {
                url = URLHelper.UpdateParameterInUrl(url, "maxsidesize", DefaultMaxSideSize.ToString());
            }

            // Add latest version requirement for live site
            if (IsLiveSite)
            {
                // Add requirement for latest version of files for current document
                string newparams = "latestforhistoryid=" + HistoryID;
                newparams += "&hash=" + ValidationHelper.GetHashString("h" + HistoryID, new HashSettings(""));

                url += "&" + newparams;
            }

            // Preview
            imagePreview.URL = url;
        }
    }

    #endregion


    #region "Overridden methods"

    public override void LoadSelectedItems(MediaItem item, Hashtable properties)
    {
        if (item != null)
        {
            HistoryID = item.HistoryID;

            // Display size selector only if required or image
            CurrentIsImage = (item.MediaType == MediaTypeEnum.Image);

            properties[DialogParameters.FILE_NAME] = item.Name;
            properties[DialogParameters.FILE_SIZE] = item.Size;
            properties[DialogParameters.URL_URL] = item.Url;
            properties[DialogParameters.URL_EXT] = item.Extension;
            ViewState[DialogParameters.URL_URL] = item.Url;
            ViewState[DialogParameters.URL_EXT] = item.Extension;

            LoadProperties(properties);

            LoadPreview();
        }
    }


    /// <summary>
    /// Loads the properties into control.
    /// </summary>
    /// <param name="properties">Collection with properties</param>
    public override void LoadItemProperties(Hashtable properties)
    {
        LoadProperties(properties);
        if (tabImageGeneral.Visible)
        {
            LoadPreview();
        }
    }


    public override void LoadProperties(Hashtable properties)
    {
        if (properties != null)
        {
            // Display the properties
            pnlEmpty.Visible = false;
            pnlTabs.CssClass = "Dialog_Tabs";


            #region "Image general tab"

            // Display size selector only if required or image
            string ext = ValidationHelper.GetString(properties[DialogParameters.URL_EXT], "");

            CurrentIsImage = ImageHelper.IsImage(ext);

            if (tabImageGeneral.Visible)
            {
                string url = ValidationHelper.GetString(properties[DialogParameters.URL_URL], "");
                string fileName = ValidationHelper.GetString(properties[DialogParameters.FILE_NAME], "");
                long fileSize = ValidationHelper.GetLong(properties[DialogParameters.FILE_SIZE], 0);

                lblUrlText.Text = url;
                lblNameText.Text = AttachmentHelper.GetFullFileName(Path.GetFileNameWithoutExtension(fileName), ext);

                if ((plcSizeArea.Visible = CurrentIsImage))
                {
                    lblSizeText.Text = DataHelper.GetSizeString(fileSize);
                }

                ViewState[DialogParameters.IMG_SIZETOURL] = ValidationHelper.GetBoolean(properties[DialogParameters.IMG_SIZETOURL], false);
            }

            #endregion


            #region "General items"

            ViewState[DialogParameters.URL_EXT] = (properties[DialogParameters.URL_EXT] != null ? ValidationHelper.GetString(properties[DialogParameters.URL_EXT], "") : ValidationHelper.GetString(properties[DialogParameters.IMG_EXT], ""));
            ViewState[DialogParameters.URL_URL] = ValidationHelper.GetString(properties[DialogParameters.URL_URL], "");

            EditorClientID = ValidationHelper.GetString(properties[DialogParameters.EDITOR_CLIENTID], "");

            #endregion
        }
    }


    /// <summary>
    /// Returns all parameters of the selected item as name – value collection.
    /// </summary>
    public override Hashtable GetItemProperties()
    {
        Hashtable retval = new Hashtable();


        #region "Image general tab"

        if (tabImageGeneral.Visible)
        {
            string url = lblUrlText.Text.Trim();

            retval[DialogParameters.IMG_URL] = UrlResolver.ResolveUrl(url);
            retval[DialogParameters.IMG_EXT] = ValidationHelper.GetString(ViewState[DialogParameters.URL_EXT], "");
        }

        #endregion


        #region "General items"

        retval[DialogParameters.URL_EXT] = ValidationHelper.GetString(ViewState[DialogParameters.URL_EXT], "");
        retval[DialogParameters.URL_URL] = UrlResolver.ResolveUrl(ValidationHelper.GetString(ViewState[DialogParameters.URL_URL], ""));
        retval[DialogParameters.EDITOR_CLIENTID] = EditorClientID;

        #endregion


        return retval;
    }


    /// <summary>
    /// Clears the properties form.
    /// </summary>
    public override void ClearProperties(bool hideProperties)
    {
        // Hide the properties
        pnlEmpty.Visible = hideProperties;
        pnlTabs.CssClass = (hideProperties ? "DialogElementHidden" : "Dialog_Tabs");
    }

    #endregion
}