<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/CMSWebParts/DancingGoat.Samples/DancingGoatConsents.ascx.cs" Inherits="CMSWebParts_DancingGoat_Samples_DancingGoatConsents" %>

<cms:LocalizedLabel ID="lblInfo" runat="server" ResourceString="dancinggoat.privacy.revocationsuccessful" CssClass="InfoLabel" EnableViewState="false" Visible="false" />
<cms:LocalizedLabel ID="lblNoData" runat ="server" ResourceString="dancinggoat.privacy.nodatafound" EnableViewState="false" />
<asp:Repeater runat="server" ID="rptConsents">
    <ItemTemplate>
        <div class="consent-item">
            <div class="row">
                <div class="col-md-10">
                    <h3 class="consent-heading"><%# HttpUtility.HtmlEncode(Eval("ConsentDisplayName")) %></h3>
                </div>
                <div class="col-md-2">
                    <cms:LocalizedButton runat="server" ID="btnRevoke" ResourceString="dancinggoat.privacy.revoke" ButtonStyle="Primary" CommandArgument='<%# Eval("ConsentID") %>' OnClick="btnRevoke_Click" />
                </div>
            </div>
            <div class="row">
                <p><%# Eval("ConsentShortText") %></p>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>