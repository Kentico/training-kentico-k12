<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Friends_Controls_Friends_Approve"  Codebehind="Friends_Approve.ascx.cs" %>

<asp:Panel ID="pnlBody" runat="server">
    <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblComment" runat="server" ResourceString="general.comment"
                    DisplayColon="true" EnableViewState="false" />
            </div>
            <div class="editing-form-value-cell">
                <cms:ExtendedTextArea ID="txtComment" runat="server"
                    Rows="4" EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ResourceString="administration.users.email"
                    DisplayColon="true" EnableViewState="false" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox ID="chkSendEmail" Checked="true" runat="server" CssClass="ContentCheckbox" EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ResourceString="sendmessage.sendmessage" DisplayColon="True" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox ID="chkSendMessage" Checked="true" runat="server" CssClass="ContentCheckbox" EnableViewState="false" />
            </div>
        </div>
    </div>

    <div class="PageFooterLine">
        <div class="FloatRight">
            <cms:LocalizedButton ID="btnApprove" OnClick="btnApprove_Click" runat="server" ResourceString="general.saveandclose"
                ButtonStyle="Primary" EnableViewState="false" />
        </div>
    </div>
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" Visible="false" />
</asp:Panel>