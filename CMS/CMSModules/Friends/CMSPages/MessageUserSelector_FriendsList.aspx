<%@ Page Language="C#" MasterPageFile="~/CMSMasterPages/LiveSite/EmptyPage.master"
    AutoEventWireup="true" Theme="Default"
    Inherits="CMSModules_Friends_CMSPages_MessageUserSelector_FriendsList"  Codebehind="MessageUserSelector_FriendsList.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsUserList.ascx" TagName="FriendsList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="LiveSiteDialog">
        <div class="PageContent">
            <cms:FriendsList ID="friendsListElem" runat="server" />
        </div>
    </div>
</asp:Content>
