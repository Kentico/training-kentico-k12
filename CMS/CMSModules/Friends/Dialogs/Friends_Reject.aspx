<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Dialogs_Friends_Reject"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalSimplePage.master"
    Title="Reject friendship"  Codebehind="Friends_Reject.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/Friends_Reject.ascx" TagName="FriendsReject"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <cms:FriendsReject ID="FriendsReject" IsLiveSite="false" runat="server" Visible="true" />
    </div>
</asp:Content>