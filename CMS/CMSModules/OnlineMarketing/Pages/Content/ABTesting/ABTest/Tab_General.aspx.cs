using System;

using CMS.OnlineMarketing.Web.UI;
using CMS.UIControls;


[EditedObject("om.abtest", "objectID")]
[Security(Resource = "CMS.ABTest", UIElements = "Settings")]
[Security(Resource = "CMS.ABTest", UIElements = "Detail")]
public partial class CMSModules_OnlineMarketing_Pages_Content_ABTesting_ABTest_Tab_General : CMSABTestPage
{
    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        MessagesPlaceHolder = plcMess;
        
        base.OnInit(e);
    }

    #endregion
}