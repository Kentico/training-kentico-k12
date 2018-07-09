<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Forms_PaymentForm"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Forms/PaymentForm.ascx.cs" %>
<asp:Panel runat="server" ID="pnlPayment">
    <asp:Panel runat="server" ID="pnlError" CssClass="ErrorLabel" Visible="false">
        <asp:Label runat="server" ID="lblError" EnableViewState="false" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInfo" CssClass="InfoLabel" Visible="false">
        <asp:Label runat="server" ID="lblInfo" EnableViewState="false" />
    </asp:Panel>
    <div class="BlockContent">
        <div class="OrderSummary">
            <cms:LocalizedLabel ResourceString="PaymentSummary.OrderId" ID="lblOrderId" runat="server" EnableViewState="false" />
            <asp:Label ID="lblOrderIdValue" runat="server" EnableViewState="false" />
        </div>
        <div class="PaymentSummary">
            <cms:LocalizedLabel ResourceString="PaymentSummary.Payment" ID="lblPayment" runat="server" EnableViewState="false" />
            <asp:Label ID="lblPaymentValue" runat="server" EnableViewState="false" />
        </div>
        <div class="PaymentPriceSummary">
            <cms:LocalizedLabel ResourceString="PaymentSummary.TotalPrice" ID="lblTotalPrice" runat="server" EnableViewState="false" />
            <asp:Label ID="lblTotalPriceValue" runat="server" EnableViewState="false" />
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlPaymentDataContainer" CssClass="PaymentGatewayDataContainer tinyBox" />
</asp:Panel>
<cms:CMSButton runat="server" ID="btnProcessPayment" ButtonStyle="Primary" CssClass="ProcessPaymentButton" OnClick="btnProcessPayment_Click" Text="Process payment" />
