<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Viewers_ShoppingCartTotals"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Viewers/ShoppingCartTotals.ascx.cs" %>

<div id="totalViewer" class="TotalViewer" runat="server" visible="False">
    <div class="Label">
        <asp:Label ID="lblLabel" Visible="false" runat="server" />
    </div>
    <div class="Value">
        <asp:Label ID="lblValue" runat="server" />
    </div>
</div>
<cms:BasicUniView runat="server" ID="uvMultiBuySummary" Visible="false" />