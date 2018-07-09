<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_Dialogs_MessageUserSelector_ContactList" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master"
    Theme="Default" EnableEventValidation="false"  Codebehind="MessageUserSelector_ContactList.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/SelectFromContactList.ascx" TagName="ContactList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent">
        <cms:ContactList ID="contactListElem" runat="server" IsLiveSite="false" />
        <br class="ClearBoth" />
    </div>
</asp:Content>
