<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Membership_Pages_Users_General_User_Kicked"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="Default" Title="Users - kicked users"
     Codebehind="User_Kicked.aspx.cs" EnableEventValidation="false" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel ID="pnlUsers" runat="server" CssClass="UsersList">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" />
        <cms:UniGrid ID="gridElem" runat="server" GridName="User_OnlineSession.xml" OrderBy="UserName"
            Columns="UserID, UserName, FullName, Email, UserNickName, UserCreated, UserPrivilegeLevel"
            IsLiveSite="false" ShowObjectMenu="false" RememberStateByParam="" />
    </asp:Panel>
</asp:Content>
