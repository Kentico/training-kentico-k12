<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="~/CMSWebParts/Ecommerce/CustomerEditAddress.ascx.cs" Inherits="CMSWebParts_Ecommerce_CustomerEditAddress" %>

<cms:LocalizedLabel ID="lblError" runat="server" CssClass="ErrorLabel" Visible="false" EnableViewState="false" />
<cms:UIContextPanel runat="server" ID="pnlUiContext">
    <cms:UIForm runat="server" ID="EditForm" RedirectUrlAfterCreate="" ObjectType="Ecommerce.Address" IsLiveSite="True" />
</cms:UIContextPanel>

