using System;
using System.Collections;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;

using TreeElemNode = System.Web.UI.WebControls.TreeNode;

public partial class CMSModules_Content_CMSDesk_Properties_Advanced_EditableContent_Tree : CMSModalPage
{
    #region "Private & protected variables"

    private string selectedNodeType;
    private string selectedNodeName;

    #endregion


    #region "Properties"

    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }

    #endregion


    #region "Page events"

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        // Ensure document manager
        EnsureDocumentManager = true;
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ScriptHelper.RegisterJQuery(Page);

        DocumentManager.LocalMessagesPlaceHolder = plcMess;
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;

        var user = MembershipContext.AuthenticatedUser;

        // Check 'read' permissions
        if (!user.IsAuthorizedPerResource("CMS.Content", "Read"))
        {
            RedirectToAccessDenied("CMS.Content", "Read");
        }

        // Check UIProfile
        if (!user.IsAuthorizedPerUIElement("CMS.Content", "Properties.General"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.General");
        }

        if (!user.IsAuthorizedPerUIElement("CMS.Content", "General.Advanced"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "General.Advanced");
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Get query parameters
        selectedNodeType = QueryHelper.GetString("selectednodetype", "webpart");
        selectedNodeName = QueryHelper.GetString("selectednodename", string.Empty);
        hdnCurrentNodeType.Value = selectedNodeType;
        hdnCurrentNodeName.Value = selectedNodeName;

        bool enabled = DocumentManager.AllowSave;
        btnDelete.Enabled = enabled && !string.IsNullOrEmpty(selectedNodeName);
        if (btnDelete.Enabled)
        {
            btnDelete.OnClientClick = "return DeleteItem();";
            btnDelete.ToolTip = GetString("Development-WebPart_Tree.DeleteItem");
        }

        btnNew.Enabled = enabled;
        if (btnNew.Enabled)
        {
            btnNew.OnClientClick = "return CreateNew();";
            btnNew.ToolTip = GetString("Development-WebPart_Tree.NewWebPart");
        }

        string imageUrl = "Design/Controls/Tree";

        // Initialize page
        if (CultureHelper.IsUICultureRTL())
        {
            imageUrl = GetImageUrl("RTL/" + imageUrl);
        }
        else
        {
            imageUrl = GetImageUrl(imageUrl);
        }
        webpartsTree.LineImagesFolder = imageUrl;
        regionsTree.LineImagesFolder = imageUrl;

        if (Node != null)
        {
            string webPartRootAttributes;
            string regionRootAttributes;

            if (string.IsNullOrEmpty(selectedNodeName))
            {
                switch (selectedNodeType)
                {
                    case "webpart":
                        webPartRootAttributes = "class=\"ContentTreeSelectedItem\" id=\"treeSelectedNode\"";
                        regionRootAttributes = "class=\"ContentTreeItem\" ";
                        break;

                    case "region":
                        webPartRootAttributes = "class=\"ContentTreeItem\" ";
                        regionRootAttributes = "class=\"ContentTreeSelectedItem\" id=\"treeSelectedNode\"";
                        break;
                    default:
                        webPartRootAttributes = "class=\"ContentTreeSelectedItem\" id=\"treeSelectedNode\"";
                        regionRootAttributes = "class=\"ContentTreeItem\" ";
                        break;
                }
            }
            else
            {
                webPartRootAttributes = "class=\"ContentTreeSelectedItem\" id=\"treeSelectedNode\"";
                regionRootAttributes = "class=\"ContentTreeItem\" ";
            }

            // Create tree menus
            TreeElemNode rootWebpartNode = new TreeElemNode();
            rootWebpartNode.Text = "<span " + webPartRootAttributes + " onclick=\"SelectNode('','webpart', this);\"><span class=\"Name\">" + ScriptHelper.GetLocalizedString("EditableWebparts.Root", false) + "</span></span>";
            rootWebpartNode.Expanded = true;
            rootWebpartNode.NavigateUrl = "#";

            TreeElemNode rootRegionNode = new TreeElemNode();
            rootRegionNode.Text = "<span " + regionRootAttributes + " onclick=\"SelectNode('','region', this);\"><span class=\"Name\">" + ScriptHelper.GetLocalizedString("EditableRegions.Root", false) + "</span></span>";
            rootRegionNode.Expanded = true;
            rootRegionNode.NavigateUrl = "#";

            // Editable web parts
            webpartsTree.Nodes.Add(rootWebpartNode);
            if (Node.DocumentContent.EditableWebParts.Count > 0)
            {
                foreach (DictionaryEntry webPart in Node.DocumentContent.EditableWebParts)
                {
                    string key = webPart.Key.ToString();
                    string name = EditableItems.GetFirstKey(key);
                    AddNode(rootWebpartNode, name, "webpart");
                }
            }

            // Editable regions
            regionsTree.Nodes.Add(rootRegionNode);
            if (Node.DocumentContent.EditableRegions.Count > 0)
            {
                foreach (DictionaryEntry region in Node.DocumentContent.EditableRegions)
                {
                    string key = region.Key.ToString();
                    string name = EditableItems.GetFirstKey(key);
                    AddNode(rootRegionNode, name, "region");
                }
            }
        }

        // Delete item if requested from query string
        string nodeType = QueryHelper.GetString("nodetype", null);
        string nodeName = QueryHelper.GetString("nodename", null);
        if (!RequestHelper.IsPostBack() && !String.IsNullOrEmpty(nodeType) && QueryHelper.GetBoolean("deleteItem", false))
        {
            DeleteItem(nodeType, nodeName);
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Adds node to parent node.
    /// </summary>
    /// <param name="parentNode">Parent node</param>
    /// <param name="nodeName">Name of node</param>
    /// <param name="nodeType">Type of node</param>
    private void AddNode(TreeElemNode parentNode, string nodeName, string nodeType)
    {
        TreeElemNode newNode = new TreeElemNode();
        string cssClass = "ContentTreeItem";
        string elemId = string.Empty;

        // Select proper node
        if (selectedNodeName == nodeName)
        {
            if (webpartsTree.Nodes.Count > 0)
            {
                webpartsTree.Nodes[0].Text = webpartsTree.Nodes[0].Text.Replace("ContentTreeSelectedItem", "ContentTreeItem").Replace("treeSelectedNode", string.Empty);
            }
            else if (regionsTree.Nodes.Count > 0)
            {
                regionsTree.Nodes[0].Text = regionsTree.Nodes[0].Text.Replace("ContentTreeSelectedItem", "ContentTreeItem").Replace("treeSelectedNode", string.Empty);
            }
            if (nodeType == selectedNodeType)
            {
                cssClass = "ContentTreeSelectedItem";
                elemId = "id=\"treeSelectedNode\"";
            }
        }
        newNode.Text = "<span class=\"" + cssClass + "\" " + elemId + " onclick=\"SelectNode(" + ScriptHelper.GetString(nodeName) + ", " + ScriptHelper.GetString(nodeType) + ", this);\"><span class=\"Name\">" + HTMLHelper.HTMLEncode(nodeName) + "</span></span>";
        newNode.NavigateUrl = "#";
        parentNode.ChildNodes.Add(newNode);
    }


    /// <summary>
    /// Deletes item.
    /// </summary>
    protected void DeleteItem(string nodeType, string nodeName)
    {
        if (DocumentManager.AllowSave)
        {
            // Remove key from hashtable
            switch (nodeType)
            {
                case "webpart":
                    Node.DocumentContent.EditableWebParts.Remove(nodeName);
                    break;

                case "region":
                    Node.DocumentContent.EditableRegions.Remove(nodeName);
                    break;
            }

            // Save node
            SaveNode();

            // Refresh
            ltlScript.Text += ScriptHelper.GetScript("document.location.replace('" + ResolveUrl("~/CMSModules/Content/CMSDesk/Properties/Advanced/EditableContent/tree.aspx") + "?nodeid=" + Node.NodeID + "&selectednodetype=" + nodeType + "'); parent.frames['main'].SelectNode('','" + nodeType + "');");
        }
        else
        {
            ltlScript.Text += ScriptHelper.GetAlertScript(string.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), Node.NodeAliasPath));
        }
    }


    /// <summary>
    /// Saves node, ensures workflow.
    /// </summary>
    protected void SaveNode()
    {
        // Save content
        DocumentManager.SaveDocument();
    }


    void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        var node = e.Node;
        node.SetValue("DocumentContent", node.DocumentContent.GetContentXml());
    }

    #endregion
}
