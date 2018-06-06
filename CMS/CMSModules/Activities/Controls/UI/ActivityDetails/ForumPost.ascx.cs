using System;

using CMS.Activities;
using CMS.Activities.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;


public partial class CMSModules_Activities_Controls_UI_ActivityDetails_ForumPost : ActivityDetail
{
    #region "Methods"

    public override bool LoadData(ActivityInfo ai)
    {
        if ((ai == null) || !ModuleManager.IsModuleLoaded(ModuleName.FORUMS))
        {
            return false;
        }

        switch (ai.ActivityType)
        {
            case PredefinedActivityType.FORUM_POST:
            case PredefinedActivityType.SUBSCRIPTION_FORUM_POST:
                break;
            default:
                return false;
        }

        int nodeId = ai.ActivityNodeID;
        lblDocIDVal.Text = GetLinkForDocument(nodeId, ai.ActivityCulture);

        if (ai.ActivityType == PredefinedActivityType.FORUM_POST)
        {
            GeneralizedInfo iinfo = ModuleCommands.ForumsGetForumPostInfo(ai.ActivityItemDetailID);
            if (iinfo != null)
            {
                plcComment.Visible = true;
                lblPostSubjectVal.Text = HTMLHelper.HTMLEncode(ValidationHelper.GetString(iinfo.GetValue("PostSubject"), null));
                txtPost.Text = ValidationHelper.GetString(iinfo.GetValue("PostText"), null);
            }
        }

        return true;
    }

    #endregion
}