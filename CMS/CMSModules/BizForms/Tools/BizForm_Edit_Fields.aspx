<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="BizForm Fields" Inherits="CMSModules_BizForms_Tools_BizForm_Edit_Fields"
    EnableEventValidation="false" Theme="Default"  Codebehind="BizForm_Edit_Fields.aspx.cs" %>

<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/FieldEditor/FieldEditor.ascx"
    TagName="FieldEditor" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:FieldEditor ID="FieldEditor" runat="server" IsLiveSite="false" AllowDummyFields="true"/>
</asp:Content>
