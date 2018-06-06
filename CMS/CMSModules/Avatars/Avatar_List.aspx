<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Avatars_Avatar_List"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Avatars - List"  Codebehind="Avatar_List.aspx.cs" %>

<%@ Register src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" tagname="UniGrid" tagprefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UniGrid runat="server" ID="unigridAvatarList" GridName="Avatar_List.xml" IsLiveSite="false" />
</asp:Content>