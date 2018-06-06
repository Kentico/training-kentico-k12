using System;

using CMS.Base.Web.UI;
using CMS.Helpers;


public partial class CMSInlineControls_WebPartZone : InlineUserControl
{
    #region "Properties"

    /// <summary>
    /// Control parameter.
    /// </summary>
    public override string Parameter
    {
        get
        {
            return zoneElem.ID;
        }
        set
        {
            zoneElem.ID = value;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Method that is called when the control content is loaded.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();

        SetupControl();
    }


    /// <summary>
    /// Load event handler.
    /// </summary>
    protected void SetupControl()
    {
        // Init the zone ID
        string zoneId = ValidationHelper.GetString(GetValue("ID"), "");
        if (!String.IsNullOrEmpty(zoneId))
        {
            zoneElem.ID = zoneId;
        }
    }

    #endregion
}