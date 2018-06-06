using System;

using CMS.Core;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSTemplates_CorporateSite_Root : TemplateMasterPage
{
    protected override void CreateChildControls()
    {
        base.CreateChildControls();

        PageManager = manPortal;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ltlTags.Text = HeaderTags;
        SetupShoppingCartPreview();
    }


    private void SetupShoppingCartPreview()
    {
        lnkShoppingCart.NavigateUrl = URLHelper.ResolveUrl(ECommerceSettings.ShoppingCartURL(SiteContext.CurrentSiteID));
        lnkMyAccount.NavigateUrl = URLHelper.ResolveUrl(SettingsKeyInfoProvider.GetValue("CMSMyAccountURL", SiteContext.CurrentSiteID));
        lnkMyWishList.NavigateUrl = URLHelper.ResolveUrl(ECommerceSettings.WishListURL(SiteContext.CurrentSiteID));

        var cart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();

        lblPrice.Text = CurrencyInfoProvider.GetFormattedPrice(cart.GrandTotal, cart.Currency);
    }
}