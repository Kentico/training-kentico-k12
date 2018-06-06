<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Forms_CustomerDetail"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Forms/CustomerDetail.ascx.cs" %>

<cms:LocalizedLabel ID="lblError" runat="server" CssClass="ErrorLabel" Visible="false" />
<cms:UIContextPanel ID="pnlUiContext" runat="server">
    <cms:UIForm ID="customerForm" runat="server" ObjectType="Ecommerce.Customer" RedirectUrlAfterCreate="" />
</cms:UIContextPanel>