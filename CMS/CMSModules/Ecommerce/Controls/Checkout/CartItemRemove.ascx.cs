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
/// Remove button control for shopping cart transformation
/// </summary>
public partial class CMSModules_Ecommerce_Controls_Checkout_CartItemRemove : CMSCheckoutWebPart
{
    #region "Variables"

    private string mControlType;
    private string mImageURL;
    private string mControlLabel;
    private ShoppingCartItemInfo mShoppingCartItemInfoObject;
    private CMSAbstractWebPart mShoppingCartContent;

    #endregion


    #region "Private properties"

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
            if (mShoppingCartItemInfoObject == null)
            {
                // Gets the current displayed CartItemInfo by ID                
                mShoppingCartItemInfoObject = ShoppingCart.CartItems.FirstOrDefault(i => i.CartItemID == CartItemID);
            }

            return mShoppingCartItemInfoObject;
        }
        set
        {
            mShoppingCartItemInfoObject = value;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// ID of the cart item to handle.
    /// </summary>
    public int CartItemID
    {
        get;
        set;
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
            mControlType = value;
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
            mImageURL = value;
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
            mControlLabel = value;
        }
    }


    /// <summary>
    /// Gets or sets the remove button CSS class.
    /// </summary>
    public string RemoveButtonCssClass
    {
        get
        {
            return btnRemove.CssClass;
        }
        set
        {
            btnRemove.CssClass = value;
        }
    }

    #endregion


    #region "Event handling"

    /// <summary>
    /// Removes the current cart item and the associated product options from the shopping cart.
    /// </summary>
    protected void Remove(object sender, EventArgs e)
    {
        var shoppingService = Service.Resolve<IShoppingService>();

        shoppingService.RemoveItemFromCart(ShoppingCartItemInfoObject);

        // Raise the change event for all subscribed web parts
        ComponentEvents.RequestEvents.RaiseEvent(this, e, SHOPPING_CART_CHANGED);
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Initializes the control.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (!StopProcessing)
        {
            // Set common UniButton properties
            btnRemove.ImageAltText = ResHelper.GetString("com.removecartitem");
            btnRemove.Visible = true;
            btnRemove.ShowAsButton = IsButton;

            // The control is set up according to the chosen control type
            if (IsImage)
            {
                btnRemove.ImageUrl = ImageURL;
            }
            else if (IsLink || (String.IsNullOrEmpty(ImageURL) && IsImage) || IsButton)
            {
                btnRemove.ResourceString = ControlLabel;
            }
        }
    }


    /// <summary>
    /// Handle the visibility of the control.
    /// </summary>    
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        bool ItemIsProductOption = (ShoppingCartItemInfoObject != null) && ShoppingCartItemInfoObject.IsProductOption;
        bool CartContentIsReadOnly = ValidationHelper.GetBoolean(ShoppingCartContent.GetValue("ReadOnlyMode"), false);
        // Remove is invisible for product option or in ReadOnly mode
        btnRemove.Visible = !(ItemIsProductOption || CartContentIsReadOnly);
    }

    #endregion
}