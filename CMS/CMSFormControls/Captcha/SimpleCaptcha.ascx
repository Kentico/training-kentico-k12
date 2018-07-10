<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="SimpleCaptcha.ascx.cs"
    Inherits="CMSFormControls_Captcha_SimpleCaptcha" %>
<div>
    <cms:LocalizedLabel ID="lblSecurityCode" runat="server" EnableViewState="false" Visible="false" />
</div>
<table class="CaptchaTable">
    <tr>
        <td>
            <cms:CMSTextBox ID="txtSecurityCode" runat="server" CssClass="CaptchaTextBox"/>
        </td>
        <td>
            &nbsp;<asp:Image ID="imgSecurityCode" AlternateText="Security code" runat="server"
                EnableViewState="false" />
        </td>
        <asp:PlaceHolder runat="server" ID="plcAfterText" Visible="false">
            <td class="CaptchaAfterText">
                <asp:Label ID="lblAfterText" runat="server" EnableViewState="false" />
            </td>
        </asp:PlaceHolder>
    </tr>
</table>
