using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Community;
using CMS.DataEngine;
using CMS.Globalization;
using CMS.Globalization.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_FriendsRejectedList : CMSAdminListControl
{
    #region "Private variables"

    protected string dialogsUrl = "~/CMSModules/Friends/Dialogs/";
    private bool mUseEncapsulation = true;
    private UserInfo currentUserInfo = null;
    private SiteInfo currentSiteInfo = null;
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
    /// Determines whether use showing/hiding list of rejected friends
    /// </summary>
    public bool UseEncapsulation
    {
        get
        {
            return mUseEncapsulation;
        }
        set
        {
            mUseEncapsulation = value;
        }
    }


    /// <summary>
    /// Indicates if the show/hide link should be displayed.
    /// </summary>
    public bool ShowLink
    {
        get
        {
            return plcLink.Visible;
        }
        set
        {
            plcLink.Visible = value;
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
            gridElem.StopProcessing = true;
            // Do not load data
        }
        else
        {
            // Check permissions
            RaiseOnCheckPermissions("Read", this);

            // Check 'manage' permission
            var currentUser = MembershipContext.AuthenticatedUser;
            friendsManagePermission = currentUser != null && (currentUser.IsAuthorizedPerResource("CMS.Friends", "Manage") || (currentUser.UserID == UserID));

            if (StopProcessing)
            {
                return;
            }

            if (IsLiveSite)
            {
                dialogsUrl = "~/CMSModules/Friends/CMSPages/";
            }
            Visible = true;

            // Register the dialog script
            ScriptHelper.RegisterDialogScript(Page);

            // Register script for action 'Remove'
            StringBuilder actionScript = new StringBuilder();
            actionScript.AppendLine("function FM_RemoveAction_" + ClientID + "()");
            actionScript.AppendLine("{");
            actionScript.AppendLine("   if(!" + gridElem.GetCheckSelectionScript(false) + ")");
            actionScript.AppendLine("   {");
            actionScript.AppendLine("      if (confirm(" + ScriptHelper.GetString(GetString("friends.ConfirmRemove")) + "))");
            actionScript.AppendLine("      {");
            actionScript.AppendLine(Page.ClientScript.GetPostBackEventReference(btnRemoveSelected, null) + ";");
            actionScript.AppendLine("      }");
            actionScript.AppendLine("   }");
            actionScript.AppendLine("   else");
            actionScript.AppendLine("   {");
            actionScript.AppendLine("       FM_ShowLabel('" + lblInfo.ClientID + "');");
            actionScript.AppendLine("   }");
            actionScript.AppendLine("   return false;");
            actionScript.AppendLine("}");

            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "actionScript_" + ClientID, ScriptHelper.GetScript(actionScript.ToString()));
            // Add action to button
            btnRemoveSelected.OnClientClick = "return FM_RemoveAction_" + ClientID + "();";
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

            // Register the refreshing script
            string refreshScript = ScriptHelper.GetScript("function refreshFriendsList(){" + Page.ClientScript.GetPostBackEventReference(hdnRefresh, string.Empty) + "} if (window.top) { window.top.refreshFriendsList = refreshFriendsList}");
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "friendsListRefresh", refreshScript);

            // Register approval script
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
            approveScript.AppendLine("      modalDialog(url + '&ids=' + items, 'rejectDialog', 720, 320);");
            approveScript.AppendLine("   }");
            approveScript.AppendLine("   else");
            approveScript.AppendLine("   {");
            approveScript.AppendLine("       FM_ShowLabel('" + lblInfo.ClientID + "');");
            approveScript.AppendLine("   }");
            approveScript.AppendLine("   return false;");
            approveScript.AppendLine("}");
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "approveScript_" + ClientID, ScriptHelper.GetScript(approveScript.ToString()));

            // Add action to button
            btnApproveSelected.OnClientClick = "return FM_Approve_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + ApproveDialogUrl + "');";
            btnApproveSelected.Enabled = friendsManagePermission;

            if (!RequestHelper.IsPostBack())
            {
                pnlInner.Visible = !ShowLink;
            }
            btnShowHide.ResourceString = !pnlInner.Visible ? "friends.showrejected" : "friends.hiderejected";

            if (!UseEncapsulation)
            {
                plcLink.Visible = false;
                pnlInner.Visible = true;
            }

            // Setup grid
            gridElem.OnAction += gridElem_OnAction;
            gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
            gridElem.OrderBy = "UserName";
            gridElem.IsLiveSite = IsLiveSite;

            // Where condition
            gridElem.WhereCondition = "(FriendStatus = " + Convert.ToInt32(FriendshipStatusEnum.Rejected) + ") AND (FriendRejectedBy = " + UserID + ")";
            
            // Add parameter @UserID
            if (gridElem.QueryParameters == null)
            {
                gridElem.QueryParameters = new QueryDataParameters();
            }
            gridElem.QueryParameters.Add("@UserID", UserID);

            pnlLinkButtons.Visible = IsLiveSite;

            if (!IsLiveSite)
            {
                var headerActions = ((CMSPage)Page).CurrentMaster.HeaderActions;
                headerActions.AddAction(new HeaderAction
                {
                    Text = GetString("friends.friendapproveall"),
                    OnClientClick = "return FM_Approve_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + ApproveDialogUrl + "');",
                    Enabled = friendsManagePermission
                });
                headerActions.AddAction(new HeaderAction
                {
                    Text = GetString("friends.friendremoveall"),
                    OnClientClick = "return FM_RemoveAction_" + ClientID + "();",
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


    #region "Button handling"

    protected void btnShowHide_Click(object sender, EventArgs e)
    {
        pnlInner.Visible = !pnlInner.Visible;
        btnShowHide.ResourceString = !pnlInner.Visible ? "friends.showrejected" : "friends.hiderejected";
    }


    protected void btnRemoveSelected_Click(object sender, EventArgs e)
    {
        // If there user selected some items
        if (gridElem.SelectedItems.Count > 0)
        {
            RaiseOnCheckPermissions(PERMISSION_MANAGE, this);

            // Get all needed friendships
            DataSet friendships = FriendInfoProvider.GetFriends()
                .WhereIn("FriendID", gridElem.SelectedItems.Select(id => ValidationHelper.GetInteger(id, 0)).ToList());

            if (!DataHelper.DataSourceIsEmpty(friendships))
            {
                // Delete all these friendships
                foreach (DataRow friendship in friendships.Tables[0].Rows)
                {
                    FriendInfoProvider.DeleteFriendInfo(new FriendInfo(friendship));
                }
            }
            gridElem.ResetSelection();
            // Reload grid
            gridElem.ReloadData();
        }
    }

    #endregion


    #region "UniGrid actions"

    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName)
        {
            case "remove":
                if (sender is CMSGridActionButton)
                {
                    // Get action button
                    CMSGridActionButton deleteBtn = (CMSGridActionButton)sender;
                    deleteBtn.Enabled = friendsManagePermission;

                    return deleteBtn;
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
                    DataRowView drv = UniGridFunctions.GetDataRowView((DataControlFieldCell)approveBtn.Parent);
                    // Add custom reject action
                    approveBtn.OnClientClick = "return FM_Approve_" + ClientID + "('" + drv["FriendID"] + "',null,'" + ApproveDialogUrl + "');";
                    approveBtn.Enabled = friendsManagePermission;
                    return approveBtn;
                }
                else
                {
                    return string.Empty;
                }

            case "friendrejectedwhen":
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


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that throws event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void gridElem_OnAction(string actionName, object actionArgument)
    {
        RaiseOnCheckPermissions(PERMISSION_MANAGE, this);
        switch (actionName)
        {
            case "remove":
                FriendInfoProvider.DeleteFriendInfo(ValidationHelper.GetInteger(actionArgument, 0));
                gridElem.ReloadData();
                break;
        }
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
