<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Inherits="CMSModules_MyDesk_MyProfile_MyProfile_MyDetails"
    Theme="Default"  Codebehind="MyProfile_MyDetails.aspx.cs" %>

<%@ Register Src="~/CMSModules/Membership/Controls/MyProfile.ascx" TagName="MyProfile"
    TagPrefix="cms" %>
    
<asp:Content ContentPlaceHolderID="plcContent" ID="content" runat="server">
    <cms:MyProfile ID="ucMyDetails" runat="server" Visible="true" AlternativeFormName="cms.user.EditProfileMyDesk" />
</asp:Content>
