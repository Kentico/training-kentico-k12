<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Membership_Logon_LogonForm"
     Codebehind="~/CMSWebParts/Membership/Logon/LogonForm.ascx.cs" %>

<asp:Panel ID="pnlBody" runat="server" CssClass="logon-page-background">
    <%-- Logon part --%>
    <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Default.aspx" RenderOuterTable="false">
        <LayoutTemplate>
            <asp:Panel runat="server" ID="pnlLogin" DefaultButton="LoginButton" CssClass="logon-panel">
                <cms:LocalizedLabel ID="lblTokenInfo" runat="server" EnableViewState="False" Visible="false" CssClass="logon-token-info" />
                <%-- Form start --%>
                <div class="form-horizontal" role="form">

                    <%-- Token ID step --%>
                    <asp:PlaceHolder runat="server" ID="plcTokenInfo" Visible="false">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblTokenIDlabel" runat="server" CssClass="control-label" ResourceString="mfauthentication.label.token" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:LocalizedLabel ID="lblTokenID" runat="server" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <%-- Logon step --%>
                    <asp:PlaceHolder runat="server" ID="plcLoginInputs">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblUserName" runat="server" AssociatedControlID="UserName" CssClass="control-label" ResourceString="LogonForm.UserName" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="UserName" runat="server" MaxLength="100" />
                                <cms:CMSRequiredFieldValidator ID="rfvUserNameRequired" runat="server" ControlToValidate="UserName" Display="Dynamic" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblPassword" runat="server" AssociatedControlID="Password" CssClass="control-label" ResourceString="LogonForm.Password" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="Password" runat="server" TextMode="Password" MaxLength="110" />
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <%-- Passcode step --%>
                    <asp:PlaceHolder runat="server" ID="plcPasscodeBox" Visible="false">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel ID="lblPasscode" runat="server" AssociatedControlID="txtPasscode" CssClass="control-label" ResourceString="mfauthentication.label.passcode" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtPasscode" runat="server" MaxLength="110" />
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <%-- Form End --%>
                </div>

                <cms:CMSCheckBox ID="chkRememberMe" runat="server" ResourceString="LogonForm.RememberMe" CssClass="logon-remember-me-checkbox" />
                <cms:LocalizedLabel ID="FailureText" runat="server" EnableViewState="False" CssClass="error-label" Visible="false" />

                <cms:LocalizedButton ID="LoginButton" runat="server" ButtonStyle="Primary" CommandName="Login" EnableViewState="false" 
                    ResourceString="LogonForm.LogOnButton" />
            </asp:Panel>
        </LayoutTemplate>
    </asp:Login>


    <%-- Password retrieval part--%>
    <cms:CMSUpdatePanel runat="server" ID="pnlUpdatePasswordRetrievalLink" UpdateMode="Conditional">
        <ContentTemplate>
            <cms:LocalizedLinkButton ID="lnkPasswdRetrieval" runat="server" EnableViewState="false" OnClick="lnkPasswdRetrieval_Click" 
                CssClass="logon-password-retrieval-link" ResourceString="LogonForm.lnkPasswordRetrieval" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>

    <cms:CMSUpdatePanel runat="server" ID="pnlUpdatePasswordRetrieval" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlPasswdRetrieval" runat="server" CssClass="logon-panel-password-retrieval" DefaultButton="btnPasswdRetrieval" Visible="False">

                <%-- Form start --%>
                <div class="form-horizontal" role="form">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel ID="lblPasswdRetrieval" runat="server" EnableViewState="false" AssociatedControlID="txtPasswordRetrieval" 
                                CssClass="control-label" ResourceString="LogonForm.lblPasswordRetrieval" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtPasswordRetrieval" runat="server" />                          
                        </div>
                    </div>
                    <%-- Form End --%>
                </div>

                <cms:LocalizedButton ID="btnPasswdRetrieval" runat="server" EnableViewState="false" ButtonStyle="Default" 
                    CssClass="logon-password-retrieval-button" ResourceString="LogonForm.btnPasswordRetrieval" />
                <asp:Label ID="lblResult" runat="server" Visible="false" EnableViewState="false" CssClass="logon-password-retrieval-result" />

            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger EventName="Click" ControlID="lnkPasswdRetrieval" />
        </Triggers>
    </cms:CMSUpdatePanel>

</asp:Panel>
