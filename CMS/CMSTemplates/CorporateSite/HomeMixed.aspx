<%@ Page Language="C#" MasterPageFile="Root.master" AutoEventWireup="true" Inherits="CMSTemplates_CorporateSite_HomeMixed"
    ValidateRequest="false" EnableEventValidation="false"  Codebehind="HomeMixed.aspx.cs" %>

<%@ Register Src="~/CMSWebParts/Widgets/WidgetActions.ascx" TagName="WidgetActions"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSWebParts/General/MobileDeviceRedirection.ascx" TagName="MobileDeviceRedirection"
    TagPrefix="cms" %>
<asp:Content ID="cntMain" ContentPlaceHolderID="plcMain" runat="Server">
    <div class="topHome">
        <div class="padding">
            <div class="inner textContent">
                <cms:CMSEditableRegion runat="server" ID="WT" RegionType="HtmlEditor" RegionTitle="Top content text" />
            </div>
        </div>
    </div>
    <cms:CMSPagePlaceholder ID="plcLeft" runat="server">
        <LayoutTemplate>
            <!-- Container -->
            <div class="home">
                <div class="inner">
                    <cms:WidgetActions runat="server" ID="wW" WidgetZoneType="Editor" />
                    <cms:MobileDeviceRedirection runat="server" ID="MobileDeviceRedirection" SmallDeviceRedirectionURL="~/Mobile/Home.aspx"
                        LargeDeviceRedirectionURL="~/Mobile/Home.aspx" />
                    <div class="center">
                        <div class="padding">
                            <div class="textContent">
                                <cms:CMSEditableRegion runat="server" ID="WC" RegionType="HtmlEditor" RegionTitle="Content text" DialogHeight="500" />
                            </div>
                        </div>
                    </div>
                    <div class="left">
                        <div class="padding">
                            <cms:CMSWebPartZone runat="server" ID="zL" ZoneTitle="Left zone" />
                        </div>
                    </div>
                    <div class="right">
                        <div class="padding">
                            <cms:CMSWebPartZone runat="server" ID="zR" ZoneTitle="Right zone" />
                        </div>
                    </div>
                </div>
            </div>
        </LayoutTemplate>
    </cms:CMSPagePlaceholder>
</asp:Content>
