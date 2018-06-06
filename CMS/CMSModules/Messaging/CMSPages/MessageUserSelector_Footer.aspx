<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="MessageUserSelector_Footer.aspx.cs"
    Inherits="CMSModules_Messaging_CMSPages_MessageUserSelector_Footer" Theme="Default"
    MasterPageFile="~/CMSMasterPages/LiveSite/EmptyPage.master" %>

<asp:Content ID="content" ContentPlaceHolderID="plcContent" runat="server">
    <div class="PageFooterLine">
        <div class="FloatRight">
            <cms:LocalizedButton ID="btnCancel" runat="server" ResourceString="dialogs.actions.cancel"
                ButtonStyle="Primary" EnableViewState="false" OnClientClick="return CloseDialog();" />
            <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
        </div>
    </div>
</asp:Content>
