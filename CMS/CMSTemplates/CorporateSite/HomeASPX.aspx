<%@ Page Language="C#" MasterPageFile="Root.master" AutoEventWireup="true" Inherits="CMSTemplates_CorporateSite_HomeASPX"
    ValidateRequest="false"  Codebehind="HomeASPX.aspx.cs" EnableEventValidation="false" %>

<%@ Register Src="~/CMSWebParts/General/MobileDeviceRedirection.ascx" TagName="MobileDeviceRedirection"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSWebParts/Newsletters/NewsletterSubscriptionWebPart.ascx" TagName="NewsletterSubscription"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSWebParts/Polls/Poll.ascx" TagName="Poll" TagPrefix="cms" %>
<%@ Register Src="~/CMSWebParts/Viewers/Effects/ScrollingText.ascx" TagName="ScrollingText" TagPrefix="cms" %>
<%@ Register Src="~/CMSWebParts/Ecommerce/Products/RandomProducts.ascx" TagName="RandomProducts" TagPrefix="cms" %>
<asp:Content ID="cntMain" ContentPlaceHolderID="plcMain" runat="Server">
    <div class="topHome">
        <div class="padding">
            <div class="inner textContent">
                <cms:CMSEditableRegion runat="server" ID="TopText" RegionType="HtmlEditor" RegionTitle="Top content text" />
            </div>
        </div>
    </div>
    <!-- Container -->
    <div class="home">
        <div class="inner">
            <cms:MobileDeviceRedirection runat="server" ID="MobileDeviceRedirection" SmallDeviceRedirectionURL="~/Mobile/Home.aspx"
                LargeDeviceRedirectionURL="~/Mobile/Home.aspx" />
            <div class="center">
                <div class="padding">
                    <div class="textContent">
                        <cms:CMSEditableRegion runat="server" ID="ContentText" RegionType="HtmlEditor" RegionTitle="Content text"
                            DialogHeight="500" />
                    </div>
                </div>
            </div>
            <div class="left">
                <div class="padding">
                    <cms:WebPartContainer ID="wpcNewsletterSubscription" runat="server" ContainerName="CorporateSite.LightGradientBoxLeft"
                        ContainerTitle="Newsletter" ContainerCSSClass="newsletter">
                        <cms:NewsletterSubscription runat="server" ID="NewsletterSubscription" NewsletterName="CorporateNewsletter"
                            AllowUserSubscribers="false" DisplayLastName="true" DisplayFirstName="true" />
                    </cms:WebPartContainer>
                    <cms:WebPartContainer ID="wpcFeaturedProducts" runat="server" ContainerName="CorporateSite.LightGradientBoxLeft"
                        ContainerTitle="Featured product" ContainerCSSClass="product">
                        <cms:RandomProducts runat="server" ID="FeaturedProduct" Path="/%" OnlyNRandomProducts="1" ProductPublicStatusName="FeaturedProduct" TransformationName="CorporateSite.Transformations.ProductFeatured" ZeroRowsText="There are currently no products." />
                    </cms:WebPartContainer>
                </div>
            </div>
            <div class="right">
                <div class="padding">
                    <cms:WebPartContainer id="wpcNews" runat="server" ContainerName="CorporateSite.LightGradientBoxRight"
                        ContainerTitle="Latest news" ContainerCSSClass="news" >
                        <cms:ScrollingText ID="News" runat="server" Path="/%" ClassNames="cms.news" OrderBy="NewsReleaseDate DESC" SelectTopN="10" TransformationName="CorporateSite.Transformations.NewsHome" DivWidth="200" DivHeight="150" ZeroRowsText="There are currently no news." JsMoveTime="1000" JsStopTime="3000" />
					</cms:WebPartContainer>
                    <cms:WebPartContainer ID="wpcPoll" runat="server" ContainerName="CorporateSite.LightGradientBoxRight"
                        ContainerTitle="Poll" ContainerCSSClass="poll">
                        <cms:Poll runat="server" ID="Poll" PollCodeName="NewWebsitePoll" CountType="Absolute"
                            CheckVoted="False" ShowGraph="true" ShowResultsAfterVote="true" />
                    </cms:WebPartContainer>
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
