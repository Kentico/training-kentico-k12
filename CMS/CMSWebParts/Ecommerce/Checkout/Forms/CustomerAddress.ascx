<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Checkout_Forms_CustomerAddress"  Codebehind="~/CMSWebParts/Ecommerce/Checkout/Forms/CustomerAddress.ascx.cs" %>

<asp:Panel runat="server" ID="pnlAddress">
    <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel" Visible="false" EnableViewState="false" />
    <asp:Panel runat="server" ID="pnlShowAddress" CssClass="ShowAddressCheckbox">
        <cms:CMSCheckBox ID="chkShowAddress" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="chkShowAddress_OnCheckedChanged" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlAddressSelector">
        <div class="form-horizontal ecommerce_address address_selector">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel runat="server" ID="lblAddress" AssociatedControlID="drpAddresses" ResourceString="com.address" CssClass="control-label" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSDropDownList runat="server" ID="drpAddresses" CssClass="CustomerAddressSelector" DataTextField="AddressName" DataValueField="AddressID" AutoPostBack="true" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <cms:UIContextPanel ID="pnlUiContext" runat="server">
        <cms:UIForm ID="addressForm" runat="server" ObjectType="Ecommerce.Address" AlternativeFormName="OrdersAddress" RedirectUrlAfterCreate="" />
    </cms:UIContextPanel>
</asp:Panel>
