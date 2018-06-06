<%@ Page Language="C#" Theme="default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    AutoEventWireup="true" Inherits="CMSModules_Friends_MyFriends_MyFriends_Rejected"  Codebehind="MyFriends_Rejected.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsRejectedList.ascx" TagName="FriendsListRejected"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:FriendsListRejected ID="FriendsListRejected" runat="server" Visible="true" IsLiveSite="false" />
</asp:Content>
