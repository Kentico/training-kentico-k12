<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_MyDetails_MyOrders"
     Codebehind="MyOrders.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<div class="MyOrders">
    <cms:UniGrid runat="server" ID="gridOrders" OrderBy="OrderDate DESC" Columns="OrderID,OrderDate,OrderStatusID,OrderCurrencyID,OrderGrandTotal,OrderTrackingNumber"
        ObjectType="ecommerce.order">
        <GridColumns>
            <ug:Column Source="OrderID" Caption="$MyOrders.OrderID$" Wrap="false" />
            <ug:Column Source="OrderDate" ExternalSourceName="#userdatetimegmt" Caption="$MyOrders.OrderDate$" Wrap="false" />
            <ug:Column Source="##ALL##" ExternalSourceName="GrandTotal" Caption="$MyOrders.TotalPrice$"
                Wrap="false" Sort="OrderGrandTotalInMainCurrency" />
            <ug:Column Source="OrderStatusID" ExternalSourceName="#transform: ecommerce.orderstatus : {% StatusDisplayName %}" Caption="$MyOrders.Status$" Wrap="false" />
            <ug:Column Name="OrderTrackingNumber" Source="OrderTrackingNumber" Caption="$MyOrders.TrackingNumber$" Wrap="false" />
            <ug:Column Source="OrderID" ExternalSourceName="Invoice" Caption="$MyOrders.Invoice$"
                Wrap="false" AllowSorting="false" />
            <ug:Column Name="downloads" Source="OrderID" ExternalSourceName="downloads" Caption="$myorders.downloads$"
                Wrap="false" AllowSorting="false" />
            <ug:Column Name="OrderToShoppingCart" Source="OrderID" ExternalSourceName="OrderToShoppingCart" Caption="$shoppingcart.addtoshoppingcart$"
                Wrap="false" AllowSorting="false" />
            <ug:Column CssClass="filling-column" />
        </GridColumns>
        <GridOptions DisplayFilter="false" />
    </cms:UniGrid>
</div>
