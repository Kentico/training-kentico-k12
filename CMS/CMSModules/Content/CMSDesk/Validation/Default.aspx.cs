using System;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_Validation_Default : CMSValidationPage
{
    #region "Page events"

    protected override void OnPreInit(EventArgs e)
    {
        DocumentManager.RedirectForNonExistingDocument = false;

        PortalContext.UpdateViewMode(PortalContext.ViewMode);
        
        base.OnPreInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        frameHeader.Attributes.Add("src", "header.aspx" + RequestContext.CurrentQueryString);

        if (CultureHelper.IsUICultureRTL())
        {
            ControlsHelper.ReverseFrames(colsFrameset);
        }
    }

    #endregion
}