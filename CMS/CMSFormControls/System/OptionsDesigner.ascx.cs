using System;

using CMS.FormEngine.Web.UI;
using CMS.Helpers;


public partial class CMSFormControls_System_OptionsDesigner : FormEngineUserControl
{
    /// <summary>
    /// Gets or sets options selected by default in format "option1|option2".
    /// </summary>
    public override object Value
    {
        get
        {
            return optionDesignerElem.Value;
        }
        set
        {
            optionDesignerElem.Value = ValidationHelper.GetString(value, string.Empty);
        }
    }


    /// <summary>
    /// Indicates if more than one option can be selected.
    /// </summary>
    public bool AllowMultipleChoice
    {
        get
        {
            return optionDesignerElem.AllowMultipleChoice;
        }
        set
        {
            optionDesignerElem.AllowMultipleChoice = value;
        }
    }


    /// <summary>
    /// Gets or sets options definition in format "value;name" separated by new line.
    /// </summary>
    public string OptionsDefinition
    {
        get
        {
            return optionDesignerElem.OptionsDefinition;
        }
        set
        {
            optionDesignerElem.OptionsDefinition = value;
        }
    }


    protected void optionDesignerElem_OptionAdded(object sender, EventArgs e)
    {
        RaiseOnChanged();
    }
}