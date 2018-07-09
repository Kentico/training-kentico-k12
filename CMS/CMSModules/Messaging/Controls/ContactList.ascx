<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_ContactList"
     Codebehind="ContactList.ascx.cs" %>
<%@ Register Src="~/CMSModules/Membership/FormControls/Users/selectuser.ascx" TagName="SelectUser"
    TagPrefix="cms" %>
<cms:CMSUpdatePanel ID="pnlContactList" runat="server">
    <ContentTemplate>
        <div class="PageContent">
            <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
            <div class="ListPanel">
                <cms:LocalizedHeading ID="headTitle" Level="4" runat="server" CssClass="listing-title" ResourceString="contactlist.available" EnableViewState="false" />
                <cms:SelectUser ID="usUsers" runat="server" SelectionMode="Multiple" />
            </div>
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>
