using System;
using System.Drawing;

using CMS.Community;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.Messaging;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.DocumentEngine;
using CMS.MacroEngine;

public partial class CMSWebParts_Community_Friends_FriendshipManagement : CMSAbstractWebPart
{
    #region "Variables"

    protected FriendInfo friendship;
    protected UserInfo friend;
    protected FriendsActionEnum action = FriendsActionEnum.Request;
    protected CurrentUserInfo currentUser = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether to send notification message.
    /// </summary>
    public bool SendNotificationMessage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SendNotificationMessage"), false);
        }
        set
        {
            SetValue("SendNotificationMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether to send notification e-mail.
    /// </summary>
    public bool SendNotificationEmail
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SendNotificationEmail"), false);
        }
        set
        {
            SetValue("SendNotificationEmail", value);
        }
    }


    /// <summary>
    /// Gets or sets the value of already approved friendship text.
    /// </summary>
    public string AlreadyApprovedCaption
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("AlreadyApprovedCaption"), GetString("friends.friendshipalreadyapproved"));
        }
        set
        {
            SetValue("AlreadyApprovedCaption", value);
        }
    }


    /// <summary>
    /// Gets or sets the value of approved friendship text.
    /// </summary>
    public string ApprovedCaption
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ApprovedCaption"), GetString("friends.friendshipapproved"));
        }
        set
        {
            SetValue("ApprovedCaption", value);
        }
    }


    /// <summary>
    /// Gets or sets the value of already rejected friendship text.
    /// </summary>
    public string AlreadyRejectedCaption
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("AlreadyRejectedCaption"), GetString("friends.friendshipisrejected"));
        }
        set
        {
            SetValue("AlreadyRejectedCaption", value);
        }
    }


    /// <summary>
    /// Gets or sets the value of rejected friendship text.
    /// </summary>
    public string RejectedCaption
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("RejectedCaption"), GetString("friends.friendshiprejected"));
        }
        set
        {
            SetValue("RejectedCaption", value);
        }
    }


    /// <summary>
    /// Gets or sets the path of the My friends page.
    /// </summary>
    public string MyFriendsPath
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("MyFriendsPath"), string.Empty);
        }
        set
        {
            SetValue("MyFriendsPath", value);
        }
    }


    /// <summary>
    /// Gets or sets the value of My friends link text.
    /// </summary>
    public string MyFriendsCaption
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("MyFriendsCaption"), GetString("friends.myfriendslink"));
        }
        set
        {
            SetValue("MyFriendsCaption", value);
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
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
            if (AuthenticationHelper.IsAuthenticated())
            {
                // Get requested action
                action = (FriendsActionEnum)Enum.Parse(typeof(FriendsActionEnum), QueryHelper.GetString("action", "request"), true);
                friendship = CommunityContext.CurrentFriendship;
                friend = CommunityContext.CurrentFriend;

                // Prepare My Friends link
                lnkMyFriends.Text = MyFriendsCaption;
                if (MyFriendsPath != string.Empty)
                {
                    lnkMyFriends.NavigateUrl = URLHelper.GetAbsoluteUrl(DocumentURLProvider.GetUrl(MyFriendsPath));
                }
                else
                {
                    lnkMyFriends.Visible = false;
                }

                // Validate requested action
                if (!ValidateAction())
                {
                    return;
                }

                btnApprove.Click += btnApprove_Click;
                btnReject.Click += btnReject_Click;

                if (friendship != null)
                {
                    // If friendship is rejected -> display error
                    switch (friendship.FriendStatus)
                    {
                        case FriendshipStatusEnum.Rejected:
                            plcMessage.Visible = true;
                            plcConfirm.Visible = false;
                            lblInfo.Text = AlreadyRejectedCaption;
                            lblInfo.ForeColor = Color.Red;
                            break;

                        case FriendshipStatusEnum.Approved:
                            plcMessage.Visible = true;
                            plcConfirm.Visible = false;
                            lblInfo.Text = AlreadyApprovedCaption;
                            lblInfo.ForeColor = Color.Red;
                            break;
                        default:
                            plcMessage.Visible = false;
                            plcConfirm.Visible = true;
                            btnApprove.Text = GetString("general.approve");
                            btnReject.Text = GetString("general.reject");

                            string profilePath = GroupMemberInfoProvider.GetMemberProfilePath(friend.UserName, SiteContext.CurrentSiteName);
                            string profileUrl = ResolveUrl(DocumentURLProvider.GetUrl(profilePath));
                            string link = "<a href=\"" + profileUrl + "\" >" + HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(friend.UserName, friend.FullName, true)) + "</a>";
                            lblConfirm.Text = string.Format(GetString("friends.approvaltext"), link);
                            break;
                    }
                }
                else
                {
                    Visible = false;
                }
            }
            else
            {
                plcMessage.Visible = true;
                plcConfirm.Visible = false;
                lblInfo.ForeColor = Color.Red;
                lblInfo.Text = GetString("friends.notloggedin");
            }
        }
    }


    protected void btnReject_Click(object sender, EventArgs e)
    {
        lblInfo.Text = RejectedCaption;
        friendship.FriendStatus = FriendshipStatusEnum.Rejected;
        friendship.FriendRejectedBy = friendship.FriendRequestedUserID;
        friendship.FriendRejectedWhen = DateTime.Now;
        FriendInfoProvider.SetFriendInfo(friendship);

        plcConfirm.Visible = false;
        plcMessage.Visible = true;

        action = FriendsActionEnum.Reject;

        // Send notification
        SentNotification();
    }


    protected void btnApprove_Click(object sender, EventArgs e)
    {
        lblInfo.Text = ApprovedCaption;
        friendship.FriendStatus = FriendshipStatusEnum.Approved;
        friendship.FriendApprovedBy = friendship.FriendRequestedUserID;
        friendship.FriendApprovedWhen = DateTime.Now;
        FriendInfoProvider.SetFriendInfo(friendship);

        plcConfirm.Visible = false;
        plcMessage.Visible = true;

        action = FriendsActionEnum.Approve;

        // Send notification
        SentNotification();
    }


    /// <summary>
    /// Send notifications.
    /// </summary>
    private void SentNotification()
    {
        if (SendNotificationMessage || SendNotificationEmail)
        {
            // Get e-mail template
            EmailTemplateInfo template = null;
            // Get message subject
            string messageSubject = null;

            switch (action)
            {
                case FriendsActionEnum.Approve:
                    template = EmailTemplateProvider.GetEmailTemplate("Friends.Approve",
                                                                      SiteContext.CurrentSiteName);
                    messageSubject = ApprovedCaption;
                    break;

                case FriendsActionEnum.Reject:
                    template = EmailTemplateProvider.GetEmailTemplate("Friends.Reject",
                                                                      SiteContext.CurrentSiteName);
                    messageSubject = RejectedCaption;
                    break;
            }
            if (template == null)
            {
                return;
            }

            // Get user infos
            UserInfo recipient = UserInfoProvider.GetFullUserInfo(friendship.FriendUserID);
            UserInfo sender = UserInfoProvider.GetFullUserInfo(friendship.FriendRequestedUserID);

            MacroResolver resolver = MacroContext.CurrentResolver;
            resolver.SetAnonymousSourceData(sender, recipient, friendship);
            resolver.SetNamedSourceData("Sender", sender);
            resolver.SetNamedSourceData("Recipient", recipient);
            resolver.SetNamedSourceData("Friendship", friendship);
            resolver.SetNamedSourceData("FORMATTEDSENDERNAME", Functions.GetFormattedUserName(sender.UserName), false);

            if (SendNotificationMessage)
            {
                // Set message info object
                MessageInfo mi = new MessageInfo();
                mi.MessageLastModified = DateTime.Now;
                mi.MessageSent = DateTime.Now;
                mi.MessageRecipientUserID = recipient.UserID;
                mi.MessageRecipientNickName = TextHelper.LimitLength(Functions.GetFormattedUserName(recipient.UserName, recipient.FullName, recipient.UserNickName, true), 200);
                mi.MessageSenderUserID = friendship.FriendRequestedUserID;
                mi.MessageSenderNickName = TextHelper.LimitLength(Functions.GetFormattedUserName(sender.UserName, sender.FullName, sender.UserNickName, true), 200);
                mi.MessageSenderDeleted = true;
                mi.MessageSubject = TextHelper.LimitLength(resolver.ResolveMacros(template.TemplateSubject), 200);
                mi.MessageBody = resolver.ResolveMacros(template.TemplatePlainText);
                MessageInfoProvider.SetMessageInfo(mi);
            }
            if (SendNotificationEmail && !String.IsNullOrEmpty(recipient.Email) &&
                !String.IsNullOrEmpty(sender.Email))
            {
                // Send e-mail
                EmailMessage message = new EmailMessage();
                message.EmailFormat = EmailFormatEnum.Default;
                message.Recipients = Functions.GetFormattedUserName(recipient.UserName, true) + " <" + recipient.Email + ">";
                message.From = Functions.GetFormattedUserName(sender.UserName, true) + " <" + sender.Email + ">";
                message.Subject = messageSubject;

                EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, message, template, resolver, false);
            }
        }
    }


    /// <summary>
    /// Validate requested action.
    /// </summary>
    private bool ValidateAction()
    {
        if ((friendship != null) && (MembershipContext.AuthenticatedUser.UserID != friendship.FriendRequestedUserID))
        {
            plcMessage.Visible = true;
            plcConfirm.Visible = false;
            lblInfo.ForeColor = Color.Red;
            lblInfo.Text = GetString("friends.notauthorized");
            return false;
        }

        return true;
    }
}