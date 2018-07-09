<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Departments_FormControls_DepartmentRolesSelector"  Codebehind="~/CMSModules/Departments/FormControls/DepartmentRolesSelector.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector" TagPrefix="cms" %>
<cms:CMSUpdatePanel ID="pnlUpdate" runat="server">
    <ContentTemplate>
        <cms:UniSelector ObjectType="cms.role" SelectionMode="Multiple"
            OrderBy="RoleDisplayName" ResourcePrefix="roleselect" runat="server"
            ID="usRoles" />
    </ContentTemplate>
</cms:CMSUpdatePanel>