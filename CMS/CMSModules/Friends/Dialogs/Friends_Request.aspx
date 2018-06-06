<%@ Page Language="C#" AutoEventWireup="true"
    Title="Add a new friend" Inherits="CMSModules_Friends_Dialogs_Friends_Request"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalSimplePage.master"  Codebehind="Friends_Request.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/Friends_Request.ascx" TagName="FriendsRequest"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <cms:FriendsRequest ID="FriendsRequest" IsLiveSite="false" runat="server" Visible="true" />
    </div>
</asp:Content>