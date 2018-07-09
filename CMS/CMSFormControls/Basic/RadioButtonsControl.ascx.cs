using System;
using System.Web.UI.WebControls;

using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;


public partial class CMSFormControls_Basic_RadioButtonsControl : ListFormControl
{
    #region "Variables"

    private RepeatDirection mRepeatDirection = RepeatDirection.Vertical;
    private RepeatLayout mRepeatLayout = RepeatLayout.Flow;
    private int mRepeatColumns = -1;
    private const string DEFAULT_CSS_CLASS = "RadioButtonList";

    #endregion


    #region "Protected properties"

    protected override ListControl ListControl
    {
        get
        {
            return list;
        }
    }


    protected override ListSelectionMode SelectionMode
    {
        get
        {
            return ListSelectionMode.Single;
        }
    }


    protected override string FormControlName
    {
        get
        {
            return FormFieldControlTypeCode.RADIOBUTTONS;
        }
    }


    protected override string DefaultCssClass
    {
        get
        {
            return DEFAULT_CSS_CLASS;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Returns display name of the value.
    /// </summary>
    public override string ValueDisplayName
    {
        get
        {
            return (list.SelectedItem == null ? list.Text : list.SelectedItem.Text);
        }
    }


    /// <summary>
    /// Specifies the direction in which items of a list control are displayed.
    /// </summary>
    public RepeatDirection RepeatDirection
    {
        get
        {
            string direction = ValidationHelper.GetString(GetValue("repeatdirection"), String.Empty);
            if (!Enum.TryParse(direction, true, out mRepeatDirection))
            {
                mRepeatDirection = RepeatDirection.Vertical;
            }

            return mRepeatDirection;
        }
        set
        {
            mRepeatDirection = value;
        }
    }


    /// <summary>
    /// Specifies the layout of items in a list control.
    /// </summary>
    public RepeatLayout RepeatLayout
    {
        get
        {
            string layout = ValidationHelper.GetString(GetValue("RepeatLayout"), String.Empty);
            if (!Enum.TryParse(layout, true, out mRepeatLayout))
            {
                mRepeatLayout = RepeatLayout.Flow;
            }

            return mRepeatLayout;
        }
        set
        {
            mRepeatLayout = value;
        }
    }


    /// <summary>
    /// Specifies the number of columns to display in the list control. The default is 0, which indicates that this property is not set.
    /// </summary>
    public int RepeatColumns
    {
        get
        {
            if (mRepeatColumns < 0)
            {
                mRepeatColumns = ValidationHelper.GetInteger(GetValue("RepeatColumns"), 0);
            }
            return mRepeatColumns;
        }
        set
        {
            mRepeatColumns = value;
        }
    }

    #endregion


    #region "Control events"

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Set control direction, layout and columns
        list.RepeatDirection = RepeatDirection;
        list.RepeatLayout = RepeatLayout;
        list.RepeatColumns = RepeatColumns;
    }

    #endregion
}