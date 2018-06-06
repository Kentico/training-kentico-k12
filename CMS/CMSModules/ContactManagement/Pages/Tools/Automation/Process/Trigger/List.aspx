<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="List.aspx.cs" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Objectworkflowtrigger list" Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Process_Trigger_List"
    Theme="Default" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/HeaderActions.ascx" TagName="HeaderActions"
    TagPrefix="cms" %>

<asp:Content ID="cntActions" runat="server" ContentPlaceHolderID="plcActions">
    <cms:CMSUpdatePanel ID="pnlActons" runat="server">
        <ContentTemplate>
            <div class="control-group-inline header-actions-container">
                <cms:HeaderActions ID="headerActions" runat="server" IsLiveSite="false" />
            </div>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UniGrid runat="server" ID="gridElem" ObjectType="cms.objectworkflowtrigger"
        OrderBy="TriggerDisplayName" Columns="TriggerID, TriggerMacroCondition, TriggerType, TriggerDisplayName, TriggerObjectType, TriggerTargetObjectID, TriggerTargetObjectType, TriggerParameters"
        IsLiveSite="false" EditActionUrl="Edit.aspx?objectworkflowtriggerId={0}">
        <GridActions Parameters="TriggerID">
            <ug:Action Name="edit" Caption="$General.Edit$" FontIconClass="icon-edit" FontIconStyle="Allow" />
            <ug:Action Name="#delete" ExternalSourceName="delete" Caption="$General.Delete$"
                FontIconClass="icon-bin" FontIconStyle="Critical" Confirmation="$General.ConfirmDelete$" ModuleName="CMS.OnlineMarketing" Permissions="ManageProcesses" />
        </GridActions>
        <GridColumns>
            <ug:Column Source="TriggerDisplayName" Caption="$ma.trigger.name$" Wrap="false">
                <Filter Type="text" />
            </ug:Column>
            <ug:Column Source="##ALL##" Caption="$ma.trigger.type$" Wrap="false" ExternalSourceName="type"
                AllowSorting="false" />
            <ug:Column Source="TriggerMacroCondition" Caption="$ma.trigger.macrocondition$" Wrap="false"
                AllowSorting="false" ExternalSourceName="condition" />
            <ug:Column CssClass="filling-column" />
        </GridColumns>
        <GridOptions DisplayFilter="true" />
    </cms:UniGrid>
</asp:Content>
