<%@ Control Language="C#" AutoEventWireup="false" Inherits="CMSModules_AdminControls_Controls_Class_FormBuilder_FormBuilder"
    CodeBehind="FormBuilder.ascx.cs" %>

<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/FormBuilder/Settings.ascx" TagName="Settings"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/FormBuilder/FormComponents.ascx" TagName="FormComponents"
    TagPrefix="cms" %>

<asp:Panel ID="pnlFormBuilder" runat="server" DefaultButton="hdnFormBuilderUpdate" CssClass="form-builder">
    <cms:FormComponents runat="server" ID="pnlFormComponents" />
    <div class="form-builder-form">
        <cms:CMSUpdatePanel ID="pnlUpdateForm" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
            <ContentTemplate>
                <cms:MessagesPlaceHolder ID="plcMessagesHolder" runat="server" ShortID="m" UseRelativePlaceHolder="false" />
                <asp:Button ID="hdnFormBuilderUpdate" ClientIDMode="Static" runat="server" CssClass="HiddenButton" />
                <cms:LocalizedLabel ID="lblEmptyFormPlaceholder" runat="server" ResourceString="FormBuilder.EmptyFormPlaceholder" CssClass="empty-form-placeholder" Style="display: none" EnableViewState="false" />
                <cms:BasicForm runat="server" ID="formElem" IsLiveSite="false" IsDesignMode="true" EnableViewState="false"
                    DefaultFieldLayout="TwoColumns" MarkRequiredFields="true" AutomaticLabelWidth="true" FormButtonCssClass="btn btn-primary" />
            </ContentTemplate>
        </cms:CMSUpdatePanel>
    </div>
    <span id="lblSaveInfo" class="form-builder-info" style="display: none"></span>
    <cms:Settings runat="server" ID="pnlSettings" />
</asp:Panel>
