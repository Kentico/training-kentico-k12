<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_BizForms_Tools_BizForm_Edit_Layout"
    Theme="Default" EnableEventValidation="false"  Codebehind="BizForm_Edit_Layout.aspx.cs"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/AdminControls/Controls/Class/Layout.ascx" TagName="Layout"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:Layout ID="layoutElem" runat="server" IsLiveSite="false" />
</asp:Content>
