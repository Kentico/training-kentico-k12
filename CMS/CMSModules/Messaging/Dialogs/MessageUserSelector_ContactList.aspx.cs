using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Dialogs_MessageUserSelector_ContactList : CMSPage
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Messaging))
        {
            contactListElem.StopProcessing = true;
            contactListElem.Visible = false;
        }
    }
}