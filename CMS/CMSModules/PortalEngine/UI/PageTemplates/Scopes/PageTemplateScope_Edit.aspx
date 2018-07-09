<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_PortalEngine_UI_PageTemplates_Scopes_PageTemplateScope_Edit"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Page Template Edit - Scopes list"
    Theme="Default"  Codebehind="PageTemplateScope_Edit.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/FormControls/Documents/selectpath.ascx" TagName="selectpath" TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Classes/selectclass.ascx" TagName="SelectClass" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/PortalEngine/FormControls/PageTemplates/PageTemplateScopeLevels.ascx"
    TagName="SelectLevels" TagPrefix="cms" %>
<asp:Content runat="server" ContentPlaceHolderID="plcContent">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblPath" ResourceString="template.scopes.path"
                    DisplayColon="true"></cms:LocalizedLabel>
            </div>
            <div class="editing-form-value-cell">
                <cms:selectpath ID="pathElem" runat="server" IsLiveSite="false" SinglePathMode="true" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDocumentType" ResourceString="general.documenttype"
                    DisplayColon="true"></cms:LocalizedLabel>
            </div>
            <div class="editing-form-value-cell">
                <cms:SelectClass runat="server" ID="classElem" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblCulture" ResourceString="general.culture"
                    DisplayColon="true"></cms:LocalizedLabel>
            </div>
            <div class="editing-form-value-cell">
                <cms:SelectCulture runat="server" ID="cultureElem" DisplayClearButton="false" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblLevels" ResourceString="template.scopes.levels"
                    DisplayColon="true"></cms:LocalizedLabel>
            </div>
            <div class="editing-form-value-cell">
                <cms:SelectLevels runat="server" ID="levelElem" Level="10" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-value-cell editing-form-value-cell-offset">
                <cms:FormSubmitButton runat="server" ID="btnOk" ResourceString="general.ok"
                    OnClick="btnOk_Click" />
            </div>
        </div>
    </div>
</asp:Content>
