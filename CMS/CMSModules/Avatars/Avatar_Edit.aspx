<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Avatars_Avatar_Edit"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="Default" Title="Avatars - Edit"  Codebehind="Avatar_Edit.aspx.cs" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:LocalizedLabel runat="server" ID="lblSharedInfo" Visible="false" EnableViewState="false" />
    <div class="form-horizontal">
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" DisplayColon="true" ID="lblAvatarName" ResourceString="avat.avatarname" EnableViewState="false" AssociatedControlID="txtAvatarName" ShowRequiredMark="True" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSTextBox runat="server" ID="txtAvatarName" MaxLength="200" />
                <cms:CMSRequiredFieldValidator runat="server" ID="valAvatarName" ControlToValidate="txtAvatarName" EnableViewState="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" DisplayColon="true" runat="server" ID="lblAvatarType" ResourceString="avat.avatartype" EnableViewState="false" AssociatedControlID="drpAvatarType" />

            </div>
            <div class="editing-form-value-cell">
                <cms:CMSDropDownList runat="server" ID="drpAvatarType" AutoPostBack="true" />
            </div>
        </div>
        <asp:PlaceHolder runat="server" ID="plcImage" Visible="false">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" DisplayColon="true" runat="server" ID="lblImage" ResourceString="avat.image" EnableViewState="false" AssociatedControlID="imgAvatar" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:Image runat="server" ID="imgAvatar" EnableViewState="false" CssClass="form-control-image" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblUpload" ResourceString="uploader.upload" EnableViewState="false" AssociatedControlID="uploadAvatar" />
            </div>
            <div class="editing-form-value-cell">
                <cms:CMSFileUpload runat="server" ID="uploadAvatar" CssClass="form-control-upload" />
            </div>
        </div>
        <asp:PlaceHolder ID="plcDefaultUserAvatar" runat="server" EnableViewState="false"
            Visible="false">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDefaultUserAvatar" DisplayColon="true" ResourceString="avat.DefaultUserAvatar" EnableViewState="false" AssociatedControlID="chkDefaultUserAvatar" />

                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox runat="server" ID="chkDefaultUserAvatar" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDefaultMaleUserAvatar" DisplayColon="true" ResourceString="avat.DefaultMaleUserAvatar" EnableViewState="false" AssociatedControlID="chkDefaultMaleUserAvatar" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox runat="server" ID="chkDefaultMaleUserAvatar" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDefaultFemaleUserAvatar" DisplayColon="true" ResourceString="avat.DefaultFemaleUserAvatar" EnableViewState="false" AssociatedControlID="chkDefaultFemaleUserAvatar" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox runat="server" ID="chkDefaultFemaleUserAvatar" />
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcDefaultGroupAvatar" runat="server" EnableViewState="false"
            Visible="false">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblDefaultGroupAvatar" DisplayColon="true" ResourceString="avat.DefaultGroupAvatar" EnableViewState="false" AssociatedControlID="chkDefaultGroupAvatar" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox runat="server" ID="chkDefaultGroupAvatar" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <div class="editing-form-value-cell editing-form-value-cell-offset">
                <cms:FormSubmitButton runat="server" ID="btnOk" OnClick="btnOK_Click" EnableViewState="false" />
            </div>
        </div>
    </div>
</asp:Content>