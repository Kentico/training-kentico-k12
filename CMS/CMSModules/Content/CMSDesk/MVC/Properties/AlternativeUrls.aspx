<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="AlternativeUrls.aspx.cs" Inherits="CMSModules_Content_CMSDesk_MVC_Properties_AlternativeUrls"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<%@ Register Namespace="CMS.FormEngine.Web.UI" TagPrefix="cms" Assembly="CMS.FormEngine.Web.UI" %>

<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:CMSInfoPanel ID="pnlCannotManageAlternativeUrl" runat="server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <asp:Panel ID="pnlAddAlternativeUrl" runat="server">
        <cms:LocalizedHeading runat="server" ID="headTitle" Level="4" ResourceString="content.ui.propertiesalternativeurls" EnableViewState="false" />
        <div class="form-horizontal form-filter alternativeurl-form">
            <div class="form-group">
                <div class="filter-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblAltUrl" runat="server" EnableViewState="false" 
                        ResourceString="alternativeurl.newurl" DisplayColon="true" />
                </div>
                <div class="filter-form-value-cell-wide">
                    <cms:TextBoxWithPlaceholder ID="txtAltUrl" runat="server" MaxLength="450" />
                </div>
            </div>
            <div class="form-group form-group-buttons">
                <cms:LocalizedButton ID="btnOk" runat="server" OnClick="btnOk_Click" ResourceString="general.add" EnableViewState="false" ButtonStyle="Primary" />
            </div>
        </div>
    </asp:Panel>
    <cms:LocalizedHeading runat="server" ID="headGrid" Level="4" ResourceString="alternativeurl.gridtitle" EnableViewState="false" DisplayColon="true" />
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server">
        <ContentTemplate>
            <cms:UniGrid ID="gridUrls" ShortID="g" runat="server" ObjectType="cms.alternativeurl" IsLiveSite="false" Columns="AlternativeUrlID, AlternativeUrlUrl"
                OrderBy="AlternativeUrlUrl" ShowExportMenu="true" ShowObjectMenu="false" OnOnAction="gridUrls_OnAction" OnOnExternalDataBound="gridUrls_OnExternalDataBound">
                <GridActions>
                    <ug:Action Name="edit" Caption="$general.edit$" FontIconClass="icon-edit" FontIconStyle="Allow" />
                    <ug:Action Name="delete" Caption="$General.Delete$" FontIconClass="icon-bin" FontIconStyle="Critical" Confirmation="$general.confirmdelete$" />
                    <ug:Action Name="openurl" Caption="$alternativeurl.openurl$" FontIconClass="icon-eye" FontIconStyle="Allow" ExternalSourceName="openurl" />
                </GridActions>
                <GridColumns>
                    <ug:Column Source="##ALL##" Caption="$general.url$" ExternalSourceName="url" Wrap="false" MaxLength="150" Width="100%" />
                </GridColumns>
                <GridOptions DisplayFilter="false" AllowSorting="false" />
            </cms:UniGrid>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Content>