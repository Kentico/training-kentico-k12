<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Community_Friends_FriendsList"  Codebehind="~/CMSWebParts/Community/Friends/FriendsList.ascx.cs" %>
<%@ Register Src="~/CMSModules/Friends/Controls/FriendsList.ascx" TagName="FriendsList"
    TagPrefix="cms" %>
<cms:FriendsList ID="lstFriends" runat="server" />