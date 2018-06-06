using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_CMSPages_MessageUserSelector_ContactList : LivePage
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

    protected void Page_Load(object sender, EventArgs e)
    {
        // Add styles
        RegisterDialogCSSLink();
    }
}