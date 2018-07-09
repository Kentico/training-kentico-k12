<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_PageTemplates_PageTemplate_HeaderTab"
    Theme="Default" ValidateRequest="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Page Template Edit - Header"  Codebehind="PageTemplate_HeaderTab.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:ObjectLockingPanel runat="server" ID="pnlObjectLocking">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell label-full-width">
                    <asp:Label ID="lblTemplateHeader" runat="server" CssClass="control-label editing-form-label" />
                </div>
                <div class="editing-form-value-cell textarea-full-width">
                    <cms:ExtendedTextArea ID="txtTemplateHeader" runat="server" EnableViewState="false"
                        EditorMode="Advanced" Width="98%" Height="270" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ResourceString="template.allowheaderinherit"
                        DisplayColon="True" AssociatedControlID="chkAllowInherit" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox runat="server" ID="chkAllowInherit" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ResourceString="template.inheritheaderparent"
                        DisplayColon="True" AssociatedControlID="chkInheritParent" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox runat="server" ID="chkInheritParent" />
                </div>
            </div>
        </div>
    </cms:ObjectLockingPanel>
</asp:Content>