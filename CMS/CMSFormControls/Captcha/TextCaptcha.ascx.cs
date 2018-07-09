using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;

public partial class CMSFormControls_Captcha_TextCaptcha : FormEngineUserControl
{
    #region "Variables"

    private List<TextBox> textBoxList;

    #endregion


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
            pnlAnswer.Enabled = value;
        }
    }


    /// <summary>
    /// Number of textboxs.
    /// </summary>
    public int Count
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("Count"), 5);
        }
        set
        {
            SetValue("Count", value);
        }
    }


    /// <summary>
    /// String separator between textboxs.
    /// </summary>
    public string Separator
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Separator"), "-");
        }
        set
        {
            SetValue("Separator", value);
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
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return GetCaptachaValue();
        }
        set
        {
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

    protected override void CreateChildControls()
    {
        base.CreateChildControls();

        CreateTextBoxs();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Capta image url with anti cache query string parameter
        imgSecurityCode.ImageUrl = ResolveUrl(string.Format("~/CMSPages/Dialogs/CaptchaImage.aspx?hash={0}&captcha={1}&useWarp=0&width={2}&height={3}", Guid.NewGuid(), ClientID, ImageWidth, ImageHeight));

        // Show info label
        pnlSecurityLbl.Visible = ShowInfoLabel;

        // Show after text
        lblAfterText.Visible = ShowAfterText;
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Regenerate CAPTCHA if possible
        if (!RequestHelper.IsAsyncPostback() || ControlsHelper.IsInUpdatePanel(this))
        {
            GenerateNew();

            // Update update panel if needed
            if (ControlsHelper.IsInUpdatePanel(this))
            {
                ControlsHelper.UpdateCurrentPanel(this);
            }
        }
    }


    /// <summary>
    /// Generates new CAPTCHA.
    /// </summary>
    public void GenerateNew()
    {
        ClearTextBoxs();

        var captchaValue = GenerateRandomCode();
        WindowHelper.Add("CaptchaImageText" + ClientID, captchaValue);
    }


    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    public override bool IsValid()
    {
        bool isValid = false;
        object savedValue = WindowHelper.GetItem("CaptchaImageText" + ClientID);

        if (savedValue != null)
        {
            var captchaValue = GetCaptachaValue();
            string generatedCaptcha = ValidationHelper.GetString(savedValue, string.Empty);
            isValid = (captchaValue == generatedCaptcha);
        }

        return isValid;
    }


    /// <summary>
    /// Creates textboxs.
    /// </summary>
    private void CreateTextBoxs()
    {
        textBoxList = new List<TextBox>(Count);
        pnlAnswer.Controls.Clear();

        for (int i = 0; i < Count; i++)
        {
            LocalizedLabel sepLabel = null;
            var txtBox = new CMSTextBox
            {
                ID = "captcha_" + i,
                MaxLength = 1,
                CssClass = "CaptchaTextBoxSmall"
            };

            if (i > 0)
            {
                sepLabel = new LocalizedLabel
                {
                    Text = Separator,
                    CssClass = "form-control-text"
                };

                pnlAnswer.Controls.Add(sepLabel);
            }

            pnlAnswer.Controls.Add(txtBox);

            textBoxList.Add(txtBox);

            if (sepLabel != null)
            {
                // Connect the separator with next textbox so each textbox has its own label
                sepLabel.AssociatedControlClientID = txtBox.ClientID;
            }
            else if (Form == null)
            {
                // If inside form, the label tag is provided by form label, otherwise it has own label
                lblSecurityCode.AssociatedControlClientID = txtBox.ClientID;
            }
        }
    }


    /// <summary>
    /// Returns a string of Count random digits.
    /// </summary>
    private string GenerateRandomCode()
    {
        var random = new Random(ClientID.GetHashCode() + (int)DateTime.Now.Ticks);

        var codes = new List<int>(Count);

        for (var i = 0; i < Count; i++)
        {
            codes.Add(random.Next(10));
        }

        return string.Join(Separator, codes);
    }


    /// <summary>
    /// Gets CAPTCHA value.
    /// </summary>
    private string GetCaptachaValue()
    {
        var values = new List<string>(textBoxList.Select(txt => txt.Text));

        return string.Join(Separator, values);
    }


    /// <summary>
    /// Clears textboxs.
    /// </summary>
    private void ClearTextBoxs()
    {
        foreach (var txtBox in textBoxList)
        {
            txtBox.Text = string.Empty;
        }
    }

    #endregion
}