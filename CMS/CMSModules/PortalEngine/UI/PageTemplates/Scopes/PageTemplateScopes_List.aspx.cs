using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_PageTemplates_Scopes_PageTemplateScopes_List : CMSEditTemplatePage
{
    #region "Variables"

    private int siteID;

    #endregion


    #region "Events"

    /// <summary>
    /// Page load event.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
        {
            RedirectToAccessDenied(GetString("template.scopes.denied"));
        }

        // Master page settings
        CurrentMaster.DisplaySiteSelectorPanel = true;

        if (!RequestHelper.IsPostBack())
        {
            if (PageTemplate != null)
            {
                radAllPages.Checked = PageTemplate.PageTemplateForAllPages;
                radSelectedScopes.Checked = !PageTemplate.PageTemplateForAllPages;
            }
            siteID = QueryHelper.GetInteger("siteid", 0);
            if (siteID > 0)
            {
                selectSite.Value = siteID;
            }
        }
        else
        {
            siteID = ValidationHelper.GetInteger(selectSite.Value, 0);
        }

        // Display only assigned sites
        selectSite.UniSelector.WhereCondition = "SiteID IN (SELECT SiteID FROM CMS_PageTemplateSite WHERE PageTemplateID = " + PageTemplateID + ") OR SiteID IN (SELECT PageTemplateScopeSiteID FROM CMS_PageTemplateScope WHERE PageTemplateScopeTemplateID = " + PageTemplateID + ")";

        // Show scopes content only if option template can be used within scopes is selected
        if (radAllPages.Checked)
        {
            pnlContent.Visible = false;
            CurrentMaster.DisplaySiteSelectorPanel = false;
        }
        else
        {
            CurrentMaster.DisplaySiteSelectorPanel = true;
            pnlContent.Visible = true;
            // New item link
            SetAction(0, GetString("template.scopes.newscope"), "javascript: AddNewItem()");
        }

        // Setup unigrid
        unigridScopes.OnAction += unigridScopes_OnAction;
        unigridScopes.OnExternalDataBound += unigridScopes_OnExternalDataBound;
        unigridScopes.WhereCondition = GenerateWhereCondition();
        unigridScopes.ZeroRowsText = GetString("general.nodatafound");

        // Set site selector
        selectSite.DropDownSingleSelect.AutoPostBack = true;
        selectSite.AllowAll = false;
        selectSite.OnlyRunningSites = false;
        selectSite.UniSelector.SpecialFields.Add(new SpecialField { Text = GetString("template.scopes.global"), Value = "0" });
        selectSite.UniSelector.OnSelectionChanged += UniSelector_OnSelectionChanged;

        // Register correct script for new item
        ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "AddNewItem", ScriptHelper.GetScript(
            "function AddNewItem() { this.window.location = '" + ResolveUrl("PageTemplateScope_Edit.aspx?templateid=" + PageTemplateID + "&siteID=" + siteID) + "'} "));
    }


    /// <summary>
    /// On selection changed.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private void UniSelector_OnSelectionChanged(object sender, EventArgs e)
    {
        pnlUpdate.Update();
    }


    /// <summary>
    /// On unigrids external databond.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="sourceName">Name</param>
    /// <param name="parameter">Parameter</param>    
    private object unigridScopes_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            // Class name
            case "documenttype":
                int classID = ValidationHelper.GetInteger(parameter, 0);
                DataClassInfo dataClass = DataClassInfoProvider.GetDataClassInfo(classID);
                if (dataClass != null)
                {
                    return dataClass.ClassDisplayName;
                }
                else
                {
                    return GetString("general.all");
                }

            // Culture
            case "culture":
                int cultureID = ValidationHelper.GetInteger(parameter, 0);
                if (cultureID > 0)
                {
                    CultureInfo culture = CultureInfoProvider.GetCultureInfo(cultureID);
                    if (culture != null)
                    {
                        return culture.CultureCode;
                    }
                }
                return GetString("general.all");

            // Levels
            case "levels":
                string levels = ValidationHelper.GetString(parameter, String.Empty);
                if (string.IsNullOrEmpty(levels))
                {
                    return GetString("general.all");
                }

                // Format levels
                levels = levels.Replace("/", String.Empty).Replace("{", " ").Replace("}", ",");
                return levels.TrimEnd(',');
        }

        return null;
    }


    /// <summary>
    /// Unigrid on action.
    /// </summary>
    /// <param name="actionName">Action name</param>
    /// <param name="actionArgument">Argument</param>
    private void unigridScopes_OnAction(string actionName, object actionArgument)
    {
        switch (actionName.ToLowerCSafe())
        {
            case "edit":
                URLHelper.Redirect(UrlResolver.ResolveUrl("PageTemplateScope_Edit.aspx?scopeid=" + ValidationHelper.GetString(actionArgument, "0") + "&templateid=" + PageTemplateID + "&siteID=" + siteID));
                break;

            case "delete":
                PageTemplateScopeInfoProvider.DeletePageTemplateScopeInfo(ValidationHelper.GetInteger(actionArgument, 0));
                break;
        }
    }


    /// <summary>
    /// Radiobutton checked changed.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    protected void radAllPages_CheckedChanged(object sender, EventArgs e)
    {
        UpdatePageTemplateInfo();
    }


    /// <summary>
    /// Radiobutton checked changed.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    protected void radSelectedScopes_CheckedChanged(object sender, EventArgs e)
    {
        UpdatePageTemplateInfo();
    }


    /// <summary>
    /// Updates PageTemplateForAllPages property of template info.
    /// </summary>
    private void UpdatePageTemplateInfo()
    {
        if (PageTemplate != null)
        {
            PageTemplate.PageTemplateForAllPages = radAllPages.Checked;
            PageTemplateInfoProvider.SetPageTemplateInfo(PageTemplate);
        }

        ShowChangesSaved();
    }


    /// <summary>
    /// Generates where condition for unigrid.
    /// </summary>    
    private string GenerateWhereCondition()
    {
        string where = "PageTemplateScopeTemplateID=" + PageTemplateID + "AND PageTemplateScopeSiteID";

        if (siteID > 0)
        {
            return where + " = " + siteID;
        }
        else
        {
            return where + " IS NULL";
        }
    }

    #endregion
}