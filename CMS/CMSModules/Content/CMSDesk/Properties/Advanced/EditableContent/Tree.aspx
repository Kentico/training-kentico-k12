<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Advanced_EditableContent_Tree"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/Tree.master"  Codebehind="Tree.aspx.cs" %>
<%@ Import Namespace="CMS.Base" %>
<asp:Content ID="cntMenu" runat="server" ContentPlaceHolderID="plcMenu">
    <div class="tree-actions-panel">
        <div class="tree-actions">
            <cms:CMSAccessibleButton runat="server" ID="btnNew" IconCssClass="icon-plus" IconOnly="true" />
            <cms:CMSAccessibleButton runat="server" ID="btnDelete" IconCssClass="icon-bin" IconOnly="true" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcTree">
    <script type="text/javascript">
        //<![CDATA[
        var currentNode = null;
        var treeUrl = '<%=ResolveUrl("~/CMSModules/Content/CMSDesk/Properties/Advanced/EditableContent/tree.aspx") + ((Node != null) ? "?nodeid=" + Node.NodeID : String.Empty)%>';
            var isAuthorizedToModify = <%=DocumentManager.AllowSave.ToString().ToLowerCSafe()%>;

        // Refresh node action
        function RefreshNode(nodeName, nodeType, nodeId) {
            if (currentNode != null) {
                currentNode.firstChild.innerHTML = nodeName;
                // Dynamically create onclick event
                currentNode.onclick = function() { SelectNode(nodeName, nodeType, this); };
            }
        }


        // Opens confirmation dialog and delete item 
        function DeleteItem() {
            if (confirm('<%=GetString("editablecontent.confirmdelete")%>')) {
                    document.location.replace(treeUrl + '&deleteItem=true&nodename=' + document.getElementById('<%=hdnCurrentNodeName.ClientID%>').value + '&nodetype=' + document.getElementById('<%=hdnCurrentNodeType.ClientID%>').value);
                }
                return false;
            }


            // Opens 'Create new' menu on main panel
            function CreateNew() {
                parent.frames['main'].CreateNew(document.getElementById('<%=hdnCurrentNodeType.ClientID%>').value);
                return false;
            }


            function updateMenuItem(selector, condition, className) {
                var item = $cmsj(selector);
                if (item.length > 0) {
                    if (className) {
                        item.toggleClass(className, condition);
                    }
                    if (condition) {
                        item.attr('disabled', 'disabled');
                    }
                    else {
                        item.removeAttr('disabled');
                    }
                }
            }
            

            // Sets proper image for 'new item' link and disable delete link for root item
            function UpdateMenu(selectedNodeName) {
                updateMenuItem('#' + '<%=btnNew.ClientID %>', ((selectedNodeName != '') && (selectedNodeName != null)) || !isAuthorizedToModify);
                updateMenuItem('#' + '<%=btnDelete.ClientID %>',  (selectedNodeName == '') || (selectedNodeName == null) || !isAuthorizedToModify);
            }

            // Selects node action
            function SelectNode(nodeName, nodeType, nodeElem) {
                if (currentNode == null) {
                    currentNode = document.getElementById('treeSelectedNode');
                }
                if ((currentNode != null) && (nodeElem != null)) {
                    currentNode.className = 'ContentTreeItem';
                }
                parent.frames['main'].SelectNode(nodeName, nodeType);
                document.getElementById('<%=hdnCurrentNodeName.ClientID%>').value = nodeName;
                document.getElementById('<%=hdnCurrentNodeType.ClientID%>').value = nodeType;

                if (nodeElem != null) {
                    currentNode = nodeElem;
                    if (currentNode != null) {
                        currentNode.className = 'ContentTreeSelectedItem';
                    }
                }

                UpdateMenu(nodeName);
            }

            //]]>
    </script>
    <cms:MessagesPlaceHolder ID="plcMess" runat="server" IsLiveSite="false" />
    <asp:HiddenField ID="hdnCurrentNodeType" runat="server" />
    <asp:HiddenField ID="hdnCurrentNodeName" runat="server" />
    <asp:TreeView ID="webpartsTree" runat="server" CssClass="ContentTree" ShowLines="True" EnableViewState="false" />
    <br />
    <asp:TreeView ID="regionsTree" runat="server" CssClass="ContentTree" ShowLines="True" EnableViewState="false" />
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
</asp:Content>
