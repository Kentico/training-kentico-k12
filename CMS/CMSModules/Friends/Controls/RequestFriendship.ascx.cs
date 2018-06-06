using System;

using CMS.Base.Web.UI;
using CMS.Modules;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_RequestFriendship : CMSUserControl
{
    #region "Private variables"

    private int mUserId = 0;
    private int mRequestedUserId = 0;
    private string mLinkText = string.Empty;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets User ID.
    /// </summary>
    public int UserID
    {
        get
        {
            return mUserId;
        }
        set
        {
            mUserId = value;
        }
    }


    /// <summary>
    /// Gets or sets Requested User ID.
    /// </summary>
    public int RequestedUserID
    {
        get
        {
            return mRequestedUserId;
        }
        set
        {
            mRequestedUserId = value;
        }
    }


    /// <summary>
    /// Gets or sets link text.
    /// </summary>
    public string LinkText
    {
        get
        {
            return mLinkText == string.Empty ? GetString("Friends_List.NewItemCaption") : mLinkText;
        }
        set
        {
            mLinkText = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing)
        {
            string script = "function displayRequest_" + ClientID + "(){ \n" + "modalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/CMSPages/Friends_Request.aspx") + "?userid=" + UserID + "&requestid= " + RequestedUserID + "' ,'requestFriend', 810, 460); \n} \n";

            ScriptHelper.RegisterStartupScript(this, GetType(), "displayModalRequest_" + ClientID, ScriptHelper.GetScript(script));
            ScriptHelper.RegisterDialogScript(Page);

            lnkFriendsRequest.Attributes.Add("onclick", "javascript:displayRequest_" + ClientID + "();");
            lnkFriendsRequest.Style.Add("cursor", "pointer");
            lnkFriendsRequest.Text = LinkText;
        }
    }
}
