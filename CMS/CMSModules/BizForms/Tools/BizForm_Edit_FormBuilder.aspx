<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="BizForm Fields" Inherits="CMSModules_BizForms_Tools_BizForm_Edit_FormBuilder"
    EnableEventValidation="false" Theme="Default"  Codebehind="BizForm_Edit_FormBuilder.aspx.cs" %>

<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/FormBuilder/FormBuilder.ascx"
    TagName="FormBuilder" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:FormBuilder ID="FormBuilder" runat="server" />
</asp:Content>
