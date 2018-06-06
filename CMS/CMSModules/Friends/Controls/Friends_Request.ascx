<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Friends_Controls_Friends_Request"  Codebehind="Friends_Request.ascx.cs" %>

<%@ Register Src="~/CMSModules/Membership/FormControls/Users/selectuser.ascx" TagName="selectuser" TagPrefix="cms" %>

<asp:Panel ID="pnlBody" runat="server">
    <cms:MessagesPlaceholder ID="plcMess" runat="server" />
    <div class="form-horizontal">
        <asp:PlaceHolder ID="plcUserSelect" runat="server">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblUser" runat="server" ResourceString="general.user" DisplayColon="true"
                        EnableViewState="false" AssociatedControlID="selectUser" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:selectuser ID="selectUser" runat="server" Visible="true" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblComment" runat="server" ResourceString="general.comment"
                    DisplayColon="true" EnableViewState="false" AssociatedControlID="txtComment" />
            </div>
            <div class="editing-form-value-cell">
                <cms:ExtendedTextArea ID="txtComment" runat="server" Rows="7"
                    EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ResourceString="administration.users.email" runat="server" DisplayColon="True" AssociatedControlID="chkSendEmail" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox ID="chkSendEmail" Checked="true" runat="server" CssClass="ContentCheckbox" EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ResourceString="sendmessage.sendmessage" runat="server" DisplayColon="True" AssociatedControlID="chkSendMessage" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSCheckBox ID="chkSendMessage" runat="server" Checked="true" CssClass="ContentCheckbox" EnableViewState="false" />
            </div>
        </div>
        <asp:PlaceHolder ID="plcAdministrator" runat="server" Visible="false" EnableViewState="false">
            <div class="form-group">
                <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ResourceString="friends.automaticapproval" runat="server" DisplayColon="True" AssociatedControlID="chkAutomaticApprove" />
            </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox ID="chkAutomaticApprove" runat="server" Checked="false"
                        CssClass="ContentCheckbox" EnableViewState="false" />
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
    <div class="PageFooterLine">
        <div class="FloatRight">
            <cms:LocalizedButton ID="btnRequest" OnClick="btnRequest_Click" runat="server" ResourceString="general.saveandclose"
               ButtonStyle="Primary" EnableViewState="false" />
        </div>
    </div>
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" Visible="false" />
</asp:Panel>