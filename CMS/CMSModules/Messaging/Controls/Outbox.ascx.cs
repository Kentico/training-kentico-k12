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


public partial class CMSModules_Messaging_Controls_Outbox : CMSUserControl
{
    #region "Variables & enums"

    private string mZeroRowsText;
    private int mPageSize = 10;
    private bool mShowOriginalMessage = true;
    private bool mMarkReadMessage;
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
        Delete = 1
    }

    #endregion


    #region "Private properties"

    /// <summary>
    /// Current message id.
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
        }
    }


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
            plcMess.IsLiveSite = value;
            ucSendMessage.IsLiveSite = value;
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
            outboxGrid.ZeroRowsText = HTMLHelper.HTMLEncode(mZeroRowsText);
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
            return outboxGrid;
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
            outboxGrid.PageSize = value.ToString();
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


    /// <summary>
    /// Mark read messages.
    /// </summary>
    public bool MarkReadMessage
    {
        get
        {
            return mMarkReadMessage;
        }
        set
        {
            mMarkReadMessage = value;
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

    #endregion


    #region "Page events"

    protected override void EnsureChildControls()
    {
        base.EnsureChildControls();
        if (outboxGrid == null)
        {
            pnlOutbox.LoadContainer();
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
            outboxGrid.Pager.FirstPageText = "&lt;&lt;";
            outboxGrid.Pager.LastPageText = "&gt;&gt;";
            outboxGrid.Pager.PreviousPageText = "&lt;";
            outboxGrid.Pager.NextPageText = "&gt;";
            outboxGrid.Pager.PreviousGroupText = "...";
            outboxGrid.Pager.NextGroupText = "...";
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
        pnlAction.Visible = outboxGrid.Visible && (outboxGrid.GridView.Rows.Count > 0);

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
            outboxGrid.StopProcessing = true;
        }
        else
        {
            bool isPostBack = RequestHelper.IsPostBack();

            if (!isPostBack)
            {
                outboxGrid.DelayedReload = false;
            }
            else
            {
                // Find postback invoker
                if (!ControlsHelper.CausedPostBack(outboxGrid, btnNewMessage, ucSendMessage.SendButton, ucSendMessage.CancelButton, btnForward, btnDelete, btnOk, lnkBackHidden))
                {
                    outboxGrid.DelayedReload = false;
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
                outboxGrid.OnExternalDataBound += outboxGrid_OnExternalDataBound;
                outboxGrid.GridView.RowDataBound += GridView_RowDataBound;
                outboxGrid.OnAction += outboxGrid_OnAction;
                // Set where condition clause
                outboxGrid.WhereCondition = "MessageSenderUserID=" + MembershipContext.AuthenticatedUser.UserID + " AND (MessageSenderDeleted=0 OR MessageSenderDeleted IS NULL)";
                outboxGrid.GridView.DataBound += GridView_DataBound;
                outboxGrid.OnBeforeDataReload += outboxGrid_OnBeforeDataReload;
                outboxGrid.OnBeforeSorting += outboxGrid_OnBeforeSorting;
                outboxGrid.OnShowButtonClick += outboxGrid_OnShowButtonClick;
                outboxGrid.OnPageSizeChanged += outboxGrid_OnPageSizeChanged;
                outboxGrid.IsLiveSite = IsLiveSite;
                // Setup inner controls
                ucSendMessage.IsLiveSite = IsLiveSite;
                ucViewMessage.IsLiveSite = IsLiveSite;
                ucViewMessage.MessageMode = MessageModeEnum.Outbox;
                ucViewMessage.Message = Message;

                // Create and assign javascripts confirmation
                btnDelete.OnClientClick = "return confirm(" + ScriptHelper.GetString(GetString("Messsaging.DeletionConfirmation")) + ");";

                // Register events
                ucSendMessage.SendButtonClick += ucSendMessage_SendButtonClick;
                btnNewMessage.Click += btnNewMessage_Click;
                btnForward.Click += btnForward_Click;
                btnDelete.Click += btnDelete_Click;
                lnkBackHidden.Click += lnkBackHidden_Click;

                StringBuilder actionScript = new StringBuilder();
                actionScript.Append(
                    @"function PerformAction_", ClientID, @"(selectionFunction, actionId, actionLabel, whatId) {
    var confirmation = null;
    var label = document.getElementById(actionLabel);
    var action = document.getElementById(actionId).value;
    var whatDrp = document.getElementById(whatId);
    if (action == '", (int)Action.SelectAction, @"') {
        label.innerHTML = ", ScriptHelper.GetString(GetString("MassAction.SelectSomeAction")), @";
    }
    else if (eval(selectionFunction) && (whatDrp.value == '", (int)What.SelectedMessages, @"')) {
        label.innerHTML = ", ScriptHelper.GetString(GetString("Messaging.SelectMessages")), @";
    }
    else if (action == '", (int)Action.Delete, @"') {
        confirmation = ", ScriptHelper.GetString(GetString("Messaging.ConfirmDelete")), @";
    }
    if (confirmation != null) {
        return confirm(confirmation);
    }
    return false;
}
");

                ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "actionScript", ScriptHelper.GetScript(actionScript.ToString()));

                // Add action to button
                btnOk.OnClientClick = "return PerformAction_" + ClientID + "('" + outboxGrid.GetCheckSelectionScript() + "','" + drpAction.ClientID + "','" + lblActionInfo.ClientID + "','" + drpWhat.ClientID + "');";

                btnOk.Click += btnOk_Click;

                if (!isPostBack)
                {
                    // Initialize dropdown lists
                    drpWhat.Items.Add(new ListItem(GetString("Messaging." + What.SelectedMessages), Convert.ToInt32(What.SelectedMessages).ToString()));
                    drpWhat.Items.Add(new ListItem(GetString("Messaging." + What.AllMessages), Convert.ToInt32(What.AllMessages).ToString()));

                    drpAction.Items.Add(new ListItem(GetString("general." + Action.SelectAction), Convert.ToInt32(Action.SelectAction).ToString()));
                    drpAction.Items.Add(new ListItem(GetString("Messaging.Action." + Action.Delete), Convert.ToInt32(Action.Delete).ToString()));
                }

                if (outboxGrid.PagerConfig == null)
                {
                    outboxGrid.PagerConfig = new UniGridPagerConfig();
                }
                outboxGrid.PagerConfig.DefaultPageSize = PageSize;
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
        UpdateBreadcrumbs(GetString("messaging.newmessage"));

        // Initialize new message control
        pnlBackToList.Visible = true;
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
    /// View message.
    /// </summary>
    private void ViewMessage()
    {
        UpdateBreadcrumbs(GetString("messaging.viewmessage"));

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
    /// Forward message.
    /// </summary>
    private void ForwardMesssage()
    {
        UpdateBreadcrumbs(GetString("messaging.forwardmessage"));

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
    /// Delete message.
    /// </summary>
    private void DeleteMessage()
    {
        try
        {
            MessageInfoProvider.DeleteSentMessage(CurrentMessageId);
            ShowConfirmation(GetString("Messsaging.MessageDeleted"));
            pnlList.Visible = true;
            pnlNew.Visible = false;
            pnlView.Visible = false;
            pnlBackToList.Visible = false;
            outboxGrid.DelayedReload = false;
            MessageMode = null;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }


    /// <summary>
    /// Perform selected action (Delete).
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
            where = SqlHelper.GetWhereCondition<int>("MessageID", outboxGrid.SelectedItems, false);
            resultMessage = GetString("Messaging." + What.SelectedMessages);
        }
        else
        {
            return;
        }

        // Action 'Delete'
        if ((action == Action.Delete))
        {
            // Delete selected messages
            MessageInfoProvider.DeleteSentMessages(MembershipContext.AuthenticatedUser.UserID, where);

            resultMessage += " " + GetString("Messaging.Action.Result.Deleted");

            ShowConfirmation(resultMessage);

            outboxGrid.ResetSelection();
            outboxGrid.ReloadData();
        }
    }

    #endregion


    #region "Buttons actions"

    protected void outboxGrid_OnShowButtonClick(object sender, EventArgs e)
    {
        outboxGrid.DelayedReload = false;
    }


    protected void lnkBackHidden_Click(object sender, EventArgs e)
    {
        pnlList.Visible = true;
        pnlNew.Visible = false;
        pnlView.Visible = false;
        pnlBackToList.Visible = false;
        ucSendMessage.SendMessageMode = MessageActionEnum.None;
        outboxGrid.ReloadData();
        ClearBreadcrumbs();
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteMessage();
        outboxGrid.ReloadData();
    }


    protected void btnForward_Click(object sender, EventArgs e)
    {
        ForwardMesssage();
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
            MessageMode = null;
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
            // Mark read message
            if (MarkReadMessage)
            {
                DateTime messageRead = DataHelper.GetDateTimeValue(rowView.Row, "MessageRead", DateTimeHelper.ZERO_TIME);
                if (messageRead == DateTimeHelper.ZERO_TIME)
                {
                    e.Row.CssClass += " Unread";
                }
            }
        }
    }


    protected void outboxGrid_OnBeforeDataReload()
    {
        // Bind footer
        BindFooter();
    }


    protected void outboxGrid_OnPageSizeChanged()
    {
        outboxGrid.ReloadData();
    }


    protected void outboxGrid_OnBeforeSorting(object sender, EventArgs e)
    {
        outboxGrid.ReloadData();
    }


    protected object outboxGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "messagerecipientnickname":
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

            case "messageread":
                // Get date read
                return GetDateRead(parameter);
        }
        return parameter;
    }


    protected void GridView_DataBound(object sender, EventArgs e)
    {
        // Setup column styles
        if (!MarkReadMessage)
        {
            outboxGrid.GridView.Columns[4].Visible = false;
        }
    }


    protected void outboxGrid_OnAction(string actionName, object actionArgument)
    {
        CurrentMessageId = ValidationHelper.GetInteger(actionArgument, 0);

        // Force reinitialization of message object if differs from currently processed message
        if ((Message != null) && (Message.MessageID != CurrentMessageId))
        {
            mMessage = null;
        }

        // Check permissions - user has to be sender
        if ((Message == null) || (Message.MessageSenderUserID != MembershipContext.AuthenticatedUser.UserID))
        {
            return;
        }

        switch (actionName)
        {
            // Delete message
            case "delete":
                DeleteMessage();
                break;

            // View message
            case "view":
                ViewMessage();
                break;

            // Forward message
            case "forward":
                ForwardMesssage();
                break;
        }
    }

    #endregion


    #region "Protected methods"

    /// <summary>
    /// Bind the data.
    /// </summary>
    private void BindFooter()
    {
        ShowInformation(string.Format(GetString("Messaging.NumOfMessages"), MessageInfoProvider.GetSentMessagesCount(MembershipContext.AuthenticatedUser.UserID)));
    }


    protected string GetDateRead(object messageReadDate)
    {
        DateTime messageRead = ValidationHelper.GetDateTime(messageReadDate, DateTimeHelper.ZERO_TIME);

        if (currentUserInfo == null)
        {
            currentUserInfo = MembershipContext.AuthenticatedUser;
        }
        if (currentSiteInfo == null)
        {
            currentSiteInfo = SiteContext.CurrentSite;
        }
        DateTime currentDateTime = ValidationHelper.GetDateTime(messageReadDate, DateTimeHelper.ZERO_TIME);

        if (messageRead != DateTimeHelper.ZERO_TIME)
        {
            if (IsLiveSite)
            {
                return TimeZoneUIMethods.ConvertDateTime(currentDateTime, this).ToString();
            }

            return TimeZoneHelper.ConvertToUserTimeZone(currentDateTime, true, currentUserInfo, currentSiteInfo);
        }

        return GetString("Messaging.OutboxMessageUnread");
    }


    /// <summary>
    /// Set label values.
    /// </summary>
    protected void SetupLabels()
    {
        if (pnlNew.Visible)
        {
            if (ucSendMessage.SendMessageMode == MessageActionEnum.Forward)
            {
                headNewMessage.ResourceString = "Messaging.Forward";
                headOriginalMessage.ResourceString = "Messaging.OriginalMessage";
            }

            pnlActions.Visible = false;
        }

        ucViewMessage.StopProcessing = !pnlView.Visible;
    }

    #endregion


    #region "Breadcrumbs methods"

    /// <summary>
    /// Initializes breadcrumbs items.
    /// </summary>
    private void InitializeBreadcrumbs()
    {
        if (!ShowBreadcrumbs)
        {
            ucBreadcrumbs.HideBreadcrumbs = true;
            if (PageBreadcrumbs != null)
            {
                PageBreadcrumbs.Items.Clear();
                PageBreadcrumbs.AddBreadcrumb(new BreadcrumbItem
                {
                    Text = GetString("Messaging.BackToList"),
                    RedirectUrl = UIContextHelper.GetElementUrl("CMS.Messaging", "MyMessages.Outbox", false)
                });

                PageBreadcrumbs.AddBreadcrumb(new BreadcrumbItem());
            }
        }
        else
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