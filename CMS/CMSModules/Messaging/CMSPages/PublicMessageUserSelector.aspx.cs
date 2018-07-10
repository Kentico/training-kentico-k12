using System;

using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Messaging_CMSPages_PublicMessageUserSelector : CMSLiveModalPage
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
        Page.Title = PageTitle.TitleText = GetString("Messaging.MessageUserSelector.HeaderCaption");
    }
}