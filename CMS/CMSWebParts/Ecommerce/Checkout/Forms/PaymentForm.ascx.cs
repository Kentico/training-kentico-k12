using System;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;


public partial class CMSWebParts_Ecommerce_Checkout_Forms_PaymentForm : CMSAbstractWebPart, IGatewayFormLoader
{
    #region "Variables"

    private CMSPaymentGatewayProvider mPaymentGatewayProvider;
    private CMSPaymentGatewayForm mPaymentDataForm;
    private ShoppingCartInfo mShoppingCart;
    private int mOrderId;

    #endregion


    #region "Properties"

    private ShoppingCartInfo ShoppingCart
    {
        get
        {
            return mShoppingCart ?? (mShoppingCart = ShoppingCartInfoProvider.GetShoppingCartInfoFromOrder(mOrderId));
        }
    }


    /// <summary>
    /// Payment gateway provider instance.
    /// </summary>
    public CMSPaymentGatewayProvider PaymentGatewayProvider
    {
        get
        {
            if ((mPaymentGatewayProvider == null) && (ShoppingCart != null))
            {
                mPaymentGatewayProvider = CMSPaymentGatewayProvider.GetPaymentGatewayProvider<CMSPaymentGatewayProvider>(ShoppingCart.ShoppingCartPaymentOptionID);
            }

            return mPaymentGatewayProvider;
        }
        set
        {
            mPaymentGatewayProvider = value;
        }
    }


    /// <summary>
    /// Page where the user should be redirected after successful payment.
    /// </summary>
    public string RedirectAfterPurchase
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RedirectAfterPurchase"), "");
        }
        set
        {
            SetValue("RedirectAfterPurchase", value);
        }
    }


    /// <summary>
    /// Button text to be displayed on Process payment button.
    /// </summary>
    public string ProcessPaymentButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ProcessPaymentButtonText"), "");
        }
        set
        {
            SetValue("ProcessPaymentButtonText", value);
        }
    }

    #endregion


    #region "Page methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        EnsureChildControls();
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        var isLiveSite = (PortalContext.ViewMode == ViewModeEnum.LiveSite) || IsLiveSite;
        // Allow provider initialization and redirection only for live site attempts
        if (isLiveSite && ((PaymentGatewayProvider == null) && (mOrderId > 0)))
        {
            URLHelper.Redirect(UrlResolver.ResolveUrl(RedirectAfterPurchase));
        }

        // Cancel setup for undefined cart.
        if (ShoppingCart != null)
        {
            SetupControl();
        }

        btnProcessPayment.Text = ProcessPaymentButtonText;
    }

    #endregion


    #region "Event handling"

    protected void btnProcessPayment_Click(object sender, EventArgs e)
    {
        if (PaymentGatewayProvider != null && PaymentGatewayProvider.OrderId > 0)
        {
            // Validate data if web part is not placed in wizard
            if (!String.IsNullOrEmpty(mPaymentDataForm.ValidateData()))
            {
                // Do not continue if validation failed
                return;
            }

            // Prevent duplicit payment
            if (PaymentGatewayProvider.IsPaymentCompleted)
            {
                ShowError(ResHelper.GetString("paymentgateway.form.duplicitpayment"));
                return;
            }

            PaymentResultInfo result = null;

            // Skip payment when user is not authorized
            if (PaymentGatewayProvider.IsUserAuthorizedToFinishPayment(MembershipContext.AuthenticatedUser, ShoppingCart, !IsLiveSite))
            {
                var data = mPaymentDataForm.GetPaymentGatewayData();
                result = Service.Resolve<IPaymentGatewayProcessor>().ProcessPayment(PaymentGatewayProvider, data);
            }

            // Show info message
            if (!string.IsNullOrEmpty(PaymentGatewayProvider.InfoMessage))
            {
                lblInfo.Visible = true;
                lblInfo.Text = PaymentGatewayProvider.InfoMessage;
            }

            // Show error message
            if (!string.IsNullOrEmpty(PaymentGatewayProvider.ErrorMessage))
            {
                ShowError(PaymentGatewayProvider.ErrorMessage);
            }
            
            if (PaymentGatewayProvider.UseDelayedPayment())
            {
                // Process succesfull authorization of delayed payment
                HandleDelayedPaymentResult(result);
            }
            else
            {
                // Redirect after successful direct payment
                HandleDirectPaymentResult(result);
            }
        }
    }

    #endregion


    #region "Wizard methods"

    /// <summary>
    /// Validates data.
    /// </summary>
    protected override void ValidateStepData(object sender, StepEventArgs e)
    {
        base.ValidateStepData(sender, e);

        if (!StopProcessing && !string.IsNullOrEmpty(mPaymentDataForm?.ValidateData()))
        {
            e.CancelEvent = true;
        }
    }

    #endregion


    #region "Methods"

    protected override void CreateChildControls()
    {
        string orderHash = QueryHelper.GetString("o", string.Empty);
        mOrderId = ValidationHelper.GetInteger(WindowHelper.GetItem(orderHash), 0);

        base.CreateChildControls();

        if (PaymentGatewayProvider != null)
        {
            try
            {
                PaymentGatewayProvider.OrderId = mOrderId;

                // Init paymentDataContainer
                mPaymentDataForm = Service.Resolve<IPaymentGatewayFormFactory>().GetPaymentGatewayForm(PaymentGatewayProvider, this);

                if (mPaymentDataForm != null)
                {
                    pnlPaymentDataContainer.Controls.Clear();
                    pnlPaymentDataContainer.Controls.Add(mPaymentDataForm);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("PaymentForm", "EXCEPTION", ex);
            }
        }
    }


    private void SetupControl()
    {
        // Payment and order summary
        lblTotalPriceValue.Text = CurrencyInfoProvider.GetFormattedPrice(ShoppingCart.GrandTotal, ShoppingCart.Currency);
        lblOrderIdValue.Text = Convert.ToString(mOrderId);

        if (ShoppingCart.PaymentOption != null)
        {
            lblPaymentValue.Text = ResHelper.LocalizeString(ShoppingCart.PaymentOption.PaymentOptionDisplayName, encode: true);
        }

        // Payment form is visible only if payment method is selected
        if (PaymentGatewayProvider == null)
        {
            ShowError(GetString("com.checkout.paymentoptionnotselected"));
        }
    }


    private void ShowError(string text)
    {
        pnlError.Visible = true;
        lblError.Text = text;
    }


    /// <summary>
    /// Handles successfull direct payment.
    /// </summary>
    /// <remarks>
    /// When <see cref="PaymentResultInfo.PaymentIsCompleted"/> is set to true and <see cref="RedirectAfterPurchase"/> is set, redirect to this url is performed.
    /// When <see cref="PaymentResultInfo.PaymentIsCompleted"/> is set to false and <see cref="PaymentResultInfo.PaymentApprovalUrl"/> is set, redirect to this url is performed.
    /// </remarks>
    private void HandleDirectPaymentResult(PaymentResultInfo result)
    {
        if (result == null)
        {
            return;
        }

        if (result.PaymentIsCompleted && !string.IsNullOrEmpty(RedirectAfterPurchase))
        {
            URLHelper.Redirect(UrlResolver.ResolveUrl(RedirectAfterPurchase));
        }
        else if (!result.PaymentIsFailed && !string.IsNullOrEmpty(result.PaymentApprovalUrl))
        {
            URLHelper.Redirect(result.PaymentApprovalUrl);
        }
    }


    /// <summary>
    /// Handles successfull authorization of delayed payment.
    /// </summary>
    /// <remarks>
    /// When <see cref="PaymentResultInfo.PaymentApprovalUrl"/> is set redirect to this url is performed.
    /// </remarks>
    private void HandleDelayedPaymentResult(PaymentResultInfo result)
    {
        if (result == null)
        {
            return;
        }

        if (!result.PaymentIsFailed && !string.IsNullOrEmpty(result.PaymentApprovalUrl))
        {
            URLHelper.Redirect(result.PaymentApprovalUrl);
        }

        if (result.PaymentIsAuthorized && !result.PaymentIsFailed && !string.IsNullOrEmpty(RedirectAfterPurchase))
        {
            URLHelper.Redirect(UrlResolver.ResolveUrl(RedirectAfterPurchase));
        }
    }


    public CMSPaymentGatewayForm LoadFormControl(string path)
    {
        return LoadControl(path) as CMSPaymentGatewayForm;
    }

    #endregion
}