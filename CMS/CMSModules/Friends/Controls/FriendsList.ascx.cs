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


public partial class CMSModules_Friends_Controls_FriendsList : CMSAdminListControl
{
    #region "Private variables"

    private string dialogsUrl = "~/CMSModules/Friends/Dialogs/";
    private UserInfo currentUserInfo = null;
    private SiteInfo currentSiteInfo = null;
    private string mDialogUrl = null;
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
    /// Gets dialog url.
    /// </summary>
    protected string DialogUrl
    {
        get
        {
            if (mDialogUrl == null)
            {
                mDialogUrl = ApplicationUrlHelper.ResolveDialogUrl(dialogsUrl + "Friends_Reject.aspx") + "?userid=" + UserID;
            }

            return mDialogUrl;
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
        gridElem.FilterLimit = 0;

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

            // Register the refreshing script
            string refreshScript = ScriptHelper.GetScript("function refreshFriendsList(){" + Page.ClientScript.GetPostBackEventReference(hdnRefresh, string.Empty) + "} if (window.top) { window.top.refreshFriendsList = refreshFriendsList}");
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "friendsListRefresh", refreshScript);

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

            // Hide label
            lblInfo.Attributes["style"] = "display: none;";

            // Setup grid
            gridElem.OnAction += gridElem_OnAction;
            gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
            gridElem.OrderBy = "UserName";
            gridElem.IsLiveSite = IsLiveSite;

            // Where condition
            gridElem.WhereCondition = "FriendStatus = " + Convert.ToInt32(FriendshipStatusEnum.Approved);

            // Add parameter @UserID
            if (gridElem.QueryParameters == null)
            {
                gridElem.QueryParameters = new QueryDataParameters();
            }
            gridElem.QueryParameters.Add("@UserID", UserID);

            pnlLinkButtons.Visible = IsLiveSite;
            if (IsLiveSite || friendsManagePermission)
            {
                btnRejectSelected.OnClientClick = "return FM_Reject_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + DialogUrl + "');";
                btnRemoveSelected.OnClientClick = "return FM_RemoveAction_" + ClientID + "();";
            }
            else
            {
                btnRejectSelected.Enabled = false;
                btnRemoveSelected.Enabled = false;
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        bool actionsVisible = HasData() || gridElem.FilterIsSet;

        // Add action buttons
        if (actionsVisible && !IsLiveSite)
        {
            var headerActions = ((CMSPage)Page).CurrentMaster.HeaderActions;
            headerActions.AddAction(new HeaderAction()
            {
                Text = GetString("friends.friendrejectall"),
                OnClientClick = "return FM_Reject_" + ClientID + "(null,'" + gridElem.GetSelectionFieldClientID() + "','" + DialogUrl + "');",
                Enabled = friendsManagePermission,
            });
            headerActions.AddAction(new HeaderAction()
            {
                Text = GetString("friends.friendremoveall"),
                OnClientClick = "return FM_RemoveAction_" + ClientID + "();",
                Enabled = friendsManagePermission,
            });
            headerActions.ReloadData();
        }

        plcNoData.Visible = actionsVisible;

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


    #region "Methods"

    /// <summary>
    /// Indicates if there are some data.
    /// </summary>
    public bool HasData()
    {
        return (!DataHelper.DataSourceIsEmpty(gridElem.GridView.DataSource));
    }

    #endregion


    #region "Grid events"

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

            case "reject":
                if (sender is CMSGridActionButton)
                {
                    // Get action button
                    CMSGridActionButton rejectBtn = (CMSGridActionButton)sender;
                    // Get full row view
                    DataRowView drv = UniGridFunctions.GetDataRowView((DataControlFieldCell)rejectBtn.Parent);
                    // Add custom reject action
                    if (friendsManagePermission)
                    {
                        rejectBtn.OnClientClick = "return FM_Reject_" + ClientID + "('" + drv["FriendID"] + "',null,'" + DialogUrl + "');";
                    }
                    else
                    {
                        rejectBtn.Enabled = false;
                    }

                    return rejectBtn;
                }
                else
                {
                    return string.Empty;
                }

            case "friendapprovedwhen":
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


    #region "Button handling"

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
}
