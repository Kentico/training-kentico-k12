<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Avatars_Controls_AvatarsGallery"  Codebehind="AvatarsGallery.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/Pager/UIPager.ascx" TagName="UIPager"
    TagPrefix="cms" %>

<asp:Literal runat="server" ID="ltlScript" />
<div>
    <asp:Panel runat="Server" ID="pnlAvatars" ScrollBars="Auto">
        <asp:HiddenField ID="hiddenAvatarGuid" runat="server" />
        <cms:LocalizedLabel runat="server" ID="lblInfo" Visible="false" EnableViewState="false" />
        <cms:BasicRepeater ID="repAvatars" runat="server">
            <ItemTemplate>
                <div class="avatar-list-item">
                    <img src="<%#avatarUrl + Eval("AvatarGuid")%>" title="<%#HTMLHelper.HTMLEncode(Eval("AvatarName") as string)%>" alt="<%#HTMLHelper.HTMLEncode(Eval("AvatarName") as string)%>"
                        id="<%#"avat" + Eval("AvatarGuid")%>" onclick="markImage(this.id);" />
                </div>
            </ItemTemplate>
        </cms:BasicRepeater>
    </asp:Panel>
</div>
<div>
    <cms:UIPager ID="pgrAvatars" runat="server" QueryStringKey="tpage" CurrentPageSize="5" ShowPageSize="false" GroupSize="10"
        PagerMode="Querystring" EnableViewState="false" />
</div>