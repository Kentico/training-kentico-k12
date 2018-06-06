<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Friends_Controls_Friends_Reject"  Codebehind="Friends_Reject.ascx.cs" %>

<asp:Panel ID="pnlBody" runat="server">
    <cms:MessagesPlaceholder ID="plcMess" runat="server" />
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblComment" runat="server" ResourceString="general.comment"
                    DisplayColon="true" AssociatedControlID="txtComment" />
                </div>
            <div class="editing-form-value-cell">
                <cms:ExtendedTextArea ID="txtComment" runat="server" Rows="4" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                 <cms:LocalizedLabel CssClass="control-label" runat="server" ResourceString="administration.users.email" DisplayColon="True" AssociatedControlID="chkSendEmail" />
            </div>
            <div class="editing-form-value-cell">
                 <cms:CMSCheckBox ID="chkSendEmail" runat="server" 
                    CssClass="ContentCheckbox" Checked="true" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                 <cms:LocalizedLabel CssClass="control-label" runat="server"  ResourceString="sendmessage.sendmessage" DisplayColon="True" AssociatedControlID="chkSendMessage" />
            </div>
            <div class="editing-form-value-cell">
                 <cms:CMSCheckBox ID="chkSendMessage" runat="server"
                    CssClass="ContentCheckbox" Checked="true" />
            </div>
        </div>
    </div>
    <div class="PageFooterLine">
        <div class="FloatRight">
            <cms:LocalizedButton ID="btnReject" OnClick="btnReject_Click" runat="server" ResourceString="general.saveandclose"
                ButtonStyle="Primary" />
        </div>
    </div>
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" Visible="false" />
</asp:Panel>