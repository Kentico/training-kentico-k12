<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Syndication_ProductsRSSFeed"  Codebehind="~/CMSWebParts/Ecommerce/Syndication/ProductsRSSFeed.ascx.cs" %>
<%@ Register TagPrefix="cms" Namespace="CMS.Ecommerce.Web.UI" Assembly="CMS.Ecommerce.Web.UI" %>
<cms:ProductsDataSource runat="server" ID="srcProducts" />
<cms:RSSFeed ID="rssFeed" runat="server" />