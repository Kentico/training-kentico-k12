<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_Controls_MyDetails_MyCredit"
     Codebehind="MyCredit.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<div class="MyCredit">
    <div style="padding-bottom: 5px;" class="TotalCredit">
        <cms:LocalizedLabel ResourceString="Ecommerce.MyCredit.TotalCredit" ID="lblCredit" runat="server" EnableViewState="false" />
        <asp:Label ID="lblCreditValue" runat="server" EnableViewState="false" />
    </div>
    <cms:UniGrid runat="server" ID="gridCreditEvents" Columns="EventID,EventDate,EventName,EventCreditChange,EventDescription"
        OrderBy="EventDate DESC" ObjectType="ecommerce.creditevent">
        <GridColumns>
            <ug:Column Source="EventDate" ExternalSourceName="eventdate" Caption="$Ecommerce.MyCredit.EventDate$"
                Wrap="false" />
            <ug:Column Source="EventName" Caption="$Ecommerce.MyCredit.EventName$" Wrap="false" />
            <ug:Column Source="EventCreditChange" ExternalSourceName="eventcreditchange" Caption="$Ecommerce.MyCredit.EventCreditChange$"
                Wrap="false" />
            <ug:Column Source="EventDescription" Caption="$Ecommerce.MyCredit.EventDescription$"
                Wrap="true" />
            <ug:Column CssClass="filling-column" />
        </GridColumns>
        <GridOptions DisplayFilter="false" />
    </cms:UniGrid>
</div>
