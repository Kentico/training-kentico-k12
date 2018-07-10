using System;

using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;

public partial class CMSFormControls_Captcha_SimpleCaptcha : FormEngineUserControl
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }
        set
        {
            base.Enabled = value;
            txtSecurityCode.Enabled = value;
        }
    }


    /// <summary>
    /// Indicates whether the info label should be displayed.
    /// </summary>
    public bool ShowInfoLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowInfoLabel"), false);
        }
        set
        {
            SetValue("ShowInfoLabel", value);
        }
    }


    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return txtSecurityCode.Text;
        }
        set
        {
            txtSecurityCode.Text = (string)value;
        }
    }


    /// <summary>
    /// Width of the CAPTCHA image.
    /// </summary>
    public int ImageWidth
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ImageWidth"), 80);
        }
        set
        {
            SetValue("ImageWidth", value);
        }
    }


    /// <summary>
    /// Height of the CAPTCHA image.
    /// </summary>
    public int ImageHeight
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ImageHeight"), 20);
        }
        set
        {
            SetValue("ImageHeight", value);
        }
    }


    /// <summary>
    /// Indicates whether the after text should be displayed.
    /// </summary>
    public bool ShowAfterText
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowAfterText"), false);
        }
        set
        {
            SetValue("ShowAfterText", value);
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Captcha image URL with anti cache query string parameter
        imgSecurityCode.ImageUrl = ResolveUrl(string.Format("~/CMSPages/Dialogs/CaptchaImage.aspx?hash={0}&captcha={1}&width={2}&height={3}", Guid.NewGuid(), ClientID, ImageWidth, ImageHeight));

        // Show info label
        if (ShowInfoLabel)
        {
            if (Form == null)
            {
                // If inside form, the label tag is provided by form label, otherwise it has own label
                lblSecurityCode.AssociatedControlClientID = txtSecurityCode.ClientID;
            }
            lblSecurityCode.Text = ResHelper.GetString("SecurityCode.lblSecurityCode");
            lblSecurityCode.Visible = true;
        }

        // Show after text
        plcAfterText.Visible = ShowAfterText;
        if (plcAfterText.Visible)
        {
            lblAfterText.Text = GetString("SecurityCode.Aftertext");
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
         // Regenerate CAPTCHA if possible
        if (!RequestHelper.IsAsyncPostback() || ControlsHelper.IsInUpdatePanel(this))
        {

            // Regenerate security code after postback
            GenerateNew();

            // Update update panel if needed
            if (ControlsHelper.IsInUpdatePanel(this))
            {
                ControlsHelper.UpdateCurrentPanel(this);
            }
        }
    }


    /// <summary>
    /// Generates new code.
    /// </summary>
    public void GenerateNew()
    {
        txtSecurityCode.Text = string.Empty;
        WindowHelper.Add("CaptchaImageText" + ClientID, GenerateRandomCode());
    }


    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    public override bool IsValid()
    {
        bool isValid = false;
        if (WindowHelper.GetItem("CaptchaImageText" + ClientID) != null)
        {
            isValid = (txtSecurityCode.Text == Convert.ToString(WindowHelper.GetItem("CaptchaImageText" + ClientID)));
        }

        return isValid;
    }


    /// <summary>
    /// Returns a string of six random digits.
    /// </summary>
    private string GenerateRandomCode()
    {
        Random random = new Random(ClientID.GetHashCode() + (int)DateTime.Now.Ticks);

        string s = string.Empty;

        for (int i = 0; i < 6; i++)
        {
            s = String.Concat(s, random.Next(10).ToString());
        }

        return s;
    }

    #endregion
}