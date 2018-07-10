using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Content_Controls_Dialogs_YouTube_YouTubeProperties : ItemProperties
{
    #region "Private properties"

    /// <summary>
    /// Returns the default width of the YouTube video.
    /// </summary>
    private int DefaultWidth
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["DefaultHeight"], 0);
        }
        set
        {
            ViewState["DefaultHeight"] = value;
        }
    }


    /// <summary>
    /// Returns the default height of the YouTube video.
    /// </summary>
    private int DefaultHeight
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["DefaultWidth"], 0);
        }
        set
        {
            ViewState["DefaultWidth"] = value;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterJQuery(Page);
        ScriptHelper.RegisterStartupScript(this, typeof(String), "InsertScript", ScriptHelper.GetScript(
               string.Format("window.insertItem = function(){{{0}}}",
                             Page.ClientScript.GetPostBackEventReference(btnHiddenInsert, string.Empty))));
        if (!StopProcessing)
        {
            // Refresh button
            btnRefresh.Text = GetString("dialogs.web.refresh");

            // YouTube default sizes control
            youTubeSizes.OnSelectedItemClick = ControlsHelper.GetPostBackEventReference(btnDefaultSizesHidden, "");

            btnHiddenPreview.Click += btnHiddenPreview_Click;
            btnHiddenInsert.Click += btnHiddenInsert_Click;
            btnHiddenSizeRefresh.Click += btnHiddenSizeRefresh_Click;
            btnDefaultSizesHidden.Click += btnDefaultSizesHidden_Click;

            sizeElem.CustomRefreshCode = ControlsHelper.GetPostBackEventReference(btnHiddenSizeRefresh, "") + ";return false;";

            CMSDialogHelper.RegisterDialogHelper(Page);

            ScriptHelper.RegisterStartupScript(Page, typeof(Page), "YTLoading", string.Format("Loading('{0}');", GetString("dialogs.youtube.preview").Replace("\'", "\\\'")), true);

            SetupOnChange();

            if (!RequestHelper.IsPostBack())
            {
                sizeElem.Locked = true;
                sizeElem.Width = DefaultWidth = 425;
                sizeElem.Height = DefaultHeight = 264;

                Hashtable dialogParameters = SessionHelper.GetValue("DialogParameters") as Hashtable;
                if ((dialogParameters != null) && (dialogParameters.Count > 0))
                {
                    LoadItemProperties(dialogParameters);
                    SessionHelper.SetValue("DialogParameters", null);
                }

                youTubeSizes.SelectedWidth = sizeElem.Width;
                youTubeSizes.SelectedHeight = sizeElem.Height;
            }

            youTubeSizes.LoadSizes(new[] { 425, 264, 480, 295, 560, 340, 640, 385 });
        }
    }


    protected void btnHiddenSizeRefresh_Click(object sender, EventArgs e)
    {
        sizeElem.Width = DefaultWidth;
        sizeElem.Height = DefaultHeight;

        LoadPreview();
    }


    protected void btnDefaultSizesHidden_Click(object sender, EventArgs e)
    {
        sizeElem.Width = youTubeSizes.SelectedWidth;
        sizeElem.Height = youTubeSizes.SelectedHeight;

        DefaultWidth = youTubeSizes.SelectedWidth;
        DefaultHeight = youTubeSizes.SelectedHeight;

        LoadPreview();
    }


    protected void btnHiddenPreview_Click(object sender, EventArgs e)
    {
        LoadPreview();
    }


    protected void btnHiddenInsert_Click(object sender, EventArgs e)
    {
        if (Validate())
        {
            Hashtable ytProperties = GetItemProperties();

            ScriptHelper.RegisterStartupScript(Page, typeof(Page), "insertYouTube", ScriptHelper.GetScript(CMSDialogHelper.GetYouTubeItem(ytProperties)));
        }
    }


    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadPreview();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Loads the preview with proper data.
    /// </summary>
    private void LoadPreview()
    {
        string url = FixPreviewUrl(txtLinkText.Text);
        if (!string.IsNullOrEmpty(url))
        {
            txtLinkText.Text = url;

            previewElem.AutoPlay = false; // Always ignore auto play in preview
            previewElem.Fs = chkFullScreen.Checked;
            previewElem.Height = 264;
            previewElem.Width = 425;
            previewElem.Rel = chkIncludeRelated.Checked;
            previewElem.Url = url;
        }
    }


    private string FixPreviewUrl(string url)
    {
        url = url.Trim();

        if (!ValidationHelper.IsURL(url))
        {
            return null;
        }

        if (!String.IsNullOrEmpty(url))
        {
            string videoID = null;
            if (url.Contains("youtu.be/"))
            {
                // Get video ID from short youtube URL
                Regex youTuBeRegex = RegexHelper.GetRegex("youtu.be/([^&]+)&?$?", true);
                Match m = youTuBeRegex.Match(url);
                if (m.Success)
                {
                    videoID = m.Groups[1].Value;
                }
            }
            else
            {
                // Get video ID from youtube URL
                Regex youTubeComRegex = RegexHelper.GetRegex("www.youtube.com/watch\\?.*&?v=([^&]+).*", true);
                Match m = youTubeComRegex.Match(url);
                if (m.Success)
                {
                    videoID = m.Groups[1].Value;
                }
            }

            String protocol = URLHelper.GetProtocol(url);

            if (!String.IsNullOrEmpty(videoID))
            {
                url = String.Format("{0}://www.youtube.com/watch?v={1}", String.IsNullOrEmpty(protocol) ? "http" : protocol, videoID);
            }
        }

        return url;
    }


    private void SetupOnChange()
    {
        string postBackRef = string.Format("setTimeout({0},100);", ScriptHelper.GetString(ControlsHelper.GetPostBackEventReference(btnHiddenPreview, "")));

        txtLinkText.Attributes["onchange"] = postBackRef;
        sizeElem.HeightTextBox.Attributes["onchange"] = postBackRef;
        sizeElem.WidthTextBox.Attributes["onchange"] = postBackRef;
        chkAutoplay.InputAttributes["onclick"] = postBackRef;
        chkFullScreen.InputAttributes["onclick"] = postBackRef;
        chkIncludeRelated.InputAttributes["onclick"] = postBackRef;
    }

    #endregion


    #region "Overridden methods"

    /// <summary>
    /// Loads the properties into control.
    /// </summary>
    /// <param name="properties">Collection of properties</param>
    public override void LoadItemProperties(Hashtable properties)
    {
        if (properties != null)
        {
            bool autoplay = ValidationHelper.GetBoolean(properties[DialogParameters.YOUTUBE_AUTOPLAY], false);
            bool fullScreen = ValidationHelper.GetBoolean(properties[DialogParameters.YOUTUBE_FS], false);
            bool relatedVideos = ValidationHelper.GetBoolean(properties[DialogParameters.YOUTUBE_REL], false);
            string url = ValidationHelper.GetString(properties[DialogParameters.YOUTUBE_URL], "");
            int width = ValidationHelper.GetInteger(properties[DialogParameters.YOUTUBE_WIDTH], 425);
            int height = ValidationHelper.GetInteger(properties[DialogParameters.YOUTUBE_HEIGHT], 264);

            DefaultWidth = width;
            DefaultHeight = height;

            chkAutoplay.Checked = autoplay;
            chkFullScreen.Checked = fullScreen;
            chkIncludeRelated.Checked = relatedVideos;
            txtLinkText.Text = url;
            sizeElem.Width = width;
            sizeElem.Height = height;

            LoadPreview();
        }
    }


    /// <summary>
    /// Returns all parameters of the selected item as name – value collection.
    /// </summary>
    public override Hashtable GetItemProperties()
    {
        Hashtable retval = new Hashtable();

        retval[DialogParameters.YOUTUBE_AUTOPLAY] = chkAutoplay.Checked;
        retval[DialogParameters.YOUTUBE_FS] = chkFullScreen.Checked;
        retval[DialogParameters.YOUTUBE_HEIGHT] = sizeElem.Height;
        retval[DialogParameters.YOUTUBE_REL] = chkIncludeRelated.Checked;
        retval[DialogParameters.YOUTUBE_URL] = txtLinkText.Text.Trim();
        retval[DialogParameters.YOUTUBE_WIDTH] = sizeElem.Width;
        retval[DialogParameters.OBJECT_TYPE] = "youtubevideo";

        return retval;
    }


    /// <summary>
    /// Clears the properties form.
    /// </summary>
    public override void ClearProperties(bool hideProperties)
    {
        sizeElem.Height = 425;
        sizeElem.Width = 264;

        chkAutoplay.Checked = false;
        chkIncludeRelated.Checked = true;
        chkFullScreen.Checked = true;

        previewElem.Url = "";
    }


    /// <summary>
    /// Validates all the user input.
    /// </summary>
    public override bool Validate()
    {
        string errorMessage = "";

        if (!ValidationHelper.IsURL(txtLinkText.Text))
        {
            errorMessage += " " + GetString("dialogs.youtube.invalidlink");
        }

        if (!sizeElem.Validate())
        {
            errorMessage += " " + GetString("dialogs.youtube.invalidsize");
        }

        errorMessage = errorMessage.Trim();
        if (errorMessage != "")
        {
            ScriptHelper.RegisterStartupScript(Page, typeof(Page), "YouTubePropertiesError", ScriptHelper.GetAlertScript(errorMessage));
            return false;
        }
        return true;
    }

    #endregion
}
