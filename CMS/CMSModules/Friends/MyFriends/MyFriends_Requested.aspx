<%@ Page Language="C#" AutoEventWireup="true" Theme="default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Inherits="CMSModules_Friends_MyFriends_MyFriends_Requested"  Codebehind="MyFriends_Requested.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsRequestedList.ascx" TagName="FriendsListRequested"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:FriendsListRequested ID="FriendsListRequested" runat="server" Visible="true"
        IsLiveSite="false" />
</asp:Content>
