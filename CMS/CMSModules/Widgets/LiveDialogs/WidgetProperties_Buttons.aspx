<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Widgets_LiveDialogs_WidgetProperties_Buttons"
    Theme="default"  Codebehind="WidgetProperties_Buttons.aspx.cs" MasterPageFile="~/CMSMasterPages/LiveSite/EmptyPage.master" %>

<asp:Content ID="pnlContent" ContentPlaceHolderID="plcContent" runat="server">
    <div class="LiveSiteDialog">
        <asp:Panel runat="server" ID="pnlScroll" CssClass="PageFooterLine">
            <div class="FloatRight">
                <cms:CMSButton ID="btnOk" runat="server" ButtonStyle="Primary" /><cms:CMSButton
                    ID="btnCancel" runat="server" ButtonStyle="Primary" /><cms:CMSButton ID="btnApply"
                        runat="server" ButtonStyle="Primary" />
            </div>
        </asp:Panel>
    </div>
    <asp:Literal runat="server" ID="ltlScript" EnableViewState="false" />
</asp:Content>
