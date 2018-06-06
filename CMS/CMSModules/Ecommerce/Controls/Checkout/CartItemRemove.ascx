<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_Checkout_CartItemRemove"  Codebehind="CartItemRemove.ascx.cs" %>
<%@ Register TagPrefix="cms" TagName="UniButton" Src="~/CMSAdminControls/UI/UniControls/UniButton.ascx" %>

<cms:UniButton ID="btnRemove" CssClass="RemoveButton"  runat="server" OnClick="Remove" />