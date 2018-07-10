<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Viewers_MessagePanel"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Viewers/MessagePanel.ascx.cs" %>

<div runat="server" EnableViewState="false" ID="messageWrapper" Visible="false" class="MessageLabelWrapper">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ErrorLabel" />
</div>