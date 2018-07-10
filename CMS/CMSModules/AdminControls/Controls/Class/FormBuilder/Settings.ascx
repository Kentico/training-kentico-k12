<%@ Control Language="C#" AutoEventWireup="false" Inherits="CMSModules_AdminControls_Controls_Class_FormBuilder_Settings"
     Codebehind="Settings.ascx.cs" %>

<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/FormBuilder/FieldProperties.ascx" TagName="FieldProperties"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/FormBuilder/FieldValidation.ascx" TagName="FieldValidation"
    TagPrefix="cms" %>

<cms:CMSUpdatePanel ID="pnlUpdateSettings" UpdateMode="Conditional" runat="server" class="settings-panel">
    <ContentTemplate>
        <cms:CMSPanel ID="pnlNoSelectedField" runat="server" CssClass="settings-content">
            <cms:MessagesPlaceHolder ID="mphNoSelected" runat="server" />
        </cms:CMSPanel>
        <asp:Panel runat="server" ID="pnlEdit" CssClass="pnl-edit">
            <asp:HiddenField runat="server" ID="hdnSelectedTab" />
            <cms:CMSButtonGroup runat="server" ID="btnGroup" />
            <div id="form-builder-properties">
                <cms:FieldProperties ID="pnlProperties" runat="server" />
            </div>
            <div id="form-builder-validation">
                <cms:FieldValidation ID="pnlValidation" runat="server" />
            </div>
        </asp:Panel>
    </ContentTemplate>
</cms:CMSUpdatePanel>
