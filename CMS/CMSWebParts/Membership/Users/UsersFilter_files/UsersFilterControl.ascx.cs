using System;

using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;

public partial class CMSWebParts_Membership_Users_UsersFilter_files_UsersFilterControl : CMSAbstractBaseFilterControl
{
    #region "Public properties"

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


    /// <summary>
    /// Gets or sets the activity link text.
    /// </summary>
    public string SortActivityLinkText
    {
        get
        {
            if (string.IsNullOrEmpty(lnkSortByActivity.Text))
            {
                lnkSortByActivity.Text = ResHelper.GetString("membersfilter.activity");
            }
            return lnkSortByActivity.Text;
        }
        set
        {
            lnkSortByActivity.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the user name link text.
    /// </summary>
    public string SortUserNameLinkText
    {
        get
        {
            if (string.IsNullOrEmpty(lnkSortByUserName.Text))
            {
                lnkSortByUserName.Text = ResHelper.GetString("general.username");
            }
            return lnkSortByUserName.Text;
        }
        set
        {
            lnkSortByUserName.Text = value;
        }
    }

    #endregion


    /// <summary>
    /// OnLoad override - check wheter filter is set.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        // Set filter only if it is not filter request
        if (Request.Form[btnSelect.UniqueID] == null)
        {
            // Try to get where condition
            string wherePart = ValidationHelper.GetString(ViewState["FilterCondition"], string.Empty);
            if (!string.IsNullOrEmpty(wherePart))
            {
                // Set where condition and raise OnFilter change event
                WhereCondition = GenerateWhereCondition(wherePart);
                // Raise event
                RaiseOnFilterChanged();
            }
        }
        if ((Request.Form[lnkSortByActivity.UniqueID] == null) && (Request.Form[lnkSortByUserName.UniqueID] == null))
        {
            string orderByPart = ValidationHelper.GetString(ViewState["OrderClause"], string.Empty);
            if (!string.IsNullOrEmpty(orderByPart))
            {
                // Set order by clause and raise OnFilter change event
                OrderBy = orderByPart;
                // Raise event
                RaiseOnFilterChanged();
            }
        }
        lblSortBy.Text = ResHelper.GetString("general.sortby") + ":";
        lnkSortByActivity.Text = SortActivityLinkText;
        lnkSortByUserName.Text = SortUserNameLinkText;
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
        // Set where condition
        WhereCondition = GenerateWhereCondition(txtValue.Text);
        // Save filter condition
        ViewState["FilterCondition"] = txtValue.Text;
        // Raise OnFilterChange event
        RaiseOnFilterChanged();
    }


    protected void lnkSortByUserName_Click(object sender, EventArgs e)
    {
        // Get order by clause from viewstate
        OrderBy = ValidationHelper.GetString(ViewState["OrderClause"], string.Empty);
        // Set new order by clause
        OrderBy = OrderBy.Contains("UserName ASC") ? "UserName DESC" : "UserName ASC";
        // Save new order by clause to viewstate
        ViewState["OrderClause"] = OrderBy;
        // Raise OnFilterChange event
        RaiseOnFilterChanged();
    }


    protected void lnkSortByActivity_Click(object sender, EventArgs e)
    {
        // Get order by clause from viewstate
        OrderBy = ValidationHelper.GetString(ViewState["OrderClause"], string.Empty);
        // Set new order by clause
        OrderBy = OrderBy.Contains("UserActivityPoints DESC") ? "UserActivityPoints ASC" : "UserActivityPoints DESC";
        // Save new order by clause to viewstate
        ViewState["OrderClause"] = OrderBy;
        // Raise OnFilterChange event
        RaiseOnFilterChanged();
    }


    /// <summary>
    /// Generates where condition.
    /// </summary>
    /// <param name="searchPhrase">Phrase to be searched</param>
    /// <returns>Where condition for given phrase.</returns>
    protected static string GenerateWhereCondition(string searchPhrase)
    {
        searchPhrase = SqlHelper.GetSafeQueryString(searchPhrase, false);
        string whereCondition = "(UserName LIKE N'%" + searchPhrase + "%') OR ";
        whereCondition += "(UserNickName LIKE N'%" + searchPhrase + "%')";
        return whereCondition;
    }
}