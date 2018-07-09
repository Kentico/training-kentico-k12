using System;

using CMS.Activities;
using CMS.Activities.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;


public partial class CMSModules_Activities_Controls_UI_ActivityDetails_EventBooking : ActivityDetail
{
    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    public override bool LoadData(ActivityInfo ai)
    {
        if ((ai == null) || !ModuleManager.IsModuleLoaded(ModuleName.EVENTMANAGER) || (ai.ActivityType != PredefinedActivityType.EVENT_BOOKING))
        {
            return false;
        }

        int nodeId = ai.ActivityNodeID;
        ucDetails.AddRow("om.activitydetails.documenturl", GetLinkForDocument(nodeId, ai.ActivityCulture), false);

        GeneralizedInfo iinfo = ProviderHelper.GetInfoById(PredefinedObjectType.EVENTATTENDEE, ai.ActivityItemID);
        if (iinfo != null)
        {
            ucDetails.AddRow("om.activitydetails.attendee", String.Format("{0} {1} ({2})",
                                                                          ValidationHelper.GetString(iinfo.GetValue("AttendeeFirstName"), null),
                                                                          ValidationHelper.GetString(iinfo.GetValue("AttendeeLastName"), null),
                                                                          ValidationHelper.GetString(iinfo.GetValue("AttendeeEmail"), null)));
        }

        return ucDetails.IsDataLoaded;
    }

    #endregion
}