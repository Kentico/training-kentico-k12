<%@ Control Language="C#" AutoEventWireup="True"  Codebehind="TemplateSelection.ascx.cs"
    Inherits="CMSModules_Content_CMSDesk_New_TemplateSelection" %>
<%@ Register Src="~/CMSModules/PortalEngine/Controls/Layout/PageTemplateSelector.ascx"
    TagName="PageTemplateSelector" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/PortalEngine/Controls/Layout/LayoutFlatSelector.ascx"
    TagName="LayoutFlatSelector" TagPrefix="cms" %>
<div class="template-selection-control">
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <cms:UIPlaceHolder runat="server" ID="plcRadioButtonsNew" ElementName="New" ModuleName="CMS.Content" OnOnBeforeCheck="OnBeforeUICheck">
                <cms:UIPlaceHolder runat="server" ID="plcRadioButtons" ElementName="New.SelectTemplate"
                    ModuleName="CMS.Content" OnOnBeforeCheck="OnBeforeUICheck">
                    <div class="radio-list-horizontal">
                        <cms:UIPlaceHolder runat="server" ID="plcUseTemplate" ElementName="SelectTemplate.UseTemplate"
                            ModuleName="CMS.Content" OnOnBeforeCheck="OnBeforeUICheck">
                            <cms:CMSRadioButton ID="radUseTemplate" runat="server" GroupName="NewPage"
                                ResourceString="NewPage.UseTemplate" AutoPostBack="true" />
                        </cms:UIPlaceHolder>
                        <cms:UIPlaceHolder runat="server" ID="plcInherit" ElementName="SelectTemplate.InheritFromParent"
                            ModuleName="CMS.Content" OnOnBeforeCheck="OnBeforeUICheck">
                            <cms:CMSRadioButton ID="radInherit" runat="server" GroupName="NewPage"
                                ResourceString="NewPage.Inherit" AutoPostBack="true" />
                        </cms:UIPlaceHolder>
                        <cms:UIPlaceHolder runat="server" ID="plcCreateBlank" ElementName="SelectTemplate.CreateBlank"
                            ModuleName="CMS.Content" OnOnBeforeCheck="OnBeforeUICheck">
                            <cms:CMSRadioButton ID="radCreateBlank" runat="server"
                                GroupName="NewPage" ResourceString="NewPage.CreateBlank" AutoPostBack="true"
                                Checked="false" />
                        </cms:UIPlaceHolder>
                        <cms:UIPlaceHolder runat="server" ID="plcCreateEmpty" ElementName="SelectTemplate.CreateEmpty"
                            ModuleName="CMS.Content" OnOnBeforeCheck="OnBeforeUICheck">
                            <cms:CMSRadioButton ID="radCreateEmpty" runat="server"
                                GroupName="NewPage" ResourceString="NewPage.CreateEmpty" AutoPostBack="true"
                                Checked="false" />
                        </cms:UIPlaceHolder>
                    </div>
                </cms:UIPlaceHolder>
            </cms:UIPlaceHolder>
            <div class="TemplateSelectorWrap">
                <asp:PlaceHolder ID="plcTemplateSelector" runat="server">
                    <cms:PageTemplateSelector ID="templateSelector" runat="server" ShortID="ts" Mode="newpage" ShowEmptyCategories="false"
                        IsLiveSite="false" IsNewPage="true" SelectFunctionName="PTT_SelectNode" />
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcInherited" runat="server">
                    <div class="ItemSelector">
                        <div class="InheritedTemplate">
                            <cms:LocalizedLabel ID="lblIngerited" runat="server" EnableViewState="false" />
                        </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plcLayout" runat="server">
                    <asp:PlaceHolder ID="plcLayoutSelector" runat="server">
                        <cms:LayoutFlatSelector ID="layoutSelector" ShortID="ls" runat="server" IsLiveSite="false" />
                    </asp:PlaceHolder>
                    <div class="CopyLayoutPanel">
                        <cms:CMSCheckBox runat="server" ID="chkLayoutPageTemplate" Checked="true" ResourceString="NewPage.LayoutPageTemplate" />
                    </div>
                </asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</div>
