<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Membership_Pages_Users_General_User_Online"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="Default" Title="Users - online users" EnableEventValidation="false"  Codebehind="User_Online.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel ID="pnlUsers" runat="server" CssClass="UsersList">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" Visible="false" />
        <cms:UniGrid ID="gridElem" runat="server" GridName="User_OnlineSession.xml" OrderBy="UserName"
            Columns="UserID, UserName, FullName, Email, UserNickName, UserCreated, UserPrivilegeLevel"
            IsLiveSite="false" ShowObjectMenu="false" RememberStateByParam="" />
        <br />
        <asp:Label runat="server" ID="lblGeneralInfo" EnableViewState="false" CssClass="UsersOnline" />
    </asp:Panel>
</asp:Content>
