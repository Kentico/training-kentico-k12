using System;
using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.Helpers;


public partial class CMSInlineControls_YouTubeControl : InlineUserControl
{
    #region "Properties"

    /// <summary>
    /// Url of youtube media.
    /// </summary>
    public string Url
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Url"), "");
        }
        set
        {
            SetValue("Url", value);
        }
    }


    /// <summary>
    /// Enable full screen for youtube player.
    /// </summary>
    public bool Fs
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("Fs"), false);
        }
        set
        {
            SetValue("Fs", value);
        }
    }


    /// <summary>
    /// Enable auto play for youtube player.
    /// </summary>
    public bool AutoPlay
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AutoPlay"), false);
        }
        set
        {
            SetValue("AutoPlay", value);
        }
    }


    /// <summary>
    /// Enable relative videos in youtube player.
    /// </summary>
    public bool Rel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("Rel"), false);
        }
        set
        {
            SetValue("Rel", value);
        }
    }


    /// <summary>
    /// Width of youtube player.
    /// </summary>
    public int Width
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("Width"), 0);
        }
        set
        {
            SetValue("Width", value);
        }
    }


    /// <summary>
    /// Height of youtube player.
    /// </summary>
    public int Height
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("Height"), 0);
        }
        set
        {
            SetValue("Height", value);
        }
    }


    /// <summary>
    /// Control parameter.
    /// </summary>
    public override string Parameter
    {
        get
        {
            return Url;
        }
        set
        {
            Url = value;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Render control
    /// </summary>
    /// <param name="writer">Writer</param>
    protected override void Render(HtmlTextWriter writer)
    {
        // Render YouTube preview only if Url is set
        if (!string.IsNullOrEmpty(Url))
        {
            YouTubeVideoParameters ytParams = new YouTubeVideoParameters();
            ytParams.Url = Url;
            ytParams.FullScreen = Fs;
            ytParams.AutoPlay = AutoPlay;
            ytParams.RelatedVideos = Rel;
            ytParams.Width = Width;
            ytParams.Height = Height;

            writer.Write(MediaHelper.GetYouTubeVideo(ytParams));
        }

        base.Render(writer);
    }

    #endregion
}