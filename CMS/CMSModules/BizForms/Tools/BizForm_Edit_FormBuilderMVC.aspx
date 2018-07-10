<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" CodeBehind="BizForm_Edit_FormBuilderMVC.aspx.cs" Inherits="CMSModules_BizForms_Tools_BizForm_Edit_FormBuilderMVC"  Theme="Default" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
     <cms:UILayout runat="server">
        <Panes>
            <cms:UILayoutPane ID="formBuilderFrame" runat="server" Direction="Center" RenderAs="Iframe" AppendSrc="true" Src="#" />
        </Panes>
    </cms:UILayout>
</asp:Content>