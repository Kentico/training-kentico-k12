<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_Checkout_CartItemPriceDetail"  Codebehind="CartItemPriceDetail.ascx.cs" %>
<asp:Label ID="lblPriceDetailAction" runat="server" Text='<%#GetPriceDetailLink(Eval("CartItemGuid"))%>'
    CssClass="ProductPriceDetailLink" ToolTip='<%#GetString("com.showpricedetail")%>' EnableViewState="false" />