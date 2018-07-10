<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_Profile_ChangePassword"  Codebehind="~/CMSWebParts/Membership/Profile/ChangePassword.ascx.cs" %>

<%@ Register Src="~/CMSModules/Membership/FormControls/Passwords/PasswordStrength.ascx" TagName="PasswordStrength"
    TagPrefix="cms" %>

<asp:Panel ID="pnlWebPart" runat="server" DefaultButton="btnOk" CssClass="change-password">
    <asp:Label runat="server" ID="lblInfo" CssClass="InfoLabel" EnableViewState="false"
        Visible="false" />
    <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel" EnableViewState="false"
        Visible="false" />
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label CssClass="control-label" ID="lblOldPassword" AssociatedControlID="txtOldPassword" runat="server" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox ID="txtOldPassword" runat="server" TextMode="Password" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblNewPassword" runat="server" />
            </div>
            <div class="editing-form-value-cell">
                <cms:PasswordStrength runat="server" ID="passStrength" TextBoxClass="" AllowEmpty="true" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label CssClass="control-label" ID="lblConfirmPassword" AssociatedControlID="txtConfirmPassword" runat="server" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox ID="txtConfirmPassword" runat="server" TextMode="Password" />
            </div>
        </div>
        <div class="form-group form-group-submit">
            <cms:CMSButton ID="btnOk" runat="server" OnClick="btnOk_Click" ButtonStyle="Primary" ValidationGroup="PasswordChange" />
        </div>
    </div>
</asp:Panel>
