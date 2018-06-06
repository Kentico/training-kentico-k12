<%@ Page Language="C#" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" AutoEventWireup="true"
    Theme="Default" Inherits="CMSModules_Friends_Dialogs_MessageUserSelector_FriendsList"  Codebehind="MessageUserSelector_FriendsList.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsUserList.ascx" TagName="FriendsUserList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <cms:FriendsUserList ID="friendsUserListElem" runat="server" IsLiveSite="false" />
        <br class="ClearBoth" />
    </div>
</asp:Content>
