using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;


public partial class CMSAdminControls_ContextMenus_UserContextMenu : CMSContextMenuControl, IPostBackEventHandler
{
    #region "Variables"

    private CurrentUserInfo currentUser = null;
    protected bool isInIgnoreList = false;
    protected bool isInContactList = false;
    protected int requestedUserId = 0;

    #endregion


    #region "Properties"

    /// <summary>
    /// Indicates if the community module is loaded.
    /// </summary>
    public bool CommunityPresent
    {
        get
        {
            if (!RequestStockHelper.Contains("commPresent"))
            {
                RequestStockHelper.Add("commPresent", ModuleManager.IsModuleLoaded(ModuleName.COMMUNITY));
            }
            return ValidationHelper.GetBoolean(RequestStockHelper.GetItem("commPresent"), false);
        }
    }


    /// <summary>
    /// Indicates if the messaging module is loaded.
    /// </summary>
    public bool MessagingPresent
    {
        get
        {
            if (!RequestStockHelper.Contains("messagingPresent"))
            {
                RequestStockHelper.Add("messagingPresent", ModuleManager.IsModuleLoaded(ModuleName.MESSAGING));
            }

            return ValidationHelper.GetBoolean(RequestStockHelper.GetItem("messagingPresent"), false);
        }
    }

    #endregion


    #region "Events handling"

    /// <summary>
    /// OnLoad event.
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        repItem.ItemDataBound += repItem_ItemDataBound;

        currentUser = MembershipContext.AuthenticatedUser;
        string script = "";

        // Friendship request
        script += "function ContextFriendshipRequest(id) { \n" +
                  "modalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/CMSPages/Friends_Request.aspx") + "?userid=" + currentUser.UserID + "&requestid=' + id,'requestFriend', 810, 460); \n" +
                  " } \n";

        // Friendship rejection
        script += "function ContextFriendshipReject(id) { \n" +
                  "modalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/CMSPages/Friends_Reject.aspx") + "?userid=" + currentUser.UserID + "&requestid=' + id , 'rejectFriend', 720, 320); \n" +
                  " } \n";

        // Send private message
        script += "function ContextPrivateMessage(id) { \n" +
                  "modalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/CMSPages/SendMessage.aspx") + "?userid=" + currentUser.UserID + "&requestid=' + id , 'sendMessage', 700, 650); \n" +
                  " } \n";

        // Add to contact list
        script += "function ContextAddToContactList(usertoadd) { \n" +
                  "if(confirm(" + ScriptHelper.GetString(ResHelper.GetString("messaging.contactlist.addconfirmation")) + "))" +
                  "{" +
                  Page.ClientScript.GetPostBackEventReference(this, "addtocontactlist", false) +
                  "} } \n";

        // Add to ignore list
        script += "function ContextAddToIgnoretList(usertoadd) { \n" +
                  "if(confirm(" + ScriptHelper.GetString(ResHelper.GetString("messaging.ignorelist.addconfirmation")) + "))" +
                  "{" +
                  Page.ClientScript.GetPostBackEventReference(this, "addtoignorelist", false) +
                  "} } \n";

        // Group invitation
        script += "function ContextGroupInvitation(id) { \nmodalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Groups/CMSPages/InviteToGroup.aspx") + "?invitedid=' + id , 'inviteToGroup', 500, 450); \n } \n";

        // Redirect to sign in URL
        string signInUrl = MacroResolver.Resolve(AuthenticationHelper.GetSecuredAreasLogonPage(SiteContext.CurrentSiteName));
        if (signInUrl != "")
        {
            signInUrl = "window.location.replace('" + URLHelper.AddParameterToUrl(ResolveUrl(signInUrl), "ReturnURL", Server.UrlEncode(RequestContext.CurrentURL)) + "');";
        }

        script += "function ContextRedirectToSignInUrl() { \n" + signInUrl + "} \n";

        // Register menu management scripts
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "UserContextMenuManagement", ScriptHelper.GetScript(script));

        // Register the dialog script
        ScriptHelper.RegisterDialogScript(Page);
    }


    /// <summary>
    /// Bounding event.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event arguments</param>
    protected void repItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Panel pnlItem = (Panel)e.Item.FindControl("pnlItem");
        if (pnlItem != null)
        {
            int count = ValidationHelper.GetInteger(((DataRowView)e.Item.DataItem)["Count"], 0) - 1;
            if (e.Item.ItemIndex == count)
            {
                pnlItem.CssClass = "item-last";
            }

            string action = (string)((DataRowView)e.Item.DataItem)["ActionScript"];
            pnlItem.Attributes.Add("onclick", action + ";");
        }
    }


    /// <summary>
    /// Postback handling.
    /// </summary>
    /// <param name="eventArgument">Argument of postback event</param>
    public void RaisePostBackEvent(string eventArgument)
    {
        if ((eventArgument == null))
        {
            return;
        }

        // Get ID of user
        int selectedId = ValidationHelper.GetInteger(ContextMenu.Parameter, 0);
        if (selectedId == 0)
        {
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "UserContextMenuError", ScriptHelper.GetScript("alert('No user was selected.');"));
        }
        else
        {
            // Add only if messaging is present
            if (MessagingPresent)
            {
                // Add to contact or ignore list
                switch (eventArgument)
                {
                    case "addtoignorelist":
                        ModuleCommands.MessagingAddToIgnoreList(currentUser.UserID, selectedId);
                        break;

                    case "addtocontactlist":
                        ModuleCommands.MessagingAddToContactList(currentUser.UserID, selectedId);
                        break;
                }
            }
        }
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        requestedUserId = ValidationHelper.GetInteger(ContextMenu.Parameter, 0);

        DataTable table = new DataTable();
        table.Columns.Add("ActionDisplayName");
        table.Columns.Add("ActionScript");

        // Get resource strings prefix
        string resourcePrefix = ContextMenu.ResourcePrefix;

        // Add only if community is present
        if (CommunityPresent)
        {
            // Friendship request
            if ((requestedUserId != currentUser.UserID) && UIHelper.IsFriendsModuleEnabled(SiteContext.CurrentSiteName))
            {
                FriendshipStatusEnum status = currentUser.HasFriend(requestedUserId);
                bool authenticated = AuthenticationHelper.IsAuthenticated();

                // If friendship exists add reject action or request friendship
                if (status == FriendshipStatusEnum.Approved)
                {
                    table.Rows.Add(new object[] { ResHelper.GetString(resourcePrefix + ".rejectfriendship|friends.rejectfriendship"), authenticated ? "ContextFriendshipReject(GetContextMenuParameter('" + ContextMenu.MenuID + "'))" : "ContextRedirectToSignInUrl()" });
                }
                else if ((status == FriendshipStatusEnum.None) || currentUser.IsPublic())
                {
                    table.Rows.Add(new object[] { ResHelper.GetString(resourcePrefix + ".requestfriendship|friends.requestfriendship"), authenticated ? "ContextFriendshipRequest(GetContextMenuParameter('" + ContextMenu.MenuID + "'))" : "ContextRedirectToSignInUrl()" });
                }

                // Group invitation
                table.Rows.Add(new object[] { ResHelper.GetString(resourcePrefix + ".invite|groupinvitation.invite"), authenticated ? "ContextGroupInvitation(GetContextMenuParameter('" + ContextMenu.MenuID + "'))" : "ContextRedirectToSignInUrl()" });
            }
        }

        // Add only if messaging is present
        if (MessagingPresent)
        {
            // Check if user is in ignore list
            isInIgnoreList = ModuleCommands.MessagingIsInIgnoreList(currentUser.UserID, requestedUserId);

            // Check if user is in contact list
            isInContactList = ModuleCommands.MessagingIsInContactList(currentUser.UserID, requestedUserId);
            bool authenticated = AuthenticationHelper.IsAuthenticated();

            table.Rows.Add(new object[] { ResHelper.GetString(resourcePrefix + ".sendmessage|sendmessage.sendmessage"), authenticated ? "ContextPrivateMessage(GetContextMenuParameter('" + ContextMenu.MenuID + "'))" : "ContextRedirectToSignInUrl()" });

            // Not for the same user
            if (requestedUserId != currentUser.UserID)
            {
                // Add to ignore list or add to contact list actions
                if (!isInIgnoreList)
                {
                    table.Rows.Add(new object[] { ResHelper.GetString(resourcePrefix + ".addtoignorelist|messsaging.addtoignorelist"), authenticated ? "ContextAddToIgnoretList(GetContextMenuParameter('" + ContextMenu.MenuID + "'))" : "ContextRedirectToSignInUrl()" });
                }

                if (!isInContactList)
                {
                    table.Rows.Add(new object[] { ResHelper.GetString(resourcePrefix + ".addtocontactlist|messsaging.addtocontactlist"), authenticated ? "ContextAddToContactList(GetContextMenuParameter('" + ContextMenu.MenuID + "'))" : "ContextRedirectToSignInUrl()" });
                }
            }
        }

        // Add count column
        DataColumn countColumn = new DataColumn();
        countColumn.ColumnName = "Count";
        countColumn.DefaultValue = table.Rows.Count;

        table.Columns.Add(countColumn);
        repItem.DataSource = table;
        repItem.DataBind();
    }

    #endregion
}
