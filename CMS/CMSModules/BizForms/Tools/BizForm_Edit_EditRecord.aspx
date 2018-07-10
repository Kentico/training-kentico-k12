<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_BizForms_Tools_BizForm_Edit_EditRecord" Theme="Default"
    ValidateRequest="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="BizForm edit - New record" EnableEventValidation="false"  Codebehind="BizForm_Edit_EditRecord.aspx.cs" %>

<asp:Content ID="cntCtrl" ContentPlaceHolderID="plcControls" runat="server">
        <table cellspacing="0" cellpadding="0" border="0">
            <tbody>
                <tr>
                    <td>
                        <cms:CMSCheckBox ID="chkSendNotification" runat="server" ResourceString="bizform.sendnotification" />
                    </td>
                    <td style="width: 20px;">&nbsp;</td>
                    <td>
                        <cms:CMSCheckBox ID="chkSendAutoresponder" runat="server" ResourceString="bizform.sendautoresponder" />
                    </td>
                </tr>
            </tbody>
        </table>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:BizForm ID="formElem" runat="server" IsLiveSite="false" DefaultFormLayout="Divs" MarkRequiredFields="True" />
</asp:Content>
