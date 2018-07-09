using System;

using CMS.Base.Web.UI;

using System.Linq;
using System.Web.UI.WebControls;

using CMS.FormEngine;
using CMS.Helpers;
using CMS.FormEngine.Web.UI;

public partial class CMSFormControls_Basic_MultipleChoiceControl : ListFormControl
{
    #region "Variables"

    private RepeatDirection mRepeatDirection = RepeatDirection.Vertical;
    private RepeatLayout mRepeatLayout = RepeatLayout.Flow;
    private int mRepeatColumns = -1;
    private const string DEFAULT_CSS_CLASS = "CheckBoxListField";

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
            return ListSelectionMode.Multiple;
        }
    }


    protected override string FormControlName
    {
        get
        {
            return FormFieldControlTypeCode.MULTIPLECHOICE;
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
    /// Returns selected value display names separated with comma.
    /// </summary>
    public override string ValueDisplayName
    {
        get
        {
            return String.Join(", ", list.GetSelectedItems().Select(i => i.Text));
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


    #region "Methods"

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