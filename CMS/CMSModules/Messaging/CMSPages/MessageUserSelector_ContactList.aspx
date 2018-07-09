<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_CMSPages_MessageUserSelector_ContactList" MasterPageFile="~/CMSMasterPages/LiveSite/EmptyPage.master"
    Theme="Default" EnableEventValidation="false"  Codebehind="MessageUserSelector_ContactList.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/SelectFromContactList.ascx" TagName="SelectFromContactList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="LiveSiteDialog">
        <div class="PageContent">
            <cms:SelectFromContactList ID="contactListElem" runat="server" IsLiveSite="true" />
        </div>
    </div>
</asp:Content>
