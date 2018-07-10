using System;
using System.Linq;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

/// <summary>
/// Cart Item Units control for cart content web part transformation
/// </summary>
public partial class CMSModules_Ecommerce_Controls_Checkout_CartItemUnits : CMSCheckoutWebPart
{
    #region "Variables"

    private string mControlType;
    private string mImageURL;
    private string mControlLabel;
    private int mCartItemID;
    private bool? mShowUpdate;
    private string mFormControlName;
    private ShoppingCartItemInfo mShoppingCartItemInfoObject;
    private CMSAbstractWebPart mShoppingCartContent;

    #endregion


    #region "Private properties"

    private bool ReadOnly
    {
        get
        {
            return ValidationHelper.GetBoolean(ShoppingCartContent.GetValue("ReadOnlyMode"), false);
        }
    }


    /// <summary>
    /// The parent web part, in which transformation the current control is placed.
    /// </summary>
    private CMSAbstractWebPart ShoppingCartContent
    {
        get
        {
            if (mShoppingCartContent == null)
            {
                // Gets the parent control
                mShoppingCartContent = ControlsHelper.GetParentControl<CMSAbstractWebPart>(this);

                if (mShoppingCartContent != null)
                {
                    return mShoppingCartContent;
                }
            }

            return mShoppingCartContent;
        }
    }


    /// <summary>
    /// The current ShoppingCartInfo object on which the transformation is applied.
    /// </summary>
    private ShoppingCartItemInfo ShoppingCartItemInfoObject
    {
        get
        {
            return mShoppingCartItemInfoObject ?? (mShoppingCartItemInfoObject = ShoppingCart.CartItems.FirstOrDefault(i => i.CartItemID == CartItemID));
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Indicates, if the update control should be shown to update the count of the items in the shopping cart.
    /// </summary>
    public bool ShowUpdate
    {
        get
        {
            if (mShowUpdate == null)
            {
                mShowUpdate = false;
            }

            return mShowUpdate.Value;
        }
        set
        {
            mShowUpdate = ValidationHelper.GetBoolean(value, false);
        }
    }


    /// <summary>
    /// ID of the cart item to handle.
    /// </summary>
    public int CartItemID
    {
        get
        {
            return mCartItemID;
        }
        set
        {
            mCartItemID = ValidationHelper.GetInteger(value, 0);
        }

    }


    /// <summary>
    /// The control type, which should be used. Options are: "Link", "Button" and "Image".
    /// </summary>
    public string ControlType
    {
        get
        {
            if (string.IsNullOrEmpty(mControlType))
            {
                mControlType = "Button";
            }

            return mControlType;
        }
        set
        {
            mControlType = ValidationHelper.GetString(value, "Button");
        }
    }


    /// <summary>
    /// The form control code name from Form controls.
    /// </summary>
    public string UnitFormControlName
    {
        get
        {
            if (string.IsNullOrEmpty(mFormControlName))
            {
                mFormControlName = "TextBoxControl";
            }

            return mFormControlName;
        }
        set
        {
            mFormControlName = ValidationHelper.GetString(value, "TextBoxControl");
            unitCountFormControl.FormControlName = mFormControlName;
        }
    }


    /// <summary>
    /// The image URL when the control is displayed as an image.
    /// </summary>
    public string ImageURL
    {
        get
        {
            return mImageURL;
        }
        set
        {
            mImageURL = ValidationHelper.GetString(value, "");
        }
    }


    /// <summary>
    /// Indicates, if the control should be displayed as a button.
    /// </summary>
    public bool IsButton
    {
        get
        {
            return ControlType.ToLowerCSafe() == "button";
        }
    }


    /// <summary>
    /// Indicates, if the control should be displayed as an image.
    /// </summary>
    public bool IsImage
    {
        get
        {
            return ControlType.ToLowerCSafe() == "image";
        }
    }


    /// <summary>
    /// Indicates, if the control should be displayed as a link.
    /// </summary>
    public bool IsLink
    {
        get
        {
            return ControlType.ToLowerCSafe() == "link";
        }
    }


    /// <summary>
    /// The label to be used on the link or button. 
    /// </summary>
    public string ControlLabel
    {
        get
        {
            if (string.IsNullOrEmpty(mControlLabel))
            {
                mControlLabel = "checkout.update";
            }

            return mControlLabel;
        }
        set
        {
            mControlLabel = ValidationHelper.GetString(value, "checkout.update");
        }
    }


    /// <summary>
    /// Gets or sets the unit form control CSS class.
    /// </summary>
    public string UnitFormControlCssClass
    {
        get
        {
            return unitCountFormControl.CssClass;
        }
        set
        {
            unitCountFormControl.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the apply button CSS class.
    /// </summary>
    public string ApplyButtonCssClass
    {
        get
        {
            return btnUpdate.CssClass;
        }
        set
        {
            btnUpdate.CssClass = value;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Load event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        SetupControl();
    }


    /// <summary>
    /// Handle the visibility and the properties of the control, which are based on the parent control.
    /// </summary>  
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (ShoppingCartItemInfoObject != null)
        {
            unitCountFormControl.Value = ShoppingCartItemInfoObject.CartItemUnits.ToString();
            unitCountFormControl.CssClass = "UnitCountControl";

            // If a product option, disable editing or if the parent web part is in read only mode
            if (ShoppingCartItemInfoObject.IsProductOption || ReadOnly)
            {
                unitCountFormControl.Enabled = false;
            }
            else
            {
                // Manage update button visibility
                btnUpdate.Visible = pnlButton.Visible = ShowUpdate;
            }
        }
    }

    #endregion


    #region "Event handling"

    /// <summary>
    /// Updates the unit count for the current cart item and it's children.
    /// </summary>    
    public void Update(object sender, EventArgs e)
    {
        // Disable update operation for ReadOnly mode
        if (ReadOnly)
        {
            return;
        }

        var shoppingService = Service.Resolve<IShoppingService>();

        if ((ShoppingCartItemInfoObject != null) && !ShoppingCartItemInfoObject.IsProductOption)
        {
            var count = ValidationHelper.GetInteger(unitCountFormControl.Value, -1);
            // Do nothing (leave old value) for invalid/same input
            if ((count < 0) || (count == ShoppingCartItemInfoObject.CartItemUnits))
            {
                return;
            }

            if (count == 0)
            {
                shoppingService.RemoveItemFromCart(ShoppingCartItemInfoObject);
            }
            else
            {
                shoppingService.UpdateItemQuantity(ShoppingCartItemInfoObject, count);
            }
        }

        // Raise the change event for all subscribed web parts
        ComponentEvents.RequestEvents.RaiseEvent(this, e, SHOPPING_CART_CHANGED);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Initializes the control.
    /// </summary>
    public void SetupControl()
    {
        if (!StopProcessing)
        {
            // Show unit count control
            unitCountFormControl.Visible = true;
            // Set up the update control properties
            btnUpdate.ImageAltText = ResHelper.GetString("com.updatecartitemcount");
            btnUpdate.Visible = true;
            btnUpdate.ShowAsButton = IsButton;

            // The control is set up according to the chosen control type
            if (IsImage)
            {
                btnUpdate.ImageUrl = ImageURL;
            }
            else if (IsLink || (String.IsNullOrEmpty(ImageURL) && IsImage) || IsButton)
            {
                btnUpdate.ResourceString = ControlLabel;
            }
        }
    }

    #endregion
}