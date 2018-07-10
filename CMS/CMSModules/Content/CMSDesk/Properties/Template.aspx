<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Template"
    Theme="Default"  CodeBehind="Template.aspx.cs" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/PortalEngine/FormControls/PageTemplates/PageTemplateLevels.ascx"
    TagName="PageTemplateLevel" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>

<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:editmenu ID="menuElem" runat="server" HandleWorkflow="false" IsLiveSite="false" />
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel ID="pnlContent" runat="server">
        <asp:Panel runat="server" ID="pnlActions" CssClass="NodePermissions">
            <cms:LocalizedHeading runat="server" ID="headTemplate" Level="4" ResourceString="PageProperties.Template" EnableViewState="false" />
            <cms:CMSUpdatePanel runat="server" ID="pnlUpdate">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hdnSelected" />
                    <div class="form-group">
                        <div class="radio-list-vertical">
                            <cms:CMSRadioButton runat="server" ID="radInherit" ResourceString="Template.Inherit"
                                GroupName="Template" AutoPostBack="true" OnCheckedChanged="RadChanged" />

                            <asp:PlaceHolder ID="plcAllCultures" runat="server" Visible="true">
                                <cms:CMSRadioButton runat="server" ID="radAllCultures" ResourceString="Template.AllCultures"
                                    GroupName="Template" AutoPostBack="true" OnCheckedChanged="RadChanged" />
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="plcThisCulture" runat="server" Visible="true">
                                <cms:CMSRadioButton runat="server" ID="radThisCulture" ResourceString="Template.ThisCulture"
                                    GroupName="Template" AutoPostBack="true" OnCheckedChanged="RadChanged" />
                            </asp:PlaceHolder>
                        </div>
                    </div>
                    <div class="form-group control-group-inline">
                        <cms:CMSTextBox ID="txtTemplate" runat="server" ReadOnly="True" /><cms:CMSButton ID="btnSelect" runat="server" EnableViewState="false" OnClick="btnSelect_Click" ButtonStyle="Default" />
                    </div>

                    <div class="form-group btns-vertical">
                        <cms:UIPlaceHolder ID="plcUISave" runat="server" ModuleName="CMS.Content" ElementName="Template.SaveAsNew">
                            <cms:LocalizedButton ID="btnSave" runat="server" ButtonStyle="Default" ResourceString="PageProperties.Save" EnableViewState="false" />
                        </cms:UIPlaceHolder>
                        <cms:UIPlaceHolder ID="plcUIClone" runat="server" ModuleName="CMS.Content" ElementName="Template.CloneAdHoc">
                            <cms:LocalizedButton ID="btnClone" runat="server" ButtonStyle="Default" OnClick="btnClone_Click"
                                EnableViewState="false" ResourceString="PageProperties.Clone" />
                        </cms:UIPlaceHolder>
                        <cms:UIPlaceHolder ID="plcUIEdit" runat="server" ModuleName="CMS.Content" ElementName="Template.EditProperties">
                            <cms:LocalizedButton ID="btnEditTemplateProperties" runat="server" ButtonStyle="Default"
                                EnableViewState="false" ResourceString="PageProperties.EditTemplateProperties" />
                        </cms:UIPlaceHolder>
                    </div>
                    <cms:UIPlaceHolder ID="plcUILevels" runat="server" ModuleName="CMS.Content" ElementName="Template.InheritContent">
                        <asp:Panel runat="server" ID="pnlInherits" CssClass="NodePermissions">
                            <cms:LocalizedHeading runat="server" ID="headInherits" Level="4" ResourceString="PageProperties.InheritLevels" EnableViewState="false" />
                            <cms:PageTemplateLevel runat="server" ID="inheritElem"></cms:PageTemplateLevel>
                        </asp:Panel>
                    </cms:UIPlaceHolder>
                </ContentTemplate>
            </cms:CMSUpdatePanel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
