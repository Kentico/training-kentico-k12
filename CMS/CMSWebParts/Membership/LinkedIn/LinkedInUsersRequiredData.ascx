<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_LinkedIn_LinkedInUsersRequiredData"  Codebehind="~/CMSWebParts/Membership/LinkedIn/LinkedInUsersRequiredData.ascx.cs" %>

<%@ Register Src="~/CMSModules/Membership/FormControls/Passwords/PasswordStrength.ascx" TagName="PasswordStrength"
    TagPrefix="cms" %>
<asp:Label runat="server" Visible="false" ID="lblInfo" EnableViewState="false" CssClass="InfoLabel" />
<asp:PlaceHolder runat="server" ID="plcForm">
    <table>
        <asp:PlaceHolder ID="plcError" runat="server" Visible="false">
            <tr>
                <td colspan="2">
                    <cms:LocalizedLabel ID="lblError" runat="server" EnableViewState="false" CssClass="ErrorLabel" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcContent" runat="server" Visible="true">
            <tr style="vertical-align: top;">
                <td>
                    <asp:Panel ID="pnlExistingUser" runat="server" CssClass="existingUser" DefaultButton="btnOkExist">
                        <h4>
                            <cms:LocalizedLabel ID="lblExistingUserLabel" ResourceString="mem.liveid.existUser"
                                EnableViewState="false" runat="server" /></h4>
                        <table>
                            <tr>
                                <td>
                                    <cms:LocalizedLabel ID="lblUserName" AssociatedControlID="txtUserName" ResourceString="general.username"
                                        DisplayColon="true" EnableViewState="false" runat="server" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtUserName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cms:LocalizedLabel ID="lblPassword" AssociatedControlID="txtPassword" ResourceString="general.password"
                                        DisplayColon="true" EnableViewState="false" runat="server" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtPassword" TextMode="Password" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <cms:LocalizedButton ID="btnOkExist" ResourceString="general.ok" runat="server" OnClick="btnOkExist_Click"
                                        EnableViewState="false" ButtonStyle="Default" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlNewUser" runat="server" CssClass="newUser" DefaultButton="btnOkNew">
                        <h4>
                            <cms:LocalizedLabel ID="lblNewUser" ResourceString="mem.liveid.newUser" runat="server"
                                EnableViewState="false" /></h4>
                        <table>
                            <tr>
                                <td>
                                    <cms:LocalizedLabel ID="lblUserNameNew" AssociatedControlID="txtUserNameNew" ResourceString="general.username"
                                        DisplayColon="true" EnableViewState="false" runat="server" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtUserNameNew" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cms:LocalizedLabel ID="lblEmail" AssociatedControlID="txtEmail" ResourceString="general.email"
                                        DisplayColon="true" EnableViewState="false" runat="server" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtEmail" runat="server" />
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="plcPasswordNew" runat="server" Visible="false">
                                <tr>
                                    <td class="FieldLabel FieldLabelTop">
                                        <cms:LocalizedLabel ID="lblPasswordNew" AssociatedControlID="passStrength" ResourceString="general.password"
                                            DisplayColon="true" EnableViewState="false" runat="server" />
                                    </td>
                                    <td>
                                        <cms:PasswordStrength runat="server" ID="passStrength" TextBoxClass="" ValidationGroup="LinkedInNewUser" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <cms:LocalizedLabel ID="lblConfirmPassword" AssociatedControlID="txtConfirmPassword"
                                            ResourceString="mem.liveid.confirmpassword" DisplayColon="true" EnableViewState="false"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <cms:CMSTextBox ID="txtConfirmPassword" TextMode="Password" runat="server" />
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <td></td>
                                <td>
                                    <cms:LocalizedButton ID="btnOkNew" ResourceString="general.ok" runat="server" OnClick="btnOkNew_Click"
                                        EnableViewState="false" ButtonStyle="Default" ValidationGroup="LinkedInNewUser" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
</asp:PlaceHolder>