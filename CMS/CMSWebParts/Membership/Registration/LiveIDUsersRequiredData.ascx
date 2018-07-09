<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Membership_Registration_LiveIDUsersRequiredData"
     Codebehind="~/CMSWebParts/Membership/Registration/LiveIDUsersRequiredData.ascx.cs" %>
<%@ Register Src="~/CMSModules/Membership/FormControls/Passwords/PasswordStrength.ascx"
    TagName="PasswordStrength" TagPrefix="cms" %>
<asp:Label runat="server" Visible="false" ID="lblInfo" EnableViewState="false" CssClass="InfoLabel" />
<asp:PlaceHolder runat="server" ID="plcForm">
    <asp:PlaceHolder ID="plcError" runat="server" Visible="false">
        <div class="form-group">
            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="ErrorLabel" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcContent" runat="server" Visible="true">
        <asp:Panel ID="pnlExistingUser" runat="server" CssClass="existing-user" DefaultButton="btnOkExist">
            <h4>
                <cms:LocalizedLabel ID="lblExistingUserLabel" ResourceString="mem.liveid.existUser"
                    EnableViewState="false" runat="server" /></h4>
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblUserName" AssociatedControlID="txtUserName" ResourceString="general.username"
                            DisplayColon="true" EnableViewState="false" runat="server" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtUserName" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblPassword" AssociatedControlID="txtPassword" ResourceString="general.password"
                            DisplayColon="true" EnableViewState="false" runat="server" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtPassword" TextMode="Password" runat="server" />
                    </div>
                </div>
                <div class="form-group form-group-submit">
                        <cms:LocalizedButton ID="btnOkExist" ResourceString="general.ok" runat="server" OnClick="btnOkExist_Click"
                            EnableViewState="false" ButtonStyle="Default" />
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlNewUser" runat="server" CssClass="new-user" DefaultButton="btnOkNew">
            <h4>
                <cms:LocalizedLabel ID="lblNewUser" ResourceString="mem.liveid.newUser" runat="server"
                    EnableViewState="false" /></h4>
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblUserNameNew" AssociatedControlID="txtUserNameNew" ResourceString="general.username"
                            DisplayColon="true" EnableViewState="false" runat="server" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtUserNameNew" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblEmail" AssociatedControlID="txtEmail" ResourceString="general.email"
                            DisplayColon="true" EnableViewState="false" runat="server" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtEmail" runat="server" />
                    </div>
                </div>
                <asp:PlaceHolder ID="plcPasswordNew" runat="server" Visible="false">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblPasswordNew" ResourceString="general.password"
                                DisplayColon="true" EnableViewState="false" runat="server" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:PasswordStrength runat="server" ID="passStrength" TextBoxClass="" ValidationGroup="LiveIDNewUser" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblConfirmPassword" AssociatedControlID="txtConfirmPassword"
                                ResourceString="mem.liveid.confirmpassword" DisplayColon="true" EnableViewState="false"
                                runat="server" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtConfirmPassword" TextMode="Password" runat="server" />
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div class="form-group form-group-submit">
                    <cms:LocalizedButton ID="btnOkNew" ResourceString="general.ok" runat="server" OnClick="btnOkNew_Click"
                        EnableViewState="false" ButtonStyle="Default" ValidationGroup="LiveIDNewUser" />
                </div>
            </div>
        </asp:Panel>
    </asp:PlaceHolder>
</asp:PlaceHolder>
