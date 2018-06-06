using System;
using System.Linq;
using System.Data;

using CMS.Base;

using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Community;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_FriendsRequestedList : CMSAdminListControl
{
    #region "Private variables"

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

            // Register the dialog script
            ScriptHelper.RegisterDialogScript(Page);

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
            btnRemoveSelected.Enabled = friendsManagePermission;
            // Hide label
            lblInfo.Attributes["style"] = "display: none;";

            // Register the refreshing script
            string refreshScript = ScriptHelper.GetScript("function refreshFriendsList(){" + Page.ClientScript.GetPostBackEventReference(hdnRefresh, string.Empty) + "} if (window.top) { window.top.refreshFriendsList = refreshFriendsList}");
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "friendsListRefresh", refreshScript);

            gridElem.OnAction += gridElem_OnAction;
            gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
            gridElem.OrderBy = "UserName";
            gridElem.IsLiveSite = IsLiveSite;

            // Default where condition
            gridElem.WhereCondition = "((FriendStatus = " + Convert.ToInt32(FriendshipStatusEnum.Waiting) + " AND FriendUserID = " + UserID + ") OR (FriendStatus = " + Convert.ToInt32(FriendshipStatusEnum.Rejected) + " AND FriendRejectedBy <> " + UserID + "))";

            // Add parameter @UserID
            if (gridElem.QueryParameters == null)
            {
                gridElem.QueryParameters = new QueryDataParameters();
            }
            gridElem.QueryParameters.Add("@UserID", UserID);

            pnlLinkButtons.Visible = IsLiveSite;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        bool actionsVisible = plcNoData.Visible = HasData() || gridElem.FilterIsSet;

        // Add action buttons
        if (actionsVisible && !IsLiveSite)
        {
            var headerActions = ((CMSPage)Page).CurrentMaster.HeaderActions;
            headerActions.AddAction(new HeaderAction()
            {
                Text = GetString("friends.friendremoveall"),
                OnClientClick = "return FM_RemoveAction_" + ClientID + "();",
                Enabled = friendsManagePermission
            });
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
        switch (sourceName.ToLowerCSafe())
        {
            case "remove":
                bool rejected = ValidationHelper.GetString(((DataRowView)((GridViewRow)parameter).DataItem).Row["FriendStatus"], string.Empty) == "1";
                // Disable checkbox
                GridViewRow row = (GridViewRow)parameter;

                Control control = ControlsHelper.GetChildControl(row, typeof(CMSCheckBox));
                if (control != null)
                {
                    CMSCheckBox checkBox = (CMSCheckBox)control;
                    checkBox.Enabled = !rejected;
                }
                // Disable button
                if (rejected || !friendsManagePermission)
                {
                    CMSGridActionButton button = ((CMSGridActionButton)sender);
                    button.Enabled = false;
                }
                break;

            case "status":
                // Set status (rejected/waiting)
                FriendshipStatusEnum status =
                    (FriendshipStatusEnum)Enum.Parse(typeof(FriendshipStatusEnum), parameter.ToString());
                switch (status)
                {
                    case FriendshipStatusEnum.Waiting:
                        parameter = "<span class=\"Waiting\">" + GetString("friends.waiting") + "</span>";
                        break;

                    case FriendshipStatusEnum.Rejected:
                        parameter = "<span class=\"Rejected\">" + GetString("general.rejected") + "</span>";
                        break;
                }
                break;

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

        FriendInfo fi = FriendInfoProvider.GetFriendInfo(ValidationHelper.GetInteger(actionArgument, 0));
        if (fi != null)
        {
            switch (actionName)
            {
                case "remove":
                    if (fi.FriendStatus != FriendshipStatusEnum.Rejected)
                    {
                        FriendInfoProvider.DeleteFriendInfo(fi);
                    }
                    gridElem.ReloadData();
                    break;
            }
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
                    FriendInfo fi = new FriendInfo(friendship);
                    if (fi.FriendStatus != FriendshipStatusEnum.Rejected)
                    {
                        FriendInfoProvider.DeleteFriendInfo(fi);
                    }
                }
            }
            gridElem.ResetSelection();
            // Reload grid
            gridElem.ReloadData();
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
