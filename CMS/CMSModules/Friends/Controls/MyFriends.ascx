<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Controls_MyFriends"  Codebehind="MyFriends.ascx.cs" %>
<%@ Register Src="~/CMSModules/Friends/Controls/FriendsList.ascx" TagName="FriendsList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Friends/Controls/FriendsToApprovalList.ascx" TagName="FriendsToApprovalList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Friends/Controls/FriendsRequestedList.ascx" TagName="FriendsRequestedList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Friends/Controls/FriendsRejectedList.ascx" TagName="FriendsRejectedList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Friends/Controls/RequestFriendship.ascx" TagName="RequestFriendship"
    TagPrefix="cms" %>
<asp:Panel runat="server" ID="pnlBody" CssClass="MyFriends">

<cms:BasicTabControl ID="tabMenu" runat="server" />

    <div class="TabsContent FriendsBox">
        <asp:PlaceHolder ID="plcFriends" runat="server">
            <div>
                <cms:RequestFriendship ID="requestFriendshipElem1" runat="server" />
            </div>
            <br />
            <h3>
                <cms:LocalizedLiteral ID="ltlFriendsList" runat="server" ResourceString="friends.mycurrentfriends"
                    EnableViewState="false" /></h3>
            <cms:FriendsList ID="friendsListElem" runat="server" />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcApprovalFriends" runat="server">
            <br />
            <br />
            <br />
            <h3>
                <cms:LocalizedLiteral ID="ltlFriendsApprovalList" runat="server" ResourceString="friends.waitingforapproval"
                    EnableViewState="false" /></h3>
            <cms:FriendsToApprovalList ID="friendsToApprovalListElem" runat="server" />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcRequestedFriends" runat="server">
            <div>
                <cms:RequestFriendship ID="requestFriendshipElem2" runat="server" />
            </div>
            <br />
            <cms:FriendsRequestedList ID="friendsRequestedListElem" runat="server" />
        </asp:PlaceHolder>
        <cms:FriendsRejectedList ID="friendsRejectedListElem" runat="server" />
    </div>
</asp:Panel>
