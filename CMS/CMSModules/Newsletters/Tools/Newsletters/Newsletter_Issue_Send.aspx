<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Newsletter_Issue_Send.aspx.cs"
    Inherits="CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_Send" Theme="Default"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/Newsletters/Controls/SendIssue.ascx" TagPrefix="cms"
    TagName="SendIssue" %>
<%@ Register Src="~/CMSModules/Newsletters/Controls/SendVariantIssue.ascx" TagPrefix="cms"
    TagName="SendVariant" %>
<%@ Register Src="~/CMSModules/Newsletters/Controls/SendIssueTemplateBased.ascx" TagPrefix="cms"
    TagName="SendIssueTemplateBased" %>
<%@ Register Src="~/CMSAdminControls/UI/SmartTip.ascx" TagPrefix="cms"
    TagName="SmartTip" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:CMSUpdatePanel ID="pnlU" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <cms:MessagesPlaceHolder runat="server" ID="plcMess" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>

    <cms:AlertLabel runat="server" ID="lblUrlWarning" AlertType="Warning" Visible="False" CssClass="hide" EnableViewState="false" />

    <cms:SendIssue ID="sendElem" runat="server" ShowScheduler="false" ShowSendDraft="true"
        ShowSendLater="false" ShortID="s" Visible="false" />
    <cms:SendIssueTemplateBased ID="sendElem_TemplateBased" runat="server" ShortID="si_tb" Visible="false" Mode="Send" />
    <cms:SendVariant ID="sendVariant" runat="server" ShortID="sv" Visible="false" />
    <cms:SmartTip ID="stChecklist" runat="server" EnableViewState="false" Content="{$newsletter.issue.smarttip.content$}"
        ExpandedHeader="{$newsletter.issue.smarttip.header$}" CollapsedHeader="{$newsletter.issue.smarttip.header$}" />
</asp:Content>
