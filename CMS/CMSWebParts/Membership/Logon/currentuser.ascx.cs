using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Membership_Logon_currentuser : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the text of label which is displayed in front of user info text.
    /// </summary>
    public string LabelText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("LabelText"), GetString("Webparts_Membership_CurrentUser.CurrentUser"));
        }
        set
        {
            SetValue("LabelText", value);
            lblLabel.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether this webpart is displayed only when user is authenticated.
    /// </summary>
    public bool ShowOnlyWhenAuthenticated
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOnlyWhenAuthenticated"), true);
        }
        set
        {
            SetValue("ShowOnlyWhenAuthenticated", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether label text is displayed.
    /// </summary>
    public bool ShowLabelText
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowLabelText"), true);
        }
        set
        {
            SetValue("ShowLabelText", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether user full name is displayed.
    /// </summary>
    public bool ShowUserFullName
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowUserFullName"), true);
        }
        set
        {
            SetValue("ShowUserFullName", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether use name is displayed.
    /// </summary>
    public bool ShowUserName
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowUserName"), true);
        }
        set
        {
            SetValue("ShowUserName", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the username should be hidden for external users.
    /// </summary>
    public bool HideUserNameForExternalUsers
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideUserNameForExternalUsers"), false);
        }
        set
        {
            SetValue("HideUserNameForExternalUsers", value);
        }
    }


    /// <summary>
    /// Gets or sets url used for authenticated user.
    /// </summary>
    public string AuthenticatedLinkUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AuthenticatedLinkUrl"), "");
        }
        set
        {
            SetValue("AuthenticatedLinkUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets url used for public user.
    /// </summary>
    public string PublicLinkUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PublicLinkUrl"), "");
        }
        set
        {
            SetValue("PublicLinkUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets the name of css for label which is displayed in front of user info text.
    /// </summary>
    public string LabelCSS
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LabelCSS"), lblLabel.CssClass);
        }
        set
        {
            SetValue("LabelCSS", value);
            lblLabel.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the name of css which is used for user info label.
    /// </summary>
    public string UserCSS
    {
        get
        {
            return ValidationHelper.GetString(GetValue("UserCSS"), "CurrentUserName");
        }
        set
        {
            SetValue("UserCSS", value);
        }
    }

    #endregion


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do not process
        }
        else
        {
            EnableViewState = false;
            // Set labels text
            lblLabel.Text = LabelText;
            lblLabel.Visible = ShowLabelText;

            // According to visibility writeout fullname and username
            string userInfo = "<span class=\"" + UserCSS + "\">";

            // Display full name
            if (ShowUserFullName)
            {
                userInfo += HTMLHelper.HTMLEncode(MembershipContext.AuthenticatedUser.FullName);

                // Display user name
                if (ShowUserName && !(MembershipContext.AuthenticatedUser.IsExternal && HideUserNameForExternalUsers))
                {
                    userInfo += " (" + HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(MembershipContext.AuthenticatedUser.UserName, true)) + ") ";
                }
            }
            // Display user name
            else if (ShowUserName && !(MembershipContext.AuthenticatedUser.IsExternal && HideUserNameForExternalUsers))
            {
                userInfo += HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(MembershipContext.AuthenticatedUser.UserName, true));
            }

            userInfo += "</span>";

            // Set sign in or sign out url to the username link or hide it
            if (!MembershipContext.AuthenticatedUser.IsPublic())
            {
                if (AuthenticatedLinkUrl != String.Empty)
                {
                    userInfo = "<a href=\"" + AuthenticatedLinkUrl + "\">" + userInfo + "</a>";
                }
            }
            else
            {
                if (PublicLinkUrl != String.Empty)
                {
                    userInfo = "<a href=\"" + PublicLinkUrl + "\">" + userInfo + "</a>";
                }
            }

            ltrSignLink.Text = userInfo;

            // Set label CSS class
            lblLabel.CssClass = LabelCSS;
            lblLabel.Text = LabelText;
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Visible = true;
    }


    /// <summary>
    /// OnPreRender override.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SetupControl();
        // Set visibility with according to property setting and current user status
        Visible = (!ShowOnlyWhenAuthenticated || !MembershipContext.AuthenticatedUser.IsPublic());
    }
}