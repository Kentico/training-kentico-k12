using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;


public partial class CMSWebParts_Ecommerce_ShoppingCart_ShoppingCartWebPart : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the url where customer is redirected after purchase.
    /// </summary>
    public string RedirectAfterPurchase
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RedirectAfterPurchase"), cartElem.RedirectAfterPurchase);
        }
        set
        {
            SetValue("RedirectAfterPurchase", value);
            cartElem.RedirectAfterPurchase = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether retrieval of forgotten password is enabled.
    /// </summary>
    public bool PasswordRetrieval
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PasswordRetrieval"), true);
        }
        set
        {
            SetValue("PasswordRetrieval", value);
            cartElem.EnablePasswordRetrieval = value;
        }
    }


    /// <summary>
    /// Gets or sets the conversion track name used after successful registration.
    /// </summary>
    public string RegistrationTrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RegistrationTrackConversionName"), "");
        }
        set
        {
            if (value.Length > 400)
            {
                value = value.Substring(0, 400);
            }
            SetValue("RegistrationTrackConversionName", value);
            cartElem.RegistrationTrackConversionName = value;
        }
    }


    /// <summary>
    /// Gets or sets the conversion track name used after successful order.
    /// </summary>
    public string OrderTrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderTrackConversionName"), "");
        }
        set
        {
            if (value.Length > 400)
            {
                value = value.Substring(0, 400);
            }
            SetValue("OrderTrackConversionName", value);
            cartElem.OrderTrackConversionName = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether product price detail link is displayed.
    /// </summary>
    public bool EnableProductPriceDetail
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EnableProductPriceDetail"), false);
        }
        set
        {
            SetValue("EnableProductPriceDetail", value);
            cartElem.EnableProductPriceDetail = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether step images should be displayed.
    /// </summary>
    public bool DisplayStepImages
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayStepImages"), false);
        }
        set
        {
            SetValue("DisplayStepImages", value);
            cartElem.DisplayStepImages = value;
        }
    }


    /// <summary>
    /// Gets or sets the HTML code of the image step separator.
    /// </summary>
    public string ImageStepSeparator
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ImageStepSeparator"), cartElem.ImageStepSeparator);
        }
        set
        {
            SetValue("ImageStepSeparator", value);
            cartElem.ImageStepSeparator = value;
        }
    }


    /// <summary>
    /// Gets or sets the text which is displayed for required fields in form.
    /// </summary>
    public string RequiredFieldsMark
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RequiredFieldsMark"), "");
        }
        set
        {
            SetValue("RequiredFieldsMark", value);
            cartElem.RequiredFieldsMark = value;
        }
    }


    /// <summary>
    /// XML definition of the custom checkout process
    /// </summary>
    public string CheckoutProcess
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CheckoutProcess"), "");
        }
        set
        {
            SetValue("CheckoutProcess", value);
        }
    }

    #endregion


    #region "Registration properties"

    /// <summary>
    /// Gets or sets the email where the new registration notification should be sent to.
    /// </summary>
    public string SendNewRegistrationNotificationToAddress
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SendNewRegistrationNotificationToAddress"), cartElem.SendNewRegistrationNotificationToAddress);
        }
        set
        {
            SetValue("SendNewRegistrationNotificationToAddress", value);
            cartElem.SendNewRegistrationNotificationToAddress = value;
        }
    }


    /// <summary>
    /// Gets or sets the roles where the new user should be assign to after the registration.
    /// </summary>
    public string AssignToRoles
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AssignToRoles"), cartElem.AssignToRoles);
        }
        set
        {
            SetValue("AssignToRoles", value);
            cartElem.AssignToRoles = value;
        }
    }


    /// <summary>
    /// Gets or sets the sites where the new user should be assign to after the registration.
    /// </summary>
    public string AssignToSites
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AssignToSites"), cartElem.AssignToSites);
        }
        set
        {
            SetValue("AssignToSites", value);
            cartElem.AssignToSites = value;
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
            // Do nothing
        }
        else
        {
            NotResolveProperties = "AddToShoppingCartConversionValue;OrderConversionValue;RegistrationConversionValue";
            // Set shopping cart properties
            cartElem.RedirectAfterPurchase = RedirectAfterPurchase;
            cartElem.EnablePasswordRetrieval = PasswordRetrieval;
            cartElem.RegistrationTrackConversionName = RegistrationTrackConversionName;
            cartElem.OrderTrackConversionName = OrderTrackConversionName;
            cartElem.SendNewRegistrationNotificationToAddress = SendNewRegistrationNotificationToAddress;
            cartElem.AssignToRoles = AssignToRoles;
            cartElem.AssignToSites = AssignToSites;
            cartElem.DisplayStepImages = DisplayStepImages;
            cartElem.ImageStepSeparator = ImageStepSeparator;
            cartElem.EnableProductPriceDetail = EnableProductPriceDetail;
            cartElem.RequiredFieldsMark = RequiredFieldsMark;

            if (string.IsNullOrEmpty(CheckoutProcess))
            {
                // Load checkout process from e-commerce configuration
                cartElem.CheckoutProcessType = CheckoutProcessEnum.LiveSite;
            }
            else
            {
                // Load custom checkout process from web part
                cartElem.CheckoutProcessType = CheckoutProcessEnum.Custom;
                CheckoutProcessInfo process = new CheckoutProcessInfo(CheckoutProcess);
                cartElem.LoadCheckoutProcess(process);
            }
        }
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }
}