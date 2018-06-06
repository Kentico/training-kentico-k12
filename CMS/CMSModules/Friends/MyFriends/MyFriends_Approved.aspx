<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Friends_MyFriends_MyFriends_Approved" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Theme="default"  Codebehind="MyFriends_Approved.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsList.ascx" TagName="FriendsList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:FriendsList ID="FriendsList" runat="server" Visible="true" IsLiveSite="false" />
</asp:Content>
