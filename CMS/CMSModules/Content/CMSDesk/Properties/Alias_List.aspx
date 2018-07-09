<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Alias_List"
    Theme="Default" CodeBehind="Alias_List.aspx.cs" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Documents/DocumentURLPath.ascx" TagName="DocumentURLPath"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>

<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:editmenu ID="menuElem" runat="server" HandleWorkflow="false" IsLiveSite="false" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnlPageContent" runat="server">
        <cms:UIPlaceHolder ID="pnlUIAlias" runat="server" ModuleName="CMS.Content" ElementName="URLs.Path">
            <asp:Panel ID="pnlAlias" runat="server">
                <cms:LocalizedHeading runat="server" ID="headAlias" Level="4" ResourceString="GeneralProperties.DocumentAlias" EnableViewState="false" />
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblAlias" runat="server" EnableViewState="false" 
                                ResourceString="GeneralProperties.Alias" ShowRequiredMark="true" AssociatedControlID="txtAlias" DisplayColon="true" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtAlias" runat="server" />
                            <cms:CMSRequiredFieldValidator ID="valAlias" runat="server" ControlToValidate="txtAlias"
                                Display="Dynamic" EnableViewState="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </cms:UIPlaceHolder>
        <cms:UIPlaceHolder ID="pnlUIPath" runat="server" ModuleName="CMS.Content" ElementName="URLs.URLPath">
            <asp:Panel ID="pnlURLPath" runat="server">
                <cms:LocalizedHeading runat="server" ID="headURLPath" Level="4" ResourceString="content.ui.urlsurlpath" EnableViewState="false" />
                <cms:DocumentURLPath runat="server" ID="ctrlURL" />
            </asp:Panel>
        </cms:UIPlaceHolder>
        <cms:UIPlaceHolder ID="pnlUIExtended" runat="server" ModuleName="CMS.Content" ElementName="URLs.ExtendedProperties">
            <asp:Panel ID="pnlExtended" runat="server">
                <cms:LocalizedHeading runat="server" ID="headExtended" Level="4" ResourceString="doc.urls.extendedproperties" EnableViewState="false" />
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <asp:Label CssClass="control-label" ID="lblExtensions" runat="server" EnableViewState="false" AssociatedControlID="txtExtensions" />
                        </div>
                        <div class="editing-form-value-cell">
                            <div class="control-group-inline">
                                <cms:CMSCheckBox ID="chkCustomExtensions" runat="server" OnCheckedChanged="chkCustomExtensions_CheckedChanged"
                                    AutoPostBack="true" />
                            </div>
                            <div class="control-group-inline">
                                <cms:CMSTextBox ID="txtExtensions" runat="server" MaxLength="100" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </cms:UIPlaceHolder>
        <cms:UIPlaceHolder ID="pnlUIDocumentAlias" runat="server" ModuleName="CMS.Content"
            ElementName="URLs.Aliases">
            <asp:Panel runat="server" ID="pnlDocumentAlias">
                <cms:LocalizedHeading runat="server" ID="headDocumentAlias" Level="4" ResourceString="content.ui.urlsaliases" EnableViewState="false" />
                <div class="form-group">
                    <cms:LocalizedButton ID="btnNewAlias" runat="server" ButtonStyle="Default" EnableViewState="false" ResourceString="doc.urls.addnewalias" />
                </div>
                <div class="form-group">
                    <cms:UniGrid ID="UniGridAlias" runat="server" GridName="Alias_List.xml" IsLiveSite="false"
                        OrderBy="AliasID" />
                </div>
            </asp:Panel>
        </cms:UIPlaceHolder>
    </asp:Panel>
    <div class="Clear">
    </div>
</asp:Content>
