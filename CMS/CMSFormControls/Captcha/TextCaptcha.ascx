<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="TextCaptcha.ascx.cs" Inherits="CMSFormControls_Captcha_TextCaptcha" %>
<div class="text-captcha">
    <asp:Panel runat="server" ID="pnlSecurityLbl" Visible="false">
        <cms:LocalizedLabel ID="lblSecurityCode" runat="server" EnableViewState="false" ResourceString="SecurityCode.lblSecurityCodeText" />
    </asp:Panel>
    <table class="CaptchaTable">
        <tr>
            <td>&nbsp;<asp:Image ID="imgSecurityCode" AlternateText="Security code" runat="server"
                EnableViewState="false" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlAnswer" runat="server" EnableViewState="false">
                </asp:Panel>
            </td>
            <td class="CaptchaAfterText">
                <cms:LocalizedLabel ID="lblAfterText" runat="server" EnableViewState="false" Visible="false" ResourceString="SecurityCode.Aftertext" />
            </td>
        </tr>
    </table>
</div>
