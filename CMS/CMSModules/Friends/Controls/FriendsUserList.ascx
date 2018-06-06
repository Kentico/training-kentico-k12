<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Friends_Controls_FriendsUserList"  Codebehind="FriendsUserList.ascx.cs" %>
<%@ Register src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" tagname="UniGrid" tagprefix="cms" %>
<div class="ListPanel">
    <cms:UniGrid id="gridFriends" runat="server" GridName="~/CMSModules/Friends/Controls/FriendsUserList.xml" HideFilterButton="true" />
</div>
