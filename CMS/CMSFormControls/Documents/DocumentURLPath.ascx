<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSFormControls_Documents_DocumentURLPath"
     Codebehind="DocumentURLPath.ascx.cs" %>

<asp:Panel ID="pnlContainer" runat="server">
    <cms:CMSUpdatePanel runat="server" ID="pnlUpdate">
        <ContentTemplate>
            <div class="form-horizontal">
                <asp:PlaceHolder runat="server" ID="plcCustom">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblCustom" runat="server" EnableViewState="false" ResourceString="GeneralProperties.UseCustomUrlPath"
                                DisplayColon="true" AssociatedControlID="chkCustomUrl" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSCheckBox ID="chkCustomUrl" runat="server" OnCheckedChanged="chkCustomUrl_CheckedChanged"
                                AutoPostBack="true" />
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblType" runat="server" EnableViewState="false" ResourceString="URLPath.Type"
                            DisplayColon="true" AssociatedControlID="pnlType" />
                    </div>
                    <div class="editing-form-value-cell">
                        <asp:Panel runat="server" ID="pnlType" CssClass="radio-list-vertical">
                            <cms:CMSRadioButton runat="server" ID="radPage" ResourceString="URLPath.Standard"
                                Checked="true" GroupName="URL" AutoPostBack="true" />
                            <cms:CMSRadioButton runat="server" ID="radRoute" ResourceString="URLPath.Route"
                                GroupName="URL" AutoPostBack="true" />
                        </asp:Panel>
                    </div>
                </div>
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:LocalizedLabel CssClass="control-label" ID="lblPath" runat="server" EnableViewState="false" ResourceString="URLPath.Path"
                            DisplayColon="true" AssociatedControlID="txtUrlPath" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:CMSTextBox ID="txtUrlPath" runat="server" MaxLength="450" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Panel>