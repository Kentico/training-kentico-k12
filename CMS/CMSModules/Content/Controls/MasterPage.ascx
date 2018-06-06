<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="MasterPage.ascx.cs" Inherits="CMSModules_Content_Controls_MasterPage" %>
<%@ Register Src="~/CMSFormControls/Inputs/LargeTextArea.ascx" TagName="LargeTextArea"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/HeaderActions.ascx" TagName="HeaderActions"
    TagPrefix="cms" %>
<cms:CMSUpdatePanel runat="server" ID="pnlUpdate">
    <ContentTemplate>
        <div runat="server" id="pnlMenu" class="PreviewMenu">
            <div class="cms-edit-menu">
                <cms:HeaderActions runat="server" ID="headerActions" IsLiveSite="false" />
                <div class="Clear"></div>
            </div>
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>
<asp:Panel runat="server" ID="pnlBody" CssClass="PreviewBody">
    <div class="PageContent">
        <cms:MessagesPlaceHolder runat="server" ID="pnlMessagePlaceholder" IsLiveSite="false" />
        <input type="hidden" name="saveChanges" id="saveChanges" value="0" />
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td>
                    <cms:CMSTextArea runat="server" ID="txtDocType" Rows="1" Width="100%" /><br />
                    <asp:Label runat="server" ID="lblAfterDocType" EnableViewState="false" CssClass="HTMLCode" />
                </td>
            </tr>
            <tr>
                <td>
                    <span class="HTMLCode">
                        <%=mHead%></span>
                    <div class="MasterPageLeft">
                        <cms:LargeTextArea ID="txtHeadTags" runat="server" Width="100%" Rows="2" AllowMacros="false"
                            CssClass="MasterPageHeaderTags" />
                    </div>
                    <asp:Label runat="server" ID="lblAfterHeadTags" EnableViewState="false" CssClass="HTMLCode" />
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td nowrap="nowrap">
                                <asp:Label runat="server" ID="lblBodyStart" EnableViewState="false" CssClass="HTMLCode" />
                            </td>
                            <td class="MasterPageClassTextBox">
                                <cms:CMSTextBox runat="server" ID="txtBodyCss" Width="100%" />
                            </td>
                            <td nowrap="nowrap">&nbsp;
                                    <asp:Label runat="server" ID="lblBodyEnd" EnableViewState="false" CssClass="HTMLCode" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>                    
                    <div class="MasterPageBodyScript">
                        <cms:LargeTextArea ID="txtBodyScripts" runat="server" Width="100%" Rows="2" AllowMacros="true"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlLayout" CssClass="MasterPageLayout">
                        <div class="MasterPageLayoutScroll">
                            <asp:Literal runat="server" ID="ltlLayoutCode" EnableViewState="false"></asp:Literal>
                        </div>
                    </asp:Panel>
                    <asp:Label runat="server" ID="lblAfterLayout" EnableViewState="false" CssClass="HTMLCode" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
