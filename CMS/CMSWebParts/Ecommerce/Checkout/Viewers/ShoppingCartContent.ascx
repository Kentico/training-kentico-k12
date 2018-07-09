<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Viewers_ShoppingCartContent"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Viewers/ShoppingCartContent.ascx.cs" %>

<asp:Panel ID="pnlCartContent" runat="server" CssClass="CartContent" >
    <cms:BasicUniView runat="server" ID="shoppingCartUniView"  CssClass="PanelShoppingCartContent"  />
    <asp:Label runat="server" ID="lblShoppingCartEmpty" Visible="false" CssClass="ShoppingCartEmpty" />
    <div class="clear"></div>
    <asp:Label runat="server" ID="lblError" Visible="false" CssClass="ErrorLabel" />
</asp:Panel>
