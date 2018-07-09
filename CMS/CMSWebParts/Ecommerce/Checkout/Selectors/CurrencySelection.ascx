<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Selectors_CurrencySelection"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Selectors/CurrencySelection.ascx.cs" %>
<%@ Register Src="~/CMSModules/Ecommerce/FormControls/CurrencySelector.ascx" TagName="CurrencySelector" TagPrefix="cms" %>

<asp:Panel ID="pnlCurrency" runat="server" CssClass="PanelCurrency">
    <cms:CurrencySelector ID="selectCurrency" runat="server" DisplayOnlyWithExchangeRate="true" CssClass="SelectorClass" AddAllItemsRecord="false" IsLiveSite="true" AutoPostBack="True" />
</asp:Panel>
