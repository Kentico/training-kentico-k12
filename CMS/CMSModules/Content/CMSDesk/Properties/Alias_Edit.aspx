<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Alias_Edit"
    Theme="Default"  Codebehind="Alias_Edit.aspx.cs" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSFormControls/Cultures/SiteCultureSelector.ascx" TagName="SiteCultureSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Documents/DocumentURLPath.ascx" TagName="DocumentURLPath"
    TagPrefix="cms" %>

<asp:Content runat="server" ID="content" ContentPlaceHolderID="plcContent">
    <cms:DocumentURLPath runat="server" ID="ctrlURL" HideCustom="true" />
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblAction" ResourceString="doc.urls.aliasaction"
                    DisplayColon="true" AssociatedControlID="drpAction" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSDropDownList runat="server" ID="drpAction" CssClass="DropDownField" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label CssClass="control-label" ID="lblDocumentCulture" runat="server" AssociatedControlID="cultureSelector"/>
            </div>
            <div class="editing-form-value-cell">
                <cms:SiteCultureSelector runat="server" ID="cultureSelector" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <asp:Label CssClass="control-label" ID="lblURLExtensions" runat="server" AssociatedControlID="txtURLExtensions" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox ID="txtURLExtensions" runat="server" MaxLength="100" />
            </div>
        </div>
        <asp:PlaceHolder runat="server" ID="pnlUrlInfo">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblaliasInfo" ResourceString="aliasnode.text"
                        DisplayColon="true" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Label ID="lblUrlInfoText" runat="server" CssClass="form-control-text" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <div class="editing-form-value-cell editing-form-value-cell-offset">
                <cms:FormSubmitButton ID="btnOk" runat="server" OnClick="btnOK_Click" />
            </div>
        </div>
    </div>
</asp:Content>