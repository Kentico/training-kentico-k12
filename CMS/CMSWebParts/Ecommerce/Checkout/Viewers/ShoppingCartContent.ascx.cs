using System;
using System.Linq;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DocumentEngine.Web.UI;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine;


/// <summary>
/// Cart content web part
/// </summary>
public partial class CMSWebParts_Ecommerce_Checkout_Viewers_ShoppingCartContent : CMSCheckoutWebPart
{
    #region "Variables"

    private bool orderChanged;
    private string cartGuidQueryString;

    #endregion


    #region "Constants"

    private const string EMPTY_CART_REDIRECTED = "EMPTY_CART_REDIRECTED";
    private const string GUID_QUERY_PARAMETER = "cg";

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets the name of the hierarchical transformation which is used for displaying the results.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), "");
        }
        set
        {
            SetValue("TransformationName", value);
        }
    }


    private string CartGuidQueryString
    {
        get
        {
            return cartGuidQueryString ?? (cartGuidQueryString = QueryHelper.GetString(GUID_QUERY_PARAMETER, String.Empty));
        }
    }

    #endregion


    #region "Life cycle"

    /// <summary>
    /// OnInit event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Subscribe to the checkout events
        SubscribeToCheckoutEvents();

        if (ShouldLoadAbandonedCart())
        {
            TryReplaceCartWithNewOneUsingGuidInUrl();
            RedirectToCartWithoutGuidInUrl();
        }
    }


    /// <summary>
    /// Load event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        SetupControl();

        // Clear Empty cart redirection flag
        SessionHelper.SetValue(EMPTY_CART_REDIRECTED, false);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ScriptHelper.RegisterDialogScript(Page);
        ControlsHelper.UpdateCurrentPanel(this);
    }

    #endregion


    #region "Event handling"

    /// <summary>
    /// Updates the web part according to the new the new shopping cart values.
    /// </summary>
    private void Update(object sender, EventArgs e)
    {
        orderChanged = true;

        var wasRedirectedOnce = ValidationHelper.GetBoolean(SessionHelper.GetValue(EMPTY_CART_REDIRECTED), false);

        // One full page reload for empty cart
        if (ShoppingCart.IsEmpty && !wasRedirectedOnce)
        {
            SessionHelper.SetValue(EMPTY_CART_REDIRECTED, true);
            URLHelper.Redirect(RequestContext.CurrentURL);
        }

        SetupControl();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Validates the data.
    /// </summary>
    protected override void ValidateStepData(object sender, StepEventArgs e)
    {
        base.ValidateStepData(sender, e);

        if (!CheckShoppingCart())
        {
            e.CancelEvent = true;
        }
    }


    /// <summary>
    /// Subscribes the web part to the Checkout events.
    /// </summary>
    private void SubscribeToCheckoutEvents()
    {
        ComponentEvents.RequestEvents.RegisterForEvent(SHOPPING_CART_CHANGED, Update);
    }


    /// <summary>
    /// Performs an inventory check for all items in the shopping cart and show message if required.
    /// </summary>
    /// <returns>True if all items are available in sufficient quantities, otherwise false</returns>
    private bool CheckShoppingCart()
    {
        // Remove error message before reevaluation
        lblError.Text = string.Empty;

        if (ShoppingCart.IsEmpty)
        {
            lblError.Visible = true;
            lblError.Text = ResHelper.GetString("com.checkout.cartisempty");
            return false;
        }

        var validationErrors = ShoppingCartInfoProvider.ValidateShoppingCart(ShoppingCart);

        // Try to show message through Message Panel web part
        if (validationErrors.Any() || orderChanged)
        {
            CMSEventArgs<string> args = new CMSEventArgs<string>();
            args.Parameter = validationErrors
                    .Select(e => HTMLHelper.HTMLEncode(e.GetMessage()))
                    .Join("<br />");

            ComponentEvents.RequestEvents.RaiseEvent(this, args, MESSAGE_RAISED);

            // If Message Panel web part is not present (Parameter is cleared by web part after successful handling), show error message in error label
            if (!string.IsNullOrEmpty(args.Parameter) && validationErrors.Any())
            {
                lblError.Visible = true;
                lblError.Text = validationErrors
                    .Select(e => HTMLHelper.HTMLEncode(e.GetMessage()))
                    .Join("<br />");
            }
        }
        return !validationErrors.Any();
    }


    /// <summary>
    /// Sets up the web part.
    /// </summary>
    public void SetupControl()
    {
        if (!StopProcessing)
        {
            if (!ShoppingCart.IsEmpty)
            {
                shoppingCartUniView.DataSource = new GroupedEnumerable<ShoppingCartItemInfo>(ShoppingCart.CartItems, GetKey, GetLevel);
                LoadTransformations();
                CheckShoppingCart();
            }
            else
            {
                // Display empty shopping cart message
                lblShoppingCartEmpty.Text = GetString("commercecart.emptycart");
                lblShoppingCartEmpty.Visible = true;

                // Update the UniView
                shoppingCartUniView.DataSource = null;
                shoppingCartUniView.ReBind();
            }
        }
    }


    /// <summary>
    /// Function for building the GroupedEnumerable. Returns the level on which the item should be placed in the object.
    /// </summary>
    protected int GetLevel(ShoppingCartItemInfo Item)
    {
        return (Item.IsProductOption || Item.IsBundleItem) ? 1 : 0;
    }


    /// <summary>
    /// Function for building the GroupedEnumerable. Returns the key according to which the hierarchy is build. If it is a top level item it returns 0.
    /// </summary>
    protected object GetKey(ShoppingCartItemInfo Item)
    {
        if (!(Item.IsProductOption || Item.IsBundleItem))
        {
            return 0;
        }

        return Item.CartItemParentGUID;
    }


    /// <summary>
    /// Load transformations with dependence.
    /// </summary>
    protected void LoadTransformations()
    {
        if (!String.IsNullOrEmpty(TransformationName))
        {
            TransformationInfo ti = TransformationInfoProvider.GetTransformation(TransformationName);

            if (ti != null)
            {
                // Setting up the common BasicUniView properties
                shoppingCartUniView.HierarchicalDisplayMode = HierarchicalDisplayModeEnum.Inner;
                shoppingCartUniView.RelationColumnID = "CartItemGuid";

                if (ti.TransformationIsHierarchical)
                {
                    // Setting up the hierarchical transformation.
                    HierarchicalTransformations ht = new HierarchicalTransformations("CartItemGUID");
                    ht.LoadFromXML(ti.TransformationHierarchicalXMLDocument);

                    // Setting up the BasicUniView
                    shoppingCartUniView.Transformations = ht;
                    shoppingCartUniView.UseNearestItemForHeaderAndFooter = true;
                }
                else
                {
                    // Setting up the BasicUniView with a non-hierarchical transformation
                    shoppingCartUniView.ItemTemplate = TransformationHelper.LoadTransformation(shoppingCartUniView, ti.TransformationFullName);
                }

                // Makes sure new data is loaded if the date changes and transformation needs to be reloaded
                shoppingCartUniView.DataBind();
            }
        }
    }


    private void TryReplaceCartWithNewOneUsingGuidInUrl()
    {
        try
        {
            // Try to restore abandoned cart from DB and set its clone as CurrentShoppingCart
            Guid cartGuid = Guid.Parse(CartGuidQueryString);
            ShoppingCartInfo abandonedCart = ShoppingCartInfoProvider.GetShoppingCartInfo(cartGuid);

            if ((abandonedCart != null) && (Service.Resolve<IShoppingService>().GetCurrentShoppingCart().ShoppingCartGUID != abandonedCart.ShoppingCartGUID))
            {
                Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(CloneShoppingCartInfo(abandonedCart));
            }
        }
        catch (FormatException)
        {
            EventLogProvider.LogException("Load abandoned cart", "LOADABANDONEDCART", null, CurrentSite.SiteID, "Invalid format of GUID");
        }
    }


    private void RedirectToCartWithoutGuidInUrl()
    {
        string currentUrlWithoutGuid = URLHelper.RemoveParameterFromUrl(RequestContext.CurrentURL, GUID_QUERY_PARAMETER);
        URLHelper.Redirect(currentUrlWithoutGuid);
    }


    private bool ShouldLoadAbandonedCart()
    {
        return !String.IsNullOrEmpty(CartGuidQueryString);
    }


    /// <summary>
    /// Creates a new shopping cart record and corresponding shopping cart items records in the database
    /// as a copy of given shopping cart info.
    /// Currency is set according to the currency of the given cart.
    /// If a configuration of cart item is no longer available it is not cloned to new shopping cart.
    /// <see cref="AbstractUserControl.CurrentUser"/> is set as the new cart owner (only non-public user is set).
    /// </summary>
    /// <param name="originalCart">Original cart to clone.</param>
    /// <returns>Created shopping cart info.</returns>
    private ShoppingCartInfo CloneShoppingCartInfo(ShoppingCartInfo originalCart)
    {
        using (new CMSActionContext { UpdateTimeStamp = false })
        {
            var cartClone = ShoppingCartFactory.CreateCart(CurrentSite.SiteID, CurrentUser.IsPublic() ? null : CurrentUser);

            cartClone.ShoppingCartCurrencyID = originalCart.ShoppingCartCurrencyID;
            cartClone.ShoppingCartLastUpdate = originalCart.ShoppingCartLastUpdate;
            ShoppingCartInfoProvider.SetShoppingCartInfo(cartClone);

            ShoppingCartInfoProvider.CopyShoppingCartItems(originalCart, cartClone);

            // Set the shopping cart items into the database
            cartClone.CartItems.ForEach(i => i.Generalized.SetObject());

            return cartClone;
        }
    }

    #endregion
}
