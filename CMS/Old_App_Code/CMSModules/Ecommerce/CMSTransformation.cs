using System.Text;

using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.Search;

namespace CMS.DocumentEngine.Web.UI
{
    /// <summary>
    /// SKU transformation methods
    /// </summary>
    public partial class CMSTransformation
    {
        #region "Properties and variables"

        private SKUInfo mSKU;


        /// <summary>
        /// SKU data loaded from the current data row
        /// </summary>
        public SKUInfo SKU
        {
            get
            {
                if (mSKU == null)
                {
                    var cartItem = DataItem as ShoppingCartItemInfo;

                    // Try to get the SKU object from the ShoppingCartItemInfo object first, if available
                    if (cartItem != null)
                    {
                        mSKU = cartItem.SKU;
                    }
                    else if (DataItem is SearchResultItem)
                    {
                        var resultItem = (SearchResultItem)DataItem;

                        mSKU = new SKUInfo(resultItem.ResultData);
                    }
                    else
                    {
                        // Get the SKU object
                        var row = DataRowView?.Row;
                        if (row != null)
                        {
                            mSKU = new SKUInfo(row);
                        }
                    }
                }

                return mSKU;
            }
        }

        #endregion


        #region "Prices"

        /// <summary>
        /// Returns SKU catalog price based on the SKU data and the data of the current shopping cart.
        /// Returned value is rounded to a number of decimal places specified by the cart's currency setting.
        /// Taxes are included based on the site settings.
        /// </summary>
        public decimal GetSKUPrice()
        {
            return EcommerceTransformationFunctions.GetSKUPrice(SKU);
        }


        /// <summary>
        /// Returns formatted product catalog price based on the SKU data and the data of the current shopping cart.
        /// Returned value is rounded to a number of decimal places specified by the cart's currency setting.
        /// Taxes are included based on the site settings.
        /// </summary>
        public string GetSKUFormattedPrice()
        {
            var price = GetSKUPrice();

            return GetFormattedPrice(price);
        }


        /// <summary>
        /// Returns SKU list price based on the SKU data and the data of the current shopping cart.
        /// Returned value is rounded to a number of decimal places specified by the cart's currency setting.
        /// </summary>
        public decimal GetSKUListPrice()
        {
            return EcommerceTransformationFunctions.GetSKUListPrice(SKU);
        }


        /// <summary>
        /// Returns formatted SKU list price based on the SKU data and the data of the current shopping cart.
        /// Returned value is rounded to a number of decimal places specified by the cart's currency setting.
        /// Catalog discounts are not included, taxes are included based on the site settings.
        /// </summary>
        public string GetSKUFormattedListPrice()
        {
            var listPrice = GetSKUListPrice();

            return GetFormattedPrice(listPrice);
        }


        /// <summary>
        /// Returns SKURetailPrice if defined, otherwise returns price before discounts.
        /// Returned value is rounded to a number of decimal places specified by the cart's currency setting.
        /// Returns zero if price saving is zero.
        /// </summary>
        public decimal GetSKUOriginalPrice()
        {
            return EcommerceTransformationFunctions.GetSKUOriginalPrice(SKU);
        }


        /// <summary>
        /// Returns formatted SKURetailPrice if defined, otherwise returns price before discounts.
        /// Returned value is rounded to a number of decimal places specified by the cart's currency setting.
        /// Returns zero if price saving is zero.
        /// </summary>
        public string GetSKUFormattedOriginalPrice()
        {
            var originalPrice = GetSKUOriginalPrice();

            return GetFormattedPrice(originalPrice);
        }


        /// <summary>
        /// Returns amount of saved money based on the difference between product selling price and product list price or price before discounts.
        /// Catalog discounts and/or taxes are included based on the site settings.
        /// </summary>
        /// <param name="percentage">True - result is percentage, False - result is in the current currency</param>
        public decimal GetSKUPriceSaving(bool percentage = false)
        {
            return EcommerceTransformationFunctions.GetSKUPriceSaving(SKU, percentage);
        }


        /// <summary>
        /// Returns formatted string representing amount of saved money based on the difference between product selling price and product list price or price before discounts.
        /// Catalog discounts and/or taxes are included based on the site settings.
        /// </summary>
        public string GetSKUFormattedPriceSaving()
        {
            var saving = GetSKUPriceSaving();

            return GetFormattedPrice(saving);
        }


        /// <summary>
        /// Returns the price formatted according to the properties of the current shopping cart's currency. Rounding is based on the settings of the shopping cart's site.
        /// </summary>
        /// <param name="price">Price to be formatted</param>
        /// <param name="round">True - price is rounded before formatting, according to the settings of the cart's site</param>
        public string FormatPrice(decimal price, bool round = true)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (round)
            {
                var rounder = Service.Resolve<IRoundingServiceFactory>().GetRoundingService(cart.ShoppingCartSiteID);
                price = rounder.Round(price, cart.Currency);
            }

            return CurrencyInfoProvider.GetFormattedPrice(price, cart.Currency);
        }


        /// <summary>
        /// Returns product tax.
        /// </summary>
        public decimal GetSKUTax()
        {
            return EcommerceTransformationFunctions.GetSKUTax(SKU);
        }


        /// <summary>
        /// Returns formatted product tax according to the current currency properties.
        /// </summary>
        public string GetFormattedSKUTax()
        {
            var tax = GetSKUTax();
            return GetFormattedPrice(tax);
        }


        private static string GetFormattedPrice(decimal price)
        {
            return CurrencyInfoProvider.GetFormattedPrice(price, ECommerceContext.CurrentShoppingCart?.Currency);
        }

        #endregion


        #region "SKU properties"

        /// <summary>
        /// Returns value of the specified product public status column.
        /// If the product is evaluated as a new product in the store, public status set by 'CMSStoreNewProductStatus' setting is used, otherwise product public status is used.
        /// </summary>
        /// <param name="column">Name of the product public status column the value should be retrieved from</param>
        public object GetSKUIndicatorProperty(string column)
        {
            return EcommerceTransformationFunctions.GetSKUIndicatorProperty(SKU, column);
        }


        /// <summary>
        /// Indicates if the given SKU can be bought by the customer based on the SKU inventory properties.
        /// </summary>
        public bool IsSKUAvailableForSale()
        {
            return SKUInfoProvider.IsSKUAvailableForSale(SKU);
        }


        /// <summary>
        /// Indicates the real stock status of SKU based on SKU items available.
        /// </summary>
        public bool IsSKUInStock()
        {
            return SKUInfoProvider.IsSKUInStock(SKU);
        }


        /// <summary>
        /// Gets the SKU node alias. If there are multiple nodes for this SKU the first occurrence is returned.
        /// If there is not a single one node for this SKU, empty string is returned.
        /// </summary>
        public string GetSKUNodeAlias()
        {
            return EcommerceTransformationFunctions.GetSKUNodeAlias(SKU);
        }

        #endregion


        #region "SKU URLs"

        /// <summary>
        /// Returns SKU permanent URL.
        /// </summary>
        public string GetSKUUrl()
        {
            return EcommerceTransformationFunctions.GetProductUrlByID(SKU.SKUID, ResHelper.LocalizeString(SKU.SKUName));
        }


        /// <summary>
        /// Returns SKU image URL including dimension's modifiers (width, height) and site name parameter if product is from different site than current.
        /// If image URL is not specified, SKU default image URL is used.
        /// </summary>
        /// <param name="width">Image requested width</param>
        /// <param name="height">Image requested height</param>
        public string GetSKUImageUrl(int width, int height)
        {
            return EcommerceTransformationFunctions.GetSKUImageUrl(SKU.SKUImagePath, width, height, 0, SKU.SKUSiteID);
        }


        /// <summary>
        /// Returns SKU image URL including dimension's modifiers (width, height) and site name parameter if product is from different site than current.
        /// If image URL is not specified, SKU default image URL is used.
        /// </summary>
        /// <param name="maxSideSize">Image requested maximum side size</param>
        public string GetSKUImageUrl(int maxSideSize)
        {
            return EcommerceTransformationFunctions.GetSKUImageUrl(SKU.SKUImagePath, 0, 0, maxSideSize, SKU.SKUSiteID);
        }


        /// <summary>
        /// Returns permanent URL of the specified product. Does not uses hash tables.
        /// </summary>
        /// <param name="skuGUID">SKU Guid</param>
        /// <param name="skuName">SKU name</param>
        /// <param name="siteName">Site name</param>
        public string GetProductUrl(object skuGUID, object skuName, object siteName)
        {
            return EcommerceTransformationFunctions.GetProductUrl(skuGUID, skuName, siteName);
        }


        /// <summary>
        /// Returns permanent URL of the specified product. Uses hash tables.
        /// </summary>
        /// <param name="skuID">SKU ID</param>
        /// <param name="skuName">SKU name</param>
        /// <param name="siteName">Site name</param>
        public string GetProductUrlByID(object skuID, object skuName, object siteName)
        {
            return EcommerceTransformationFunctions.GetProductUrlByID(skuID, skuName, siteName);
        }


        /// <summary>
        /// Returns URL of the specified product with feed parameter.
        /// </summary>
        /// <param name="skuGUID">SKU GUID</param>
        /// <param name="skuName">SKU name</param>
        /// <param name="siteName">Site name</param>
        public string GetProductUrlForFeed(object skuGUID, object skuName, object siteName)
        {
            return EcommerceTransformationFunctions.GetProductUrlForFeed(GetFeedName(), skuGUID, skuName, siteName);
        }

        #endregion


        /// <summary>
        /// Generates Google Tag Manager product object in JSON format for <see cref="SKU"/>.
        /// </summary>
        /// <param name="additionalData">Data with additional non-conflicting key value pairs to be merged with <see cref="SKU"/>.</param>
        /// <param name="purpose">Contextual information fitting for customizations.</param>
        /// <seealso cref="GtmProductHelper.MapSKU(SKUInfo, object, string)"/>
        /// <seealso cref="GtmDataHelper.SerializeToJson(GtmData, string)"/>
        public string GetGtmProductJson(object additionalData = null, string purpose = null)
        {
            return EcommerceTransformationFunctions.GetGtmProductJson(SKU, additionalData, purpose);
        }


        /// <summary>
        /// Returns names and values of discounts for the current cart item surrounded with li tag.
        /// </summary>
        public string GetMultiBuyDiscountNames()
        {
            var cartItem = DataItem as ShoppingCartItemInfo;

            var summary = cartItem?.DiscountSummary;
            if (summary != null)
            {
                // Join discount names and values as list items
                var sb = new StringBuilder();
                foreach (var discount in summary)
                {
                    sb.Append($"<li>{HTMLHelper.HTMLEncode(ResHelper.LocalizeString(discount.Name))}");
                    sb.Append($" <strong>({HTMLHelper.HTMLEncode(GetFormattedPrice(discount.Value))})</strong></li>");
                }

                return sb.ToString();
            }

            return string.Empty;
        }
    }
}
