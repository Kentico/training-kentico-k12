using System;

using CMS.Base;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_PageTemplates_Scopes_PageTemplateScope_Edit : CMSEditTemplatePage
{
    #region "Variables"

    private int scopeID;
    private int siteID;
    private int cultureId;
    private int classId;
    private PageTemplateScopeInfo ptsi;

    #endregion


    #region "Page and controls events"

    /// <summary>
    /// Page load event.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
        {
            RedirectToAccessDenied(GetString("template.scopes.denied"));
        }

        // Show changes saved message
        if (QueryHelper.GetInteger("saved", 0) == 1 && !RequestHelper.IsPostBack())
        {
            ShowChangesSaved();
        }

        // Get template id, scope id and site id
        scopeID = QueryHelper.GetInteger("scopeid", 0);
        siteID = QueryHelper.GetInteger("siteid", 0);

        // Get sitename
        string siteName = string.Empty;
        if (siteID > 0)
        {
            SiteInfo site = SiteInfoProvider.GetSiteInfo(siteID);
            if (site != null)
            {
                siteName = " (" + site.DisplayName + ")";
            }
        }

        // Breakcrumbs initialization        		
        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = GetString("template.scopes"),
            RedirectUrl = "~/CMSModules/PortalEngine/UI/PageTemplates/Scopes/PageTemplateScopes_List.aspx?siteid=" + siteID + "&templateid=" + PageTemplateID,
        });

        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = GetString("template.scopes.new") + siteName,
        });


        // Set up controls
        cultureElem.CurrentSelector.AllowEmpty = false;
        cultureElem.CurrentSelector.SelectionMode = SelectionModeEnum.SingleDropDownList;
        cultureElem.ReturnColumnName = "CultureID";
        cultureElem.DisplayAllValue = true;
        cultureElem.SiteID = -1;

        classElem.DisplayAllValue = true;

        // Get scope info
        ptsi = (scopeID > 0) ? PageTemplateScopeInfoProvider.GetPageTemplateScopeInfo(scopeID) : new PageTemplateScopeInfo();

        // Set edited object
        EditedObject = ptsi;

        if (scopeID > 0)
        {
            if (ptsi != null)
            {
                cultureId = ptsi.PageTemplateScopeCultureID;
                classId = ptsi.PageTemplateScopeClassID;
                PageBreadcrumbs.Items[1].Text = ptsi.PageTemplateScopePath + siteName;

                // Load fields
                if (!RequestHelper.IsPostBack())
                {
                    pathElem.Value = ptsi.PageTemplateScopePath;
                    classElem.Value = ptsi.PageTemplateScopeClassID;
                    cultureElem.Value = ptsi.PageTemplateScopeCultureID;
                    levelElem.Value = ptsi.PageTemplateScopeLevels;
                }
            }
        }
        else if (!RequestHelper.IsPostBack())
        {
            pathElem.Value = "/";
        }

        // Select for site scope only available cultures
        if (siteID > 0)
        {
            pathElem.SiteID = siteID;
            cultureElem.CurrentSelector.WhereCondition = "CultureID IN (SELECT CultureID FROM CMS_SiteCulture WHERE SiteID = " + siteID + ") OR CultureID = " + cultureId;
            classElem.WhereCondition = "(ClassID IN (SELECT ClassID FROM CMS_ClassSite  WHERE SiteID = " + siteID + ") OR ClassID = " + classId + ")";
        }
    }


    /// <summary>
    /// Button OK click event.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        // Validation
        string path = ValidationHelper.GetString(pathElem.Value, "");
        if (string.IsNullOrEmpty(path))
        {
            ShowError(GetString("template.scopes.emptypath"));
            return;
        }

        // Update database

        // Class
        int classID = ValidationHelper.GetInteger(classElem.Value, 0);
        // No class selected or selected '(all)'
        if (classID <= 0)
        {
            ptsi.SetValue("PageTemplateScopeClassID", null);
        }
        else
        {
            ptsi.PageTemplateScopeClassID = classID;
        }

        // Culture
        int cultureID = ValidationHelper.GetInteger(cultureElem.Value, 0);
        if (cultureID == 0)
        {
            ptsi.SetValue("PageTemplateScopeCultureID", null);
        }
        else
        {
            ptsi.PageTemplateScopeCultureID = cultureID;
        }

        // Levels
        string levels = ValidationHelper.GetString(levelElem.Value, "");
        if (string.IsNullOrEmpty(levels))
        {
            ptsi.SetValue("PageTemplateScopeLevels", null);
        }
        else
        {
            ptsi.PageTemplateScopeLevels = levels;
        }

        // Other columns       
        ptsi.PageTemplateScopePath = ValidationHelper.GetString(pathElem.Value, "");
        ptsi.PageTemplateScopeTemplateID = PageTemplateID;

        if (ptsi.PageTemplateScopeID == 0)
        {
            // Site
            if (siteID != 0)
            {
                ptsi.PageTemplateScopeSiteID = siteID;
            }
        }

        // Insert or update
        PageTemplateScopeInfoProvider.SetPageTemplateScopeInfo(ptsi);

        // Redirect
        string url = RequestContext.CurrentURL;
        url = URLHelper.UpdateParameterInUrl(url, "scopeid", ptsi.PageTemplateScopeID.ToString());
        url = URLHelper.AddParameterToUrl(url, "saved", "1");
        URLHelper.Redirect(ResolveUrl(url));
    }

    #endregion
}