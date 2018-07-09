<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_IgnoreList"
     Codebehind="IgnoreList.ascx.cs" %>
<%@ Register Src="~/CMSModules/Membership/FormControls/Users/selectuser.ascx" TagName="SelectUser"
    TagPrefix="cms" %>
<cms:CMSUpdatePanel ID="pnlIgnoreList" runat="server">
    <ContentTemplate>
        <div class="PageContent">
            <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
            <div class="ListPanel">
                <cms:LocalizedHeading ID="headTitle" Level="4" runat="server" CssClass="listing-title" ResourceString="ignorelist.available" EnableViewState="false" />
                <cms:SelectUser ID="usUsers" runat="server" SelectionMode="Multiple" />
            </div>
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>
