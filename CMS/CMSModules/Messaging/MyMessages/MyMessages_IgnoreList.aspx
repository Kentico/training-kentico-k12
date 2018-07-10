<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Inherits="CMSModules_Messaging_MyMessages_MyMessages_IgnoreList"
    Title="My messages - Inbox" ValidateRequest="false" Theme="Default"  Codebehind="MyMessages_IgnoreList.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/IgnoreList.ascx" TagName="IgnoreList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:IgnoreList ID="ignoreListElem" ShortID="l" runat="server" IsLiveSite="false" />
</asp:Content>
