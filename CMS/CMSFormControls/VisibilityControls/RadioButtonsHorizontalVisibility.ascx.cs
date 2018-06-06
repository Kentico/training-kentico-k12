using System.Web.UI.WebControls;

using CMS.FormEngine.Web.UI;


public partial class CMSFormControls_VisibilityControls_RadioButtonsHorizontalVisibility : FormEngineVisibilityControl
{
    #region "Public properties"

    /// <summary>
    /// Radio-button control is used for field visibility type selection.
    /// </summary>
    protected override ListControl VisibilityControl
    {
        get
        {
            return rblVisibility;
        }
    }

    #endregion
}