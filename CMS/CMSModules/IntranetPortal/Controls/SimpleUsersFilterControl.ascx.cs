using System;

using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;

public partial class CMSModules_IntranetPortal_Controls_SimpleUsersFilterControl : CMSAbstractBaseFilterControl
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


    /// <summary>
    /// Generates where condition.
    /// </summary>
    /// <param name="searchPhrase">Phrase to be searched</param>
    /// <returns>Where condition for given phrase.</returns>
    protected static string GenerateWhereCondition(string searchPhrase)
    {
        searchPhrase = SqlHelper.GetSafeQueryString(searchPhrase, false);
        string whereCondition = "(FullName LIKE N'%" + searchPhrase + "%')";
        return whereCondition;
    }
}