using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;


public partial class CMSModules_Ecommerce_Controls_Checkout_CartItemPriceDetail : CMSCheckoutWebPart
{
    #region "Variables"

    private int mCartItemID;
    private string mImagePath;
    private ShoppingCartItemInfo mShoppingCartItemInfoObject;

    #endregion


    #region "Private properties"

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
                IEnumerable<ShoppingCartItemInfo> ieScii = (ShoppingCart.CartItems.Where(i => i.CartItemID == CartItemID));

                if (ieScii.Any())
                {
                    mShoppingCartItemInfoObject = ieScii.First();
                }
                else
                {
                    // Isn't available anymore in the listing
                    return null;
                }
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
    /// Gets or sets the price detail label CSS class.
    /// </summary>
    public string PriceDetailCssClass
    {
        get
        {
            return lblPriceDetailAction.CssClass;
        }
        set
        {
            lblPriceDetailAction.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the path to image for this control.
    /// Default value: Design/Controls/UniGrid/Actions/detail.png
    /// </summary>
    public string ImagePath
    {
        get
        {
            if (string.IsNullOrEmpty(mImagePath))
            {
                mImagePath = "Design/Controls/UniGrid/Actions/detail.png";
            }

            return mImagePath;
        }
        set
        {
            mImagePath = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // Register product price detail dialog script
        StringBuilder script = new StringBuilder();
        script.Append("function ShowProductPriceDetail(cartItemGuid, sessionName) {");
        script.Append("if (sessionName != \"\"){sessionName =  \"&cart=\" + sessionName;}");
        string detailUrl = IsLiveSite ? "~/CMSModules/Ecommerce/CMSPages/ShoppingCartSKUPriceDetail.aspx" : "~/CMSModules/Ecommerce/Controls/ShoppingCart/ShoppingCartSKUPriceDetail.aspx";
        script.Append(string.Format("modalDialog('{0}?itemguid=' + cartItemGuid + sessionName, 'ProductPriceDetail', 750, 560); }}", ResolveUrl(detailUrl)));
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "ProductPriceDetail", ScriptHelper.GetScript(script.ToString()));
    }


    /// <summary>
    /// Returns price detail link.
    /// </summary>
    protected string GetPriceDetailLink(object value)
    {
        if (ShoppingCartItemInfoObject != null)
        {
            return string.Format("<img src=\"{0}\" onclick=\"javascript: ShowProductPriceDetail('{1}', '{2}')\" alt=\"{3}\" class=\"ProductPriceDetailImage\" style=\"cursor:pointer;\" />",
                GetImageUrl(ImagePath),
                ShoppingCartItemInfoObject.CartItemGUID, "",
                GetString("shoppingcart.productpricedetail")
                );
        }

        return "";
    }
}
