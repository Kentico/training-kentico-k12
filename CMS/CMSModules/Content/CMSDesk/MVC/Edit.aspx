<%@ Page Title="" Language="C#" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CMSModules_Content_CMSDesk_MVC_Edit" Theme="Default" %>

<%@ Register Src="~/CMSModules/Content/Controls/EditMenu.ascx" TagName="editmenu" TagPrefix="cms" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <div class="preview-edit-panel">
        <cms:editmenu runat="server" ID="editMenu" ShortID="m" />
    </div>
    <div class="page-builder">
        <cms:MessagesPlaceHolder runat="server" ID="plcMess" OffsetX="16" OffsetY="16" UseRelativePlaceHolder="False">
            <cms:AlertLabel runat="server" ID="alSuccess" AlertType="Confirmation" Text="&nbsp;" CssClass="alert-success-absolute hidden" EnableViewState="False" />
            <cms:AlertLabel runat="server" ID="alWarning" AlertType="Warning" Text="&nbsp;" CssClass="alert-dismissable alert-warning-absolute hidden" EnableViewState="False" />
            <cms:AlertLabel runat="server" ID="alError" AlertType="Error" Text="&nbsp;" CssClass="alert-dismissable alert-error-absolute hidden" EnableViewState="False" />
        </cms:MessagesPlaceHolder>
        <iframe width="100%" height="100%" id="pageview" name="pageview" scrolling="auto" frameborder="0" runat="server" class="ContentFrame scroll-area"></iframe>
    </div>
</asp:Content>
