using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.Base;
using CMS.UIControls;
using CMS.DataEngine;

public partial class CMSMessages_KickedUser : MessagePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        titleElem.TitleText = GetString("kicked.header");
        Page.Title = GetString("kicked.header");
        lblInfo.Text = String.Format(GetString("kicked.info"), SettingsKeyInfoProvider.GetIntValue("CMSDenyLoginInterval"));

        // Back link
        lnkBack.Text = GetString("general.Back");
        lnkBack.NavigateUrl = "~/";
    }
}