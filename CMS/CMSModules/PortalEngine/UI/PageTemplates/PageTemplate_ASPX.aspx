<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_PageTemplates_PageTemplate_ASPX"
    Theme="Default" ValidateRequest="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Page Template Edit - Header"  Codebehind="PageTemplate_ASPX.aspx.cs" %>

<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<asp:Content ID="cntControls" runat="server" ContentPlaceHolderID="plcSiteSelector">
    <div class="radio-list-horizontal">
        <cms:CMSRadioButton GroupName="Code" runat="server" ID="radSlave" AutoPostBack="true"
            Checked="true" />
        <cms:CMSRadioButton GroupName="Code" runat="server" ID="radMaster" AutoPostBack="true" />
        <cms:CMSRadioButton GroupName="Code" runat="server" ID="radTemplate" AutoPostBack="true" />
        <cms:CMSRadioButton GroupName="Code" runat="server" ID="radTemplateOnly" AutoPostBack="true" />
    </div>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel ID="pnlContainer" runat="Server" DefaultButton="btnRefresh" CssClass="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel ID="lblSite" runat="server" ResourceString="general.site" DisplayColon="true" CssClass="control-label editing-form-label" />
            </div>
            <div class="editing-form-value-cell">
                <cms:SiteSelector ID="siteSelector" runat="server" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <asp:PlaceHolder runat="server" ID="plcMasterTemplate">
                <div class="editing-form-label-cell">
                    <asp:Label ID="lblMaster" runat="server" CssClass="control-label editing-form-label" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextBox runat="server" ID="txtMaster" Text="MainMenu" />
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label ID="lblName" runat="server" CssClass="control-label editing-form-label" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox runat="server" ID="txtName" />
                <cms:CMSButton runat="server" ID="btnSave" OnClick="btnSave_Click" ButtonStyle="Default" />
                <cms:CMSButton runat="server" ID="btnRefresh" ButtonStyle="Default" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell label-full-width">
                <asp:Label runat="server" ID="lblCodeInfo" EnableViewState="false" CssClass="control-label editing-form-label" />
            </div>
            <div class="editing-form-value-cell textarea-full-width">
                <cms:ExtendedTextArea ID="txtCode" runat="server" EnableViewState="false" ReadOnly="true"
                    EditorMode="Advanced" Width="100%" Height="300px" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell label-full-width">
                <asp:Label runat="server" ID="lblCodeBehindInfo" EnableViewState="false" CssClass="control-label editing-form-label" />
            </div>
            <div class="editing-form-value-cell textarea-full-width">
                <cms:ExtendedTextArea ID="txtCodeBehind" runat="server" EnableViewState="false" ReadOnly="true"
                    EditorMode="Advanced" Language="CSharp" Width="100%" Height="300px" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
