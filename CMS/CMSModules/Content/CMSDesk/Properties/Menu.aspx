<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Menu"
    Theme="Default" CodeBehind="Menu.aspx.cs" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:editmenu ID="menuElem" runat="server" ShowReject="true" ShowSubmitToApproval="true" ShowProperties="false" IsLiveSite="false" />
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <script type="text/javascript">
        //<![CDATA[

        function enableTextBoxes(txtId) {
            var txt1 = document.getElementById('<% = txtJavaScript.ClientID %>');
            var txt2 = document.getElementById('<% = txtUrl.ClientID %>');
            var txtIna = document.getElementById('<% = txtUrlInactive.ClientID %>');

            if ((txtId == '') || (txtId == 'inactive')) {
                if (txt1 != null) {
                    txt1.disabled = true;
                }
                if (txt2 != null) {
                    txt2.disabled = true;
                }
                if ((txtId == 'inactive') && (txtIna != null)) {
                    txtIna.disabled = false;
                }
                else if (txtIna != null) {
                    txtIna.disabled = true;
                }
            }

            if ((txtId == 'java') && (txtIna != null)) {
                if (txt1 != null) {
                    txt1.disabled = false;
                }
                if (txt2 != null) {
                    txt2.disabled = true;
                }
                if (txtIna != null) {
                    txtIna.disabled = true;
                }
            }

            if ((txtId == 'url') && (txtIna != null)) {
                if (txt1 != null) {
                    txt1.disabled = true;
                }
                if (txt2 != null) {
                    txt2.disabled = false;
                }
                if (txtIna != null) {
                    txtIna.disabled = true;
                }
            }
        }
        //]]>
    </script>
    <asp:Panel ID="pnlContent" runat="server">
        <asp:Label ID="lblWorkflow" runat="server" CssClass="InfoLabel" EnableViewState="false"
            Visible="false" />
        <asp:Panel ID="pnlForm" runat="server">
            <cms:UIPlaceHolder ID="pnlUIBasicProperties" runat="server" ModuleName="CMS.Content"
                ElementName="Menu.BasicProperties">
                <asp:Panel ID="pnlBasicProperties" runat="server">
                    <cms:LocalizedHeading runat="server" ID="headBasicProperties" Level="4" ResourceString="content.menu.basic" EnableViewState="false" />
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuCaption" runat="server" EnableViewState="false" ResourceString="MenuProperties.Caption" AssociatedControlID="txtMenuCaption" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuCaption" runat="server" MaxLength="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblShowInNavigation" runat="server" ResourceString="MenuProperties.ShowInNavigation" AssociatedControlID="chkShowInNavigation" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSCheckBox ID="chkShowInNavigation" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblShowInSitemap" runat="server" ResourceString="MenuProperties.ShowInSitemap" AssociatedControlID="chkShowInSitemap" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSCheckBox ID="chkShowInSitemap" runat="server" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <br />
            </cms:UIPlaceHolder>
            <%-- Menu action --%>
            <cms:UIPlaceHolder ID="pnlUIActions" runat="server" ModuleName="CMS.Content" ElementName="Menu.Actions">
                <asp:Panel ID="pnlActions" runat="server">
                    <cms:LocalizedHeading runat="server" ID="headActions" Level="4" ResourceString="content.menu.actions" EnableViewState="false" />
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="editing-form-value-cell-offset editing-form-value-cell">
                                <div class="radio-list-vertical">

                                    <cms:CMSRadioButton runat="server" ID="radStandard" GroupName="MenuActionGroup"
                                        Checked="true" ResourceString="MenuProperties.Standard" />
                                    <cms:CMSRadioButton runat="server" GroupName="MenuActionGroup" ID="radInactive"
                                        ResourceString="MenuProperties.InactiveRedirectToURL" />
                                    <div class="selector-subitem">
                                        <cms:CMSTextBox runat="server" ID="txtUrlInactive" MaxLength="450" />
                                    </div>
                                    <cms:CMSRadioButton runat="server" GroupName="MenuActionGroup" ID="radJavascript"
                                        ResourceString="MenuProperties.Javascript" />
                                    <div class="selector-subitem">
                                        <cms:CMSTextBox runat="server" ID="txtJavaScript" MaxLength="450" />
                                        <cms:LocalizedLabel ID="lblJavaScript" runat="server" ResourceString="MenuProperties.JavaScriptHelp"
                                            CssClass="explanation-text block" EnableViewState="false" />
                                    </div>
                                    <cms:CMSRadioButton runat="server" ID="radFirstChild" GroupName="MenuActionGroup" />
                                    <cms:CMSRadioButton runat="server" GroupName="MenuActionGroup" ID="radUrl"
                                        ResourceString="MenuProperties.Url" />
                                    <div class="selector-subitem">
                                        <cms:CMSTextBox runat="server" ID="txtUrl" MaxLength="450" />
                                        <cms:LocalizedLabel ID="lblUrl" runat="server" ResourceString="MenuProperties.UrlHelp"
                                            CssClass="explanation-text" EnableViewState="false" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </cms:UIPlaceHolder>
            <cms:UIPlaceHolder ID="pnlUISearch" runat="server" ModuleName="CMS.Content" ElementName="Menu.Search">
                <asp:Panel ID="pnlSearch" runat="server" CssClass="NodePermissions" EnableViewState="false">
                    <cms:LocalizedHeading runat="server" ID="headSearch" Level="4" ResourceString="GeneralProperties.Search" EnableViewState="false" />
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblExcludeFromSearch" runat="server" EnableViewState="false"
                                    ResourceString="GeneralProperties.ExcludeFromSearch" DisplayColon="true" AssociatedControlID="chkExcludeFromSearch" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSCheckBox ID="chkExcludeFromSearch" runat="server" />
                            </div>
                        </div>
                        <asp:PlaceHolder ID="plcAdvancedSearch" runat="server">
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <cms:LocalizedLabel CssClass="control-label" ID="lblSitemapChange" runat="server" EnableViewState="false"
                                        ResourceString="generalsitemap.change" DisplayColon="true" AssociatedControlID="drpChange" />
                                </div>
                                <div class="editing-form-value-cell">
                                    <cms:CMSDropDownList runat="server" ID="drpChange" CssClass="DropDownFieldSmall" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="editing-form-label-cell">
                                    <cms:LocalizedLabel CssClass="control-label" ID="lblSitemapPriority" runat="server" EnableViewState="false"
                                        ResourceString="generalsitemap.priority" DisplayColon="true" AssociatedControlID="drpPriority" />
                                </div>
                                <div class="editing-form-value-cell">
                                    <cms:CMSDropDownList runat="server" ID="drpPriority" CssClass="DropDownFieldSmall" />
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </asp:Panel>
                <br />
            </cms:UIPlaceHolder>
            <cms:UIPlaceHolder ID="pnlUIDesign" runat="server" ModuleName="CMS.Content" ElementName="Menu.Design">
                <%-- Menu item design --%>
                <cms:CMSPanel ID="pnlDesign" runat="server" CssClass="NodePermissions"
                    IsCollapsed="True">
                    <cms:LocalizedHeading runat="server" ID="headDesign" Level="4" ResourceString="content.menu.design" EnableViewState="false" />
                    <cms:LocalizedHeading runat="server" ID="headMenuItemDesign" Level="5" ResourceString="MenuProperties.Design" EnableViewState="false" />
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemStyle" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.Style" AssociatedControlID="txtMenuItemStyle" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemStyle" runat="server" MaxLength="100" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblCssClass" runat="server" EnableViewState="false" ResourceString="MenuProperties.CssClass" AssociatedControlID="txtCssClass" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtCssClass" runat="server" MaxLength="100" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemLeftmage" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.LeftImage" AssociatedControlID="txtMenuItemLeftImage" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemLeftImage" runat="server"
                                    MaxLength="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemImage" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.Image" AssociatedControlID="txtMenuItemImage" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemImage" runat="server" MaxLength="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemRightImage" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.RightImage" AssociatedControlID="txtMenuItemRightImage" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemRightImage" runat="server" MaxLength="200" />
                            </div>
                        </div>
                    </div>
                    <%-- Menu item highlighted --%>
                    <cms:LocalizedHeading runat="server" ID="headMenuItemDesignHighlighted" Level="5" ResourceString="MenuProperties.Highlight" EnableViewState="false" />
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemStyleHighlight" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.Style" AssociatedControlID="txtMenuItemStyleHighlight" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemStyleHighlight" runat="server" MaxLength="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblCssClassHighlight" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.CssClass" AssociatedControlID="txtCssClassHighlight" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtCssClassHighlight" runat="server" MaxLength="50" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemLeftmageHighlight" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.LeftImage" AssociatedControlID="txtMenuItemLeftImageHighlight" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemLeftImageHighlight" runat="server"
                                    MaxLength="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemImageHighlight" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.Image" AssociatedControlID="txtMenuItemImageHighlight" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemImageHighlight" runat="server"
                                    MaxLength="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblMenuItemRightImageHighlight" runat="server" EnableViewState="false"
                                    ResourceString="MenuProperties.RightImage" AssociatedControlID="txtMenuItemRightImageHighlight" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:CMSTextBox ID="txtMenuItemRightImageHighlight" runat="server"
                                    MaxLength="200" />
                            </div>
                        </div>
                    </div>
                </cms:CMSPanel>
            </cms:UIPlaceHolder>
        </asp:Panel>
    </asp:Panel>
    <div class="Clear">
    </div>
</asp:Content>
