using System;
using System.Linq;
using System.Text;

using CMS.Activities;
using CMS.Activities.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.WebAnalytics;


public partial class CMSModules_Activities_Controls_UI_ActivityDetails_PollVoting : ActivityDetail
{
    #region "Methods"

    public override bool LoadData(ActivityInfo activity)
    {
        if (activity == null ||
            activity.ActivityType != PredefinedActivityType.POLL_VOTING || 
            !ModuleEntryManager.IsModuleLoaded(ModuleName.POLLS))
        {
            return false;
        }

        GeneralizedInfo poll = ModuleCommands.PollsGetPollInfo(activity.ActivityItemID);
        if (poll != null)
        {
            string pollQuestion = ValidationHelper.GetString(poll.GetValue("PollQuestion"), null);
            ucDetails.AddRow("om.activitydetails.pollquestion", pollQuestion);
        }

        if (activity.ActivityValue != null)
        {
            string[] answerIDs = activity.ActivityValue.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder answers = new StringBuilder();
            foreach (string answerID in answerIDs)
            {
                GeneralizedInfo pollAnswer = ModuleCommands.PollsGetPollAnswerInfo(ValidationHelper.GetInteger(answerID, 0));
                if (pollAnswer != null)
                {
                    answers.Append("<div>");
                    answers.Append(HTMLHelper.HTMLEncode(ValidationHelper.GetString(pollAnswer.GetValue("AnswerText"), null)));
                    answers.Append("</div>");
                }
            }
            ucDetails.AddRow("om.activitydetails.pollanswer", answers.ToString(), false);
        }

        return ucDetails.IsDataLoaded;
    }

    #endregion
}