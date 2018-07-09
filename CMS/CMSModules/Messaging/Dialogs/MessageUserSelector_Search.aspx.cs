using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Dialogs_MessageUserSelector_Search : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Messaging);
    }
}