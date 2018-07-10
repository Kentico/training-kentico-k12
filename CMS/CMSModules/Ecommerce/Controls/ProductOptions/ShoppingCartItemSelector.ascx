<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_ProductOptions_ShoppingCartItemSelector"
     Codebehind="ShoppingCartItemSelector.ascx.cs" %>
<cms:CMSUpdatePanel ID="upnlAjax" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlContainer" runat="server" CssClass="CartItemSelectorContainer">
            <%-- Stock --%>
            <asp:Panel ID="pnlStock" runat="server" CssClass="StockContainer" EnableViewState="false" Visible="false">
                <asp:Label ID="lblStock" runat="server" CssClass="StockLabel" EnableViewState="false" />
                <asp:Label ID="lblStockValue" runat="server" EnableViewState="false" />
            </asp:Panel>
            <asp:Panel ID="pnlSeparator" runat="server" CssClass="hr" EnableViewState="false" />
            <asp:Panel ID="pnlSelectors" runat="server" CssClass="CartItemSelector form-horizontal">
                <%-- Here come product options --%>
            </asp:Panel>
            <asp:Panel ID="pnlMessages" runat="server" CssClass="MessagesContainer">
                <cms:LocalizedLabel ID="lblUnavailableVariantInfo" runat="server" Visible="false" CssClass="UnavailableVariantLabel unavailable" ResourceString="com.cartcontent.nonexistingvariantselected" />
            </asp:Panel>
            <%-- Price --%>
            <asp:Panel ID="pnlPrice" runat="server" CssClass="TotalPriceContainer" EnableViewState="false">
                <asp:Label ID="lblPrice" runat="server" CssClass="TotalPriceLabel" EnableViewState="false" />
                <asp:Label ID="lblPriceValue" runat="server" CssClass="TotalPrice" EnableViewState="false" />
            </asp:Panel>
            <asp:Panel ID="pnlButton" runat="server" CssClass="AddToCartContainer add-to-cart-container control-group-inline" EnableViewState="false">
                <%-- Add to wishlist --%>
                <asp:ImageButton ID="btnWishlist" runat="server" Visible="false" CssClass="AddToWishlistImageButton add-to-wishlist-image-button" />
                <asp:LinkButton ID="lnkWishlist" runat="server" Visible="false" CssClass="AddToWishlistLink add-to-wishlist-link" />
                <%-- Units --%>
                <cms:LocalizedLabel ID="lblUnits" runat="server" Visible="false" CssClass="UnitsLabel units-label form-control-text" ResourceString="ecommerce.shoppingcartcontent.skuunits" />
                <cms:CMSTextBox ID="txtUnits" runat="server" Visible="false" CssClass="AddToCartTextBox add-to-cart-text-box"
                    MaxLength="9" />
                <%-- Add to cart controls --%>
                <cms:CMSButton ID="btnAdd" runat="server" Visible="false" ButtonStyle="Primary" CssClass="AddToCartButton add-to-cart-button" />
                <asp:ImageButton ID="btnAddImage" runat="server" Visible="false" CssClass="AddToCartImageButton add-to-cart-image-button"
                    OnClick="btnAddImage_Click" />
                <asp:LinkButton ID="lnkAdd" runat="server" Visible="false" CssClass="AddToCartLink add-to-cart-link" />
            </asp:Panel>
        </asp:Panel>
        <asp:HiddenField runat="server" ID="hdnSKUID" Value="0" />
    </ContentTemplate>
</cms:CMSUpdatePanel>
