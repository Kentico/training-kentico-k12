<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_ViewMessage"  Codebehind="ViewMessage.ascx.cs" %>
<%@ Reference Control="~/CMSModules/Messaging/Controls/MessageUserButtons.ascx" %>
<%@ Reference Control="~/CMSAdminControls/UI/UserPicture.ascx" %>
<cms:MessagesPlaceHolder ID="plcMess" runat="server" LiveSiteOnly="true" />
<asp:Panel ID="pnlViewMessage" runat="server" Visible="false">
    <div class="form-horizontal message-view">
        <div class="form-group">
            <div class="editing-form-value-cell-offset">
                <asp:PlaceHolder runat="server" ID="plcUserPicture"></asp:PlaceHolder>
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblFromCaption" runat="Server" EnableViewState="false" DisplayColon="true" AssociatedControlID="lblFrom" />
            </div>
            <div class="editing-form-value-cell">
                    <asp:Label CssClass="form-control-text" ID="lblFrom" runat="Server" EnableViewState="false" />
                    <asp:PlaceHolder runat="server" ID="plcMessageUserButtons"></asp:PlaceHolder>
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblDateCaption" runat="Server" EnableViewState="false" DisplayColon="true" AssociatedControlID="lblDate" />
            </div>
            <div class="editing-form-value-cell">
                <asp:Label CssClass="form-control-text" ID="lblDate" runat="Server" EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblSubjectCaption" runat="Server" EnableViewState="false" DisplayColon="true" AssociatedControlID="lblSubject" />
            </div>
            <div class="editing-form-value-cell">
                <asp:Label CssClass="form-control-text" ID="lblSubject" runat="Server" EnableViewState="false" />
            </div>
        </div>
    </div>

    <asp:Label ID="lblBody" runat="server" EnableViewState="false" />
</asp:Panel>
