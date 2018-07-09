using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.DocumentEngine;

public partial class CMSWebParts_Text_QRCode : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Text
    /// </summary>
    public string Code
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("Code"), "");
        }
        set
        {
            this.SetValue("Code", value);
        }
    }


    /// <summary>
    /// Size
    /// </summary>
    public int Size
    {
        get
        {
            return ValidationHelper.GetInteger(this.GetValue("Size"), 4);
        }
        set
        {
            this.SetValue("Size", value);
        }
    }


    /// <summary>
    /// Maximum size in pixels
    /// </summary>
    public int MaxSideSize
    {
        get
        {
            return ValidationHelper.GetInteger(this.GetValue("MaxSideSize"), 0);
        }
        set
        {
            this.SetValue("MaxSideSize", value);
        }
    }


    /// <summary>
    /// Alternate text
    /// </summary>
    public string AlternateText
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("AlternateText"), "");
        }
        set
        {
            this.SetValue("AlternateText", value);
        }
    }


    /// <summary>
    /// Foreground color
    /// </summary>
    public string ForegroundColor
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("ForegroundColor"), "");
        }
        set
        {
            this.SetValue("ForegroundColor", value);
        }
    }


    /// <summary>
    /// Background color
    /// </summary>
    public string BackgroundColor
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("BackgroundColor"), "");
        }
        set
        {
            this.SetValue("BackgroundColor", value);
        }
    }


    /// <summary>
    /// Tooltip
    /// </summary>
    public string Tooltip
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("Tooltip"), "");
        }
        set
        {
            this.SetValue("Tooltip", value);
        }
    }


    /// <summary>
    /// Encoding
    /// </summary>
    public string Encoding
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("Encoding"), "B");
        }
        set
        {
            this.SetValue("Encoding", value);
        }
    }


    /// <summary>
    /// Version
    /// </summary>
    public int Version
    {
        get
        {
            return ValidationHelper.GetInteger(this.GetValue("Version"), 4);
        }
        set
        {
            this.SetValue("Version", value);
        }
    }


    /// <summary>
    /// Correction type
    /// </summary>
    public string Correction
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("Correction"), "M");
        }
        set
        {
            this.SetValue("Correction", value);
        }
    }

    #endregion

    
    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do not process
        }
        else
        {
            // Use the current URL as the default code
            string code = Code;
            if (String.IsNullOrEmpty(code))
            {
                code = URLHelper.GetAbsoluteUrl(RequestContext.RawURL);
                code = URLHelper.RemoveQuery(code);
            }

            imgCode.ImageUrl = ImageHelper.GetQRCodeUrl(code, Size, Encoding, Version, Correction, MaxSideSize, ForegroundColor, BackgroundColor);
            imgCode.ToolTip = Tooltip;
            imgCode.AlternateText = AlternateText;
        }
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }
}