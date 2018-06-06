<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_ShoppingCart_ShoppingCartCheckRegistration"
     Codebehind="ShoppingCartCheckRegistration.ascx.cs" %>
<%@ Register Src="~/CMSModules/Membership/FormControls/Passwords/PasswordStrength.ascx"
    TagName="PasswordStrength" TagPrefix="cms" %>

<cms:LocalizedHeading runat="server" ID="headTitle" Level="3" ResourceString="ShoppingCart.CheckRegistrationEdit" EnableViewState="false" />
<div class="BlockContent">
    <asp:Label ID="lblError" runat="server" EnableViewState="false" Visible="false" CssClass="ErrorLabel" />
    <%--Sign In--%>
    <asp:PlaceHolder ID="plcAccount" runat="server">
        <table>
            <tr>
                <td colspan="3">
                    <cms:CMSRadioButton ID="radSignIn" runat="server" GroupName="RadButtons" Checked="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblSignIn">
                        <tr>
                            <td rowspan="4" style="width: 25px;">&nbsp;
                            </td>
                            <td class="FieldLabel">
                                <asp:Label ID="lblUsername" AssociatedControlID="txtUsername" runat="server" EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSTextBox ID="txtUsername" runat="server" MaxLength="100" EnableViewState="false" />
                                <asp:Label ID="lblMark1" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FieldLabel">
                                <asp:Label ID="lblPsswd1" AssociatedControlID="txtPsswd1" runat="server" EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSTextBox ID="txtPsswd1" runat="server" TextMode="password" MaxLength="100" EnableViewState="false" />
                                <asp:Label ID="lblMark2" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkPasswdRetrieval" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlPasswdRetrieval" runat="server" CssClass="LoginPanelPasswordRetrieval">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPasswdRetrieval" AssociatedControlID="txtPasswordRetrieval" runat="server"
                                                    EnableViewState="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cms:CMSTextBox ID="txtPasswordRetrieval" runat="server" EnableViewState="false" MaxLength="100" />
                                                <asp:Label ID="lblMark3" runat="server" EnableViewState="false" />
                                                <cms:CMSButton ID="btnPasswdRetrieval" runat="server" ValidationGroup="PsswdRetrieval"
                                                    ButtonStyle="Primary" CssClass="ButtonSendPassword" EnableViewState="false" />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:PlaceHolder ID="plcResult" Visible="false" runat="server" EnableViewState="false">
                                                    <asp:Label ID="lblResult" runat="server" EnableViewState="false" CssClass="InfoLabel" />
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="plcErrorResult" Visible="false" runat="server" EnableViewState="false">
                                                    <asp:Label ID="lblErrorResult" runat="server" EnableViewState="false" CssClass="ErrorLabel" />
                                                </asp:PlaceHolder>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--END: Sign In--%>
            <%--New registration--%>
            <tr>
                <td colspan="3">
                    <cms:CMSRadioButton ID="radNewReg" runat="server" GroupName="RadButtons" />
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblRegistration">
                        <tr>
                            <td rowspan="4" style="width: 25px;">&nbsp;
                            </td>
                            <td class="FieldLabel">
                                <asp:Label ID="lblFirstName1" AssociatedControlID="txtFirstName1" runat="server"
                                    EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSTextBox ID="txtFirstName1" runat="server" MaxLength="100" EnableViewState="false" />
                                <asp:Label ID="lblMark4" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FieldLabel">
                                <asp:Label ID="lblLastName1" runat="server" AssociatedControlID="txtLastName1" EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSTextBox ID="txtLastName1" runat="server" MaxLength="100" EnableViewState="false" />
                                <asp:Label ID="lblMark5" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FieldLabel">
                                <asp:Label ID="lblEmail2" runat="server" AssociatedControlID="txtEmail2" EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSTextBox ID="txtEmail2" runat="server" MaxLength="100" EnableViewState="false" />
                                <asp:Label ID="lblMark6" runat="server" EnableViewState="false" />
                                <asp:Label ID="lblEmail2Err" runat="server" EnableViewState="false" Visible="false"
                                    CssClass="LineErrorLabel" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FieldLabel">
                                <asp:Label ID="lblCorporateBody" AssociatedControlID="chkCorporateBody" runat="server"
                                    EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSCheckBox runat="server" ID="chkCorporateBody" AutoPostBack="true" OnCheckedChanged="chkCorporateBody_CheckChanged" />
                            </td>
                        </tr>
                        <asp:Panel runat="server" ID="pnlCompanyAccount1" Visible="false">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="FieldLabel">
                                    <cms:LocalizedLabel ID="lblCompany1" AssociatedControlID="txtCompany1" runat="server"
                                        EnableViewState="false" ResourceString="com.companyname" DisplayColon="true" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtCompany1" runat="server" MaxLength="100" EnableViewState="false" />
                                    <asp:Label ID="lblMark15" runat="server" EnableViewState="false" Visible="false" />
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="plcOrganizationID" runat="server" Visible="false" EnableViewState="false">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="FieldLabel">
                                        <asp:Label ID="lblOrganizationID" AssociatedControlID="txtOrganizationID" runat="server"
                                            EnableViewState="false" />
                                    </td>
                                    <td>
                                        <cms:CMSTextBox ID="txtOrganizationID" runat="server" MaxLength="50" EnableViewState="false" />
                                        <asp:Label ID="lblMark16" runat="server" EnableViewState="false" Visible="false" />
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plcTaxRegistrationID" runat="server" Visible="false" EnableViewState="false">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="FieldLabel">
                                        <asp:Label ID="lblTaxRegistrationID" AssociatedControlID="txtTaxRegistrationID" runat="server"
                                            EnableViewState="false" />
                                    </td>
                                    <td>
                                        <cms:CMSTextBox ID="txtTaxRegistrationID" runat="server" MaxLength="50" EnableViewState="false" />
                                        <asp:Label ID="lblMark17" runat="server" EnableViewState="false" Visible="false" />
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </asp:Panel>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td class="FieldLabel FieldLabelTop">
                                <asp:Label ID="lblPsswd2" runat="server" AssociatedControlID="passStrength" EnableViewState="false" />
                            </td>
                            <td>
                                <cms:PasswordStrength runat="server" ID="passStrength" />
                                <div>
                                    <asp:Label ID="lblPsswdErr" runat="server" Visible="false" EnableViewState="false"
                                        CssClass="LineErrorLabel" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td class="FieldLabel">
                                <asp:Label ID="lblConfirmPsswd" AssociatedControlID="txtConfirmPsswd" runat="server"
                                    EnableViewState="false" />
                            </td>
                            <td>
                                <cms:CMSTextBox ID="txtConfirmPsswd" runat="server" TextMode="password" MaxLength="100" EnableViewState="false" />
                                <asp:Label ID="lblMark8" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--END: New registration--%>
            <%--Continue as anonymous--%>
            <asp:PlaceHolder ID="plhAnonymous" runat="server" Visible="false">
                <tr>
                    <td colspan="3">
                        <cms:CMSRadioButton ID="radAnonymous" runat="server" GroupName="RadButtons" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tblAnonymous">
                            <tr>
                                <td rowspan="5" style="width: 25px;">&nbsp;
                                </td>
                                <td class="FieldLabel">
                                    <asp:Label ID="lblFirstName2" AssociatedControlID="txtFirstName2" runat="server"
                                        EnableViewState="false" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtFirstName2" runat="server" MaxLength="100" EnableViewState="false" />
                                    <asp:Label ID="lblMark9" runat="server" EnableViewState="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="FieldLabel">
                                    <asp:Label ID="lblLastName2" AssociatedControlID="txtLastName2" runat="server" EnableViewState="false" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtLastName2" runat="server" MaxLength="100" EnableViewState="false" />
                                    <asp:Label ID="lblMark10" runat="server" EnableViewState="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="FieldLabel">
                                    <cms:LocalizedLabel ID="lblEmail3" AssociatedControlID="txtEmail3" runat="server"
                                        EnableViewState="false" ResourceString="general.email" DisplayColon="true" />
                                </td>
                                <td>
                                    <cms:CMSTextBox ID="txtEmail3" runat="server" MaxLength="100" EnableViewState="false" />
                                    <asp:Label ID="lblMark11" runat="server" EnableViewState="false" />
                                    <asp:Label ID="lblEmail3Err" runat="server" EnableViewState="false" Visible="false"
                                        CssClass="LineErrorLabel" />
                                </td>
                            </tr>
                            <tr>
                                <td class="FieldLabel">
                                    <cms:LocalizedLabel ID="lblCorporateBody2" AssociatedControlID="chkCorporateBody2"
                                        runat="server" EnableViewState="false" ResourceString="shoppingcartcheckregistration.companyrequired" />
                                </td>
                                <td>
                                    <cms:CMSCheckBox runat="server" ID="chkCorporateBody2" AutoPostBack="true" OnCheckedChanged="chkCorporateBody2_CheckChanged" />
                                </td>
                            </tr>
                            <asp:PlaceHolder runat="server" ID="plcCompanyAccount3" Visible="false">
                                <tr>
                                    <td class="FieldLabel">
                                        <cms:LocalizedLabel ID="lblCompany2" AssociatedControlID="txtCompany2" runat="server"
                                            EnableViewState="false" ResourceString="com.companyname" DisplayColon="true" />
                                    </td>
                                    <td>
                                        <cms:CMSTextBox ID="txtCompany2" runat="server" MaxLength="100" EnableViewState="false" />
                                        <asp:Label ID="lblMark21" runat="server" EnableViewState="false" Visible="false" />
                                    </td>
                                </tr>
                                <asp:PlaceHolder ID="plcOrganizationID2" runat="server" Visible="false" EnableViewState="false">
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="FieldLabel">
                                            <asp:Label ID="lblOrganizationID2" AssociatedControlID="txtOrganizationID2" runat="server"
                                                EnableViewState="false" />
                                        </td>
                                        <td>
                                            <cms:CMSTextBox ID="txtOrganizationID2" runat="server" MaxLength="50" EnableViewState="false" />
                                            <asp:Label ID="lblMark22" runat="server" EnableViewState="false" Visible="false" />
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="plcTaxRegistrationID2" runat="server" Visible="false" EnableViewState="false">
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="FieldLabel">
                                            <asp:Label ID="lblTaxRegistrationID2" AssociatedControlID="txtTaxRegistrationID2"
                                                runat="server" EnableViewState="false" />
                                        </td>
                                        <td>
                                            <cms:CMSTextBox ID="txtTaxRegistrationID2" runat="server" MaxLength="50" EnableViewState="false" />
                                            <asp:Label ID="lblMark23" runat="server" EnableViewState="false" Visible="false" />
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                            </asp:PlaceHolder>
                        </table>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <%--END: Continue as anonymous--%>
        </table>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="plcEditCustomer" EnableViewState="false">
        <table>
            <tr>
                <td class="FieldLabel" style="width: 170px">
                    <asp:Label ID="lblEditFirst" AssociatedControlID="txtEditFirst" runat="server" EnableViewState="false" />
                </td>
                <td>
                    <cms:CMSTextBox ID="txtEditFirst" runat="server" MaxLength="100" />
                    <asp:Label ID="lblMark12" runat="server" EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td class="FieldLabel" style="width: 170px">
                    <asp:Label ID="lblEditLast" AssociatedControlID="txtEditLast" runat="server" EnableViewState="false" />
                </td>
                <td>
                    <cms:CMSTextBox ID="txtEditLast" runat="server" MaxLength="100" />
                    <asp:Label ID="lblMark13" runat="server" EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td class="FieldLabel" style="width: 170px">
                    <cms:LocalizedLabel ID="lblEditEmail" AssociatedControlID="txtEditEmail" runat="server"
                        EnableViewState="false" ResourceString="general.email" DisplayColon="true" />
                </td>
                <td>
                    <cms:CMSTextBox ID="txtEditEmail" runat="server" MaxLength="100" />
                    <asp:Label ID="lblMark14" runat="server" EnableViewState="false" />
                    <asp:Label ID="lblEditEmailError" runat="server" EnableViewState="false" Visible="false"
                        CssClass="LineErrorLabel" />
                </td>
            </tr>
            <tr>
                <td class="FieldLabel" style="width: 170px">
                    <asp:Label ID="lblEditCorpBody" AssociatedControlID="chkEditCorpBody" runat="server"
                        EnableViewState="false" />
                </td>
                <td>
                    <cms:CMSCheckBox runat="server" ID="chkEditCorpBody" AutoPostBack="true" OnCheckedChanged="chkEditCorpBody_CheckChanged" />
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlCompanyAccount2" Visible="false">
                <tr>
                    <td class="FieldLabel" style="width: 170px">
                        <asp:Label ID="lblEditCompany" AssociatedControlID="txtEditCompany" runat="server"
                            EnableViewState="false" />
                    </td>
                    <td>
                        <cms:CMSTextBox ID="txtEditCompany" runat="server" MaxLength="100" />
                        <asp:Label ID="lblMark18" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <asp:PlaceHolder ID="plcEditOrgID" runat="server" Visible="false" EnableViewState="false">
                    <tr>
                        <td class="FieldLabel" style="width: 170px">
                            <asp:Label ID="lblEditOrgID" AssociatedControlID="txtEditOrgID" runat="server" EnableViewState="false" />
                        </td>
                        <td>
                            <cms:CMSTextBox ID="txtEditOrgID" runat="server" MaxLength="50" />
                            <asp:Label ID="lblMark19" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcEditTaxRegID" runat="server" Visible="false" EnableViewState="false">
                    <tr>
                        <td class="FieldLabel" style="width: 170px">
                            <asp:Label ID="lblEditTaxRegID" AssociatedControlID="txtEditTaxRegID" runat="server"
                                EnableViewState="false" />
                        </td>
                        <td>
                            <cms:CMSTextBox ID="txtEditTaxRegID" runat="server" MaxLength="50" />
                            <asp:Label ID="lblMark20" runat="server" EnableViewState="false" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </asp:Panel>
        </table>
    </asp:PlaceHolder>
</div>
