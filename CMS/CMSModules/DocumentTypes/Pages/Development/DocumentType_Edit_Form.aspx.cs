using System;

using CMS.FormEngine;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_DocumentTypes_Pages_Development_DocumentType_Edit_Form : GlobalAdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentMaster.PanelContent.CssClass = string.Empty;

        layoutElem.FormLayoutType = FormLayoutTypeEnum.Document;
        layoutElem.ObjectID = QueryHelper.GetInteger("objectid", 0);
    }
}