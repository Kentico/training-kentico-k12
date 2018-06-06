using System;

using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_FriendsSearchBox : CMSAbstractBaseFilterControl
{
    #region "Variables"

    private bool mFilterComment = true;
    private bool mFilterNickname = true;
    private bool mFilterUserName = true;
    private bool mFilterFullName = true;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets where condition.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            string where = null;
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                if (FilterUserName)
                {
                    where = SqlHelper.AddWhereCondition(where, "UserName LIKE '%" + SqlHelper.GetSafeQueryString(txtSearch.Text, false) + "%'", "OR");
                }
                if (FilterNickname)
                {
                    where = SqlHelper.AddWhereCondition(where, "UserNickname LIKE '%" + SqlHelper.GetSafeQueryString(txtSearch.Text, false) + "%'", "OR");
                }
                if (FilterFullName)
                {
                    where = SqlHelper.AddWhereCondition(where, "FullName LIKE '%" + SqlHelper.GetSafeQueryString(txtSearch.Text, false) + "%'", "OR");
                }
                if (FilterComment)
                {
                    where = SqlHelper.AddWhereCondition(where, "FriendComment LIKE '%" + SqlHelper.GetSafeQueryString(txtSearch.Text, false) + "%'", "OR");
                }
            }
            return where;
        }
    }


    /// <summary>
    /// Determines whether filter is set.
    /// </summary>
    public override bool FilterIsSet
    {
        get
        {
            return !string.IsNullOrEmpty(txtSearch.Text);
        }
    }


    /// <summary>
    /// Determines whether to filter Comment column.
    /// </summary>
    public bool FilterComment
    {
        get
        {
            return mFilterComment;
        }
        set
        {
            mFilterComment = value;
        }
    }


    /// <summary>
    /// Determines whether to filter Nickname column.
    /// </summary>
    public bool FilterNickname
    {
        get
        {
            return mFilterNickname;
        }
        set
        {
            mFilterNickname = value;
        }
    }


    /// <summary>
    /// Determines whether to filter Username column.
    /// </summary>
    public bool FilterUserName
    {
        get
        {
            return mFilterUserName;
        }
        set
        {
            mFilterUserName = value;
        }
    }


    /// <summary>
    /// Determines whether to filter Fullname column.
    /// </summary>
    public bool FilterFullName
    {
        get
        {
            return mFilterFullName;
        }
        set
        {
            mFilterFullName = value;
        }
    }

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Initialize reset link button
        UniGrid grid = FilteredControl as UniGrid;
        if (grid == null || !grid.RememberState)
        {
            btnReset.Visible = false;
        }
    }
    
    #endregion


    #region "State management"

    /// <summary>
    /// Resets filter to the default state.
    /// </summary>
    public override void ResetFilter()
    {
        txtSearch.Text = String.Empty;
    }


    /// <summary>
    /// Resets the associated UniGrid control.
    /// </summary>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.Reset();
        }
    }


    /// <summary>
    /// Applies filter on associated UniGrid control.
    /// </summary>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.ApplyFilter(sender, e);
        }
    }

    #endregion
}