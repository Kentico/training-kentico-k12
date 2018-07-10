using System;
using System.Data;

using CMS.Base;
using CMS.Helpers;

using System.Text;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.FormEngine.Web.UI;
using CMS.Globalization;
using CMS.Globalization.Web.UI;
using CMS.Membership;
using CMS.Messaging;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.UIControls.UniGridConfig;


public partial class CMSModules_Messaging_Controls_Inbox : CMSUserControl
{
    #region "Variables & enums"

    private string mZeroRowsText;
    private int mPageSize = 10;
    private bool mPasteOriginalMessage = true;
    private bool mShowOriginalMessage = true;
    protected string currentViewButtonClientId = null;
    private UserInfo currentUserInfo;
    private SiteInfo currentSiteInfo;
    private MessageInfo mMessage;
    private bool breadcrumbsInitialized;

    protected enum What
    {
        SelectedMessages = 0,
        AllMessages = 1
    }

    protected enum Action
    {
        SelectAction = 0,
        MarkAsRead = 1,
        MarkAsUnread = 2,
        Delete = 3
    }

    #endregion


    #region "Private properties"

    /// <summary>
    /// Message mode.
    /// </summary>
    private string MessageMode
    {
        get
        {
            return ValidationHelper.GetString(ViewState["MessageMode"], string.Empty);
        }
        set
        {
            ViewState["MessageMode"] = value;
        }
    }


    /// <summary>
    /// Current message ID.
    /// </summary>
    protected int CurrentMessageId
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["CurrentMessageId"], 0);
        }
        set
        {
            ViewState["CurrentMessageId"] = value;
            if (value <= 0)
            {
                mMessage = null;
            }
        }
    }


    /// <summary>
    /// Current message.
    /// </summary>
    protected MessageInfo Message
    {
        get
        {
            if ((mMessage == null) && (CurrentMessageId > 0))
            {
                mMessage = MessageInfoProvider.GetMessageInfo(CurrentMessageId);
            }
            return mMessage;
        }
    }


    /// <summary>
    /// Breadcrumbs of the page.
    /// </summary>
    private Breadcrumbs PageBreadcrumbs
    {
        get
        {
            var p = Page as CMSPage;
            if (p != null)
            {
                return p.PageBreadcrumbs;
            }

            return null;
        }
    }


    /// <summary>
    /// Indicates whether breadcrumbs should be visible depending on where the control is located.
    /// </summary>
    private bool ShowBreadcrumbs
    {
        get
        {
            if (PortalContext.ViewMode == ViewModeEnum.DashboardWidgets)
            {
                return true;
            }

            return IsLiveSite;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            ucSendMessage.IsLiveSite = value;
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Zero rows text.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return mZeroRowsText;
        }
        set
        {
            mZeroRowsText = value;
            EnsureChildControls();
            inboxGrid.ZeroRowsText = HTMLHelper.HTMLEncode(mZeroRowsText);
        }
    }


    /// <summary>
    /// Size of the page.
    /// </summary>
    public int PageSize
    {
        get
        {
            return mPageSize;
        }
        set
        {
            mPageSize = value;
            EnsureChildControls();
            inboxGrid.PageSize = value.ToString();
        }
    }


    /// <summary>
    /// Inner grid control.
    /// </summary>
    public UniGrid Grid
    {
        get
        {
            EnsureChildControls();
            return inboxGrid;
        }
    }


    /// <summary>
    /// True if original message should be pasted to the current.
    /// </summary>
    public bool PasteOriginalMessage
    {
        get
        {
            return mPasteOriginalMessage;
        }
        set
        {
            mPasteOriginalMessage = value;
            EnsureChildControls();
            ucSendMessage.PasteOriginalMessage = value;
        }
    }


    /// <summary>
    /// True if original message should be shown.
    /// </summary>
    public bool ShowOriginalMessage
    {
        get
        {
            return mShowOriginalMessage;
        }
        set
        {
            mShowOriginalMessage = value;
        }
    }

    #endregion


    #region "Page events"

    protected override void EnsureChildControls()
    {
        base.EnsureChildControls();

        if (inboxGrid == null)
        {
            pnlInbox.LoadContainer();
        }
    }


    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set pager links text on live site
        if (IsLiveSite)
        {
            inboxGrid.Pager.FirstPageText = "&lt;&lt;";
            inboxGrid.Pager.LastPageText = "&gt;&gt;";
            inboxGrid.Pager.PreviousPageText = "&lt;";
            inboxGrid.Pager.NextPageText = "&gt;";
            inboxGrid.Pager.PreviousGroupText = "...";
            inboxGrid.Pager.NextGroupText = "...";
        }

        SetupControls();
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Set info message
        if (ucViewMessage.InformationText != string.Empty)
        {
            ShowInformation(ucViewMessage.InformationText);
        }

        // Set error message
        if (ucViewMessage.ErrorText != string.Empty)
        {
            ShowError(ucViewMessage.ErrorText);
        }

        // Hide footer action panel
        pnlAction.Visible = inboxGrid.Visible && (inboxGrid.GridView.Rows.Count > 0);

        // Ensure correct breadcrumbs items if no object is edited
        string messageMode = MessageMode;
        if (!string.IsNullOrEmpty(messageMode))
        {
            UpdateBreadcrumbs(messageMode);
        }
        else
        {
            ClearBreadcrumbs();
        }
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Sets up controls.
    /// </summary>
    public void SetupControls()
    {
        if (StopProcessing)
        {
            // Do nothing
            ucSendMessage.StopProcessing = true;
            ucViewMessage.StopProcessing = true;
            inboxGrid.StopProcessing = true;
        }
        else
        {
            bool isPostBack = RequestHelper.IsPostBack();

            if (!isPostBack)
            {
                inboxGrid.DelayedReload = false;
            }
            else
            {
                // Find postback invoker
                if (!ControlsHelper.CausedPostBack(inboxGrid, btnNewMessage, ucSendMessage.SendButton, ucSendMessage.CancelButton, btnReply, btnHidden, btnForward, btnDelete, btnOk, lnkBackHidden))
                {
                    inboxGrid.DelayedReload = false;
                }

                SetupLabels();
            }

            // Show content only for authenticated users
            if (AuthenticationHelper.IsAuthenticated())
            {
                // Initialize breadcrumbs
                InitializeBreadcrumbs();

                // Show control
                Visible = true;
                // Initialize unigrid
                inboxGrid.OnExternalDataBound += inboxGrid_OnExternalDataBound;
                inboxGrid.GridView.RowDataBound += GridView_RowDataBound;
                inboxGrid.OnAction += inboxGrid_OnAction;
                // Set where condition clause
                inboxGrid.WhereCondition = "MessageRecipientUserID=" + MembershipContext.AuthenticatedUser.UserID + " AND (MessageRecipientDeleted=0 OR MessageRecipientDeleted IS NULL)";
                inboxGrid.OnBeforeDataReload += inboxGrid_OnBeforeDataReload;
                inboxGrid.OnBeforeSorting += inboxGrid_OnBeforeSorting;
                inboxGrid.OnShowButtonClick += inboxGrid_OnShowButtonClick;
                inboxGrid.OnPageSizeChanged += inboxGrid_OnPageSizeChanged;
                inboxGrid.IsLiveSite = IsLiveSite;

                // Setup inner controls
                ucSendMessage.IsLiveSite = IsLiveSite;
                ucViewMessage.IsLiveSite = IsLiveSite;
                ucViewMessage.MessageMode = MessageModeEnum.Inbox;
                ucViewMessage.Message = Message;

                // Create and assign javascript confirmation
                btnDelete.OnClientClick = "return confirm(" + ScriptHelper.GetString(GetString("Messsaging.DeletionConfirmation")) + ");";

                // Register events
                ucSendMessage.SendButtonClick += ucSendMessage_SendButtonClick;
                btnNewMessage.Click += btnNewMessage_Click;
                btnReply.Click += btnReply_Click;
                btnForward.Click += btnForward_Click;
                btnDelete.Click += btnDelete_Click;
                lnkBackHidden.Click += lnkBackHidden_Click;
                btnHidden.Click += btnHidden_Click;

                StringBuilder actionScript = new StringBuilder();
                actionScript.Append(
                    @"
function PerformAction_", ClientID, @"(selectionFunction, actionId, actionLabel, whatId) {
    var confirmation = null;
    var label = document.getElementById(actionLabel);
    var action = document.getElementById(actionId).value;
    var whatDrp = document.getElementById(whatId);
    if (action == '", (int)Action.SelectAction, @"') {
        label.innerHTML = ", ScriptHelper.GetString(GetString("MassAction.SelectSomeAction")), @"
    }
    else if (eval(selectionFunction) && (whatDrp.value == '", (int)What.SelectedMessages, @"')) {
        label.innerHTML = ", ScriptHelper.GetString(GetString("Messaging.SelectMessages")), @";
    }
    else {
        switch(action) {
            case '", (int)Action.MarkAsRead, @"':
                confirmation = ", ScriptHelper.GetString(GetString("Messaging.ConfirmMarkAsRead")), @";
                break;
            case '", (int)Action.MarkAsUnread, @"':
                confirmation = ", ScriptHelper.GetString(GetString("Messaging.ConfirmMarkAsUnread")), @";
                break;
            case '", (int)Action.Delete, @"':
                confirmation = ", ScriptHelper.GetString(GetString("Messaging.ConfirmDelete")), @";
                break;
            default:
                confirmation = null;
                break;
        }
        if (confirmation != null) {
            return confirm(confirmation)
        }
    }
    return false;
}
");

                ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "actionScript", ScriptHelper.GetScript(actionScript.ToString()));

                string messageActionScript =
                    @"
function ContextMessageAction_" + inboxGrid.ClientID + @"(action, messageId) {
    document.getElementById('" + hdnValue.ClientID + @"').value = action + ';' + messageId;" +
                    ControlsHelper.GetPostBackEventReference(btnHidden) + @";
}";
                ScriptHelper.RegisterStartupScript(this, typeof(string), "messageAction_" + ClientID + new Random(DateTime.Now.Millisecond).Next(), ScriptHelper.GetScript(messageActionScript));

                // Add action to button
                btnOk.OnClientClick = "return PerformAction_" + ClientID + "('" + inboxGrid.GetCheckSelectionScript() + "','" + drpAction.ClientID + "','" + lblActionInfo.ClientID + "','" + drpWhat.ClientID + "');";
                btnOk.Click += btnOk_Click;

                if (!isPostBack)
                {
                    // Initialize dropdown lists
                    drpWhat.Items.Add(new ListItem(GetString("Messaging." + What.SelectedMessages), Convert.ToInt32(What.SelectedMessages).ToString()));
                    drpWhat.Items.Add(new ListItem(GetString("Messaging." + What.AllMessages), Convert.ToInt32(What.AllMessages).ToString()));

                    drpAction.Items.Add(new ListItem(GetString("general." + Action.SelectAction), Convert.ToInt32(Action.SelectAction).ToString()));
                    drpAction.Items.Add(new ListItem(GetString("Messaging.Action." + Action.MarkAsRead), Convert.ToInt32(Action.MarkAsRead).ToString()));
                    drpAction.Items.Add(new ListItem(GetString("Messaging.Action." + Action.MarkAsUnread), Convert.ToInt32(Action.MarkAsUnread).ToString()));
                    drpAction.Items.Add(new ListItem(GetString("Messaging.Action." + Action.Delete), Convert.ToInt32(Action.Delete).ToString()));
                }

                if (inboxGrid.PagerConfig == null)
                {
                    inboxGrid.PagerConfig = new UniGridPagerConfig();
                }
                inboxGrid.PagerConfig.DefaultPageSize = PageSize;
            }
            else
            {
                Visible = false;
            }
        }
    }

    #endregion


    #region "Message actions"

    /// <summary>
    /// New message.
    /// </summary>
    private void NewMessage()
    {
        // Initialize new message control
        pnlBackToList.Visible = true;

        headNewMessage.ResourceString = "messaging.newmessage";
        pnlList.Visible = false;
        pnlNew.Visible = true;
        pnlActions.Visible = false;

        // Initialize new and view message
        ucSendMessage.StopProcessing = false;
        ucSendMessage.SendMessageMode = MessageActionEnum.New;
        ucSendMessage.MessageId = 0;
        ucSendMessage.Message = null;
        ucSendMessage.ReloadData();

        MessageMode = GetString("messaging.newmessage");
    }


    /// <summary>
    /// Reply message.
    /// </summary>
    private void ReplyMessage()
    {
        pnlBackToList.Visible = true;

        pnlList.Visible = false;
        pnlNew.Visible = true;
        pnlView.Visible = ShowOriginalMessage;
        pnlActions.Visible = false;

        // Initialize new and view message
        ucSendMessage.StopProcessing = false;
        ucSendMessage.Message = Message;
        ucSendMessage.MessageSubject = GetString("Messaging.ReSign");
        ucSendMessage.SendMessageMode = MessageActionEnum.Reply;
        ucSendMessage.ReloadData();
        ucViewMessage.StopProcessing = false;
        ucViewMessage.Message = Message;
        ucViewMessage.ReloadData();
        headNewMessage.ResourceString = "Messaging.Reply";
        headOriginalMessage.ResourceString = "Messaging.OriginalMessage";

        MessageMode = GetString("messaging.replytomessage");
    }


    /// <summary>
    /// Forward message.
    /// </summary>
    private void ForwardMesssage()
    {
        pnlBackToList.Visible = true;

        pnlList.Visible = false;
        pnlNew.Visible = true;
        pnlView.Visible = ShowOriginalMessage;
        pnlActions.Visible = false;

        // Initialize new and view message
        ucSendMessage.StopProcessing = false;
        ucSendMessage.Message = Message;
        ucSendMessage.MessageSubject = GetString("Messaging.ForwardSign");
        ucSendMessage.SendMessageMode = MessageActionEnum.Forward;
        ucSendMessage.ReloadData();
        ucViewMessage.StopProcessing = false;
        ucViewMessage.Message = Message;
        ucViewMessage.ReloadData();
        headNewMessage.ResourceString = "Messaging.Forward";
        headOriginalMessage.ResourceString = "Messaging.OriginalMessage";

        MessageMode = GetString("messaging.forwardmessage");
    }


    /// <summary>
    /// View message.
    /// </summary>
    private void ViewMessage()
    {
        pnlActions.Visible = true;
        pnlList.Visible = false;
        pnlNew.Visible = false;
        pnlView.Visible = true;
        pnlBackToList.Visible = true;

        // Initialize view message control
        ucViewMessage.StopProcessing = false;
        ucViewMessage.Message = Message;
        ucViewMessage.ReloadData();

        MessageMode = GetString("messaging.viewmessage");
    }


    /// <summary>
    /// Sets message to read message.
    /// </summary>
    private void ReadMessage()
    {
        if (Message != null)
        {
            bool updateMessage = false;

            // Check message read date
            if (Message.MessageRead == DateTimeHelper.ZERO_TIME)
            {
                Message.MessageRead = DateTime.Now;
                updateMessage = true;
            }

            // Check message read flag
            if (!Message.MessageIsRead)
            {
                Message.MessageIsRead = true;
                updateMessage = true;
            }

            // Update message
            if (updateMessage)
            {
                MessageInfoProvider.SetMessageInfo(Message);
            }
        }
    }


    /// <summary>
    /// Delete message.
    /// </summary>
    private void DeleteMessage()
    {
        try
        {
            inboxGrid.DelayedReload = false;
            MessageInfoProvider.DeleteReceivedMessage(CurrentMessageId);
            ShowConfirmation(GetString("Messsaging.MessageDeleted"));
            pnlList.Visible = true;
            pnlNew.Visible = false;
            pnlView.Visible = false;
            pnlBackToList.Visible = false;
            ucBreadcrumbs.HideBreadcrumbs = true;
            ClearBreadcrumbs();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }


    /// <summary>
    /// Mark message as read.
    /// </summary>
    private void MarkAsRead()
    {
        if (Message != null)
        {
            // Mark as read
            if ((Message.MessageRead == DateTimeHelper.ZERO_TIME) || !Message.MessageIsRead)
            {
                Message.MessageIsRead = true;
                if (Message.MessageRead == DateTimeHelper.ZERO_TIME)
                {
                    Message.MessageRead = DateTime.Now;
                }
                MessageInfoProvider.SetMessageInfo(Message);
            }
        }

        inboxGrid.DelayedReload = false;
    }


    /// <summary>
    /// Mark message as unread.
    /// </summary>
    private void MarkAsUnread()
    {
        if (Message != null)
        {
            // Mark as unread
            if (Message.MessageIsRead)
            {
                Message.MessageIsRead = false;
                MessageInfoProvider.SetMessageInfo(Message);
            }
        }

        inboxGrid.DelayedReload = false;
    }


    /// <summary>
    /// Perform selected action (Mark as read, Mark as unread, Delete).
    /// </summary>
    private void PerformAction()
    {
        string resultMessage;

        Action action = (Action)ValidationHelper.GetInteger(drpAction.SelectedItem.Value, 0);
        What what = (What)ValidationHelper.GetInteger(drpWhat.SelectedItem.Value, 0);

        string where = null;

        // All messages
        if (what == What.AllMessages)
        {
            resultMessage = GetString("Messaging." + What.AllMessages);
        }
        // Selected messages
        else if (what == What.SelectedMessages)
        {
            where = SqlHelper.GetWhereCondition<int>("MessageID", inboxGrid.SelectedItems, false);
            resultMessage = GetString("Messaging." + What.SelectedMessages);
        }
        else
        {
            return;
        }

        // Action 'Delete'
        if ((action == Action.Delete))
        {
            MessageInfoProvider.DeleteReceivedMessages(MembershipContext.AuthenticatedUser.UserID, where);
            resultMessage += " " + GetString("Messaging.Action.Result.Deleted");
        }
        // Action 'Mark as read'
        else if ((action == Action.MarkAsRead))
        {
            MessageInfoProvider.MarkReadReceivedMessages(MembershipContext.AuthenticatedUser.UserID, where, DateTime.Now);
            resultMessage += " " + GetString("Messaging.Action.Result.MarkAsRead");
        }
        // Action 'Mark as unread'
        else if (action == Action.MarkAsUnread)
        {
            MessageInfoProvider.MarkUnreadReceivedMessages(MembershipContext.AuthenticatedUser.UserID, where);
            resultMessage += " " + GetString("Messaging.Action.Result.MarkAsUnread");
        }
        else
        {
            return;
        }

        if (!string.IsNullOrEmpty(resultMessage))
        {
            ShowConfirmation(resultMessage);
        }

        inboxGrid.ResetSelection();
        inboxGrid.ReloadData();
    }

    #endregion


    #region "Buttons actions"

    private void btnHidden_Click(object sender, EventArgs e)
    {
        // Process message action
        string[] args = hdnValue.Value.Split(';');
        if (args.Length == 2)
        {
            inboxGrid_OnAction(args[0], args[1]);
            inboxGrid.ReloadData();
        }
    }


    protected void inboxGrid_OnShowButtonClick(object sender, EventArgs e)
    {
        inboxGrid.DelayedReload = false;
    }


    protected void lnkBackHidden_Click(object sender, EventArgs e)
    {
        pnlList.Visible = true;
        pnlNew.Visible = false;
        pnlView.Visible = false;
        pnlBackToList.Visible = false;
        ucBreadcrumbs.HideBreadcrumbs = true;
        ucSendMessage.SendMessageMode = MessageActionEnum.None;
        inboxGrid.ReloadData();
        ClearBreadcrumbs();
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteMessage();
        inboxGrid.ReloadData();
    }


    protected void btnForward_Click(object sender, EventArgs e)
    {
        ForwardMesssage();
    }


    protected void btnReply_Click(object sender, EventArgs e)
    {
        ReplyMessage();
    }


    protected void btnNewMessage_Click(object sender, EventArgs e)
    {
        NewMessage();
    }


    protected void ucSendMessage_SendButtonClick(object sender, EventArgs e)
    {
        // If no error, inform user
        if (String.IsNullOrEmpty(ucSendMessage.ErrorText))
        {
            ShowConfirmation(ucSendMessage.InformationText);
            lnkBackHidden_Click(sender, e);
        }
        else
        {
            ucViewMessage.StopProcessing = false;
            SetupLabels();
            ucViewMessage.ReloadData();
            pnlActions.Visible = false;
        }
    }


    protected void btnOk_Click(object sender, EventArgs e)
    {
        PerformAction();
    }

    #endregion


    #region "Grid methods"

    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView rowView = ((DataRowView)e.Row.DataItem);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Mark unread message
            DateTime messageRead = ValidationHelper.GetDateTime(DataHelper.GetDataRowViewValue(rowView, "MessageRead"), DateTimeHelper.ZERO_TIME);
            bool messageIsRead = ValidationHelper.GetBoolean(DataHelper.GetDataRowViewValue(rowView, "MessageIsRead"), true);
            if ((messageRead == DateTimeHelper.ZERO_TIME) || !messageIsRead)
            {
                e.Row.CssClass += " Unread";
            }
        }
    }


    protected void inboxGrid_OnBeforeSorting(object sender, EventArgs e)
    {
        inboxGrid.ReloadData();
    }


    protected void inboxGrid_OnPageSizeChanged()
    {
        inboxGrid.ReloadData();
    }


    protected void inboxGrid_OnBeforeDataReload()
    {
        // Bind grid footer
        BindFooter();
    }


    protected object inboxGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "messagesendernickname":
            case "messagesubject":
                // Avoid XSS
                return HTMLHelper.HTMLEncode(Convert.ToString(parameter));

            case "messagesent":
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
        }
        return parameter;
    }


    protected void inboxGrid_OnAction(string actionName, object actionArgument)
    {
        CurrentMessageId = ValidationHelper.GetInteger(actionArgument, 0);

        // Force reinitialization of message object if differs from currently processed message
        if ((Message != null) && (Message.MessageID != CurrentMessageId))
        {
            mMessage = null;
        }

        // Check permissions - user has to be recipient
        if ((Message == null) || (Message.MessageRecipientUserID != MembershipContext.AuthenticatedUser.UserID))
        {
            return;
        }

        switch (actionName)
        {
            // Delete message
            case "delete":
                DeleteMessage();
                break;

            // Reply message
            case "reply":
                ReadMessage();
                ReplyMessage();
                break;

            // View message
            case "view":
                ReadMessage();
                ViewMessage();
                break;

            // Forward message
            case "forward":
                ReadMessage();
                ForwardMesssage();
                break;

            // Mark message as read
            case "markread":
                MarkAsRead();
                break;

            // Mark message as unread
            case "markunread":
                MarkAsUnread();
                break;
        }
    }

    #endregion


    #region "Protected methods"

    /// <summary>
    /// Bind the data.
    /// </summary>
    protected void BindFooter()
    {
        int userId = MembershipContext.AuthenticatedUser.UserID;
        ShowInformation(string.Format(GetString("Messaging.UnreadOfAll"), MessageInfoProvider.GetUnreadMessagesCount(userId), MessageInfoProvider.GetMessagesCount(userId)));
    }


    /// <summary>
    /// Sel label values.
    /// </summary>
    protected void SetupLabels()
    {
        if (pnlNew.Visible)
        {
            switch (ucSendMessage.SendMessageMode)
            {
                case MessageActionEnum.Forward:
                    headNewMessage.ResourceString = "Messaging.Forward";
                    headOriginalMessage.ResourceString = "Messaging.OriginalMessage";
                    break;

                case MessageActionEnum.Reply:
                    headNewMessage.ResourceString = "Messaging.Reply";
                    headOriginalMessage.ResourceString = "Messaging.OriginalMessage";
                    break;
            }

            pnlActions.Visible = false;
        }

        ucViewMessage.StopProcessing = !pnlView.Visible;
    }

    #endregion


    #region "Breadcrumbs methods"

    /// <summary>
    ///  Initializes breadcrumbs items.
    /// </summary>
    private void InitializeBreadcrumbs()
    {
        if (ShowBreadcrumbs)
        {
            ucBreadcrumbs.HideBreadcrumbs = false;
            ucBreadcrumbs.Items.Clear();
            ucBreadcrumbs.AddBreadcrumb(new BreadcrumbItem
            {
                Text = GetString("Messaging.BackToList"),
                OnClientClick = ControlsHelper.GetPostBackEventReference(lnkBackHidden) + "; return false;"
            });

            ucBreadcrumbs.AddBreadcrumb(new BreadcrumbItem());
        }
        else
        {
            ucBreadcrumbs.HideBreadcrumbs = true;
            if (PageBreadcrumbs != null)
            {
                PageBreadcrumbs.Items.Clear();
                PageBreadcrumbs.AddBreadcrumb(new BreadcrumbItem
                {
                    Text = GetString("Messaging.BackToList"),
                    RedirectUrl = UIContextHelper.GetElementUrl("CMS.Messaging", "MyMessages.Inbox", false)
                });

                PageBreadcrumbs.AddBreadcrumb(new BreadcrumbItem());
            }
        }

        breadcrumbsInitialized = true;
    }


    /// <summary>
    /// Clear breadcrumbs items.
    /// </summary>
    private void ClearBreadcrumbs()
    {
        ucBreadcrumbs.Items.Clear();
        if (PageBreadcrumbs != null)
        {
            PageBreadcrumbs.Items.Clear();
        }

        breadcrumbsInitialized = false;
    }


    /// <summary>
    /// Updates breadcrumbs item that represents current state.
    /// </summary>
    /// <param name="itemText">Text to be displayed</param>
    private void UpdateBreadcrumbs(string itemText)
    {
        if (breadcrumbsInitialized)
        {
            if (ShowBreadcrumbs)
            {
                ucBreadcrumbs.Items[1].Text = itemText;
            }
            else
            {
                PageBreadcrumbs.Items[1].Text = itemText;
            }
        }
    }

    #endregion
}