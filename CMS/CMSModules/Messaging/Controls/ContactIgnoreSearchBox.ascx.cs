using System;
using System.Text;

using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;


public partial class CMSModules_Messaging_Controls_ContactIgnoreSearchBox : CMSAbstractBaseFilterControl
{
    #region "Properties"

    /// <summary>
    /// Gets where condition.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            base.WhereCondition = GenerateWhereCondition();
            return base.WhereCondition;
        }
        set
        {
            base.WhereCondition = value;
        }
    }


    /// <summary>
    /// Filter value.
    /// </summary>
    public override object Value
    {
        get
        {
            return ValidationHelper.GetString(txtSearch.Text, string.Empty);
        }
        set
        {
            txtSearch.Text = ValidationHelper.GetString(value, string.Empty);
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// OnLoad override - check wheter filter is set.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Generate current where condition 
        WhereCondition = GenerateWhereCondition();
    }

    #endregion


    #region "Other methods"

    /// <summary>
    /// Generates where condition.
    /// </summary>
    protected string GenerateWhereCondition()
    {
        StringBuilder whereBuilder = new StringBuilder();
        if (!string.IsNullOrEmpty(txtSearch.Text))
        {
            whereBuilder.Append("UserName LIKE '%" + SqlHelper.GetSafeQueryString(txtSearch.Text, false) + "%' OR ");
            whereBuilder.Append("UserNickName LIKE '%" + SqlHelper.GetSafeQueryString(txtSearch.Text, false) + "%'");
        }
        return whereBuilder.ToString();
    }

    #endregion
}