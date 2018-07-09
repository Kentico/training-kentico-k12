<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_PageTemplates_Scopes_PageTemplateScopes_List"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Page Template Edit - Scopes list"
    Theme="Default"  Codebehind="PageTemplateScopes_List.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SiteSelector"
    TagPrefix="cms" %>
<asp:Content ID="cntSiteSelector" ContentPlaceHolderID="plcSiteSelector" runat="server">
    <asp:Panel ID="pnlSite" runat="server">
        <div class="form-horizontal form-filter">
            <div class="form-group">
                <div class="filter-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblSite" DisplayColon="true" ResourceString="general.site"
                        EnableViewState="false" />
                </div>
                <div class="filter-form-value-cell-wide">
                    <cms:SiteSelector runat="server" ID="selectSite" IsLiveSite="false" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblPageTemplateUsage" DisplayColon="true" ResourceString="template.scopes.templatecanbeused"
                    EnableViewState="false"></cms:LocalizedLabel>
            </div>
            <div class="editing-form-value-cell">
                <div class="radio-list-vertical">
                    <cms:CMSRadioButton ID="radAllPages" runat="server" GroupName="groupTemplate" Checked="true"
                        ResourceString="template.scopes.allpages" AutoPostBack="true" OnCheckedChanged="radAllPages_CheckedChanged" />
                    <cms:CMSRadioButton ID="radSelectedScopes" runat="server" GroupName="groupTemplate"
                        ResourceString="template.scopes.selectedscopes" AutoPostBack="true" OnCheckedChanged="radSelectedScopes_CheckedChanged" />
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlContent" runat="server">
        <cms:CMSUpdatePanel runat="server" ID="pnlUpdate" UpdateMode="Conditional">
            <ContentTemplate>
                <cms:UniGrid runat="server" ID="unigridScopes" GridName="PageTemplateScopes_List.xml"
                    IsLiveSite="false" OrderBy="PageTemplateScopePath" />
            </ContentTemplate>
        </cms:CMSUpdatePanel>
    </asp:Panel>
</asp:Content>
