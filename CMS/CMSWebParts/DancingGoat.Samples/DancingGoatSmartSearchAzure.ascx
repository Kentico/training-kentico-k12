<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/CMSWebParts/DancingGoat.Samples/DancingGoatSmartSearchAzure.ascx.cs" Inherits="CMSWebParts_DancingGoat_Samples_DancingGoatSmartSearchAzure" %>

<div class="col-md-4 col-lg-3 product-filter">
    <asp:TextBox runat="server" ID="txtbox" Width="100%"/>

    <h4>Coffee region</h4>
    <asp:CheckBoxList runat="server" ID="chklstFilterCountry" Visible="true" CssClass="ContentCheckBoxList checkbox-list-vertical checkbox" AutoPostBack="True" />

    <h4>Processing</h4>
    <asp:CheckBoxList runat="server" ID="chklstFilterProcessing" Visible="true" CssClass="ContentCheckBoxList checkbox-list-vertical checkbox" AutoPostBack="True" />

    <h4>Altitude</h4> (feet above sea level)
    <asp:CheckBoxList runat="server" ID="chklstFilterAltitude" Visible="true" CssClass="ContentCheckBoxList checkbox-list-vertical checkbox" AutoPostBack="True" />
    
    <h4>Sort</h4>
    <asp:DropDownList runat="server" ID="drplstOrderBy" Visible="true" style="margin-bottom: 5px"/>
    <asp:DropDownList runat="server" ID="drplstOrderByAscDesc" Visible="true" style="margin-bottom: 30px"/>

    <asp:Button Text="Find" runat="server" CssClass="btn btn-primary"  AutoPostBack="True" style="margin-bottom: 5px"/>
    <asp:Button Text="Reset" runat="server" CssClass="btn btn-primary" AutoPostBack="True" style="margin-bottom: 10px" OnClick="ButtonResetClick"/>
    
    <br/>
    <asp:Label runat="server" ID="totalCountLabel" />
</div>

<asp:Label runat="server" ID="labelPanel" Visible="true" CssClass="col-md-8 col-lg-9 product-list"/>


