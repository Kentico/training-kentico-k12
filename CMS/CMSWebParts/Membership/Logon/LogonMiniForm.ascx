<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="CMSWebParts_Membership_Logon_LogonMiniForm"  Codebehind="~/CMSWebParts/Membership/Logon/LogonMiniForm.ascx.cs" %>
<asp:Login ID="loginElem" runat="server" DestinationPageUrl="~/Default.aspx" EnableViewState="true">
    <LayoutTemplate>
        <asp:Panel ID="pnlLogonMiniForm" runat="server" DefaultButton="btnLogon" EnableViewState="true" CssClass="form-minilogon">
            <asp:PlaceHolder runat="server" ID="plcTokenInfo" Visible="false">
                <cms:LocalizedLabel ID="lblTokenInfo" runat="server" ResourceString="mfauthentication.minilogon.token.get" />
                <cms:LocalizedLabel ID="lblTokenID" runat="server" />
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plcLoginInputs">
                <cms:LocalizedLabel ID="lblUserName" runat="server" AssociatedControlID="UserName" EnableViewState="false" DisplayColon="true" />
                <cms:CMSTextBox ID="UserName" runat="server" EnableViewState="true" />
                <cms:CMSRequiredFieldValidator ID="rfvUserNameRequired" runat="server" ControlToValidate="UserName"
                    Display="Dynamic" EnableViewState="false" />
                <cms:LocalizedLabel ID="lblPassword" runat="server" AssociatedControlID="Password" EnableViewState="false" DisplayColon="true" />
                <cms:CMSTextBox ID="Password" runat="server" TextMode="Password" EnableViewState="true" />
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plcPasscodeBox" Visible="false">
                <cms:LocalizedLabel ID="lblPasscode" runat="server" AssociatedControlID="txtPasscode" ResourceString="mfauthentication.label.passcode" />
                <cms:CMSTextBox ID="txtPasscode" runat="server" MaxLength="100" />
            </asp:PlaceHolder>
            <cms:LocalizedButton ID="btnLogon" runat="server" ResourceString="LogonForm.LogOnButton" CommandName="Login" EnableViewState="false" />
            <asp:ImageButton ID="btnImageLogon" runat="server" Visible="false" CommandName="Login"
                EnableViewState="false" />
            <cms:LocalizedLabel ID="FailureText" CssClass="ErrorLabel" runat="server" EnableViewState="false" />
        </asp:Panel>
    </LayoutTemplate>
</asp:Login>
