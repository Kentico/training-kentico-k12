<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="BizForm_Edit_EditRecordMVC.aspx.cs" Theme="Default" ValidateRequest="false"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="BizForm edit - New record" EnableEventValidation="false"
    Inherits="CMSModules_BizForms_Tools_BizForm_Edit_EditRecordMVC" %>


<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UILayout ID="mvcLayout" runat="server">
        <Panes>
            <cms:UILayoutPane ID="formBuilderFrame" runat="server" Direction="Center" RenderAs="Iframe" AppendSrc="true" Src="about:blank" />
        </Panes>
    </cms:UILayout>
</asp:Content>

