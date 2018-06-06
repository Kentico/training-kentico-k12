<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Friends_CMSPages_FriendshipManagement" Title="Friendship management"
    ValidateRequest="false" Theme="Default"  Codebehind="FriendshipManagement.aspx.cs" %>

<%@ Register Src="~/CMSWebParts/Community/Friends/FriendshipManagement.ascx" TagName="FriendshipManagement"
    TagPrefix="cms" %>
<!DOCTYPE html>
<html>
<head id="headElem" runat="server" enableviewstate="false">
    <title>Friendship management</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cms:FriendshipManagement ID="managementElem" runat="server" />
    </div>
    </form>
</body>
</html>
