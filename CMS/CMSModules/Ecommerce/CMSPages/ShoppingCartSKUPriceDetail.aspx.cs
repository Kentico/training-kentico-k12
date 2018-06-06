using System;

using CMS.Core;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.UIControls;


[Title("ProductPriceDetail.Title")]
public partial class CMSModules_Ecommerce_CMSPages_ShoppingCartSKUPriceDetail : CMSLiveModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Initialize product price detail
        ucSKUPriceDetail.CartItemGuid = QueryHelper.GetGuid("itemguid", Guid.Empty);
        ucSKUPriceDetail.ShoppingCart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();

        btnClose.Text = GetString("General.Close");
        btnClose.OnClientClick = "Close(); return false;";
    }
}