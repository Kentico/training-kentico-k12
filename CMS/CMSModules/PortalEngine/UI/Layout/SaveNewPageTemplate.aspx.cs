using System;

using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_Layout_SaveNewPageTemplate : CMSModalDesignPage
{
    protected int pageTemplateId = 0;
    protected PageTemplateInfo pt = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        pageTemplateId = QueryHelper.GetInteger("templateid", 0);
        if (pageTemplateId > 0)
        {
            pt = PageTemplateInfoProvider.GetPageTemplateInfo(pageTemplateId);
        }

        categorySelector.StartingPath = QueryHelper.GetString("startingpath", String.Empty);

        // Check the authorization per UI element
        var currentUser = MembershipContext.AuthenticatedUser;
        if (!currentUser.IsAuthorizedPerUIElement("CMS.Content", new[] { "Properties", "Properties.Template", "Template.SaveAsNew" }, SiteContext.CurrentSiteName))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties;Properties.Template;Template.SaveAsNew");
        }

        PageTitle.TitleText = GetString("PortalEngine.SaveNewPageTemplate.PageTitle");

        // Set category selector
        if (!RequestHelper.IsPostBack() && (pt != null))
        {
            categorySelector.Value = pt.CategoryID.ToString();
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (pt != null)
        {
            // Limit text length
            txtTemplateDisplayName.Text = TextHelper.LimitLength(txtTemplateDisplayName.Text.Trim(), 200, "");

            // finds whether required fields are not empty
            string result = new Validator()
                .NotEmpty(txtTemplateDisplayName.Text, GetString("Administration-PageTemplate_General.ErrorEmptyTemplateDisplayName"))
                .NotEmpty(txtTemplateCodeName.Text, GetString("Administration-PageTemplate_General.ErrorEmptyTemplateCodeName"))
                .IsCodeName(txtTemplateCodeName.Text, GetString("general.invalidcodename"))
                .Result;

            if (String.IsNullOrEmpty(result))
            {
                // Check if template with given name already exists            
                if (PageTemplateInfoProvider.PageTemplateNameExists(txtTemplateCodeName.Text))
                {
                    ShowError(GetString("general.codenameexists"));
                }

                bool templateCloned = false;
                var reusableOrInherited = pt.IsReusable || QueryHelper.GetBoolean("inherits", false);

                // Clone template when page template is reusable or inherited from the parent page or template shouldn't be assigned to the current page
                // Do not clone template when page template is ad-hoc and new template should be assigned to the current page
                if (reusableOrInherited || !chkKeep.Checked)
                {
                    // Clone template with clear
                    pt = pt.Clone(true);
                    templateCloned = true;
                }

                // Moving an ad-hoc template to a reusable template
                if (!pt.IsReusable)
                {
                    // Transfer template layout from file system to the database object to ensure that the new layout file (created afterwards) will contain the correct content
                    pt.PageTemplateLayout = pt.PageTemplateLayout;
                }

                pt.CodeName = txtTemplateCodeName.Text;
                pt.DisplayName = txtTemplateDisplayName.Text;
                pt.Description = txtTemplateDescription.Text;

                pt.CategoryID = Convert.ToInt32(categorySelector.Value);

                // Reset the Ad-hoc status
                pt.IsReusable = true;
                pt.PageTemplateNodeGUID = Guid.Empty;

                pt.PageTemplateSiteID = 0;

                if (templateCloned)
                {
                    // After all properties were set, reset object original values in order to behave as a new object. This ensures that a new layout file is created and the original one is not deleted.
                    pt.ResetChanges();
                }

                try
                {
                    PageTemplateInfoProvider.SetPageTemplateInfo(pt);
                    int siteId = QueryHelper.GetInteger("siteid", 0);
                    if (siteId > 0)
                    {
                        PageTemplateInfoProvider.AddPageTemplateToSite(pt.PageTemplateId, siteId);
                    }

                    if (!chkKeep.Checked)
                    {
                        ShowInformation(GetString("PortalEngine.SaveNewPageTemplate.Saved"));

                        txtTemplateCodeName.Text = pt.CodeName;

                        pnlContent.Enabled = false;
                        btnOk.Visible = false;
                    }
                    else
                    {
                        var documentId = QueryHelper.GetInteger("documentId", 0);
                        if (reusableOrInherited && (documentId > 0))
                        {
                            // Assign the new page template to the current document
                            AssignNewTemplateToDocument(documentId, pt.PageTemplateId);
                        }

                        RegisterPageTemplateSavedScript();
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
            else
            {
                ShowError(result);
            }
        }
    }


    /// <summary>
    /// PageTemplate was changed in dialog, we have to change data in page template selector via wopener.
    /// </summary>
    private void RegisterPageTemplateSavedScript()
    {
        string selectorId = QueryHelper.GetString("selectorid", String.Empty);

        ScriptHelper.RegisterStartupScript(this, typeof(string), "SaveAsNewTemplate", ScriptHelper.GetScript(@"
        if (wopener.OnSaveAsNewPageTemplate)
        {
            wopener.OnSaveAsNewPageTemplate(" + pt.PageTemplateId + ", " + ScriptHelper.GetString(selectorId) + @");
        };
        CloseDialog();"));
    }


    /// <summary>
    /// Assignes newly created page template to the current document.
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="templateId">Template ID</param>
    private void AssignNewTemplateToDocument(int documentId, int templateId)
    {
        var node = DocumentHelper.GetDocument(documentId, new TreeProvider());
        if (node == null)
        {
            return;
        }

        if (node.NodeInheritPageTemplate)
        {
            // If node inherited page template switch to using shared template for all culture
            node.NodeInheritPageTemplate = false;
            node.NodeTemplateForAllCultures = true;
        }

        node.NodeTemplateID = node.NodeTemplateForAllCultures ? templateId : 0;
        node.DocumentPageTemplateID = templateId;

        node.Update();
    }
}
