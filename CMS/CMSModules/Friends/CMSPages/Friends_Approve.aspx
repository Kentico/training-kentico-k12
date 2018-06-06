<%@ Page Language="C#" AutoEventWireup="true"
    Title="Approve friendship" Inherits="CMSModules_Friends_CMSPages_Friends_Approve"
    Theme="default" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalSimplePage.master"  Codebehind="Friends_Approve.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/Friends_Approve.ascx" TagName="FriendsApprove"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <cms:FriendsApprove ID="FriendsApprove" runat="server" Visible="true" />
    </div>
</asp:Content>
