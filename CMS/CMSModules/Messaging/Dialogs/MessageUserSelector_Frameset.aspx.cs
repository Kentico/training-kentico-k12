using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Dialogs_MessageUserSelector_Frameset : CMSModalPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (QueryHelper.HashEnabled)
        {
            QueryHelper.ValidateHash("hash");
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Messaging);
        // Set localized page title
        Page.Title = GetString("Messaging.MessageUserSelector.HeaderCaption");
    }
}