using System;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Avatars_AvatarFilter : CMSAbstractBaseFilterControl
{
    #region "Public properties"

    public override string WhereCondition
    {
        get
        {
            return BuildWhereCondition();
        }
        set
        {
            base.WhereCondition = value;
        }
    }


    #endregion


    #region "Events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!RequestHelper.IsPostBack())
        {
            InitializeForm();
        }

        btnReset.Text = GetString("general.reset");
        btnReset.Click += btnReset_Click;
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        UniGrid grid = FilteredControl as UniGrid;
        if (grid != null)
        {
            grid.ApplyFilter(sender, e);
        }
    }



    #endregion


    #region "Private methods"

    /// <summary>
    /// Resets filter to the default state.
    /// </summary>
    public override void ResetFilter()
    {
        txtAvatarName.Text = String.Empty;
        drpAvatarType.SelectedIndex = 0;
        drpAvatarKind.SelectedIndex = 1;
        drpAvatarName.SelectedIndex = 0;
    }



    private void InitializeForm()
    {
        // Initialize first dropdown lists
        ControlsHelper.FillListWithNumberedSqlOperators(drpAvatarName);

        ControlsHelper.FillListControlWithEnum<AvatarTypeEnum>(drpAvatarType, "avat.type", useStringRepresentation: true);
        drpAvatarType.Items.Insert(0, new ListItem(GetString("general.selectall"), ""));

        drpAvatarKind.Items.Add(new ListItem(GetString("general.selectall"), "0"));
        drpAvatarKind.Items.Add(new ListItem(GetString("avat.filter.shared"), "1"));
        drpAvatarKind.Items.Add(new ListItem(GetString("avat.filter.custom"), "2"));
        // Preselect shared
        drpAvatarKind.SelectedIndex = 1;
    }


    /// <summary>
    /// Builds where condition and raises search event.
    /// </summary>
    private string BuildWhereCondition()
    {
        string whereCondition = String.Empty;

        // Avatar name
        string avatarName = txtAvatarName.Text.Trim().Replace("'", "''");
        if (!String.IsNullOrEmpty(avatarName))
        {
            // Get proper operator name
            int sqlOperatorNumber = ValidationHelper.GetInteger(drpAvatarName.SelectedValue, 0);
            string sqlOperatorName;
            switch (sqlOperatorNumber)
            {
                case 1:
                    sqlOperatorName = WhereBuilder.NOT_LIKE;
                    break;
                case 2:
                    sqlOperatorName = WhereBuilder.EQUAL;
                    break;
                case 3:
                    sqlOperatorName = WhereBuilder.NOT_EQUAL;
                    break;
                default:
                    sqlOperatorName = WhereBuilder.LIKE;
                    break;
            }

            if ((sqlOperatorName == WhereBuilder.LIKE) || (sqlOperatorName == WhereBuilder.NOT_LIKE))
            {
                avatarName = "%" + avatarName + "%";
            }

            whereCondition = "(AvatarName " + sqlOperatorName + " N'" + avatarName + "')";
        }

        // Avatar type
        if (!String.IsNullOrEmpty(drpAvatarType.SelectedValue))
        {
            if (!string.IsNullOrEmpty(whereCondition))
            {
                whereCondition += " AND ";
            }
            whereCondition += "(AvatarType = '" + SqlHelper.GetSafeQueryString(drpAvatarType.SelectedValue, false) + "')";
        }

        // Avatar kind
        int sqlKindNumber = ValidationHelper.GetInteger(drpAvatarKind.SelectedValue, 0);
        string sqlKindCode = "";
        switch (sqlKindNumber)
        {
            case 1:
                sqlKindCode = "AvatarIsCustom = 0";
                break;
            case 2:
                sqlKindCode = "AvatarIsCustom = 1";
                break;
            default:
                sqlKindCode = "";
                break;
        }

        if (!String.IsNullOrEmpty(sqlKindCode))
        {
            if (!String.IsNullOrEmpty(whereCondition))
            {
                whereCondition += " AND ";
            }
            whereCondition += "(" + sqlKindCode + ")";
        }

        return whereCondition;
    }

    #endregion


}