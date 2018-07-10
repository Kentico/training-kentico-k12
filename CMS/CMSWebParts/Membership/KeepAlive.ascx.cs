using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Membership_KeepAlive : CMSAbstractWebPart, ICallbackEventHandler
{
    private int mRefreshingInterval = 5;
    private string result = "";


    /// <summary>
    /// Interval between calls made to keep session alive.
    /// </summary>
    public int RefreshingInterval
    {
        get
        {
            mRefreshingInterval = ValidationHelper.GetInteger(GetValue("RefreshingInterval"), 5);
            return mRefreshingInterval;
        }
        set
        {
            mRefreshingInterval = value;
            SetValue("RefreshingInterval", value);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing && AuthenticationHelper.IsAuthenticated())
        {
            // Get call back reference
            string callBackRef = Page.ClientScript.GetCallbackEventReference(this, null, "ProcessResult", null);

            // Get interval in miliseconds
            int refreshInt = RefreshingInterval * 1000;

            // Register executive JavaScript code
            ltlScript.Text = ScriptHelper.GetScript("function KeepSession(){" + callBackRef + " } function ProcessResult(result, context){ Timer('" + refreshInt.ToString() + "') } " +
                                                    " function Timer(refreshingInterval){ setTimeout(\"KeepSession()\", refreshingInterval); } Timer('" + refreshInt.ToString() + "');");
        }
    }


    #region "ICallbackEventHandler Members"

    /// <summary>
    /// Gets modified result.
    /// </summary>
    public string GetCallbackResult()
    {
        return result;
    }


    /// <summary>
    /// Gets callback event result.
    /// </summary>
    public void RaiseCallbackEvent(string eventArgument)
    {
        result = (!Page.Session.IsNewSession) ? "KEEPING_ALIVE" : "NEW_CREATED";
    }

    #endregion
}
