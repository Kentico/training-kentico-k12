using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.UIControls;

public partial class CMSPages_GoogleSiteMap : XMLPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/xml";
    }
}