<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Content_Controls_Dialogs_Properties_NodeGuidProperties"  Codebehind="NodeGuidProperties.ascx.cs" %>

<%@ Register Src="~/CMSInlineControls/ImageControl.ascx" TagPrefix="cms" TagName="ImagePreview" %>

<asp:Panel runat="server" ID="pnlEmpty" Visible="true" CssClass="DialogInfoArea">
    <asp:Label runat="server" ID="lblEmpty" EnableViewState="false" />
</asp:Panel>
<cms:JQueryTabContainer ID="pnlTabs" runat="server" CssClass="DialogElementHidden">
    <cms:JQueryTab ID="tabImageGeneral" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlGeneralTab" CssClass="PageContent">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblUrl" runat="server" EnableViewState="false" DisplayColon="true"
                                ResourceString="general.permanenturl" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:LocalizedLabel ID="lblUrlText" CssClass="form-control-text" runat="server" DisplayColon="false" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblName" runat="server" EnableViewState="false" DisplayColon="true"
                                ResourceString="general.name" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:LocalizedLabel ID="lblNameText" CssClass="form-control-text" runat="server" DisplayColon="false" />
                        </div>
                    </div>
                    <asp:PlaceHolder ID="plcSizeArea" runat="server">
                        <div class="form-group">
                            <div class="editing-form-label-cell">
                                <cms:LocalizedLabel CssClass="control-label" ID="lblSize" runat="server" EnableViewState="false" DisplayColon="true"
                                    ResourceString="general.size" />
                            </div>
                            <div class="editing-form-value-cell">
                                <cms:LocalizedLabel ID="lblSizeText" CssClass="form-control-text" runat="server" DisplayColon="false" />
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="plcImagePreviewArea" runat="server">
                        <div class="form-group">
                            <div class="editing-form-value-cell editing-form-value-cell-offset">
                                <a id="aFullSize" runat="server" enableviewstate="false">
                                    <cms:ImagePreview ID="imagePreview" runat="server" />
                                </a>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </cms:JQueryTab>
</cms:JQueryTabContainer>