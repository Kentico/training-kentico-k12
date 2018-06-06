<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="default" Inherits="CMSModules_Friends_MyFriends_MyFriends_ToApprove"  Codebehind="MyFriends_ToApprove.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsToApprovalList.ascx" TagName="FriendsListToApprove"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:FriendsListToApprove ID="FriendsListToApprove" runat="server" Visible="true"
        IsLiveSite="false" />
</asp:Content>
