using System;

using CMS.UIControls;


public partial class CMSModules_Categories_CMSPages_LiveCategorySelection : CMSLiveModalPage
{
    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Get title text according to selection mode
        PageTitle.TitleText = GetString(SelectionElem.AllowMultipleSelection ? "categories.selectmultiple" : "categories.select");
        // Set actions
        SelectionElem.Actions = actionsElem;
    }


    /// <summary>
    /// Load event handler
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion
}