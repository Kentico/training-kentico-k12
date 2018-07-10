using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using CMS.Helpers;
using CMS.Messaging;
using CMS.PortalEngine.Web.UI;
using CMS.Membership;

public partial class CMSWebParts_Messaging_MessagingInfoPanel : CMSAbstractWebPart
{
    #region "Variables and structures"

    /// <summary>
    /// Current user info.
    /// </summary>
    protected CurrentUserInfo currentUser = null;

    /// <summary>
    /// Structure for storing link parameters (URL, text, visibility).
    /// </summary>
    public struct MessageLink
    {
        /// <summary>
        /// URL of the link.
        /// </summary>
        public string Url;

        /// <summary>
        /// Text of the link.
        /// </summary>
        public string Text;

        /// <summary>
        /// Indicates if link should be displayed.
        /// </summary>
        public bool Visible;


        /// <summary>
        /// Message link structure.
        /// </summary>
        public MessageLink(string url, string text, bool visible)
        {
            Url = url;
            Text = text;
            Visible = visible;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Shows unread messages info.
    /// </summary>
    public bool ShowUnreadMessagesCount
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowUnreadMessagesCount"), false);
        }
        set
        {
            SetValue("ShowUnreadMessagesCount", value);
        }
    }


    /// <summary>
    /// Shows inbox messages info.
    /// </summary>
    public bool ShowInbox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowInbox"), false);
        }
        set
        {
            SetValue("ShowInbox", value);
        }
    }


    /// <summary>
    /// Gets or sets the inbox link URL.
    /// </summary>
    public string InboxLinkUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("InboxLinkUrl"), "");
        }
        set
        {
            SetValue("InboxLinkUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets the inbox label.
    /// </summary>
    public string InboxLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("InboxLabel"), GetString("Messaging.Inbox"));
        }
        set
        {
            SetValue("InboxLabel", value);
        }
    }


    /// <summary>
    /// Shows outbox messages info.
    /// </summary>
    public bool ShowOutbox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOutbox"), false);
        }
        set
        {
            SetValue("ShowOutbox", value);
        }
    }


    /// <summary>
    /// Gets or sets the outbox link URL.
    /// </summary>
    public string OutboxLinkUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OutboxLinkUrl"), "");
        }
        set
        {
            SetValue("OutboxLinkUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets the outbox label.
    /// </summary>
    public string OutboxLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("OutboxLabel"), GetString("Messaging.Outbox"));
        }
        set
        {
            SetValue("OutboxLabel", value);
        }
    }


    /// <summary>
    /// Shows new messages info.
    /// </summary>
    public bool ShowNewMessage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowNewMessage"), false);
        }
        set
        {
            SetValue("ShowNewMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the new message link URL.
    /// </summary>
    public string NewMessageLinkUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NewMessageLinkUrl"), "");
        }
        set
        {
            SetValue("NewMessageLinkUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets the new message label.
    /// </summary>
    public string NewMessageLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("NewMessageLabel"), GetString("Messaging.NewMessage"));
        }
        set
        {
            SetValue("NewMessageLabel", value);
        }
    }


    /// <summary>
    /// Gets or sets separator used between link.
    /// </summary>
    public string LinkSeparator
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LinkSeparator"), "");
        }
        set
        {
            SetValue("LinkSeparator", value);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reload date override.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            Reload();
        }
    }


    /// <summary>
    /// Inits webpart values.
    /// </summary>
    public void Reload()
    {
        currentUser = MembershipContext.AuthenticatedUser;

        // Store link parameters to list
        List<MessageLink> messageLinkList = new List<MessageLink>();
        messageLinkList.Add(new MessageLink(InboxLinkUrl, InboxLabel, ShowInbox));
        messageLinkList.Add(new MessageLink(OutboxLinkUrl, OutboxLabel, ShowOutbox));
        messageLinkList.Add(new MessageLink(NewMessageLinkUrl, NewMessageLabel, ShowNewMessage));

        bool firstLink = true;

        // Loop throught list
        foreach (MessageLink ml in messageLinkList)
        {
            // Create link (link must be visible and without empty URL)
            if (ml.Visible && !(String.IsNullOrEmpty(ml.Url)))
            {
                HyperLink hl = new HyperLink();
                hl.Text = ml.Text;
                hl.NavigateUrl = ml.Url;
                hl.EnableViewState = false;

                // Insert separator
                if (!firstLink)
                {
                    Literal ltl = new Literal();
                    ltl.Text = LinkSeparator;
                    plcLinks.Controls.Add(ltl);
                }
                plcLinks.Controls.Add(hl);

                int messageCount = GetCount();

                // Display number of unread messages
                if (ShowUnreadMessagesCount && (ml.Url == InboxLinkUrl))
                {
                    // Use string format
                    if (ml.Text.Contains("{0}"))
                    {
                        hl.Text = String.Format(ml.Text, messageCount);
                    }
                    else if (messageCount > 0)
                    {
                        hl.Text += " ";
                        HyperLink count = new HyperLink();
                        count.EnableViewState = false;
                        count.NavigateUrl = ml.Url;
                        count.Text = "(" + messageCount + ")";
                        count.CssClass = "messagesCount";
                        plcLinks.Controls.Add(count);
                    }
                }

                firstLink = false;
            }
        }
    }


    /// <summary>
    /// Gets messages count according to caching properties.
    /// </summary>
    /// <returns>Number of new messages in the inbox</returns>
    public int GetCount()
    {
        return MessageInfoProvider.GetUnreadMessagesCount(currentUser.UserID);
    }

    #endregion
}