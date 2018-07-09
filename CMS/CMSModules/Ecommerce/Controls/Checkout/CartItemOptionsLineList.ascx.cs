using System;
using System.Collections.Generic;
using System.Linq;

using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;

public partial class CMSModules_Ecommerce_Controls_Checkout_CartItemOptionsLineList : CMSCheckoutWebPart
{
    #region "Variables"

    private ShoppingCartItemInfo mShoppingCartItemInfoObject;
    private string mTextSeparator = " ";

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
        get;
        set;
    }


    /// <summary>
    /// Gets or sets the text separator used to separate options.
    /// </summary>   
    public string TextSeparator
    {
        get
        {
            return mTextSeparator;
        }
        set
        {
            mTextSeparator = value;
        }
    }


    /// <summary>
    /// Gets or sets the options line label CSS class.
    /// </summary>   
    public string OptionsLineCssClass
    {
        get
        {
            return lblOptions.CssClass;
        }
        set
        {
            lblOptions.CssClass = value;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// PreRender event handler.
    /// </summary>    
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        EnsureOptionString();
    }
    #endregion


    #region "Methods"

    /// <summary>
    /// Ensures the options text to display.
    /// </summary>
    protected virtual void EnsureOptionString()
    {
        if (ShoppingCartItemInfoObject != null)
        {
            List<string> optionNames = new List<string>();

            foreach (ShoppingCartItemInfo optionItem in ShoppingCartItemInfoObject.ProductOptions)
            {
                // Ignore bundle items
                if (optionItem.IsBundleItem)
                {
                    continue;
                }

                string itemText = HTMLHelper.HTMLEncode(ResHelper.LocalizeString(optionItem.CartItemText));

                if (!string.IsNullOrEmpty(itemText))
                {
                    itemText = String.Format(" '{0}'", itemText);
                }
                else
                {
                    OptionCategoryInfo category = OptionCategoryInfoProvider.GetOptionCategoryInfo(optionItem.SKU.SKUOptionCategoryID);
                    // Exclude Text options without specified text
                    if ((category != null) && (category.CategoryType == OptionCategoryTypeEnum.Text))
                    {
                        continue;
                    }
                }

                optionNames.Add(HTMLHelper.HTMLEncode(ResHelper.LocalizeString(optionItem.SKU.SKUName)) + itemText);
            }

            lblOptions.Text = TextHelper.Join(TextSeparator, optionNames) ?? String.Empty;
        }
    }

    #endregion
}
