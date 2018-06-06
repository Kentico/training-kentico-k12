using System;

using CMS.Core;
using CMS.Base.Web.UI;
using CMS.DocumentEngine.Web.UI;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine;


/// <summary>
/// Discount coupon Web part
/// </summary>
public partial class CMSWebParts_Ecommerce_Checkout_InputFields_DiscountCoupon : CMSCheckoutWebPart
{
    private const string COUPON_CODE = "couponCode";


    #region "Public Properties"

    /// <summary>
    /// Gets or sets a value indicating whether [show update button].
    /// </summary>
    public bool ShowUpdateButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowUpdateButton"), false);
        }
        set
        {
            SetValue("ShowUpdateButton", value);
        }
    }


    /// <summary>
    /// Gets or sets a transformation used for rendering coupon codes.
    /// </summary>
    public string CouponCodeTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CouponCodeTransformationName"), string.Empty);
        }
        set
        {
            SetValue("CouponCodeTransformationName", value);
        }
    }

    #endregion


    #region "Private Properties"

    /// <summary>
    /// Gets or sets the coupon code value in editor.
    /// </summary>
    private string CouponCode
    {
        get
        {
            return ValidationHelper.GetString(txtCouponField.Text, "").Trim();
        }
        set
        {
            txtCouponField.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the coupon code value in editor.
    /// </summary>
    private string ErrorMessage
    {
        set
        {
            lblError.Text = value;
        }
    }

    #endregion


    #region "Life cycle"

    /// <summary>
    /// Load event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        SetupControl();
        HandlePostBack();
    }


    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        DisplayCouponCodes();
    }

    #endregion


    #region "Wizard methods"

    protected override void ValidateStepData(object sender, StepEventArgs e)
    {
        base.ValidateStepData(sender, e);

        var valid = ApplyDiscountCoupon();
        pnlError.Visible = !valid;

        // Make sure that in-memory changes persist (unsaved address, etc.)
        Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);

        // Applying discount coupon changes AppliedCouponCodes property which invalidates and recalculates shopping cart -> other web parts must be informed
        ComponentEvents.RequestEvents.RaiseEvent(this, e, SHOPPING_CART_CHANGED);

        if (!string.IsNullOrEmpty(CouponCode) && !valid)
        {
            e.CancelEvent = true;
        }
    }

    #endregion


    #region "Methods"

    private void DisplayCouponCodes()
    {
        var transformation = TransformationInfoProvider.GetTransformation(CouponCodeTransformationName);
        if (transformation != null)
        {
            rptrCouponCodes.ItemTemplate = TransformationHelper.LoadTransformation(rptrCouponCodes, transformation.TransformationFullName);
        }

        rptrCouponCodes.DataSource = GetViewModel();
        rptrCouponCodes.DataBind();
    }


    /// <summary>
    /// Checks whether entered discount coupon code is usable for this cart and applies it. Returns true if so.
    /// </summary>
    private bool ApplyDiscountCoupon()
    {
        if (string.IsNullOrEmpty(CouponCode))
        {
            return false;
        }

        var isApplied = Service.Resolve<IShoppingService>().AddCouponCode(CouponCode);
        if (isApplied)
        {
            CouponCode = null;
        }

        ErrorMessage = isApplied ? string.Empty : GetString("ecommerce.error.couponcodeisnotvalid");

        return isApplied;
    }


    private void HandlePostBack()
    {
        if (!RequestHelper.IsPostBack())
        {
            return;
        }

        var eventArgument = Page.Request.Params.Get("__EVENTARGUMENT");
        if (!string.IsNullOrEmpty(eventArgument))
        {
            var data = eventArgument.Split(':');
            if (data.Length == 2 && data[0] == COUPON_CODE)
            {
                var couponCode = data[1];
                Service.Resolve<IShoppingService>().RemoveCouponCode(couponCode);

                // Make sure that in-memory changes persist (unsaved address, etc.)
                Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);

                // Reset possible error message
                CouponCode = null;
                ErrorMessage = null;
                pnlError.Visible = false;

                ComponentEvents.RequestEvents.RaiseEvent(this, null, SHOPPING_CART_CHANGED);
            }
        }
    }


    private CouponCodesViewModel GetViewModel()
    {
        return new CouponCodesViewModel(ShoppingCart.CouponCodes);
    }


    /// <summary>
    /// Setups the control.
    /// </summary>
    public void SetupControl()
    {
        if (StopProcessing)
        {
            return;
        }

        // Update button visibility
        if (ShowUpdateButton)
        {
            btnUpdate.Visible = true;
            btnUpdate.Text = GetString("checkout.couponbutton");
        }
    }

    #endregion


    #region "Event handling"

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        pnlError.Visible = !ApplyDiscountCoupon();

        // Make sure that in-memory changes persist (unsaved address, etc.)
        Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);

        ComponentEvents.RequestEvents.RaiseEvent(this, e, SHOPPING_CART_CHANGED);
    }

    #endregion
}