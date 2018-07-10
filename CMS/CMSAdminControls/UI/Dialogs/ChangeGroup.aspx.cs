using System;

using CMS.UIControls;


public partial class CMSAdminControls_UI_Dialogs_ChangeGroup : CMSModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Save += (s, ea) => selectDocumentGroupElem.ProcessAction();
        PageTitle.TitleText = GetString("community.group.changedocumentowner");
        Page.Title = PageTitle.TitleText;
    }
}