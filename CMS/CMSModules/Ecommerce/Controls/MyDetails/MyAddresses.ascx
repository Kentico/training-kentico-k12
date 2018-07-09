<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_MyDetails_MyAddresses"
     Codebehind="MyAddresses.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<div class="MyAddresses">
    <asp:Label ID="lblError" runat="server" Visible="false" EnableViewState="false" Style="color: Red" />
    <%--Address list--%>
    <asp:PlaceHolder runat="server" ID="plhList">
        <div style="margin-bottom: 15px">
            <asp:LinkButton ID="btnNew" runat="server" OnClick="btnNew_OnClick" />
        </div>
        <cms:UniGrid runat="server" ID="gridAddresses" Columns="AddressID,AddressName" OrderBy="AddressName"
            ObjectType="ecommerce.address">
            <GridColumns>
                <ug:Column Source="AddressID" ExternalSourceName="actions" Caption="$Unigrid.Actions$"
                    Wrap="false" AllowSorting="false" />
                <ug:Column Source="AddressName" Caption="$Unigrid.Customer_Edit_Address.Columns.AddressName$"
                    Wrap="false" />
                <ug:Column CssClass="filling-column" />
            </GridColumns>
            <GridOptions DisplayFilter="false" />
        </cms:UniGrid>
    </asp:PlaceHolder>
    <%--Address edit--%>
    <asp:PlaceHolder runat="server" ID="plhEdit" Visible="false">
        <asp:Panel ID="pnlAddressEdit" runat="server" DefaultButton="btnOK">
            <div style="margin-bottom: 15px">
                <asp:LinkButton ID="btnList" runat="server" OnClick="btnList_OnClick" />&nbsp;
                <asp:Label ID="lblAddress" runat="server" />
            </div>
            <cms:LocalizedLabel ResourceString="General.ChangesSaved" runat="server" ID="lblInfo" EnableViewState="false" Visible="false" />
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel ShowRequiredMark="true" DisplayColon="true" CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.lblPersonalName" runat="server" ID="lblPersonalName" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtPersonalName" runat="server" MaxLength="200" EnableViewState="false" />
                        <cms:CMSRequiredFieldValidator ID="rqvPersonalName" runat="server" ControlToValidate="txtPersonalName"
                            ValidationGroup="Address" EnableViewState="false" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel ShowRequiredMark="true" DisplayColon="true" CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.AddressLine1Label" runat="server" ID="lblAddressLine1" EnableViewState="false" />
                    </div>

                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtAddressLine1" runat="server" MaxLength="100" EnableViewState="false" />
                        <cms:CMSRequiredFieldValidator ID="rqvLine" runat="server" ControlToValidate="txtAddressLine1"
                            ValidationGroup="Address" EnableViewState="false" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.AddressLine2Label" runat="server" ID="LocalizedLabel1" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtAddressLine2" runat="server" MaxLength="100" EnableViewState="false" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel ShowRequiredMark="true" DisplayColon="true" CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.AddressCityLabel" runat="server" ID="lblAddressCity" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtAddressCity" runat="server" MaxLength="100" EnableViewState="false" />
                        <cms:CMSRequiredFieldValidator ID="rqvCity" runat="server" ControlToValidate="txtAddressCity"
                            ValidationGroup="Address" EnableViewState="false" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel ShowRequiredMark="true" DisplayColon="true" CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.AddressZipLabel" runat="server" ID="lblAddressZip" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtAddressZip" runat="server" MaxLength="20" EnableViewState="false" />
                        <cms:CMSRequiredFieldValidator ID="rqvZipCode" runat="server" ValidationGroup="Address"
                            ControlToValidate="txtAddressZip" Display="Dynamic" EnableViewState="false" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel ShowRequiredMark="true" DisplayColon="true" CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.AddressCountryIDLabel" runat="server" ID="lblAddressCountry" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CountrySelector ID="ucCountrySelector" runat="server" UseCodeNameForSelection="false"
                            AddSelectCountryRecord="true" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ResourceString="Customer_Edit_Address_Edit.AddressDeliveryPhoneLabel" runat="server" ID="lblAddressDeliveryPhone" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtAddressDeliveryPhone" runat="server" MaxLength="100" EnableViewState="false" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-value-cell editing-form-value-cell-offset">
                        <cms:CMSButton runat="server" ID="btnOk" OnClick="btnOK_OnClick" EnableViewState="false"
                            ValidationGroup="Address" ButtonStyle="Primary" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:PlaceHolder>
    <asp:Button ID="btnHiddenEdit" runat="server" CssClass="HiddenButton" EnableViewState="false" />
    <asp:Button ID="btnHiddenDelete" runat="server" CssClass="HiddenButton" EnableViewState="false" />
    <asp:HiddenField ID="hdnID" runat="server" EnableViewState="false" />
</div>