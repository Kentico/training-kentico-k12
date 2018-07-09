using System;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


[UIElement(ModuleName.CONTENT, "Properties.Menu")]
public partial class CMSModules_Content_CMSDesk_Properties_Menu : CMSPropertiesPage
{
    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Content", "Properties.Menu"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.Menu");
        }

        // Init document manager events
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnAfterAction += DocumentManager_OnAfterAction;

        EnableSplitMode = true;

        FillSitemapOptions();

        // Register the scripts
        ScriptHelper.RegisterLoader(Page);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SetPropertyTab(TAB_MENU);

        radInactive.Attributes.Add("onclick", "enableTextBoxes('inactive')");
        radStandard.Attributes.Add("onclick", "enableTextBoxes('')");
        radUrl.Attributes.Add("onclick", "enableTextBoxes('url')");
        radJavascript.Attributes.Add("onclick", "enableTextBoxes('java')");
        radFirstChild.Attributes.Add("onclick", "enableTextBoxes('')");
        radFirstChild.Text = string.Format("{0}&nbsp;({1})", GetString("MenuProperties.FirstChild"), GetFirstChildUrl());

        pnlContent.Enabled = !DocumentManager.ProcessingAction;
    }


    private string GetFirstChildUrl()
    {
        var node = Node;
        var url = TreePathUtils.GetFirstChildUrl(node.NodeSiteName, node.NodeAliasPath, node.DocumentCulture);
        return string.IsNullOrEmpty(url) ? GetString("content.menu.nochilddocument") : string.Format("<a target=\"_blank\" href=\"{0}\" >{0}</a>", UrlResolver.ResolveUrl(url));
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ReloadData();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Fills the sitemap dropdowns values
    /// </summary>
    private void FillSitemapOptions()
    {
        drpChange.Items.Clear();

        drpChange.Items.Add(new ListItem(GetString("general.notspecified"), ""));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.always"), "always"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.hourly"), "hourly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.daily"), "daily"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.weekly"), "weekly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.monthly"), "monthly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.yearly"), "yearly"));
        drpChange.Items.Add(new ListItem(GetString("sitemapfreq.never"), "never"));

        drpPriority.Items.Clear();

        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.lowest"), "0.0"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.verylow"), "0.1"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.low"), "0.4"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.normal"), "0.5"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.high"), "0.6"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.veryhigh"), "0.9"));
        drpPriority.Items.Add(new ListItem(GetString("sitemapprior.highest"), "1.0"));
    }


    private void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        // Refresh tree
        ScriptHelper.RefreshTree(this, Node.NodeID, Node.NodeParentID);
    }


    private void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        if (!pnlUISearch.IsHidden)
        {
            // Search
            Node.DocumentSearchExcluded = chkExcludeFromSearch.Checked;
            // Sitemap
            string sitemapSettings = drpChange.SelectedValue + ";";
            // Do not keep default value in DB
            if (drpPriority.SelectedValue != "0.5")
            {
                sitemapSettings += drpPriority.SelectedValue;
            }

            // Do not keep any data if default values are specified
            if (sitemapSettings == ";")
            {
                sitemapSettings = String.Empty;
            }

            Node.DocumentSitemapSettings = sitemapSettings;
        }

        // Update the data
        if (!pnlUIBasicProperties.IsHidden)
        {
            Node.DocumentMenuCaption = txtMenuCaption.Text.Trim();
            Node.SetValue("DocumentMenuItemHideInNavigation", !chkShowInNavigation.Checked);
            Node.SetValue("DocumentShowInSiteMap", chkShowInSitemap.Checked);
        }

        if (!pnlUIDesign.IsHidden)
        {
            Node.DocumentMenuItemImage = txtMenuItemImage.Text.Trim();
            Node.DocumentMenuItemLeftImage = txtMenuItemLeftImage.Text.Trim();
            Node.DocumentMenuItemRightImage = txtMenuItemRightImage.Text.Trim();
            Node.DocumentMenuStyle = txtMenuItemStyle.Text.Trim();
            Node.SetValue("DocumentMenuClass", txtCssClass.Text.Trim());

            Node.SetValue("DocumentMenuStyleHighlighted", txtMenuItemStyleHighlight.Text.Trim());
            Node.SetValue("DocumentMenuClassHighlighted", txtCssClassHighlight.Text.Trim());
            Node.SetValue("DocumentMenuItemImageHighlighted", txtMenuItemImageHighlight.Text.Trim());
            Node.SetValue("DocumentMenuItemLeftImageHighlighted", txtMenuItemLeftImageHighlight.Text.Trim());
            Node.SetValue("DocumentMenuItemRightImageHighlighted", txtMenuItemRightImageHighlight.Text.Trim());
        }

        if (!pnlUIActions.IsHidden)
        {
            // Menu action
            txtJavaScript.Enabled = false;
            txtUrl.Enabled = false;
            txtUrlInactive.Enabled = false;

            if (radStandard.Checked)
            {
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", "");
                    Node.SetValue("DocumentMenuJavascript", "");
                    Node.SetValue("DocumentMenuItemInactive", false);
                    Node.SetValue("DocumentMenuRedirectToFirstChild", false);
                }
            }

            if (radInactive.Checked)
            {
                txtUrl.Text = txtUrlInactive.Text;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", txtUrlInactive.Text);
                    Node.SetValue("DocumentMenuJavascript", "");
                    Node.SetValue("DocumentMenuItemInactive", true);
                    Node.SetValue("DocumentMenuRedirectToFirstChild", false);
                }
            }

            if (radFirstChild.Checked)
            {
                txtJavaScript.Enabled = false;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", "");
                    Node.SetValue("DocumentMenuJavascript", "");
                    Node.SetValue("DocumentMenuItemInactive", false);
                    Node.SetValue("DocumentMenuRedirectToFirstChild", true);
                }
            }

            if (radJavascript.Checked)
            {
                txtJavaScript.Enabled = true;
                txtUrl.Enabled = false;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", "");
                    Node.SetValue("DocumentMenuJavascript", txtJavaScript.Text);
                    Node.SetValue("DocumentMenuItemInactive", false);
                    Node.SetValue("DocumentMenuRedirectToFirstChild", false);
                }
            }

            if (radUrl.Checked)
            {
                txtJavaScript.Enabled = false;
                txtUrl.Enabled = true;
                txtUrlInactive.Text = txtUrl.Text;
                if (Node != null)
                {
                    Node.SetValue("DocumentMenuRedirectUrl", txtUrl.Text.Trim());
                    Node.SetValue("DocumentMenuJavascript", "");
                    Node.SetValue("DocumentMenuItemInactive", false);
                    Node.SetValue("DocumentMenuRedirectToFirstChild", false);
                }
            }
        }
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    private void ReloadData()
    {
        if (Node != null)
        {
            // Redirect to information page when no UI elements displayed
            if (pnlUIActions.IsHidden && pnlUIBasicProperties.IsHidden && (pnlUIDesign.IsHidden || ShowContentOnlyProperties) && pnlUISearch.IsHidden)
            {
                RedirectToUINotAvailable();
            }

            if (ShowContentOnlyProperties)
            {
                pnlUIDesign.Visible = false;
                pnlUIBasicProperties.Visible = false;
                pnlUIActions.Visible = false;
                plcAdvancedSearch.Visible = false;
                headSearch.Visible = false;
            }

            if (!RequestHelper.IsPostBack() || Node.DocumentSitemapSettings == String.Empty)
            {
                // Set default value
                drpPriority.SelectedValue = "0.5";
            }

            // Get sitemap settings
            string[] siteMapSettings = Node.DocumentSitemapSettings.Split(';');
            if (siteMapSettings.Length == 2)
            {
                drpChange.SelectedValue = siteMapSettings[0];
                drpPriority.SelectedValue = ValidationHelper.GetString(siteMapSettings[1], "0.5");
            }

            // Search
            chkExcludeFromSearch.Checked = Node.DocumentSearchExcluded;

            txtMenuCaption.Text = Node.DocumentMenuCaption;
            txtMenuItemStyle.Text = Node.DocumentMenuStyle;
            txtMenuItemImage.Text = Node.DocumentMenuItemImage;
            txtMenuItemLeftImage.Text = Node.DocumentMenuItemLeftImage;
            txtMenuItemRightImage.Text = Node.DocumentMenuItemRightImage;

            if (Node.GetValue("DocumentMenuItemHideInNavigation") != null)
            {
                chkShowInNavigation.Checked = !(Convert.ToBoolean(Node.GetValue("DocumentMenuItemHideInNavigation")));
            }
            else
            {
                chkShowInNavigation.Checked = false;
            }


            chkShowInSitemap.Checked = Convert.ToBoolean(Node.GetValue("DocumentShowInSiteMap"));

            txtCssClass.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuClass"), "");

            txtMenuItemStyleHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuStyleHighlighted"), "");
            txtCssClassHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuClassHighlighted"), "");
            txtMenuItemImageHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemImageHighlighted"), "");
            txtMenuItemLeftImageHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemLeftImageHighlighted"), "");
            txtMenuItemRightImageHighlight.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuItemRightImageHighlighted"), "");

            txtJavaScript.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuJavascript"), "");
            txtUrlInactive.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuRedirectUrl"), "");
            txtUrl.Text = ValidationHelper.GetString(Node.GetValue("DocumentMenuRedirectUrl"), "");

            // Menu Action
            SetRadioActions(0);

            // Menu action priority low to high !
            if (ValidationHelper.GetString(Node.GetValue("DocumentMenuJavascript"), "") != "")
            {
                SetRadioActions(2);
            }

            if (ValidationHelper.GetString(Node.GetValue("DocumentMenuRedirectUrl"), "") != "")
            {
                SetRadioActions(3);
            }

            if (ValidationHelper.GetBoolean(Node.GetValue("DocumentMenuItemInactive"), false))
            {
                SetRadioActions(1);
            }

            if (ValidationHelper.GetBoolean(Node.GetValue("DocumentMenuRedirectToFirstChild"), false))
            {
                SetRadioActions(4);
            }

            pnlForm.Enabled = DocumentManager.AllowSave;
        }
    }


    /// <summary>
    /// Sets radio buttons for menu action.
    /// </summary>
    private void SetRadioActions(int action)
    {
        radInactive.Checked = false;
        radStandard.Checked = false;
        radUrl.Checked = false;
        radJavascript.Checked = false;
        radFirstChild.Checked = false;

        txtJavaScript.Enabled = false;
        txtUrlInactive.Enabled = false;
        txtUrl.Enabled = false;

        switch (action)
        {
            case 1:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('inactive');"));
                    radInactive.Checked = true;
                    txtUrlInactive.Enabled = true;
                    break;
                }
            case 2:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('java');"));
                    radJavascript.Checked = true;
                    txtJavaScript.Enabled = true;
                    break;
                }
            case 3:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('url');"));
                    radUrl.Checked = true;
                    txtUrl.Enabled = true;
                    break;
                }
            case 4:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('');"));
                    radFirstChild.Checked = true;
                    break;
                }
            default:
                {
                    AddScript(ScriptHelper.GetScript("enableTextBoxes('');"));
                    radStandard.Checked = true;
                    break;
                }
        }
    }


    /// <summary>
    /// Adds the script to the page
    /// </summary>
    /// <param name="script">JavaScript code</param>
    public override void AddScript(string script)
    {
        ScriptHelper.RegisterStartupScript(Page, typeof(string), script.GetHashCode().ToString(), script);
    }

    #endregion
}
