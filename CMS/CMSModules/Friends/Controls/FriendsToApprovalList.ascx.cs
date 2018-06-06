using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.Globalization;
using CMS.Globalization.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_FriendsToApprovalList : CMSAdminListControl
{
    #region "Private variables"

    protected string dialogsUrl = "~/CMSModules/Friends/Dialogs/";
    private UserInfo currentUserInfo = null;
    private SiteInfo currentSiteInfo = null;
    private string mRejectDialogUrl = null;
    private string mApproveDialogUrl = null;
    private bool friendsManagePermission;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets and sets User ID.
    /// </summary>
    public int UserID
    {
        get;
        set;
    }


    /// <summary>
    /// Gets reject dialog url.
    /// </summary>
    protected string RejectDialogUrl
    {
        get
        {
            if (mRejectDialogUrl == null)
            {
                mRejectDialogUrl = ApplicationUrlHelper.ResolveDialogUrl(dialogsUrl + "Friends_Reject.aspx") + "?userid=" + UserID;
            }

            return mRejectDialogUrl;
        }
    }


    /// <summary>
    /// Gets approve dialog url.
    /// </summary>
    protected string ApproveDialogUrl
    {
        get
        {
            if (mApproveDialogUrl == null)
            {
                mApproveDialogUrl = ApplicationUrlHelper.ResolveDialogUrl(dialogsUrl + "Friends_Approve.aspx") + "?userid=" + UserID;
            }

            return mApproveDialogUrl;
        }
    }


    /// <summary>
    /// Zero rows text.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return gridElem.ZeroRowsText;
        }
        set
        {
            gridElem.ZeroRowsText = value;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            Visible = false;
            // Do not load data
            gridElem.StopProcessing = true;
        }
        else
        {
            RaiseOnCheckPermissions(PERMISSION_READ, this);
            Visible = true;

            // Check 'manage' permission
            var currentUser = MembershipContext.AuthenticatedUser;
            friendsManagePermission = currentUser != null && (currentUser.IsAuthorizedPerResource("CMS.Friends", "Manage") || (currentUser.UserID == UserID));

            if (IsLiveSite)
            {
                dialogsUrl = "~/CMSModules/Friends/CMSPages/";
            }

            // Register the dialog script
            ScriptHelper.RegisterDialogScript(Page);

            // Register the refreshing script
            string refreshScript = ScriptHelper.GetScript("function refreshFriendsList(){" + Page.ClientScript.GetPostBackEventReference(hdnRefresh, string.Empty) + "} if (window.top) { window.top.refreshFriendsList = refreshFriendsList}");
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "friendsListRefresh", refreshScript);

            // Setup grid
            gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
            gridElem.OrderBy = "UserName";
            gridElem.IsLiveSite = IsLiveSite;
            // Default where condition
            gridElem.WhereCondition = "(FriendStatus = " + Convert.ToInt32(FriendshipStatusEnum.Waiting) + ") AND (FriendUserID <> " + UserID + ")";

            // Add parameter @UserID
            if (gridElem.QueryParameters == null)
            {
                gridElem.QueryParameters = new QueryDataParameters();
            }
            gridElem.QueryParameters.Add("@UserID", UserID);

            // Register the dialog script
            ScriptHelper.RegisterDialogScript(Page);

            // Register reject script
            StringBuilder rejectScript = new StringBuilder();
            rejectScript.AppendLine("function FM_Reject_" + ClientID + "(item, gridId, url)");
            rejectScript.AppendLine("{");
            rejectScript.AppendLine("   var items = '';");
            rejectScript.AppendLine("   if(item == null)");
            rejectScript.AppendLine("   {");
            rejectScript.AppendLine("       items = document.getElementById(gridId).value;");
            rejectScript.AppendLine("   }");
            rejectScript.AppendLine("   else");
            rejectScript.AppendLine("   {");
            rejectScript.AppendLine("       items = item;");
            rejectScript.AppendLine("   }");
            rejectScript.AppendLine("   if((url != null) && (items != '') && (items != '|'))");
            rejectScript.AppendLine("   {");
            rejectScript.AppendLine("       modalDialog(url + '&ids=' + items, 'rejectDialog', 720, 320);");
            rejectScript.AppendLine("   }");
            rejectScript.AppendLine("   else");
            rejectScript.AppendLine("   {");
            rejectScript.AppendLine("       FM_ShowLabel('" + lblInfo.ClientID + "');");
            rejectScript.AppendLine("   }");
            rejectScript.AppendLine("   return false;");
            rejectScript.AppendLine("}");
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "rejectScript_" + ClientID, ScriptHelper.GetScript(rejectScript.ToString()));

            // Register approve script
            StringBuilder approveScript = new StringBuilder();
            approveScript.AppendLine("function FM_Approve_" + ClientID + "(item, gridId, url)");
            approveScript.AppendLine("{");
            approveScript.AppendLine("   var items = '';");
            approveScript.AppendLine("   if(item == null)");
            approveScript.AppendLine("   {");
            approveScript.AppendLine("       items = document.getElementById(gridId).value;");
            approveScript.AppendLine("   }");
            approveScript.AppendLine("   else");
            approveScript.AppendLine("   {");
            approveScript.AppendLine("       items = item;");
            approveScript.AppendLine("   }");
            approveScript.AppendLine("   if((url != null) && (items != '') && (items != '|'))");
            approveScript.AppendLine("   {");
            approveScript.AppendLine("      modalDialog(url + '&ids=' + items, 'approveDialog', 720, 320);");
            approveScript.AppendLine("   }");
            approveScript.AppendLine("   else");
            approveScript.AppendLine("   {");
            approveScript.AppendLine("       FM_ShowLabel('" + lblInfo.ClientID + "');");
            approveScript.AppendLine("   }");
            approveScript.AppendLine("   return false;");
            approveScript.AppendLine("}");
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "approveScript_" + ClientID, ScriptHelper.GetScript(approveScript.ToString()));

            // Add actions to buttons
            btnApproveSelected.OnClientClick = "return FM_Approve_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + ApproveDialogUrl + "');";
            btnRejectSelected.OnClientClick = "return FM_Reject_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + RejectDialogUrl + "');";
            btnApproveSelected.Enabled = btnRejectSelected.Enabled = friendsManagePermission;
            // Hide label
            lblInfo.Attributes["style"] = "display: none;";

            // Register script for displaying label
            StringBuilder showLabelScript = new StringBuilder();
            showLabelScript.AppendLine("function FM_ShowLabel(labelId)");
            showLabelScript.AppendLine("{");
            showLabelScript.AppendLine("   var label = document.getElementById(labelId);");
            showLabelScript.AppendLine("   if (label != null)");
            showLabelScript.AppendLine("   {");
            showLabelScript.AppendLine("      label.innerHTML = " + ScriptHelper.GetString(GetString("friends.selectfriends")) + ";");
            showLabelScript.AppendLine("      label.style['display'] = 'block';");
            showLabelScript.AppendLine("   }");
            showLabelScript.AppendLine("}");
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "showLabel", ScriptHelper.GetScript(showLabelScript.ToString()));

            pnlLinkButtons.Visible = IsLiveSite;

            if (!IsLiveSite)
            {
                var headerActions = ((CMSPage)Page).CurrentMaster.HeaderActions;
                headerActions.AddAction(new HeaderAction()
                {
                    Text = GetString("friends.friendapproveall"),
                    OnClientClick = "return FM_Approve_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + ApproveDialogUrl + "');",
                    Enabled = friendsManagePermission
                });
                headerActions.AddAction(new HeaderAction()
                {
                    Text = GetString("friends.friendrejectall"),
                    OnClientClick = "return FM_Reject_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + RejectDialogUrl + "');",
                    Enabled = friendsManagePermission
                });
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        bool actionsVisible = plcNoData.Visible = HasData() || gridElem.FilterIsSet;

        // Hide action buttons when there is no data in the grid
        if (!actionsVisible && !IsLiveSite)
        {
            var headerActions = ((CMSPage)Page).CurrentMaster.HeaderActions;
            headerActions.ActionsList.Clear();
            headerActions.ReloadData();
        }

        base.OnPreRender(e);

        // Reset grid selection after multiple action
        if (RequestHelper.IsPostBack())
        {
            string invokerName = Page.Request.Params.Get(Page.postEventSourceID);
            Control invokeControl = !string.IsNullOrEmpty(invokerName) ? Page.FindControl(invokerName) : null;
            if (invokeControl == hdnRefresh)
            {
                gridElem.ResetSelection();
            }
        }
    }

    #endregion


    #region "Grid events"

    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        DataRowView drv = null;
        switch (sourceName)
        {
            case "reject":
                if (sender is CMSGridActionButton)
                {
                    // Get action button
                    CMSGridActionButton rejectBtn = (CMSGridActionButton)sender;
                    // Get full row view
                    drv = UniGridFunctions.GetDataRowView((DataControlFieldCell)rejectBtn.Parent);
                    // Add custom reject action
                    rejectBtn.OnClientClick = "return FM_Reject_" + ClientID + "('" + drv["FriendID"] + "',null,'" + RejectDialogUrl + "');";
                    rejectBtn.Enabled = friendsManagePermission;
                    return rejectBtn;
                }
                else
                {
                    return string.Empty;
                }

            case "approve":
                if (sender is CMSGridActionButton)
                {
                    // Get action button
                    CMSGridActionButton approveBtn = (CMSGridActionButton)sender;
                    // Get full row view
                    drv = UniGridFunctions.GetDataRowView((DataControlFieldCell)approveBtn.Parent);
                    // Add custom reject action
                    approveBtn.OnClientClick = "return FM_Approve_" + ClientID + "('" + drv["FriendID"] + "',null,'" + ApproveDialogUrl + "');";
                    approveBtn.Enabled = friendsManagePermission;
                    return approveBtn;
                }
                else
                {
                    return string.Empty;
                }

            case "friendrequestedwhen":
                if (currentUserInfo == null)
                {
                    currentUserInfo = MembershipContext.AuthenticatedUser;
                }
                if (currentSiteInfo == null)
                {
                    currentSiteInfo = SiteContext.CurrentSite;
                }
                DateTime currentDateTime = ValidationHelper.GetDateTime(parameter, DateTimeHelper.ZERO_TIME);
                if (IsLiveSite)
                {
                    return TimeZoneUIMethods.ConvertDateTime(currentDateTime, this);
                }
                else
                {
                    return TimeZoneHelper.ConvertToUserTimeZone(currentDateTime, true, currentUserInfo, currentSiteInfo);
                }

            case "formattedusername":
                return HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(Convert.ToString(parameter), IsLiveSite));
        }
        return parameter;
    }
    
    #endregion


    #region "Methods"

    /// <summary>
    /// Indicates if there are some data.
    /// </summary>
    public bool HasData()
    {
        return (!DataHelper.DataSourceIsEmpty(gridElem.GridView.DataSource));
    }

    #endregion
}
