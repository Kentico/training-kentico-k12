using System;

using CMS.DocumentEngine.Web.UI;
using CMS.UIControls;


public partial class CMSModules_FormControls_FormControls_DataTypeFilter : CMSAbstractBaseFilterControl
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return drpTypeSelector.Value;
        }
        set
        {
            drpTypeSelector.Value = value;
        }
    }


    /// <summary>
    /// Gets where condition for filtering.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            return (drpTypeSelector.Value.ToString() != UniSelector.US_ALL_RECORDS.ToString() ? "UserControlType = " + drpTypeSelector.Value : String.Empty);
        }
    }

    #endregion


    #region "Methods"

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        drpTypeSelector.OnFilterChanged += drpTypeSelector_OnFilterChanged;
    }


    protected void drpTypeSelector_OnFilterChanged()
    {
        FilterChanged = true; 
        RaiseOnFilterChanged();
    }

    #endregion
}
