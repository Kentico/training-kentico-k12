<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_Controls_SelectFromContactList"  Codebehind="SelectFromContactList.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<cms:CMSUpdatePanel ID="pnlContactList" runat="server">
    <ContentTemplate>
        <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
        <div class="ListPanel">
            <cms:UniGrid ID="gridContactList" runat="server" GridName="~/CMSModules/Messaging/Controls/SelectFromContactList.xml"
                OrderBy="UserName" />
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>
