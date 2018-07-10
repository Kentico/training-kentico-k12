<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Ecommerce_ShoppingCart_ShoppingCartWebPart"  Codebehind="~/CMSWebParts/Ecommerce/ShoppingCart/ShoppingCartWebPart.ascx.cs" %>
<%@ Register Src="~/CMSModules/Ecommerce/Controls/ShoppingCart/ShoppingCart.ascx" TagName="ShoppingCart" TagPrefix="cms" %>
<cms:ShoppingCart ID="cartElem" runat="server" />
