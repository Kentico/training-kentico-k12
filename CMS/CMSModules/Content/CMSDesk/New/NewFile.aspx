<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_New_NewFile"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Content - New file"
     Codebehind="NewFile.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/Controls/Attachments/DocumentAttachments/DirectUploader.ascx"
    TagName="DirectFileUploader" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/EditMenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<asp:Content ID="cntMenu" ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:EditMenu ID="menuElem" ShortID="m" runat="server" ShowProperties="false" ShowSpellCheck="true"
        IsLiveSite="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="plcContent" runat="server">
    <asp:Panel ID="pnlForm" runat="server">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label ID="lblUploadFile" runat="server" EnableViewState="false" CssClass="control-label" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:DirectFileUploader ID="ucDirectUploader" runat="server" CheckPermissions="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label ID="lblFileDescription" runat="server" EnableViewState="false" CssClass="control-label" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextArea ID="txtFileDescription" runat="server" MaxLength="500" Rows="6" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
