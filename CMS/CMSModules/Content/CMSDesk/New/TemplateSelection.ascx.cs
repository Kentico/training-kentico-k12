using System;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_New_TemplateSelection : CMSUserControl
{
    #region "Variables"

    private string mUIModuleName = "CMS.Content";
    private string mUINewElementName = "New";
    private string mUISelectTempalteElementName = "New.SelectTemplate";

    #endregion


    #region "Events"

    public event EventHandler OnBeforePersonalizationCheck;

    #endregion


    #region "Properties"

    /// <summary>
    /// Returns true if some option is available.
    /// </summary>
    public bool SomeOptionAvailable
    {
        get
        {
            return plcRadioButtons.Visible;
        }
    }


    /// <summary>
    /// Document ID for the selection.
    /// </summary>
    public int DocumentID
    {
        get
        {
            return templateSelector.DocumentID;
        }
        set
        {
            templateSelector.DocumentID = value;
        }
    }


    /// <summary>
    /// Parent Node ID.
    /// </summary>
    public int ParentNodeID
    {
        get;
        set;
    }


    /// <summary>
    /// Returns true if the newly created template is empty.
    /// </summary>
    public bool NewTemplateIsEmpty
    {
        get
        {
            return radCreateBlank.Checked || radCreateEmpty.Checked;
        }
    }


    /// <summary>
    /// State indicate which selector is currently used
    /// </summary>
    public int TemplateSelectionState
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates whether only templates for product section should be displayed.
    /// </summary>
    public bool ShowOnlyProductSectionTemplates
    {
        set
        {
            templateSelector.ShowOnlyProductSectionTemplates = value;
        }
    }


    /// <summary>
    /// Name of the module for UI personalization checks. 'CMS.Content' by default.
    /// </summary>
    public string UIModuleName
    {
        get
        {
            return mUIModuleName;
        }
        set
        {
            mUIModuleName = value;
        }
    }


    /// <summary>
    /// Name of the UI element affecting whole selection of template. Default value is 'New'.
    /// </summary>
    public string UINewElementName
    {
        get
        {
            return mUINewElementName;
        }
        set
        {
            mUINewElementName = value;
        }
    }


    /// <summary>
    /// Name of the UI element affecting 'Select template' section. Default value is 'New.SelectTemplate'.
    /// </summary>
    public string UISelectTempalteElementName
    {
        get
        {
            return mUISelectTempalteElementName;
        }
        set
        {
            mUISelectTempalteElementName = value;
        }
    }


    /// <summary>
    /// Root category ID. 
    /// </summary>
    public int RootCategoryID
    {
        get
        {
            return templateSelector.RootCategoryID;
        }
        set
        {
            templateSelector.RootCategoryID = value;
        }
    }


    /// <summary>
    /// If true, the control offers only options valid for product sections.
    /// </summary>
    public bool IsProductSection
    {
        get;
        set;
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // Select root category, don't show UI templates
        if (RootCategoryID == 0)
        {
            PageTemplateCategoryInfo ptci = PageTemplateCategoryInfoProvider.GetPageTemplateCategoryInfo("/");
            if (ptci != null)
            {
                templateSelector.RootCategoryID = ptci.CategoryId;
            }
        }

        // Check the first radio button which is visible by default
        if (!RequestHelper.IsPostBack())
        {
            if (plcRadioButtons.IsHidden || plcRadioButtonsNew.IsHidden || IsProductSection)
            {
                radInherit.Checked = true;
            }
            else
            {
                if (!plcUseTemplate.IsHidden)
                {
                    radUseTemplate.Checked = true;
                }
                else if (!plcInherit.IsHidden)
                {
                    radInherit.Checked = true;
                }
                else if (!plcCreateBlank.IsHidden)
                {
                    radCreateBlank.Checked = true;
                }
                else if (!plcCreateEmpty.IsHidden)
                {
                    radCreateEmpty.Checked = true;
                }
            }

            plcRadioButtons.Visible = !plcRadioButtons.IsHidden && !(plcUseTemplate.IsHidden && plcInherit.IsHidden && plcCreateBlank.IsHidden && plcCreateEmpty.IsHidden);

            if (!SomeOptionAvailable)
            {
                RedirectToUINotAvailable();
            }
        }

        // Check authorization per resource
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Design", "Design"))
        {
            radCreateBlank.Visible = false;
            radCreateEmpty.Visible = false;
            layoutSelector.Visible = false;

            if (plcUseTemplate.IsHidden && plcInherit.IsHidden && !plcCreateBlank.IsHidden)
            {
                RedirectToAccessDenied("CMS.Design", "Design");
            }
        }

        if (IsProductSection)
        {
            radCreateBlank.Visible = false;
            radCreateEmpty.Visible = false;
            radUseTemplate.Visible = false;

            radInherit.Enabled = false;
        }

        radUseTemplate.CheckedChanged += radOptions_CheckedChanged;
        radInherit.CheckedChanged += radOptions_CheckedChanged;
        radCreateBlank.CheckedChanged += radOptions_CheckedChanged;
        radCreateEmpty.CheckedChanged += radOptions_CheckedChanged;

        // Disable startup focus functionality
        templateSelector.UseStartUpFocus = false;

        LoadControls();
    }


    /// <summary>
    /// Gets the validation script for the selector.
    /// </summary>
    public string GetValidationScript()
    {
        if (!radInherit.Checked && !radCreateEmpty.Checked)
        {
            string errorMessage = ScriptHelper.GetString(GetString(radCreateBlank.Checked ? "NewPage.LayoutError" : "newpage.templateerror"));

            return
@"
if (UniFlat_GetSelectedValue) { 
    value = UniFlat_GetSelectedValue();
    value = value.replace(/^\\s+|\\s+$/g, '');
    if (value == '') {
        errorLabel.style.display = '';
        errorLabel.innerHTML = " + errorMessage + @"; 
        resizearea();
        return false;
    }
}";
        }

        return null;
    }


    /// <summary>
    /// Handles setting of propper UI element names and module names for UI personalization.
    /// </summary>
    protected void OnBeforeUICheck(object sender, EventArgs e)
    {
        if (OnBeforePersonalizationCheck != null)
        {
            OnBeforePersonalizationCheck(sender, e);
        }

        UIPlaceHolder plc = sender as UIPlaceHolder;

        if (plc != null)
        {
            // Set module name
            plc.ModuleName = UIModuleName;

            // Set UI element names for placeholders
            if (plc == plcRadioButtonsNew)
            {
                plc.ElementName = UINewElementName;
            }
            else if (plc == plcRadioButtons)
            {
                plc.ElementName = UISelectTempalteElementName;
            }
        }
    }


    /// <summary>
    /// Handles radio button change.
    /// </summary>
    protected void radOptions_CheckedChanged(object sender, EventArgs e)
    {
        // Template selector needs to reload its tree
        if (radUseTemplate.Checked)
        {
            templateSelector.ResetToDefault();

            // Reload template tree
            templateSelector.ReloadData(false);

            // Recalculate the items count
            templateSelector.RegisterRefreshPageSizeScript(true);

            // Enable startup focus functionality
            templateSelector.UseStartUpFocus = true;
        }
        else if (radCreateBlank.Checked)
        {
            layoutSelector.UniFlatSelector.ResetToDefault();

            // Recalculate the items count
            layoutSelector.RegisterRefreshPageSizeScript(true);

            // Enable startup focus functionality
            layoutSelector.UniFlatSelector.UseStartUpFocus = true;
        }

        // Start rezise after radio change
        ScriptHelper.RegisterStartupScript(Page, typeof(string), "ResizeRecount", ScriptHelper.GetScript("resizearea();"));

        // Update panel
        pnlUpdate.Update();
    }


    /// <summary>
    /// Returns template name of parent node.
    /// </summary>    
    private string GetParentNodePageTemplate()
    {
        TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        TreeNode node = tree.SelectSingleNode(ParentNodeID, LocalizationContext.PreferredCultureCode, tree.CombineWithDefaultCulture, false);
        if (node != null)
        {
            string colName = node.GetUsedPageTemplateIdColumn();

            int templateId = ValidationHelper.GetInteger(node.GetValue(colName), 0);
            bool inherited = false;

            // May be inherited template
            if ((templateId == 0) && (node.NodeParentID > 0))
            {
                // Get inherited page template
                templateId = ValidationHelper.GetInteger(node.GetInheritedValue(colName, node.NodeTemplateForAllCultures), 0);
                inherited = true;
            }

            if (templateId > 0)
            {
                PageTemplateInfo pti = PageTemplateInfoProvider.GetPageTemplateInfo(templateId);
                if (pti != null)
                {
                    string templateName = pti.DisplayName;
                    if (inherited)
                    {
                        templateName += " (inherited)";
                    }
                    return templateName;
                }
            }
        }

        return String.Empty;
    }


    /// <summary>
    /// Setups and load control selected by radio buttons.
    /// </summary>
    private void LoadControls()
    {
        if (radUseTemplate.Checked)
        {
            ShowTemplateSelector(true);
            ShowLayoutSelector(false);
            ShowInherited(false);
            TemplateSelectionState = 0;
        }
        else if (radInherit.Checked)
        {
            lblIngerited.Text = GetString("NewPage.InheritedTemplateName").Replace("##TEMPLATENAME##", GetParentNodePageTemplate());
            ShowTemplateSelector(false);
            ShowLayoutSelector(false);
            ShowInherited(true);
            TemplateSelectionState = 1;
        }
        else if (radCreateBlank.Checked)
        {
            ShowTemplateSelector(false);
            ShowLayoutSelector(true);
            ShowInherited(false);
            TemplateSelectionState = 2;
        }
        else
        {
            lblIngerited.Text = GetString("NewPage.BlankTemplate");
            ShowTemplateSelector(false);
            ShowLayoutSelector(false);
            ShowInherited(true);
            TemplateSelectionState = 3;
        }
    }


    /// <summary>
    /// Enables or disables template selector.
    /// </summary>    
    private void ShowTemplateSelector(bool show)
    {
        templateSelector.Visible = show;
        templateSelector.StopProcessing = !show;
    }


    /// <summary>
    /// Enables or disables layout selector.
    /// </summary>
    private void ShowLayoutSelector(bool show)
    {
        plcLayout.Visible = show;
        layoutSelector.StopProcessing = !show;
    }


    /// <summary>
    /// Enables or disables inherited label.
    /// </summary>
    private void ShowInherited(bool show)
    {
        plcInherited.Visible = show;
    }


    /// <summary>
    /// Sets the default page template ID to the selector
    /// </summary>
    /// <param name="templateId">Template ID</param>
    public void SetDefaultTemplate(int templateId)
    {
        templateSelector.SelectedItem = templateId.ToString();
    }


    /// <summary>
    /// Ensures the template from the selection and returns the template ID.
    /// </summary>
    /// <param name="documentName">Document name for the ad-hoc template</param>
    /// <param name="nodeGuid">Owner node GUID in case of ad-hoc template</param>
    /// <param name="errorMessage">Returns the error message</param>
    public PageTemplateInfo EnsureTemplate(string documentName, Guid nodeGuid, ref string errorMessage)
    {
        bool cloneAsAdHoc = false;
        bool masterOnly = false;

        PageTemplateInfo templateInfo = null;

        // Template selection
        if (radUseTemplate.Checked)
        {
            // Template page
            int templateId = ValidationHelper.GetInteger(templateSelector.SelectedItem, 0);
            if (templateId > 0)
            {
                // Get the template and check if it should be cloned
                templateInfo = PageTemplateInfoProvider.GetPageTemplateInfo(templateId);
                if (templateInfo != null)
                {
                    cloneAsAdHoc = templateInfo.PageTemplateCloneAsAdHoc;
                }
            }
            else
            {
                errorMessage = GetString("NewPage.TemplateError");

                // Reload template selector to show correct subtree
                templateSelector.ResetToDefault();
            }
        }
        else if (radInherit.Checked)
        {
            // Inherited page           
        }
        else if (radCreateBlank.Checked || radCreateEmpty.Checked)
        {
            // Create custom template info for the page
            templateInfo = new PageTemplateInfo();

            if (radCreateBlank.Checked)
            {
                // Blank page with layout
                int layoutId = ValidationHelper.GetInteger(layoutSelector.SelectedItem, 0);
                if (layoutId > 0)
                {
                    templateInfo.LayoutID = layoutId;

                    // Copy layout to selected template
                    if (chkLayoutPageTemplate.Checked)
                    {
                        templateInfo.LayoutID = 0;
                        LayoutInfo li = LayoutInfoProvider.GetLayoutInfo(layoutId);
                        if (li != null)
                        {
                            templateInfo.PageTemplateLayout = li.LayoutCode;
                            templateInfo.PageTemplateLayoutType = li.LayoutType;
                        }
                        else
                        {
                            errorMessage = GetString("NewPage.LayoutError");
                        }
                    }
                }
                else
                {
                    errorMessage = GetString("NewPage.LayoutError");
                }
            }
            else if (radCreateEmpty.Checked)
            {
                // Empty template
                templateInfo.LayoutID = 0;
                templateInfo.PageTemplateLayout = "<cms:CMSWebPartZone ZoneID=\"zoneA\" runat=\"server\" />";

                templateInfo.PageTemplateLayoutType = LayoutTypeEnum.Ascx;
            }

            if (String.IsNullOrEmpty(errorMessage))
            {
                cloneAsAdHoc = true;
                masterOnly = true;
            }
        }

        if (cloneAsAdHoc)
        {
            // Prepare ad-hoc template name
            string displayName = "Ad-hoc: " + documentName;

            // Create ad-hoc template
            templateInfo = PageTemplateInfoProvider.CloneTemplateAsAdHoc(templateInfo, displayName, SiteContext.CurrentSiteID, nodeGuid);
            
            // Set inherit only master 
            if (masterOnly)
            {
                templateInfo.InheritPageLevels = "\\";
            }

            PageTemplateInfoProvider.SetPageTemplateInfo(templateInfo);

            if (SiteContext.CurrentSite != null)
            {
                PageTemplateInfoProvider.AddPageTemplateToSite(templateInfo.PageTemplateId, SiteContext.CurrentSiteID);
            }

            CheckOutTemplate(templateInfo);
        }

        // Assign owner node GUID
        if ((templateInfo != null) && !templateInfo.IsReusable)
        {
            templateInfo.PageTemplateNodeGUID = nodeGuid;
        }

        // Reload the template selector in case of error
        if (!String.IsNullOrEmpty(errorMessage))
        {
            if (radUseTemplate.Checked)
            {
                templateSelector.ReloadData();
            }
        }

        return templateInfo;
    }


    /// <summary>
    /// Checks out the page template
    /// </summary>
    /// <param name="pageTemplateInfo">Page template to check out</param>
    private void CheckOutTemplate(PageTemplateInfo pageTemplateInfo)
    {
        var objectManager = CMSObjectManager.GetCurrent(this) ?? new CMSObjectManager();
        if (CMSObjectManager.KeepNewObjectsCheckedOut)
        {
            objectManager.CheckOutObject(pageTemplateInfo);
        }
    }
}