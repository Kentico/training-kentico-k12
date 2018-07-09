<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="BlogComment.ascx.cs" Inherits="CMSModules_Activities_Controls_UI_ActivityDetails_BlogComment" %>
<div class="form-horizontal">
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDocID" ResourceString="om.activitydetails.blogdocument"
                EnableViewState="false" DisplayColon="true" />
        </div>
        <div class="editing-form-value-cell">
            <asp:Label runat="server" CssClass="form-control-text" ID="lblDocIDVal" />
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="plcBlogDocument" Visible="false">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblBlog" ResourceString="blogs.newblog.name"
                    EnableViewState="false" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <asp:Label runat="server" CssClass="form-control-text" ID="lblBlogVal" />
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="plcComment" Visible="false" EnableViewState="false">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblComment" ResourceString="om.activitydetails.blogcomment"
                    EnableViewState="false" DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextArea runat="server" ID="txtComment"  ReadOnly="true"
                     Wrap="false" Rows="10" />
            </div>
        </div>
    </asp:PlaceHolder>
</div>