<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_View_ViewFile"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Content - View file"
     Codebehind="ViewFile.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server" ID="cntBeforeContent">
    <cms:editmenu ID="menuElem" runat="server" HandleWorkflow="false" IsLiveSite="false" />
</asp:Content>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="server">
    <asp:Panel ID="pnlContent" runat="server" CssClass="PageContent">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblFileName" runat="server" EnableViewState="false" ResourceString="general.filename"
                        DisplayColon="true" AssociatedControlID="lblFileNameText" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Label ID="lblFileNameText" runat="server" EnableViewState="false" CssClass="form-control-text" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <asp:Label CssClass="control-label" ID="lblFileSize" runat="server" EnableViewState="false" AssociatedControlID="lblFileSizeText" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Label ID="lblFileSizeText" runat="server" EnableViewState="false" CssClass="form-control-text" />
                </div>
            </div>
            <asp:PlaceHolder ID="plcSize" runat="server" Visible="false" EnableViewState="false">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <asp:Label CssClass="control-label" ID="lblSize" runat="server" EnableViewState="false" AssociatedControlID="lblSizeText" />
                    </div>
                    <div class="editing-form-value-cell">
                        <asp:Label ID="lblSizeText" runat="server" EnableViewState="false" CssClass="form-control-text" />
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plcImage" runat="server" Visible="false" EnableViewState="false">
                <div class="form-group">
                    <div class="editing-form-value-cell editing-form-value-cell-offset">
                        <asp:Image ID="imgPreview" runat="server" EnableViewState="false" />
                    </div>
                </div>
            </asp:PlaceHolder>
            <div class="form-group">
                <div class="editing-form-value-cell editing-form-value-cell-offset">
                    <asp:HyperLink ID="lnkView" runat="server" Target="_blank" EnableViewState="false" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
