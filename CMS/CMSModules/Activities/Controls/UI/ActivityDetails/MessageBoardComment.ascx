<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="MessageBoardComment.ascx.cs"
    Inherits="CMSModules_Activities_Controls_UI_ActivityDetails_MessageBoardComment" %>

<div class="form-horizontal">
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDocID" ResourceString="om.activitydetails.documenturl"
                EnableViewState="false" DisplayColon="true" />
        </div>
        <div class="editing-form-value-cell">
            <asp:Label runat="server" CssClass="form-control-text" ID="lblDocIDVal" EnableViewState="false" />
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="plcComment" Visible="false" EnableViewState="false">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblComment" ResourceString="om.activitydetails.boardcomment"
                    EnableViewState="false" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextArea runat="server" ID="txtComment" ReadOnly="true" Rows="10" Wrap="false" />
            </div>
        </div>
    </asp:PlaceHolder>
</div>