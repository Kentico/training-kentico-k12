<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Membership_Registration_RegistrationForm"
     Codebehind="~/CMSWebParts/Membership/Registration/RegistrationForm.ascx.cs" %>
<%@ Register Src="~/CMSModules/Membership/FormControls/Passwords/PasswordStrength.ascx" TagName="PasswordStrength"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Inputs/EmailInput.ascx" TagName="EmailInput"
    TagPrefix="cms" %>
<asp:Label ID="lblError" runat="server" ForeColor="red" EnableViewState="false" />
<asp:Label ID="lblText" runat="server" Visible="false" EnableViewState="false" />
<asp:Panel ID="pnlForm" runat="server" DefaultButton="btnOK">
    <div class="registration-form">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label CssClass="control-label" ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextBox ID="txtFirstName" EnableEncoding="true" runat="server" MaxLength="100" /><br />
                    <cms:CMSRequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                        Display="Dynamic" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label CssClass="control-label" ID="lblLastName" runat="server" AssociatedControlID="txtLastName" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextBox ID="txtLastName" EnableEncoding="true" runat="server" MaxLength="100" /><br />
                    <cms:CMSRequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                        Display="Dynamic" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label CssClass="control-label" ID="lblEmail" runat="server" AssociatedControlID="txtEmail" EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:EmailInput ID="txtEmail" runat="server" /><br />
                    <cms:CMSRequiredFieldValidator ID="rfvEmail" runat="server"
                        Display="Dynamic" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblPassword" runat="server" EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:PasswordStrength runat="server" ID="passStrength" ShowValidationOnNewLine="true" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label CssClass="control-label" ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword"
                        EnableViewState="false" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="100" /><br />
                    <cms:CMSRequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        Display="Dynamic" EnableViewState="false" />
                </div>
            </div>
            <asp:PlaceHolder runat="server" ID="plcMFIsRequired" Visible="false">
                <div class="form-group">
                    <div class="editing-form-value-cell editing-form-value-cell-offset">
                        <cms:CMSCheckBox ID="chkUseMultiFactorAutentization" runat="server" ResourceString="webparts_membership_registrationform.mfrequired" />
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plcCaptcha">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <asp:Label CssClass="control-label" ID="lblCaptcha" runat="server" AssociatedControlID="scCaptcha" EnableViewState="false" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:SecurityCode ID="scCaptcha" GenerateNumberEveryTime="false" ShowInfoLabel="False" runat="server" />
                    </div>
                </div>
            </asp:PlaceHolder>
            <div class="form-group form-group-submit">
                    <cms:CMSButton ID="btnOk" runat="server" OnClick="btnOK_Click" ButtonStyle="Default"
                        EnableViewState="false" />
            </div>
        </div>
    </div>
</asp:Panel>
