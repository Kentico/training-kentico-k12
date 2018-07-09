<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_Checkout_CartItemUnits"  Codebehind="CartItemUnits.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniControls/UniButton.ascx" TagName="UniButton" TagPrefix="cms" %>

<asp:Panel runat="server" ID="pnlCartItemUnits" Visible="true" DefaultButton="btnHiddenButton">
    <cms:FormControl runat="server" ID="unitCountFormControl" FormControlName="TextBoxControl" />
    <div style="display: none;">
        <asp:Button runat="server" ID="btnHiddenButton" OnClick="Update" Visible="True"/>
    </div>
    <asp:Panel runat="server" ID="pnlButton" Visible="false" CssClass="UnitCountButton">
        <cms:UniButton runat="server" ID="btnUpdate" CssClass="UpdateButton" Visible="false" OnClick="Update"/>
    </asp:Panel>
</asp:Panel>
