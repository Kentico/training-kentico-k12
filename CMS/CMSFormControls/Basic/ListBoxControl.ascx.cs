using System.Web.UI.WebControls;

using CMS.FormEngine;
using CMS.FormEngine.Web.UI;


public partial class CMSFormControls_Basic_ListBoxControl : ListFormControl
{
    #region "Variables"

    private bool mSelectionModeSet;
    private const string DEFAULT_CSS_CLASS = "ListBoxField";

    #endregion


    #region "Protected properties"

    protected override ListControl ListControl
    {
        get
        {
            return listbox;
        }
    }


    protected override ListSelectionMode SelectionMode
    {
        get
        {
            if (!mSelectionModeSet)
            {
                listbox.SelectionMode = GetValue("allowmultiplechoices", true) ? ListSelectionMode.Multiple : ListSelectionMode.Single;

                mSelectionModeSet = true;
            }

            return listbox.SelectionMode;
        }
    }


    protected override string FormControlName
    {
        get
        {
            return FormFieldControlTypeCode.LISTBOX;
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
}