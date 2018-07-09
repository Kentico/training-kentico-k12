<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="ForumPost.ascx.cs" Inherits="CMSModules_Activities_Controls_UI_ActivityDetails_ForumPost" %>

<div class="form-horizontal">
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDocID" ResourceString="om.activitydetails.documenturl"
                EnableViewState="false" DisplayColon="true" />
        </div>
        <div class="editing-form-value-cell">
            <asp:Label runat="server" CssClass="form-control-text" ID="lblDocIDVal" />
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="plcComment" Visible="false" EnableViewState="false">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblPostSubject" ResourceString="om.activitydetails.forumpostsubject"
                    EnableViewState="false" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <asp:Label runat="server" CssClass="form-control-text" ID="lblPostSubjectVal" />           
            </div>
        </div>    
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblText" ResourceString="om.activitydetails.forumpost"
                    EnableViewState="false" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextArea runat="server" ID="txtPost" ReadOnly="true" Rows="10" Wrap="false" />
            </div>
        </div>

    </asp:PlaceHolder>
</div>