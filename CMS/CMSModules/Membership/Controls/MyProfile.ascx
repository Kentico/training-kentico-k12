<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Membership_Controls_MyProfile"  Codebehind="MyProfile.ascx.cs" %>

<cms:MessagesPlaceHolder ID="plcMess" runat="server" />

<asp:Panel ID="RegForm" runat="server" CssClass="my-profile-panel">
    <cms:DataForm ID="editProfileForm" runat="server" ClassName="cms.user" />    
</asp:Panel>
