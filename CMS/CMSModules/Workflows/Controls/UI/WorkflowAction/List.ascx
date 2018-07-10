<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="CMSModules_Workflows_Controls_UI_WorkflowAction_List"  Codebehind="List.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<cms:UniGrid runat="server" ID="gridElem" OrderBy="ActionDisplayName"
    Columns="ActionID,ActionDisplayName,ActionEnabled,ActionAllowedObjects" ObjectType="cms.workflowaction" IsLiveSite="false">
    <GridActions Parameters="ActionID">
        <ug:Action Name="edit" Caption="$General.Edit$" FontIconClass="icon-edit" FontIconStyle="Allow" />
        <ug:Action Name="#delete" Caption="$General.Delete$" FontIconClass="icon-bin" FontIconStyle="Critical" Confirmation="$General.ConfirmDelete$" CommandArgument="ActionID" />
    </GridActions>
    <GridColumns>
        <ug:Column Source="ActionDisplayName" Caption="$cms.workflowaction.displayNameColumn$" Wrap="false">
            <Filter Type="text" />
        </ug:Column>
        <ug:Column Source="ActionAllowedObjects" Name="scope" Caption="$workflow.scope$" Wrap="false" ExternalSourceName="scope" AllowSorting="false" />
        <ug:Column Source="ActionEnabled" ExternalSourceName="enabled" Caption="$general.enabled$" Wrap="false">
            <Filter Type="bool" />
        </ug:Column>        
        <ug:Column CssClass="filling-column" />
    </GridColumns>
    <GridOptions DisplayFilter="true" />
</cms:UniGrid>
