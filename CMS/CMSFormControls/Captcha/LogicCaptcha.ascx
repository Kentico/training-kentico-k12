<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="LogicCaptcha.ascx.cs"
    Inherits="CMSFormControls_Captcha_LogicCaptcha" %>
<div>
    <cms:LocalizedLabel ID="lblSecurityCode" runat="server" EnableViewState="false" Visible="false" />
</div>
<table class="CaptchaTable">
    <tr>
        <td>
            <cms:CMSTextBox ID="txtAnswer" runat="server" />
            <cms:CMSRadioButton ID="rdAnswerNo" runat="server" GroupName="Answer" EnableViewState="false"
                Visible="false" ResourceString="general.false" />
            <cms:CMSRadioButton ID="rdAnswerYes" runat="server" GroupName="Answer" EnableViewState="false"
                Visible="false" ResourceString="general.true" />
        </td>
        <asp:PlaceHolder ID="plcImage" runat="server" Visible="false">
            <td>
                &nbsp;<asp:Image ID="imgSecurityCode" runat="server" EnableViewState="true" />
            </td>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcQuestion" runat="server">
            <td class="CaptchaQuestion">
                <cms:LocalizedLabel ID="lblQuestion" runat="server" EnableViewState="true" />
            </td>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcAfterText" runat="server" Visible="false">
            <td class="CaptchaAfterText">
                <cms:LocalizedLabel ID="lblAfterText" runat="server" EnableViewState="true" />
            </td>
        </asp:PlaceHolder>
    </tr>
</table>
