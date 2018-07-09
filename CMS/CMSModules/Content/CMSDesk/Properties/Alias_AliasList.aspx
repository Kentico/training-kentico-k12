<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="Alias_AliasList.aspx.cs"
    Inherits="CMSModules_Content_CMSDesk_Properties_Alias_AliasList" Theme="Default"
    MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<asp:Content runat="server" ID="cntServer" ContentPlaceHolderID="plcContent">
    <cms:UniGrid ID="ugAlias" runat="server" IsLiveSite="false" OrderBy="AliasUrlPath" ShowActionsMenu="true">
        <GridActions Parameters="AliasID">
            <ug:Action Name="edit" Caption="$General.edit$" FontIconClass="icon-edit" FontIconStyle="Allow" CommandArgument="AliasID" />
            <ug:Action Name="delete" Caption="$General.Delete$" FontIconClass="icon-bin" FontIconStyle="Critical" Confirmation="$General.ConfirmDelete$"
                CommandArgument="AliasID" />
        </GridActions>
        <GridColumns>
            <ug:Column Source="AliasURLPath" Caption="$list.urlalias$" Wrap="false">
                <Filter Type="text" Source="AliasURLPath" />
            </ug:Column>
            <ug:Column Source="##ALL##" Caption="$general.documentname$" Wrap="false" Sort="NodeName"
                ExternalSourceName="documentName">
                <Filter Type="text" Source="NodeName" />
            </ug:Column>
            <ug:Column Source="AliasExtensions" Caption="$doc.urls.urlextensions$" Wrap="false" />
            <ug:Column Source="DocumentCulture" Caption="$general.language$" Wrap="false" CssClass="main-column-100"
                ExternalSourceName="culture" Sort="AliasCulture" />
        </GridColumns>
        <GridOptions DisplayFilter="true" FilterLimit="0" />
    </cms:UniGrid>
</asp:Content>
