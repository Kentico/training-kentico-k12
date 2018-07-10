using System;

using CMS.UIControls;


public partial class CMSModules_Categories_Dialogs_CategorySelection : CMSModalPage
{
    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Get title text according to selection mode
        PageTitle.TitleText = GetString(SelectionElem.AllowMultipleSelection ? "categories.selectmultiple" : "categories.select");
        // Set actions
        SelectionElem.Actions = actionsElem;

        SetSaveJavascript("return US_Submit();");
    }
    
    #endregion
}