using System;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_Widgets_Controls_WidgetFlatSelector : CMSAdminControl
{
    #region "Variables"

    private bool mSelectGroupWidgets = false;
    private bool mSelectEditorWidgets = false;
    private bool mSelectUserWidgets = false;
    private bool mSelectDashboardWidgets = false;
    private bool mSelectInlineWidgets = false;
    private int mGroupID = 0;
    private string mTreeSelectedItem = null;
    private WidgetCategoryInfo mSelectedCategory = null;

    #endregion


    #region "Widget flat selector properties"

    /// <summary>
    /// Determines whether the flat selector will only display widgets available for group administrators.
    /// </summary>
    public bool SelectGroupWidgets
    {
        get
        {
            return mSelectGroupWidgets;
        }
        set
        {
            mSelectGroupWidgets = value;
        }
    }


    /// <summary>
    /// Determines whether the flat selector will display widgets available for editors.
    /// </summary>
    public bool SelectEditorWidgets
    {
        get
        {
            return mSelectEditorWidgets;
        }
        set
        {
            mSelectEditorWidgets = value;
        }
    }


    /// <summary>
    /// Determines whether the flat selector will only display widgets available for authenticated users.
    /// </summary>
    public bool SelectUserWidgets
    {
        get
        {
            return mSelectUserWidgets;
        }
        set
        {
            mSelectUserWidgets = value;
        }
    }


    /// <summary>
    /// Indicates whether use this selector for inline widgets.
    /// </summary>
    public bool SelectInlineWidgets
    {
        get
        {
            return mSelectInlineWidgets;
        }
        set
        {
            mSelectInlineWidgets = value;
        }
    }


    /// <summary>
    /// Determines whether the flat selector will only display widgets available for dashboard.
    /// </summary>
    public bool SelectDashboardWidgets
    {
        get
        {
            return mSelectDashboardWidgets;
        }
        set
        {
            mSelectDashboardWidgets = value;
        }
    }


    /// <summary>
    /// Gets or sets the group ID. Should be set with enabled SelectOnlyGroupWidget property.
    /// </summary>
    public int GroupID
    {
        get
        {
            return mGroupID;
        }
        set
        {
            mGroupID = value;
        }
    }

    #endregion


    #region "Flat selector properties"

    /// <summary>
    /// Returns inner instance of UniFlatSelector control.
    /// </summary>
    public UniFlatSelector UniFlatSelector
    {
        get
        {
            return flatElem;
        }
    }


    /// <summary>
    /// Gets or sets selected item in flat selector.
    /// </summary>
    public string SelectedItem
    {
        get
        {
            return flatElem.SelectedItem;
        }
        set
        {
            flatElem.SelectedItem = value;
        }
    }


    /// <summary>
    /// Gets or sets the current widget category.
    /// </summary>
    public WidgetCategoryInfo SelectedCategory
    {
        get
        {
            // If not loaded yet
            if (mSelectedCategory == null)
            {
                int categoryId = ValidationHelper.GetInteger(TreeSelectedItem, 0);
                if (categoryId > 0)
                {
                    mSelectedCategory = WidgetCategoryInfoProvider.GetWidgetCategoryInfo(categoryId);
                }
            }

            return mSelectedCategory;
        }
        set
        {
            mSelectedCategory = value;
            // Update ID
            if (mSelectedCategory != null)
            {
                mTreeSelectedItem = mSelectedCategory.WidgetCategoryID.ToString();
            }
        }
    }


    /// <summary>
    /// Gets or sets the selected item in tree, usually the category id.
    /// </summary>
    public string TreeSelectedItem
    {
        get
        {
            return mTreeSelectedItem;
        }
        set
        {
            // Clear loaded category if change
            if (value != mTreeSelectedItem)
            {
                mSelectedCategory = null;
            }
            mTreeSelectedItem = value;
        }
    }


    /// <summary>
    /// Indicates if the control should perform the operations.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            flatElem.StopProcessing = value;
            EnableViewState = !value;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            return;
        }

        // Setup flat selector
        flatElem.QueryName = "cms.widget.selectall";
        flatElem.ValueColumn = "WidgetID";
        flatElem.SearchLabelResourceString = "widget.widgetname";
        flatElem.SearchColumn = "WidgetDisplayName";
        flatElem.SelectedColumns = "WidgetName, WidgetThumbnailGUID, WidgetIconClass, WidgetDisplayName, WidgetID, WidgetSkipInsertProperties";
        flatElem.SkipPropertiesDialogColumn = "WidgetSkipInsertProperties";
        flatElem.PageSize = 15;
        flatElem.OrderBy = "WidgetDisplayName";
        flatElem.NoRecordsMessage = "widgets.norecordsincategory";
        flatElem.NoRecordsSearchMessage = "widgets.norecords";

        flatElem.OnItemSelected += new UniFlatSelector.ItemSelectedEventHandler(flatElem_OnItemSelected);

        flatElem.SearchCheckBox.Visible = true;
        flatElem.SearchCheckBox.Text = GetString("webparts.searchindescription");

        if (!RequestHelper.IsPostBack())
        {
            // Search in description default value
            flatElem.SearchCheckBox.Checked = false;
        }

        if (flatElem.SearchCheckBox.Checked)
        {
            flatElem.SearchColumn += ";WidgetDescription";
        }
    }


    /// <summary>
    /// Creates authorization and authentication where condition.
    /// </summary>
    private string CreateAuthWhereCondition()
    {
        var currentUser = MembershipContext.AuthenticatedUser;

        // Allowed for all  
        string securityWhere = " AND ((WidgetSecurity = 0) ";

        if (AuthenticationHelper.IsAuthenticated())
        {
            // Authenticated
            securityWhere += " OR (WidgetSecurity = 1)";

            // Global admin
            if (currentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
            {
                securityWhere += " OR (WidgetSecurity = 7)";
            }

            // Authorized roles
            securityWhere += " OR ((WidgetSecurity = 2)";
            if (!currentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
            {
                securityWhere += "AND (WidgetID IN ( SELECT WidgetID FROM CMS_WidgetRole WHERE RoleID IN (SELECT RoleID FROM View_CMS_UserRole_MembershipRole_ValidOnly_Joined WHERE UserID = " + currentUser.UserID + ")))))";
            }
            else
            {
                securityWhere += "))";
            }
        }
        else
        {
            securityWhere += ")";
        }
        return securityWhere;
    }


    /// <summary>
    /// On PreRender.
    /// </summary>  
    protected override void OnPreRender(EventArgs e)
    {
        if (StopProcessing)
        {
            return;
        }

        // Security
        var currentUser = MembershipContext.AuthenticatedUser;
        if (SelectGroupWidgets)
        {
            // Shows group widgets without other security checks
            string where = "WidgetForGroup = 1";

            // But user must be group admin, otherwise show nothing
            if (!currentUser.IsGroupAdministrator(GroupID) && ((PortalContext.ViewMode != ViewModeEnum.Design) || ((PortalContext.ViewMode == ViewModeEnum.Design) && (!currentUser.IsAuthorizedPerResource("CMS.Design", "Design")))))
            {
                flatElem.ErrorText = GetString("widget.notgroupadmin");
            }

            flatElem.WhereCondition = SqlHelper.AddWhereCondition(flatElem.WhereCondition, where);
        }
        else
        {
            // Create security where condition
            string securityWhere = String.Empty;
            if (SelectEditorWidgets)
            {
                securityWhere += "WidgetForEditor = 1 ";
            }
            else if (SelectUserWidgets)
            {
                securityWhere += "WidgetForUser = 1 ";
            }
            else if (SelectDashboardWidgets)
            {
                securityWhere += "WidgetForDashboard = 1 ";
            }
            else if (SelectInlineWidgets)
            {
                securityWhere += " WidgetForInline = 1 ";
            }
            else
            {
                securityWhere += " 1 = 2 ";
            }


            securityWhere += CreateAuthWhereCondition();
            flatElem.WhereCondition = SqlHelper.AddWhereCondition(flatElem.WhereCondition, securityWhere);
        }

        // Restrict to items in selected category (if not root)
        if ((SelectedCategory != null) && (SelectedCategory.WidgetCategoryParentID > 0))
        {
            flatElem.WhereCondition = SqlHelper.AddWhereCondition(flatElem.WhereCondition, "WidgetCategoryID = " + SelectedCategory.WidgetCategoryID + " OR WidgetCategoryID IN (SELECT WidgetCategoryID FROM CMS_WidgetCategory WHERE WidgetCategoryPath LIKE '" + SelectedCategory.WidgetCategoryPath + "/%')");
        }

        // Recently used items
        if (TreeSelectedItem.ToLowerCSafe() == "recentlyused")
        {
            flatElem.WhereCondition = SqlHelper.AddWhereCondition(flatElem.WhereCondition, new WhereCondition().WhereIn("WidgetName", currentUser.UserSettings.UserUsedWidgets.Split(';')).ToString(true));
        }

        // Description area and recently used
        litCategory.Text = ShowInDescriptionArea(SelectedItem);

        base.OnPreRender(e);
    }

    #endregion


    #region "Event handling"

    /// <summary>
    /// Updates description after item is selected in flat selector.
    /// </summary>
    protected string flatElem_OnItemSelected(string selectedValue)
    {
        return ShowInDescriptionArea(selectedValue);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        flatElem.ReloadData();
        flatElem.ResetToDefault();
        pnlUpdate.Update();
    }


    /// <summary>
    /// Generates HTML text to be used in description area.
    /// </summary>
    ///<param name="selectedValue">Selected item for which generate description</param>
    private string ShowInDescriptionArea(string selectedValue)
    {
        string description = String.Empty;

        if (!String.IsNullOrEmpty(selectedValue))
        {
            int widgetId = ValidationHelper.GetInteger(selectedValue, 0);
            WidgetInfo wi = WidgetInfoProvider.GetWidgetInfo(widgetId);
            if (wi != null)
            {
                description = wi.WidgetDescription;
            }
        }

        if (!String.IsNullOrEmpty(description))
        {
            return "<div class=\"Description\">" + HTMLHelper.HTMLEncode(ResHelper.LocalizeString(description)) + "</div>";
        }

        return String.Empty;
    }

    #endregion
}