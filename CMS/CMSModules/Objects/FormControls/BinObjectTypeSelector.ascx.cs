using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Synchronization;


public partial class CMSModules_Objects_FormControls_BinObjectTypeSelector : FormEngineUserControl
{
    #region "Variables"

    private bool mShowAll = true;
    private string mObjectType = null;

    #endregion


    #region "Properties"

    /// <summary>
    /// User ID to restrict object types to user recycle bin only.
    /// </summary>
    public int UserID
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if all item should be displayed.
    /// </summary>
    public bool ShowAll
    {
        get
        {
            return mShowAll;
        }
        set
        {
            mShowAll = value;
        }
    }


    /// <summary>
    /// Ensure creating child controls.
    /// </summary>
    protected override void CreateChildControls()
    {
        base.CreateChildControls();
        Reload(false);
    }


    /// <summary>
    /// Returns ClientID of the CMSDropDownList with order.
    /// </summary>
    public override string ValueElementID
    {
        get
        {
            return drpObjectTypes.ClientID;
        }
    }


    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }
        set
        {
            base.Enabled = value;
            drpObjectTypes.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return ValidationHelper.GetString(drpObjectTypes.SelectedValue, "");
        }
        set
        {
            mObjectType = ValidationHelper.GetString(value, "");
            Reload(false);
        }
    }


    public string SelectedValue
    {
        get
        {
            return ValidationHelper.GetString(ViewState["SelectedValue"], null);
        }
        set
        {
            ViewState["SelectedValue"] = value;
        }
    }


    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    public override bool IsValid()
    {
        return true;
    }


    /// <summary>
    /// Where condition to filter values.
    /// </summary>
    public string WhereCondition
    {
        get;
        set;
    }


    /// <summary>
    /// Drop-down list control.
    /// </summary>
    public CMSDropDownList DropDownListControl
    {
        get
        {
            return drpObjectTypes;
        }
    }

    #endregion


    #region "Control methods"

    /// <summary>
    /// Loads drop down list with data.
    /// </summary>
    private void ReloadData()
    {
        drpObjectTypes.Items.Clear();

        // Check if show all item should be displayed
        if (ShowAll)
        {
            drpObjectTypes.Items.Add(new ListItem(GetString("general.selectall"), ""));
        }

        // Get recycle bin object types
        DataSet dsObjectTypes = ObjectVersionHistoryInfoProvider.GetRecycleBin(UserID, WhereCondition, "VersionObjectType", -1, "DISTINCT VersionObjectType");
        if (!DataHelper.DataSourceIsEmpty(dsObjectTypes))
        {
            SortedDictionary<string, string> sdObjectTypes = new SortedDictionary<string, string>();
            foreach (DataRow dr in dsObjectTypes.Tables[0].Rows)
            {
                string objType = ValidationHelper.GetString(dr["VersionObjectType"], null);
                if (!String.IsNullOrEmpty(objType))
                {
                    // Sort object types by translated display names
                    sdObjectTypes.Add(GetString("ObjectType." + objType.Replace(".", "_")), objType);
                }
            }

            foreach (string key in sdObjectTypes.Keys)
            {
                drpObjectTypes.Items.Add(new ListItem(key, sdObjectTypes[key]));
            }
        }

        // Preselect value
        if (!String.IsNullOrEmpty(SelectedValue))
        {
            mObjectType = SelectedValue;
        }
        ListItem selectedItem = drpObjectTypes.Items.FindByValue(mObjectType);
        if (selectedItem != null)
        {
            drpObjectTypes.ClearSelection();
            selectedItem.Selected = true;
        }
        else
        {
            drpObjectTypes.SelectedIndex = 0;
            SelectedValue = null;
        }
    }


    /// <summary>
    /// Reload DDL content.
    /// </summary>
    /// <param name="force">Indicates if DDL reload should be forced</param>
    public void Reload(bool force)
    {
        drpObjectTypes.SelectedIndexChanged += new EventHandler(drpObjectTypes_SelectedIndexChanged);
        if ((drpObjectTypes.Items.Count == 0) || force)
        {
            ReloadData();
        }
    }


    /// <summary>
    /// DLL selected index changed event
    /// </summary>
    protected void drpObjectTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedValue = drpObjectTypes.SelectedValue;
    }

    #endregion
}