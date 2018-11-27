using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSModules_Ecommerce_Controls_ProductOptions_ShoppingCartItemSelector : CMSUserControl
{
    #region "Variables"

    // String variable
    private string mImageFolder;
    private string mShoppingCartUrl;
    private string mWishlistUrl;

    // Bool variables
    private bool? mSKUHasOptions;
    private bool mDataLoaded;
    private bool? mRedirectToShoppingCart;
    private bool mOneCategoryUsed;

    private int mSKUID;

    private ShoppingCartInfo mShoppingCart;
    private SKUInfo mSKU;

    private Hashtable mProductOptions;

    private List<ProductVariant> mVariants;
    private List<Tuple<int, int>> mSelectedOptionsInCategories;
    private Dictionary<int, int> mOptionsInCategories;
    private List<ProductOptionSelector> mRelatedSelectors;

    #endregion


    #region "Properties"

    /// <summary>
    /// Product ID (SKU ID).
    /// </summary>
    public int SKUID
    {
        get
        {
            return mSKUID;
        }
        set
        {
            mSKUID = value;

            // Invalidate SKU data
            mSKU = null;
            mSKUHasOptions = null;
        }
    }


    /// <summary>
    /// Product SKU data
    /// </summary>
    public SKUInfo SKU
    {
        get
        {
            return mSKU ?? (mSKU = SKUInfoProvider.GetSKUInfo(SKUID));
        }
    }


    /// <summary>
    /// Indicates if the product has product options
    /// </summary>
    public bool SKUHasOptions
    {
        get
        {
            if (!mSKUHasOptions.HasValue)
            {
                if (IsLiveSite)
                {
                    // Get cache minutes
                    int cacheMinutes = SettingsKeyInfoProvider.GetIntValue(SiteContext.CurrentSiteName + ".CMSCacheMinutes");

                    // Try to get data from cache
                    using (var cs = new CachedSection<bool?>(ref mSKUHasOptions, cacheMinutes, true, null, "skuhasoptions", SKUID))
                    {
                        if (cs.LoadData)
                        {
                            // Get the data
                            mSKUHasOptions = SKUInfoProvider.HasSKUEnabledOptions(SKUID);

                            // Save to the cache
                            if (cs.Cached)
                            {
                                // Get dependencies
                                List<string> dependencies = new List<string>();
                                dependencies.Add("ecommerce.sku|byid|" + SKUID);
                                dependencies.Add("ecommerce.sku|all");
                                dependencies.Add("ecommerce.optioncategory|all");

                                // Set dependencies
                                cs.CacheDependency = CacheHelper.GetCacheDependency(dependencies);
                            }

                            cs.Data = mSKUHasOptions;
                        }
                    }
                }
                else
                {
                    // Get the data
                    mSKUHasOptions = SKUInfoProvider.HasSKUEnabledOptions(SKUID);
                }
            }

            return mSKUHasOptions.Value;
        }
    }


    /// <summary>
    /// Indicates whether current product (SKU) is enabled, TRUE - button/link for adding product to the shopping cart is rendered, otherwise it is not rendered.
    /// </summary>
    public bool SKUEnabled
    {
        get;
        set;
    } = true;


    /// <summary>
    /// Indicates whether stock info is visible, default is false
    /// </summary>
    public bool StockVisible
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates whether unavailable variant info message is used or it is always hidden, default is false
    /// </summary>
    public bool UnavailableVariantInfoEnabled
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates whether control is used in product detail, default is false
    /// </summary>
    public bool UsedInProductDetail
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets the CSS class to display options in selectors faded.
    /// </summary>
    public string CssClassFade
    {
        get;
        set;
    } = "text-muted";


    /// <summary>
    /// Gets or sets the CSS class to display options in selectors normally.
    /// </summary>
    public string CssClassNormal
    {
        get;
        set;
    } = "normal";


    /// <summary>
    /// File name of the image which is used as a source for image button to add product to the shopping cart, default image folder is '~/App_Themes/(Skin_Folder)/Images/ShoppingCart/'.
    /// </summary>
    public string AddToCartImageButton
    {
        get;
        set;
    }


    /// <summary>
    /// Simple string or localizable string of the link to add product to the shopping cart.
    /// </summary>
    public string AddToCartLinkText
    {
        get;
        set;
    }


    /// <summary>
    /// Simple string or localizable string of Quantity label.
    /// </summary>
    public string QuantityText
    {
        get;
        set;
    } = "ecommerce.shoppingcartcontent.skuunits";


    /// <summary>
    /// Simple string or localizable string of add to shopping cart link or button.
    /// </summary>
    public string AddToCartText
    {
        get;
        set;
    } = "shoppingcart.addtoshoppingcart";


    /// <summary>
    /// Simple string or localizable string of add to shopping cart link or button tooltip.
    /// </summary>
    public string AddToCartTooltip
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if textbox for entering number of units to add to the shopping cart should be displayed, if it is hidden number of units is equal to 1.
    /// </summary>
    public bool ShowUnitsTextBox
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if product options of the current product should be displayed.
    /// </summary>
    public bool ShowProductOptions
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if "Add to Wishlist" link or image should be displayed. By default is true.
    /// </summary>
    public bool ShowWishlistLink
    {
        get;
        set;
    } = true;


    /// <summary>
    /// Default quantity when adding product to the shopping cart.
    /// </summary>
    public int DefaultQuantity
    {
        get;
        set;
    } = 1;


    /// <summary>
    /// True - total price is shown when product has product options.
    /// </summary>
    public bool ShowTotalPrice
    {
        get;
        set;
    }


    /// <summary>
    /// True - total price is shown always, no matter if product has product options
    /// </summary>
    public bool AlwaysShowTotalPrice
    {
        get;
        set;
    }


    /// <summary>
    /// Text which is displayed next to the formatted total price. If empty, default text is used.
    /// </summary>
    public string TotalPriceLabel
    {
        get;
        set;
    }


    /// <summary>
    /// Quantity of the specified product to add to the shopping cart.
    /// </summary>
    public int Quantity
    {
        get
        {
            if (ShowUnitsTextBox)
            {
                return ValidationHelper.GetInteger(txtUnits.Text.Trim(), DefaultQuantity);
            }

            return DefaultQuantity;
        }
    }


    /// <summary>
    /// Product options selected by inner selectors (string of SKUIDs separated by the comma).
    /// </summary>
    public string ProductOptions
    {
        get
        {
            var options = new StringBuilder();

            // Get product options from selectors
            foreach (var selector in OptionSelectors)
            {
                options.AppendFormat("{0},", selector.GetSelectedSKUOptions());
            }

            return options.ToString().TrimEnd(',');
        }
    }


    /// <summary>
    /// List containing product options selected by inner selectors.
    /// </summary>
    public List<ShoppingCartItemParameters> ProductOptionsParameters
    {
        get
        {
            var options = new List<ShoppingCartItemParameters>();

            // Get product options from selectors
            foreach (var selector in OptionSelectors)
            {
                options.AddRange(selector.GetSelectedOptionsParameters());
            }

            return options;
        }
    }


    /// <summary>
    /// List containing product options selected by inner selectors, which
    /// are not used in variants
    /// </summary>
    public List<ShoppingCartItemParameters> ProductOptionsParametersNonVariant
    {
        get
        {
            var options = new List<ShoppingCartItemParameters>();
            // Categories used in variants
            IEnumerable<int> usedCategoriesInVariants = new List<int>();

            if (Variants.Count != 0)
            {
                usedCategoriesInVariants = Variants.First().ProductAttributes.CategoryIDs;
            }

            // Get product options from selectors
            foreach (var selector in OptionSelectors)
            {
                // Get option category of this selector
                var category = selector.OptionCategory;
                // Ignore variant categories
                if (!usedCategoriesInVariants.Contains(category.CategoryID))
                {
                    options.AddRange(selector.GetSelectedOptionsParameters());
                }
            }

            return options;
        }
    }


    /// <summary>
    /// Image folder, default image folder is '~/App_Themes/(Skin_Folder)/Images/ShoppingCart/'.
    /// </summary>
    public string ImageFolder
    {
        get
        {
            return mImageFolder ?? (mImageFolder = GetImageUrl("ShoppingCart/"));
        }
        set
        {
            mImageFolder = value;
        }
    }


    /// <summary>
    /// Shopping cart url. By default Shopping cart url from settings is returned.
    /// </summary>
    public string ShoppingCartUrl
    {
        get
        {
            return mShoppingCartUrl ?? (mShoppingCartUrl = ECommerceSettings.ShoppingCartURL(SiteContext.CurrentSiteName));
        }
        set
        {
            mShoppingCartUrl = value;
        }
    }


    /// <summary>
    /// Indicates if user has to be redirected to shopping cart after adding an item to cart. Default value is taken from
    /// Ecommerce settings (key name "CMSStoreRedirectToShoppingCart").
    /// </summary>
    public bool RedirectToShoppingCart
    {
        get
        {
            if (!mRedirectToShoppingCart.HasValue)
            {
                mRedirectToShoppingCart = ECommerceSettings.RedirectToShoppingCart(SiteContext.CurrentSiteName);
            }

            return mRedirectToShoppingCart.Value;
        }
        set
        {
            mRedirectToShoppingCart = value;
        }
    }


    /// <summary>
    /// Wishlist url. By default Wishlist url from settings is returned.
    /// </summary>
    public string WishlistUrl
    {
        get
        {
            return mWishlistUrl ?? (mWishlistUrl = ECommerceSettings.WishListURL(SiteContext.CurrentSiteName));
        }
        set
        {
            mWishlistUrl = value;
        }
    }


    /// <summary>
    /// File name of the image which is used as a source for image button to add product to the wishlist, default image folder is '~/App_Themes/(Skin_Folder)/Images/ShoppingCart/'.
    /// </summary>
    public string AddToWishlistImageButton
    {
        get;
        set;
    }


    /// <summary>
    /// Simple string or localizable string of the link to add product to the wishlist.
    /// </summary>
    public string AddToWishlistLinkText
    {
        get;
        set;
    }


    /// <summary>
    /// Shopping cart object required for formatting of the displayed prices.
    /// If it is not set, current shopping cart from E-commerce context is used.
    /// </summary>
    public ShoppingCartInfo ShoppingCart
    {
        get
        {
            return mShoppingCart ?? (mShoppingCart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart());
        }
        set
        {
            mShoppingCart = value;
        }
    }


    /// <summary>
    /// Indicates if shopping car item selector is modal dialog.
    /// </summary>
    public bool DialogMode
    {
        get;
        set;
    }


    /// <summary>
    /// Control that is used to add the product to shopping cart.
    /// </summary>
    public Control AddToCartControl
    {
        get;
        private set;
    }


    /// <summary>
    /// Indicates if redirect to product details is enabled if other conditions are met.
    /// Set to true by default.
    /// </summary>
    public bool RedirectToDetailsEnabled
    {
        get;
        set;
    } = true;


    /// <summary>
    /// Indicates if 'Add to shopping cart' conversion should be logged (by default it is true)
    /// </summary>
    public bool TrackAddToShoppingCartConversion
    {
        get;
        set;
    } = true;


    /// <summary>
    /// Gets all variants of this product.
    /// </summary>
    private List<ProductVariant> Variants
    {
        get
        {
            if (mVariants == null)
            {
                int cacheMinutes = SettingsKeyInfoProvider.GetIntValue(SiteContext.CurrentSiteName + ".CMSCacheMinutes");

                using (var cs = new CachedSection<List<ProductVariant>>(ref mVariants, cacheMinutes, true, null, "mProductVariants", SKUID))
                {
                    if (cs.LoadData)
                    {
                        DataSet dsVariants = VariantHelper.GetVariants(SKUID);

                        mVariants = new List<ProductVariant>();
                        if (!DataHelper.DataSourceIsEmpty(dsVariants))
                        {
                            // Create all product variants objects for this product
                            foreach (DataRow dr in dsVariants.Tables[0].Rows)
                            {
                                mVariants.Add(new ProductVariant(ValidationHelper.GetInteger(dr["SKUID"], 0)));
                            }
                        }

                        // Save to the cache
                        if (cs.Cached)
                        {
                            // Get dependencies
                            var dependencies = new List<string>
                                               {
                                                   "ecommerce.sku|byid|" + SKUID,
                                                   "ecommerce.optioncategory|all"
                                               };

                            // Set dependencies
                            cs.CacheDependency = CacheHelper.GetCacheDependency(dependencies);
                        }

                        cs.Data = mVariants;
                    }
                }
            }

            return mVariants;
        }
    }


    /// <summary>
    /// Current selected options(item2 in tuple) in selectors and theirs categories(item1 in tuple)
    /// </summary>
    private List<Tuple<int, int>> SelectedOptionsInCategories
    {
        get
        {
            if (mSelectedOptionsInCategories == null)
            {
                CalculateVariantData();
            }

            return mSelectedOptionsInCategories;
        }
    }


    /// <summary>
    /// All possible options and theirs categories in dictionary with option ID as a key and category ID as value
    /// </summary>
    private Dictionary<int, int> OptionsInCategories
    {
        get
        {
            if (mOptionsInCategories == null)
            {
                CalculateVariantData();
            }

            return mOptionsInCategories;
        }
    }


    /// <summary>
    /// Gets the related selectors = selectors that are used to select options for variants.
    /// </summary>
    private IEnumerable<ProductOptionSelector> RelatedSelectors
    {
        get
        {
            if (mRelatedSelectors == null)
            {
                CalculateVariantData();
            }

            return mRelatedSelectors;
        }
    }


    /// <summary>
    /// Gets all selectors of product options.
    /// </summary>
    private IEnumerable<ProductOptionSelector> OptionSelectors
    {
        get
        {
            return pnlSelectors.Controls.OfType<ProductOptionSelector>();
        }
    }


    /// <summary>
    /// Gets the selected variant.
    /// </summary>
    private ProductVariant SelectedVariant
    {
        get
        {
            //Selected options
            var optionIds =
                from tuple in SelectedOptionsInCategories
                select tuple.Item2;

            var ids = optionIds.ToList();
            if (ids.Any())
            {
                return Variants.FirstOrDefault(variant => variant.ProductAttributes.Contains(ids));
            }

            return null;
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Fires when "Add to shopping cart" button is clicked, overrides original action.
    /// </summary>
    public event CancelEventHandler OnAddToShoppingCart;

    #endregion


    #region "Lifecycle"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        lnkAdd.Click += lnkAdd_Click;
        btnAdd.Click += btnAdd_Click;
        lnkWishlist.Click += lnkWishlist_Click;
        btnWishlist.Click += btnWishlist_Click;
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ReloadData();

        var script = "function ShoppingCartItemAddedHandler(message) { alert(message); }";
        ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "ProductAddedNotification", ScriptHelper.GetScript(script));
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (SKUID > 0)
        {
            InitializeControls();

            if (!mDataLoaded)
            {
                ReloadData();
            }
            // Set visibility for horizontal line (separator) between stock and selectors
            pnlSeparator.Visible = ShowProductOptions && SKUHasOptions;

            if (StockVisible)
            {
                SetStockInformation();
                lblStock.Text = GetString("com.shoppingcartitemselector.stock");
                pnlStock.Visible = true;
            }

            // Get count of the product options
            if (AlwaysShowTotalPrice || (ShowTotalPrice && SKUHasOptions))
            {
                // Count and show total price with options
                CalculateTotalPrice();
                // Show info message if selected variant is invalid
                EnsureUnavailableVariantMessage();

                if (!string.IsNullOrEmpty(TotalPriceLabel))
                {
                    // Use custom label
                    lblPrice.Text = GetString(TotalPriceLabel);
                }
                else
                {
                    // Use default label
                    lblPrice.Text = GetString("ShoppingCartItemSelector.TotalPrice");
                }

                pnlPrice.Visible = true;
            }
            else
            {
                // Hide total price container
                pnlPrice.Visible = false;
            }

            if (DialogMode)
            {
                pnlButton.CssClass += " PageFooterLine";
            }
        }

        hdnSKUID.Value = SKUID.ToString();

        // Show panel only when some selectors loaded
        pnlSelectors.Visible = ShowProductOptions && SKUHasOptions;

        if (pnlSelectors.Visible)
        {
            SetDisabledProductOptions();
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Reloads shopping cart item selector data.
    /// </summary>
    public void ReloadData()
    {
        if (SKUID <= 0)
        {
            return;
        }

        DebugHelper.SetContext("ShoppingCartItemSelector");

        InitializeControls();

        if (ShowProductOptions)
        {
            LoadProductOptions();
        }

        // Fill units textbox with default quantity
        if (ShowUnitsTextBox)
        {
            if (String.IsNullOrWhiteSpace(txtUnits.Text))
            {
                txtUnits.Text = DefaultQuantity.ToString();
            }
        }

        mDataLoaded = true;
        DebugHelper.ReleaseContext();
    }


    /// <summary>
    /// Sets options in selectors disabled (but still selectable) via defined variants.
    /// </summary>
    private void SetDisabledProductOptions()
    {
        if (pnlSelectors.Controls.Count == 0)
        {
            return;
        }

        var enabledOptionIds = new List<int>();
        // For each option in related selectors
        foreach (var optionId in OptionsInCategories.Keys)
        {
            // Check if this option makes variant with selected options from other categories

            // Get option category for currently processing option ID
            int currentOptionCategory = OptionsInCategories[optionId];
            var selectedOptionsFromOtherCategories = new List<int>();

            // Get selected options from other categories (exclude selected option from this option's option category)
            SelectedOptionsInCategories.ForEach(optTuple =>
            {
                if (optTuple.Item1 != currentOptionCategory)
                {
                    selectedOptionsFromOtherCategories.Add(optTuple.Item2);
                }
            });
            // Add currently processing option ID to selected option set
            selectedOptionsFromOtherCategories.Add(optionId);
            // Remove "None" selections (ID = 0)
            selectedOptionsFromOtherCategories.RemoveAll(opt => opt == 0);

            // Check if selected options from other categories with this current option have existing variant
            foreach (var variant in Variants)
            {
                if (variant.ProductAttributes.Contains(selectedOptionsFromOtherCategories))
                {
                    if (mOneCategoryUsed && !variant.Variant.SKUEnabled)
                    {
                        continue;
                    }

                    // If so, add this option into enabledOptionIds collection
                    enabledOptionIds.Add(optionId);
                }
            }
        }

        // For all related selectors set enabled options, or remove unused if there is only one OC
        foreach (var selector in RelatedSelectors)
        {
            if (mOneCategoryUsed)
            {
                selector.RemoveDisabledOptions(enabledOptionIds);
            }
            else
            {
                selector.SetEnabledOptions(enabledOptionIds);
            }
        }
    }


    /// <summary>
    /// Initializes controls.
    /// </summary>
    private void InitializeControls()
    {
        if (lnkAdd == null)
        {
            upnlAjax.LoadContainer();
        }

        if (SKUEnabled)
        {
            // Display/hide units textbox with default quantity
            if (ShowUnitsTextBox)
            {
                txtUnits.Visible = true;
                lblUnits.Visible = true;
                lblUnits.AssociatedControlID = "txtUnits";
                lblUnits.ResourceString = GetString(QuantityText);
            }

            // Display link button
            if (!String.IsNullOrEmpty(AddToCartLinkText))
            {
                lnkAdd.Visible = true;
                lnkAdd.Text = GetString(AddToCartLinkText);
                lnkAdd.ToolTip = GetString(AddToCartTooltip);

                AddToCartControl = lnkAdd;
            }
            // Display image button
            else if (!String.IsNullOrEmpty(AddToCartImageButton))
            {
                btnAddImage.ImageUrl = GetImageButtonUrl(AddToCartImageButton);
                btnAddImage.Visible = true;
                btnAddImage.AlternateText = GetString(AddToCartTooltip);
                btnAddImage.ToolTip = GetString(AddToCartTooltip);

                AddToCartControl = btnAddImage;
            }
            // Display classic button
            else
            {
                btnAdd.Visible = true;
                btnAdd.Text = GetString(AddToCartText);
                btnAdd.ToolTip = GetString(AddToCartTooltip);

                AddToCartControl = btnAdd;
            }
        }

        // Display "Add to Wishlist" link
        if (ShowWishlistLink)
        {
            if (!String.IsNullOrEmpty(AddToWishlistLinkText))
            {
                lnkWishlist.Visible = true;
                lnkWishlist.Text = ResHelper.LocalizeString(AddToWishlistLinkText);
                lnkWishlist.ToolTip = ResHelper.LocalizeString(AddToWishlistLinkText);
            }
            // Display "Add to Wishlist" image button
            else if (!String.IsNullOrEmpty(AddToWishlistImageButton))
            {
                // Image button
                btnWishlist.Visible = true;
                btnWishlist.ImageUrl = GetImageButtonUrl(AddToWishlistImageButton);
                btnWishlist.AlternateText = GetString("ShoppingCart.AddToWishlistToolTip");
                btnWishlist.ToolTip = GetString("ShoppingCart.AddToWishlistToolTip");
            }
        }
    }


    /// <summary>
    /// Returns path of the image which is used for the shopping cart selector button.
    /// </summary>
    /// <param name="imageButton">Image name or image relative path.</param>
    private string GetImageButtonUrl(string imageButton)
    {
        if (imageButton.StartsWithCSafe("~"))
        {
            return UrlResolver.ResolveUrl(imageButton);
        }

        return ImageFolder.TrimEnd('/') + "/" + imageButton;
    }


    /// <summary>
    /// Returns selected shopping cart item parameters containing product option parameters.
    /// </summary>
    public ShoppingCartItemParameters GetShoppingCartItemParameters()
    {
        // Get product options
        List<ShoppingCartItemParameters> options;
        int skuId = SKUID;

        if ((SelectedVariant != null) && (Variants.Count != 0))
        {
            skuId = SelectedVariant.Variant.SKUID;
            options = ProductOptionsParametersNonVariant;
        }
        else
        {
            // Get product options
            options = ProductOptionsParameters;
        }
        // Create parameters
        var cartItemParams = new ShoppingCartItemParameters(skuId, Quantity, options);

        // Ensure minimum allowed number of items is met
        if (SKU.SKUMinItemsInOrder > Quantity)
        {
            cartItemParams.Quantity = SKU.SKUMinItemsInOrder;
        }

        return cartItemParams;
    }


    private void lnkWishlist_Click(object sender, EventArgs e)
    {
        // Add product to wishlist
        AddProductToWishlist();
    }


    private void btnWishlist_Click(object sender, ImageClickEventArgs e)
    {
        // Add product to wishlist
        AddProductToWishlist();
    }


    private void lnkAdd_Click(object sender, EventArgs e)
    {
        // Add product to shopping cart
        AddProductToShoppingCart();
    }


    protected void btnAddImage_Click(object sender, ImageClickEventArgs e)
    {
        // Add product to shopping cart
        AddProductToShoppingCart();
    }


    private void btnAdd_Click(object sender, EventArgs e)
    {
        // Add product to shopping cart
        AddProductToShoppingCart();
    }


    private void AddProductToWishlist()
    {
        SessionHelper.SetValue("ShoppingCartUrlReferrer", RequestContext.CurrentURL);
        URLHelper.Redirect(UrlResolver.ResolveUrl(WishlistUrl + "?productid=" + SKUID));
    }


    /// <summary>
    /// Validates shopping cart item selector input data.
    /// </summary>
    private bool IsValid()
    {
        // Validates all product options
        if (ShowProductOptions)
        {
            return OptionSelectors.All(selector => selector.IsValid());
        }

        return true;
    }


    /// <summary>
    /// Adds product to the shopping cart.
    /// </summary>
    private void AddProductToShoppingCart()
    {
        SKUID = hdnSKUID.Value.ToInteger(0);

        // Validate input data
        if (!IsValid() || (SKU == null))
        {
            // Do not process
            return;
        }

        // Try to redirect before any currently selected variant checks (selector in listings issue)
        if (RedirectToDetailsEnabled)
        {
            if (!ShowProductOptions)
            {
                // Does product have some enabled product option categories?
                bool hasOptions = !DataHelper.DataSourceIsEmpty(OptionCategoryInfoProvider.GetProductOptionCategories(SKUID, true));

                if (hasOptions)
                {
                    // Redirect to product details
                    URLHelper.Redirect("~/CMSPages/Ecommerce/GetProduct.aspx?productid=" + SKUID);
                }
            }
        }

        if (SKU.HasVariants)
        {
            // Check if configured variant is available
            if ((SelectedVariant == null) || !SelectedVariant.Variant.SKUEnabled)
            {
                ScriptHelper.RegisterStartupScript(Page, typeof(string), "ShoppingCartAddItemErrorAlert", ScriptHelper.GetAlertScript(GetString("com.cartcontent.nonexistingvariantselected")));
                return;
            }
        }

        // Get cart item parameters
        var cartItemParams = GetShoppingCartItemParameters();

        string error = null;

        // Check if it is possible to add this item to shopping cart
        if (!Service.Resolve<ICartItemChecker>().CheckNewItem(cartItemParams, ShoppingCart))
        {
            error = String.Format(GetString("ecommerce.cartcontent.productdisabled"), SKU.SKUName);
        }

        if (!string.IsNullOrEmpty(error))
        {
            // Show error message and cancel adding the product to shopping cart
            ScriptHelper.RegisterStartupScript(Page, typeof(string), "ShoppingCartAddItemErrorAlert", ScriptHelper.GetAlertScript(error));
            return;
        }

        // Fire on add to shopping cart event
        var eventArgs = new CancelEventArgs();
        OnAddToShoppingCart?.Invoke(this, eventArgs);

        // If adding to shopping cart was canceled
        if (eventArgs.Cancel)
        {
            return;
        }

        // Get cart item parameters in case something changed
        cartItemParams = GetShoppingCartItemParameters();

        var shoppingService = Service.Resolve<IShoppingService>();

        var addedItem = shoppingService.AddItemToCart(cartItemParams);
        if (addedItem == null)
        {
            return;
        }

        if (TrackAddToShoppingCartConversion)
        {
            // Track 'Add to shopping cart' conversion
            ECommerceHelper.TrackAddToShoppingCartConversion(addedItem);
        }

        if (RedirectToShoppingCart)
        {
            // Set shopping cart referrer
            SessionHelper.SetValue("ShoppingCartUrlReferrer", RequestContext.CurrentURL);

            // Ensure shopping cart update
            SessionHelper.SetValue("checkinventory", true);

            // Redirect to shopping cart
            URLHelper.Redirect(UrlResolver.ResolveUrl(ShoppingCartUrl));
        }
        else
        {
            // Localize SKU name
            string skuName = (addedItem.SKU != null) ? ResHelper.LocalizeString(addedItem.SKU.SKUName) : String.Empty;

            // Validate inventory
            string validateInventoryMessage = String.Empty;

            var validationErrors = ShoppingCartInfoProvider.ValidateShoppingCart(ShoppingCart);
            if (validationErrors.Any())
            {
                validateInventoryMessage = validationErrors
                    .Select(e => HTMLHelper.HTMLEncode(e.GetMessage()))
                    .Join("<br />");
            }

            // Get product added message
            string message = String.Format(GetString("com.productadded"), skuName);

            // Add inventory check message
            if (!String.IsNullOrEmpty(validateInventoryMessage))
            {
                message += "\n\n" + validateInventoryMessage;
            }

            // Count and show total price with options
            CalculateTotalPrice();

            // Register the call of JS handler informing about added product
            ScriptHelper.RegisterStartupScript(Page, typeof(string), "ShoppingCartItemAddedHandler", "if (typeof ShoppingCartItemAddedHandler == 'function') { ShoppingCartItemAddedHandler(" + ScriptHelper.GetString(message) + "); }", true);
        }
    }


    /// <summary>
    /// Loads product options.
    /// </summary>
    private void LoadProductOptions()
    {
        DataSet dsCategories = null;

        if (IsLiveSite)
        {
            // Get cache minutes
            int cacheMinutes = SettingsKeyInfoProvider.GetIntValue(SiteContext.CurrentSiteName + ".CMSCacheMinutes");

            // Try to get data from cache
            using (var cs = new CachedSection<DataSet>(ref dsCategories, cacheMinutes, true, null, "skuoptioncategories", SKUID))
            {
                if (cs.LoadData)
                {
                    // Get all option categories for SKU
                    dsCategories = OptionCategoryInfoProvider.GetProductOptionCategories(SKUID, false);

                    // Save to the cache
                    if (cs.Cached)
                    {
                        // Get dependencies
                        var dependencies = new List<string>
                                           {
                                               "ecommerce.sku|byid|" + SKUID,
                                               "ecommerce.optioncategory|all"
                                           };

                        // Set dependencies
                        cs.CacheDependency = CacheHelper.GetCacheDependency(dependencies);
                    }

                    cs.Data = dsCategories;
                }
            }
        }
        else
        {
            // Get all option categories for SKU
            dsCategories = OptionCategoryInfoProvider.GetProductOptionCategories(SKUID, false);
        }

        // Initialize product option selectors
        if (!DataHelper.DataSourceIsEmpty(dsCategories))
        {
            mProductOptions = new Hashtable();

            // Is only one option category available for variants?
            var variantCategories = Variants.Any() ? Variants.First().ProductAttributes.CategoryIDs.ToList() : null;
            mOneCategoryUsed = variantCategories != null && variantCategories.Count == 1;

            foreach (DataRow dr in dsCategories.Tables[0].Rows)
            {
                try
                {
                    // Load control for selection product options
                    ProductOptionSelector selector = (ProductOptionSelector)LoadUserControl("~/CMSModules/Ecommerce/Controls/ProductOptions/ProductOptionSelector.ascx");

                    // Add control to the collection
                    var categoryID = ValidationHelper.GetInteger(dr["CategoryID"], 0);
                    selector.ID = "opt" + categoryID;

                    // Init selector
                    selector.LocalShoppingCartObj = ShoppingCart;
                    selector.IsLiveSite = IsLiveSite;
                    selector.CssClassFade = CssClassFade;
                    selector.CssClassNormal = CssClassNormal;
                    selector.SKUID = SKUID;
                    selector.OptionCategory = new OptionCategoryInfo(dr);

                    // If one category is used, fix the one selector with options to use only options which are not in disabled variants
                    if (mOneCategoryUsed && variantCategories.Contains(categoryID))
                    {
                        var disabled = from variant in Variants
                                       where !variant.Variant.SKUEnabled
                                       from productAttributes in variant.ProductAttributes
                                       select productAttributes.SKUID;

                        var disabledList = disabled.ToList();

                        selector.ProductOptionsInDisabledVariants = disabledList.Any() ? disabledList : null;
                    }

                    // Load all product options
                    foreach (DictionaryEntry entry in selector.ProductOptions)
                    {
                        mProductOptions[entry.Key] = entry.Value;
                    }

                    var lc = selector.SelectionControl as ListControl;
                    if (lc != null)
                    {
                        // Add Index change handler
                        lc.AutoPostBack = true;
                    }

                    pnlSelectors.Controls.Add(selector);
                }
                catch
                {
                }
            }
        }
    }


    /// <summary>
    /// Sets currently valid stock information for render.
    /// </summary>
    private void SetStockInformation()
    {
        var selectedVariant = SelectedVariant;

        var selectedSKU = (selectedVariant != null) ? selectedVariant.Variant : SKU;

        // Used in listing.
        if (!UsedInProductDetail)
        {
            switch (selectedSKU.SKUTrackInventory)
            {
                case TrackInventoryTypeEnum.ByProduct:
                    RenderStockInformation(selectedSKU.SKUAvailableItems > 0);
                    break;
                case TrackInventoryTypeEnum.ByVariants:
                    RenderStockInformation(Variants.Exists(variant => variant.Variant.SKUAvailableItems > 0));
                    break;
                default:
                    RenderStockInformation(true);
                    break;
            }
        }
        // Used in product detail.
        else
        {
            // Sku or variant is disabled or variant from given options does not exist.
            if (!selectedSKU.SKUEnabled || ((SelectedVariant == null) && (Variants.Count != 0)))
            {
                lblStockValue.Attributes.Remove("class");
                lblStockValue.Attributes.Add("class", "stock unavailable");
                lblStockValue.Text = GetString("com.variant.notavailable");
            }
            else
            {
                RenderStockInformation((selectedSKU.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (selectedSKU.SKUAvailableItems > 0));
            }
        }
    }


    /// <summary>
    /// Render information about the stock.
    /// </summary>
    /// <param name="isInStock">Is in stock</param>
    private void RenderStockInformation(bool isInStock)
    {
        if (isInStock)
        {
            lblStockValue.Attributes.Remove("class");
            lblStockValue.Attributes.Add("class", "stock available");
            lblStockValue.Text = GetString("com.shoppingcartitemselector.instock");
        }
        else
        {
            lblStockValue.Attributes.Remove("class");
            lblStockValue.Attributes.Add("class", "stock unavailable");
            lblStockValue.Text = GetString("com.shoppingcartitemselector.outofstock");
        }
    }


    /// <summary>
    /// Calculate total price with product options prices.
    /// </summary>
    private void CalculateTotalPrice()
    {
        var product = SelectedVariant?.Variant ?? SKU;

        var options = GetNonVariantOptions();

        var prices = Service.Resolve<ICatalogPriceCalculatorFactory>()
            .GetCalculator(ShoppingCart.ShoppingCartSiteID)
            .GetPrices(product, options, ShoppingCart);

        var price = prices.Price;

        lblPriceValue.Text = CurrencyInfoProvider.GetFormattedPrice((price < 0) ? 0 : price, ShoppingCart.Currency);
    }


    /// <summary>
    /// Returns all accessory and attribute product options not involved in the product variant.
    /// </summary>
    private IEnumerable<SKUInfo> GetNonVariantOptions()
    {
        foreach (var optionParam in ProductOptionsParameters)
        {
            if (mProductOptions.Contains(optionParam.SKUID))
            {
                bool isVariantOption = OptionsInCategories.Keys.Contains(optionParam.SKUID);
                if (!isVariantOption)
                {
                    var option = (SKUInfo)mProductOptions[optionParam.SKUID];
                    if (!option.IsTextAttribute)
                    {
                        yield return option;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Ensures the invalid variant message.
    /// </summary>
    private void EnsureUnavailableVariantMessage()
    {
        // Info message is not disabled and product has some variants and selected one exists;
        lblUnavailableVariantInfo.Visible = UnavailableVariantInfoEnabled &&
                                            ((SelectedVariant == null && Variants.Count != 0) ||
                                             (SelectedVariant != null && !SelectedVariant.Variant.SKUEnabled));
    }


    /// <summary>
    /// Calculates the variant data such as selected options, their categories etc.
    /// </summary>
    private void CalculateVariantData()
    {
        // Current selected options(item2 in tuple) in selectors and theirs categories(item1 in tuple)
        mSelectedOptionsInCategories = new List<Tuple<int, int>>();

        // All possible options and theirs categories in dictionary with option ID as a key and category ID as value
        mOptionsInCategories = new Dictionary<int, int>();

        mRelatedSelectors = new List<ProductOptionSelector>();

        // Categories used in variants
        IEnumerable<int> usedCategoriesInVariants = new List<int>();

        // Do not evaluate variants when there are no selectors displayed
        if (OptionSelectors.Any() && (Variants.Count != 0))
        {
            usedCategoriesInVariants = Variants.First().ProductAttributes.CategoryIDs;
        }

        // Get product options and categories from selectors
        foreach (var selector in OptionSelectors)
        {
            // Get option category of this selector
            var category = selector.OptionCategory;

            // Ignore non-attribute categories and categories not included in Variants
            if ((category.CategoryType == OptionCategoryTypeEnum.Attribute) && usedCategoriesInVariants.Contains(category.CategoryID))
            {
                string option = selector.GetSelectedSKUOptions();

                // Get selected option id for this selector and add new Tuple<category, option> to selected option collection
                int selectedOptionId = ValidationHelper.GetInteger(option, 0);
                mSelectedOptionsInCategories.Add(new Tuple<int, int>(category.CategoryID, selectedOptionId));

                // Get all options for this category from selector to reduce SQL queries
                DataSet dsAllOptions = selector.ProductOptionsData;

                if (DataHelper.DataSourceIsEmpty(dsAllOptions))
                {
                    continue;
                }

                List<int> allOptionsId = (List<int>)DataHelper.GetIntegerValues(dsAllOptions.Tables[0], "SKUID");

                // Add option ids (plus category ids) to dictionary
                allOptionsId.ForEach(opt =>
                {
                    if (!mOptionsInCategories.ContainsKey(opt))
                    {
                        mOptionsInCategories.Add(opt, category.CategoryID);
                    }
                });

                mRelatedSelectors.Add(selector);
            }
        }
    }

    #endregion
}
