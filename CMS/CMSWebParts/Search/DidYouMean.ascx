<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Search_DidYouMean"  Codebehind="~/CMSWebParts/Search/DidYouMean.ascx.cs" %>
<asp:PlaceHolder ID="plcDidYouMean" runat="server" EnableViewState="false">
    <div class="DidYouMean">
        <span class="DidYouText">
            <asp:Literal runat="server" ID="ltrText" />
        </span>
        <asp:HyperLink ID="lnkSearch" runat="server">
            <span class="DidYouValue">
                <asp:Literal ID="ltlLinkText" runat="server" />
            </span>
        </asp:HyperLink>
    </div>
</asp:PlaceHolder>
