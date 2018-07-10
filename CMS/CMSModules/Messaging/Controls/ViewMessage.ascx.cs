using System;
using System.Web.UI;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Globalization;
using CMS.Globalization.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Messaging;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Controls_ViewMessage : CMSUserControl
{
    #region "Variables"

    private MessageModeEnum mMessageMode = MessageModeEnum.Inbox;
    protected MessageInfo mMessage = null;
    protected UserInfo currentUserInfo = null;
    protected UserInfo messageUserInfo = null;
    private CMSModules_Messaging_Controls_MessageUserButtons mMessageUserButtonsControl;
    private CMSAdminControls_UI_UserPicture mUserPictureControl;

    #endregion


    #region "Private properties"

    /// <summary>
    /// Gets Message user buttons control
    /// </summary>
    private CMSModules_Messaging_Controls_MessageUserButtons MessageUserButtonsControl
    {
        get
        {
            if (mMessageUserButtonsControl == null)
            {
                mMessageUserButtonsControl = (CMSModules_Messaging_Controls_MessageUserButtons)LoadControl("~/CMSModules/Messaging/Controls/MessageUserButtons.ascx");
            }
            return mMessageUserButtonsControl;
        }
    }


    /// <summary>
    /// Gets User picture control
    /// </summary>
    private CMSAdminControls_UI_UserPicture UserPictureControl
    {
        get
        {
            if (mUserPictureControl == null)
            {
                mUserPictureControl = (CMSAdminControls_UI_UserPicture)LoadControl("~/CMSAdminControls/UI/UserPicture.ascx");
                mUserPictureControl.Height = 60;
                mUserPictureControl.Width = 60;
                mUserPictureControl.KeepAspectRatio = true;
                mUserPictureControl.UseDefaultAvatar = true;
            }
            return mUserPictureControl;
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
            base.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Message ID.
    /// </summary>
    public int MessageId
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["MessageId"], 0);
        }
        set
        {
            ViewState["MessageId"] = value;
            if (value <= 0)
            {
                mMessage = null;
            }
        }
    }


    /// <summary>
    /// Message.
    /// </summary>
    public MessageInfo Message
    {
        get
        {
            if ((mMessage == null) && (MessageId > 0))
            {
                mMessage = MessageInfoProvider.GetMessageInfo(MessageId);
            }
            return mMessage;
        }
        set
        {
            mMessage = value;
            if (mMessage != null)
            {
                ViewState["MessageId"] = mMessage.MessageID;
            }
        }
    }


    /// <summary>
    /// Message mode.
    /// </summary>
    public MessageModeEnum MessageMode
    {
        get
        {
            return mMessageMode;
        }
        set
        {
            mMessageMode = value;
        }
    }


    /// <summary>
    /// Information text.
    /// </summary>
    public string InformationText
    {
        get
        {
            return MessageUserButtonsControl.InformationText;
        }
    }


    /// <summary>
    /// Error text.
    /// </summary>
    public string ErrorText
    {
        get
        {
            return MessageUserButtonsControl.ErrorText;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Page init.
    /// </summary>
    protected void Page_Init(object sender, EventArgs e)
    {
        plcUserPicture.Controls.Add(UserPictureControl);
        plcMessageUserButtons.Controls.Add(MessageUserButtonsControl);
    }


    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Message id is set, display message details
        if (Message != null)
        {
            // Find postback invoker
            string invokerName = Page.Request.Params.Get(Page.postEventSourceID);
            // If postback was caused by user buttons
            if (invokerName.Contains(MessageUserButtonsControl.UniqueID))
            {
                StopProcessing = false;
            }
            ReloadData();
        }
    }

    #endregion


    #region "Other methods"

    public void ReloadData()
    {
        if (StopProcessing)
        {
            // Do nothing
            MessageUserButtonsControl.StopProcessing = true;
            UserPictureControl.StopProcessing = true;
        }
        else
        {
            MessageUserButtonsControl.StopProcessing = false;
            UserPictureControl.StopProcessing = false;

            if (Message != null)
            {
                // Get current user info
                currentUserInfo = MembershipContext.AuthenticatedUser;
                // Get message user info
                if (MessageMode == MessageModeEnum.Inbox)
                {
                    messageUserInfo = UserInfoProvider.GetUserInfo(Message.MessageSenderUserID);
                }
                else
                {
                    messageUserInfo = UserInfoProvider.GetUserInfo(Message.MessageRecipientUserID);
                }

                // Display only to authorized user
                if ((currentUserInfo.UserID == Message.MessageRecipientUserID) 
                    || (currentUserInfo.UserID == Message.MessageSenderUserID) 
                    || currentUserInfo.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
                {
                    pnlViewMessage.Visible = true;
                    lblDateCaption.Text = GetString("Messaging.Date");
                    lblSubjectCaption.Text = GetString("general.subject");
                    lblFromCaption.Text = (MessageMode == MessageModeEnum.Inbox) ? GetString("Messaging.From") : GetString("Messaging.To");

                    // Sender exists
                    if (messageUserInfo != null)
                    {
                        UserPictureControl.Visible = true;
                        UserPictureControl.UserID = messageUserInfo.UserID;

                        // Gravatar support
                        string avType = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSAvatarType");
                        if (avType == AvatarInfoProvider.USERCHOICE)
                        {
                           avType = messageUserInfo.UserSettings.UserAvatarType;
                        }

                        UserPictureControl.UserAvatarType = avType;
                        
                        // Disable message user buttons on live site for hidden or disabled users
                        if (IsLiveSite && !currentUserInfo.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin) && (messageUserInfo.UserIsDisabledManually || messageUserInfo.UserIsHidden))
                        {
                            MessageUserButtonsControl.RelatedUserId = 0;
                        }
                        else
                        {
                            MessageUserButtonsControl.RelatedUserId = messageUserInfo.UserID;
                        }
                        lblFrom.Text = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(messageUserInfo.UserName, messageUserInfo.FullName, messageUserInfo.UserNickName, IsLiveSite));
                    }
                    else
                    {
                        MessageUserButtonsControl.RelatedUserId = 0;
                        lblFrom.Text = HTMLHelper.HTMLEncode(Message.MessageSenderNickName);
                    }
                    string body = Message.MessageBody;
                    // Resolve macros
                    DiscussionMacroResolver dmh = new DiscussionMacroResolver();
                    body = dmh.ResolveMacros(body);

                    lblSubject.Text = HTMLHelper.HTMLEncodeLineBreaks(Message.MessageSubject);
                    if (IsLiveSite)
                    {
                        lblDate.Text = TimeZoneUIMethods.ConvertDateTime(Message.MessageSent, this).ToString();
                    }
                    else
                    {
                        lblDate.Text = TimeZoneHelper.ConvertToUserTimeZone(Message.MessageSent, true, currentUserInfo, SiteContext.CurrentSite);
                    }
                    lblBody.Text = body;
                }
            }
            else
            {
                ShowError(GetString("Messaging.MessageDoesntExist"));
            }
        }
    }

    #endregion
}