<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_Controls_Dialogs_YouTube_YouTubeProperties"
     Codebehind="YouTubeProperties.ascx.cs" %>

<%@ Register Src="~/CMSModules/Content/Controls/Dialogs/General/WidthHeightSelector.ascx"
    TagPrefix="cms" TagName="WidthHeightSelector" %>
<%@ Register Src="~/CMSModules/Content/Controls/Dialogs/YouTube/YouTubeSizes.ascx"
    TagPrefix="cms" TagName="YouTubeSizes" %>
<%@ Register Src="~/CMSInlineControls/YouTubeControl.ascx" TagPrefix="cms" TagName="YouTubeControl" %>

<cms:CMSUpdatePanel runat="server" ID="pnlUpdate" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlYouTube" runat="server" DefaultButton="btnRefresh">
            <div class="YouTubeProperties">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblUrl" runat="server" EnableViewState="false" ResourceString="dialogs.link.url"
                                DisplayColon="true" AssociatedControlID="txtLinkText" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSUpdatePanel ID="pnlUpdateYouTubeUrl" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="control-group-inline">
                                        <cms:CMSTextBox runat="server" ID="txtLinkText" CssClass="js-youtube-url" />
                                        <cms:CMSButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" EnableViewState="false" ButtonStyle="Default" />
                                    </div>
                                </ContentTemplate>
                            </cms:CMSUpdatePanel>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblFullScreen" runat="server" EnableViewState="false" ResourceString="dialogs.youtube.fullscreen"
                                DisplayColon="true" AssociatedControlID="chkFullScreen" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSCheckBox ID="chkFullScreen" runat="server" Checked="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblAutoplay" runat="server" EnableViewState="false" ResourceString="dialogs.vid.autoplay"
                                DisplayColon="true" AssociatedControlID="chkAutoplay" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSCheckBox ID="chkAutoplay" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblIncludeRelated" runat="server" EnableViewState="false"
                                ResourceString="dialogs.youtube.related" DisplayColon="true" AssociatedControlID="chkIncludeRelated" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSCheckBox ID="chkIncludeRelated" runat="server" Checked="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblDefaultSize" runat="server" EnableViewState="false" ResourceString="dialogs.youtube.defaultsize"
                                DisplayColon="true" AssociatedControlID="youTubeSizes" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:YouTubeSizes ID="youTubeSizes" runat="server" />
                        </div>
                    </div>
                    <cms:CMSUpdatePanel ID="pnlYouTubeWidthHeight" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <cms:WidthHeightSelector ID="sizeElem" runat="server" ShowLabels="true" />
                        </ContentTemplate>
                    </cms:CMSUpdatePanel>
                </div>
            </div>
            <div class="youtube-box">
                <div class="form-control js-youtube-preview-box">
                    <cms:CMSUpdatePanel ID="pnlYouTubePreview" runat="server">
                        <ContentTemplate>
                            <cms:YouTubeControl runat="server" ID="previewElem" />
                            <asp:Button ID="btnDefaultSizesHidden" runat="server" CssClass="HiddenButton" />
                            <asp:Button ID="btnHiddenPreview" runat="server" CssClass="HiddenButton" />
                            <asp:Button ID="btnHiddenInsert" runat="server" CssClass="HiddenButton" />
                            <asp:Button ID="btnHiddenSizeRefresh" runat="server" CssClass="HiddenButton" />
                        </ContentTemplate>
                    </cms:CMSUpdatePanel>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</cms:CMSUpdatePanel>
