<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSAdminControls_Debug_AnalyticsLog"
     Codebehind="AnalyticsLog.ascx.cs" %>
<cms:UIGridView runat="server" ID="gridAnalytics" ShowFooter="true" AutoGenerateColumns="false">
    <columns>
        <asp:TemplateField>
            <ItemTemplate>
                <strong><%#GetIndex()%></strong>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%#Eval("CodeName")%>
            </ItemTemplate>
            <FooterTemplate>
                <strong>IP:</strong> <asp:Literal runat="server" ID="ltlIp" EnableViewState="false" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%#GetInformation(Eval("ObjectName"), Eval("Culture"), Eval("ObjectID"))%>
            </ItemTemplate>
            <FooterTemplate>
                <strong>Agent:</strong> <asp:Literal runat="server" ID="ltlAgent" EnableViewState="false" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%#GetCount(Eval("Count"), Eval("Value"))%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%#Eval("SiteName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%#GetContext(Eval("Context"))%>
            </ItemTemplate>
        </asp:TemplateField>
    </columns>
</cms:UIGridView>

