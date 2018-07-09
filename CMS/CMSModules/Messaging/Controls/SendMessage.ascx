<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_SendMessage"  Codebehind="SendMessage.ascx.cs" %>
<%@ Register Src="~/CMSModules/Messaging/FormControls/MessageUserSelector.ascx" TagName="MessageUserSelector"
    TagPrefix="cms" %>

<asp:Panel ID="pnlSendMessage" runat="server" CssClass="send-message">
    <cms:MessagesPlaceHolder ID="plcMess" runat="server" LiveSiteOnly="true" />
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblFromCaption" runat="server" ResourceString="Messaging.From"
                    EnableViewState="false" DisplayColon="true" AssociatedControlID="lblFrom" />
            </div>
            <div class="editing-form-value-cell">
                <asp:Label ID="lblFrom" CssClass="form-control-text" runat="server" EnableViewState="false" />
                <cms:CMSTextBox ID="txtFrom" runat="server" Visible="false" EnableViewState="false"
                    MaxLength="200" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblToCaption" runat="server" ResourceString="Messaging.To"
                    EnableViewState="false" DisplayColon="true" AssociatedControlID="lblTo" />
            </div>
            <div class="editing-form-value-cell">
                <asp:Label ID="lblTo" runat="server" EnableViewState="false" CssClass="form-control-text" />
                <cms:MessageUserSelector ID="ucMessageUserSelector" runat="server" Visible="false"
                    EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel ID="lblSubjectCaption" runat="Server" CssClass="control-label"
                    EnableViewState="false" ResourceString="general.subject" AssociatedControlID="txtSubject"
                    DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox ID="txtSubject" runat="server" EnableViewState="false"
                    MaxLength="200" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel ID="lblText" runat="server" Display="false" EnableViewState="false"
                    ResourceString="messaging.body" />
            </div>
            <div class="editing-form-value-cell textarea-full-width">
                <cms:BBEditor ID="ucBBEditor" runat="server" EnableViewState="false" Rows="20"/>
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-value-cell">
                <cms:LocalizedButton ID="btnSendMessage" runat="server" ButtonStyle="Primary" ResourceString="general.send"
                    EnableViewState="false" OnClick="btnSendMessage_Click" />
                <cms:LocalizedButton ID="btnClose" runat="server" ButtonStyle="Primary" Visible="false"
                    ResourceString="general.cancel" EnableViewState="false" OnClick="btnClose_Click" />
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlNoUser" runat="server" Visible="false" EnableViewState="false">
    <asp:Label ID="lblNoUser" CssClass="Info" runat="server" EnableViewState="false" />
</asp:Panel>
<asp:HiddenField ID="hdnUserId" runat="server" />
