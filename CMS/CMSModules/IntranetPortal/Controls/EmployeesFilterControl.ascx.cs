using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;


public partial class CMSModules_IntranetPortal_Controls_EmployeesFilterControl : CMSAbstractBaseFilterControl
{
    #region "Constants"

    private const string DEPARTMENT_DOC_TYPE = "IntranetPortal.Department";

    #endregion


    #region "Variables"

    private TreeProvider mProvider = null;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets the button text.
    /// </summary>
    public string ButtonText
    {
        get
        {
            if (string.IsNullOrEmpty(btnSelect.Text))
            {
                btnSelect.Text = ResHelper.GetString("general.search");
            }
            return btnSelect.Text;
        }
        set
        {
            btnSelect.Text = value;
        }
    }


    private TreeProvider TreeProvider
    {
        get
        {
            if (mProvider == null)
            {
                mProvider = new TreeProvider(MembershipContext.AuthenticatedUser);
            }
            return mProvider;
        }
    }

    #endregion


    #region "Control methods"

    /// <summary>
    /// OnLoad override - check wheter filter is set.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        if (!RequestHelper.IsPostBack())
        {
            InitializeDepartmentFilter();
        }
        else
        {
            // Set filter only if it is not filter request
            if (Request.Form[btnSelect.UniqueID] == null)
            {
                // Try to get where condition
                txtValue.Text = ValidationHelper.GetString(ViewState["FilterCondition"], string.Empty);
            }

            if (!string.IsNullOrEmpty(txtValue.Text) || (drpDepartment.SelectedIndex != 0))
            {
                // Set where condition and raise OnFilter change event
                WhereCondition = GenerateWhereCondition(txtValue.Text);
                // Raise event
                RaiseOnFilterChanged();
            }
        }

        btnSelect.Text = ButtonText;
        base.OnLoad(e);
    }


    /// <summary>
    /// Select button handler.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        // Save filter condition
        ViewState["FilterCondition"] = txtValue.Text;

        // Raise event
        RaiseOnFilterChanged();
    }


    /// <summary>
    /// DDL change slected value handler.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Save filter condition
        ViewState["FilterCondition"] = txtValue.Text;

        // Raise event
        RaiseOnFilterChanged();
    }


    /// <summary>
    /// Generates where condition.
    /// </summary>
    /// <param name="searchPhrase">Phrase to be searched</param>
    /// <returns>Where condition for given phrase</returns>
    protected string GenerateWhereCondition(string searchPhrase)
    {
        searchPhrase = SqlHelper.GetSafeQueryString(searchPhrase, false);
        string whereCondition = null;

        // Process name filter where condition
        if (!String.IsNullOrEmpty(searchPhrase))
        {
            whereCondition = "(FullName LIKE N'%" + searchPhrase + "%')";
        }

        // Process department filter where condition
        if (drpDepartment.SelectedValue != String.Empty)
        {
            int nodeId = ValidationHelper.GetInteger(drpDepartment.SelectedValue, 0);
            int permValue = 1 << (int)NodePermissionsEnum.Read;
            string whereDepartment = "UserID IN(SELECT DISTINCT UserID FROM CMS_UserRole WHERE RoleID IN(SELECT RoleID FROM View_CMS_ACLItem_ItemsAndOperators WHERE ((Allowed & " + permValue + ") >= " + permValue + ") AND (RoleID IS NOT NULL) AND (ACLOwnerNodeID = " + nodeId + ")))";

            whereCondition = SqlHelper.AddWhereCondition(whereCondition, whereDepartment);
        }

        return whereCondition;
    }


    /// <summary>
    /// Initialize department filter.
    /// </summary>
    private void InitializeDepartmentFilter()
    {
        DataSet dsDepartments = TreeProvider.SelectNodes(SiteContext.CurrentSiteName, "/%", null, true, DEPARTMENT_DOC_TYPE, null, null, -1, true, 0, DocumentColumnLists.SELECTNODES_REQUIRED_COLUMNS + ",DocumentName");
        if (!DataHelper.DataSourceIsEmpty(dsDepartments))
        {
            // Add default value
            drpDepartment.Items.Add(new ListItem(ResHelper.GetString("employeesearch.alldepartments"), ""));

            foreach (DataRow dr in dsDepartments.Tables[0].Rows)
            {
                string depName = ValidationHelper.GetString(dr["DocumentName"], "");
                string depId = ValidationHelper.GetString(dr["NodeID"], "");
                if (!String.IsNullOrEmpty(depName) && !String.IsNullOrEmpty(depId))
                {
                    drpDepartment.Items.Add(new ListItem(depName, depId));
                }
            }
        }
    }

    #endregion
}