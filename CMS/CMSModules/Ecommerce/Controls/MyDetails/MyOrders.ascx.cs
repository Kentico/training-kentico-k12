using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Ecommerce_Controls_MyDetails_MyOrders : CMSAdminControl
{
    #region "Variables"

    private bool downloadLinksColumnVisible = false;

    #endregion


    #region "Properties"

    /// <summary>
    /// Customer ID.
    /// </summary>
    public int CustomerId
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if order tracking number should be displayed.
    /// </summary>
    public bool ShowOrderTrackingNumber
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if products in order should be added to shopping cart.
    /// </summary>
    public bool ShowOrderToShoppingCart
    {
        get;
        set;
    }


    /// <summary>
    /// If true, control does not process the data.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["StopProcessing"], false);
        }
        set
        {
            ViewState["StopProcessing"] = value;
        }
    }

    #endregion


    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing)
        {
            if (AuthenticationHelper.IsAuthenticated())
            {
                gridOrders.IsLiveSite = IsLiveSite;
                gridOrders.OnExternalDataBound += gridOrders_OnExternalDataBound;
                gridOrders.WhereCondition = "(OrderCustomerID = " + CustomerId + ") AND (OrderSiteID = " + SiteContext.CurrentSiteID + ")";

                // Set pager links text on live site
                if (IsLiveSite)
                {
                    gridOrders.Pager.FirstPageText = "&lt;&lt;";
                    gridOrders.Pager.LastPageText = "&gt;&gt;";
                    gridOrders.Pager.PreviousPageText = "&lt;";
                    gridOrders.Pager.NextPageText = "&gt;";
                    gridOrders.Pager.PreviousGroupText = "...";
                    gridOrders.Pager.NextGroupText = "...";
                }
            }
            else
            {
                // Hide if user is not authenticated
                Visible = false;
            }
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Set visibility of order tracking number column
        gridOrders.NamedColumns["OrderTrackingNumber"].Visible = ShowOrderTrackingNumber;

        // Set visibility of download links column
        gridOrders.NamedColumns["downloads"].Visible = downloadLinksColumnVisible;

        // Set visibility of order to shopping cart column
        gridOrders.NamedColumns["OrderToShoppingCart"].Visible = ShowOrderToShoppingCart;

        ScriptHelper.RegisterDialogScript(Page);
    }


    protected object gridOrders_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerInvariant())
        {
            case "grandtotal":
                DataRowView dr = (DataRowView)parameter;

                int currencyId = ValidationHelper.GetInteger(dr["OrderCurrencyID"], 0);
                var currency = CurrencyInfoProvider.GetCurrencyInfo(currencyId);

                // If order is not in main currency, show order price
                if (currency != null)
                {
                    var orderTotalPrice = ValidationHelper.GetDecimal(dr["OrderGrandTotal"], 0);
                    var priceFormatted = currency.FormatPrice(orderTotalPrice);

                    // Formatted currency
                    return HTMLHelper.HTMLEncode(priceFormatted);
                }

                return string.Empty;

            case "invoice":
                return "<a target=\"_blank\" href=\"" + UrlResolver.ResolveUrl("~/CMSModules/Ecommerce/CMSPages/GetInvoice.aspx?orderid=" + ValidationHelper.GetInteger(parameter, 0)) + "\">" + GetString("general.view") + "</a>";

            case "downloads":
                int orderId = ValidationHelper.GetInteger(parameter, 0);

                // Get order item SKU files for the order
                DataSet orderItemSkuFiles = OrderItemSKUFileInfoProvider.GetOrderItemSKUFiles(orderId);

                // If there are some downloads available for the order
                if (!DataHelper.DataSourceIsEmpty(orderItemSkuFiles))
                {
                    // Make download links column visible
                    downloadLinksColumnVisible = true;

                    // Show view action for this record
                    string url = UrlResolver.ResolveUrl("~/CMSModules/Ecommerce/CMSPages/EProducts.aspx?orderid=" + orderId);
                    return String.Format("<a href=\"#\" onclick=\"{0} \">{1}</a>",
                        ScriptHelper.GetModalDialogScript(url, "DownloadLinks", 700, 600),
                        GetString("general.view"));
                }

                return String.Empty;

            case "ordertoshoppingcart":
                int id = ValidationHelper.GetInteger(parameter, 0);

                LinkButton addToCartButton = new LinkButton
                {
                    OnClientClick = "return confirm(" + ScriptHelper.GetLocalizedString("myorders.addtocart") + ");",
                    Text = GetString("myorders.reorder")
                };

                addToCartButton.Click += (s, e) => AddToCart(id);

                return addToCartButton;

        }
        return parameter;
    }


    /// <summary>
    /// Adds content of order to current shopping cart.
    /// </summary>
    private void AddToCart(int orderId)
    {
        if (!ShowOrderToShoppingCart)
        {
            return;
        }

        // Get order
        OrderInfo order = OrderInfoProvider.GetOrderInfo(orderId);

        if (order != null)
        {
            CustomerInfo customer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);

            if (customer.CustomerUserID == CurrentUser.UserID)
            {
                // Get current shopping cart
                ShoppingCartInfo cart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();
                // Set new cart
                if (cart.ShoppingCartID == 0)
                {
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                }

                string cartUrl = ECommerceSettings.ShoppingCartURL(CurrentSite.SiteName);

                // Update shopping cart by items from order
                if (!ShoppingCartInfoProvider.UpdateShoppingCartFromOrder(cart, orderId))
                {
                    cartUrl = URLHelper.AddParameterToUrl(cartUrl, "notallreordered", "1");
                }

                // Update shopping cart items in database
                foreach (ShoppingCartItemInfo item in cart.CartItems)
                {
                    ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(item);
                }

                // Redirect to shopping cart page
                URLHelper.ResponseRedirect(cartUrl);
            }
        }
    }


    /// <summary>
    /// Overridden SetValue - because of MyAccount webpart.
    /// </summary>
    /// <param name="propertyName">Name of the property to set</param>
    /// <param name="value">Value to set</param>
    public override bool SetValue(string propertyName, object value)
    {
        base.SetValue(propertyName, value);

        if (!String.IsNullOrEmpty(propertyName))
        {
            switch (propertyName.ToLowerInvariant())
            {
                case "customerid":
                    CustomerId = ValidationHelper.GetInteger(value, 0);
                    break;

                case "showordertrackingnumber":
                    ShowOrderTrackingNumber = ValidationHelper.GetBoolean(value, false);
                    break;

                case "showordertoshoppingcart":
                    ShowOrderToShoppingCart = ValidationHelper.GetBoolean(value, false);
                    break;
            }
        }

        return true;
    }
}
