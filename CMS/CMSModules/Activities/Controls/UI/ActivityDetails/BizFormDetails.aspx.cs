using System;

using CMS.Core;
using CMS.Helpers;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CMS.UIControls;


[Title("om.activitydetals.viewrecorddetail")]
[Security(Resource = ModuleName.ACTIVITIES, Permission = "ReadActivities")]
public partial class CMSModules_Activities_Controls_UI_ActivityDetails_BizFormDetails : CMSModalPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Check permissions
        if (!QueryHelper.ValidateHash("hash"))
        {
            return;
        }

        int bizId = QueryHelper.GetInteger("bizid", 0);
        int recId = QueryHelper.GetInteger("recid", 0);

        if ((bizId > 0) && (recId > 0))
        {
            var bfi = BizFormInfoProvider.GetBizFormInfo(bizId);

            if (bfi == null)
            {
                return;
            }

            bizRecord.ItemID = recId;
            bizRecord.SiteName = SiteInfoProvider.GetSiteName(bfi.FormSiteID);
            bizRecord.FormName = bfi.FormName;
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (bizRecord != null)
        {
            bizRecord.SubmitButton.Visible = false;
        }
    }
}