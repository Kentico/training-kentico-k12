using System;

using CMS.ExternalAuthentication.Facebook;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.UIControls;


/// <summary>
/// Displays a mapping between CMS user and Facebook user.
/// </summary>
public partial class CMSModules_Membership_Controls_Facebook_Mapping : CMSUserControl
{

    #region "Private members"

    /// <summary>
    /// The value indicating whether the control is enabled.
    /// </summary>
    private bool mEnabled = true;

    /// <summary>
    /// The mapping between a Facebook user profile and a CMS user.
    /// </summary>
    private EntityMapping mMapping;
    
    
    /// <summary>
    /// The CMS user form info suitable for mapping.
    /// </summary>
    private FormInfo mUserFormInfo;
    
    
    /// <summary>
    /// The CMS user settings form info suitable fo mapping.
    /// </summary>
    private FormInfo mUserSettingsFormInfo;
    
    
    /// <summary>
    /// The entity model of Facebook user profile.
    /// </summary>
    private EntityModel mUserProfileModel;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the value indicating whether the control is enabled.
    /// </summary>
    public bool Enabled
    {
        get
        {
            return mEnabled;
        }
        set
        {
            mEnabled = value;
        }
    }

    /// <summary>
    /// Gets or sets the mapping between a Facebook user profile and a CMS user.
    /// </summary>
    public EntityMapping Mapping
    {
        get
        {
            return mMapping;
        }
        set
        {
            mMapping = value;
        }
    }

    #endregion


    #region "Protected properties"

    /// <summary>
    /// Gets the CMS user form info suitable for mapping.
    /// </summary>
    protected FormInfo UserFormInfo
    {
        get
        {
            if (mUserFormInfo == null)
            {
                IFormInfoProvider provider = new FormInfoProvider();
                mUserFormInfo = provider.GetFormInfo(UserInfo.TYPEINFO);
            }

            return mUserFormInfo;
        }
    }


    /// <summary>
    /// Gets the CMS user settings form info suitable fo mapping.
    /// </summary>
    protected FormInfo UserSettingsFormInfo
    {
        get
        {
            if (mUserSettingsFormInfo == null)
            {
                IFormInfoProvider provider = new FormInfoProvider();
                mUserSettingsFormInfo = provider.GetFormInfo(UserSettingsInfo.TYPEINFO);
            }

            return mUserSettingsFormInfo;
        }
    }


    /// <summary>
    /// Gets the entity model of Facebook user profile.
    /// </summary>
    protected EntityModel UserProfileModel
    {
        get
        {
            if (mUserProfileModel == null)
            {
                mUserProfileModel = FacebookMappingHelper.GetUserProfileModel();
            }

            return mUserProfileModel;
        }
    }

    #endregion


    #region "Life-cycle methods"

    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            InitializeControl();
        }
        catch (Exception exception)
        {
            FacebookError.Report(exception);
        }
    }

    #endregion


    #region "Protected methods"

    /// <summary>
    /// Initializes this control.
    /// </summary>
    protected void InitializeControl()
    {
        if (Mapping != null && Mapping.HasItems)
        {
            UserMappingItemRepeater.DataSource = Mapping.GetFromScope(String.Empty).Items;
            UserMappingItemRepeater.DataBind();

            UserSettingsMappingItemRepeater.DataSource = Mapping.GetFromScope("UserSettings").Items;
            UserSettingsMappingItemRepeater.DataBind();

            ContainerControl.Visible = true;
        }
        else
        {
            MessageControl.InnerText = GetString("fb.emptymapping");
            MessageControl.Visible = true;
        }
        if (!Enabled)
        {
            Attributes.Add("class", "gray");
        }
    }


    /// <summary>
    /// Retrieves a display name of the specified Facebook user profile attribute, and returns it.
    /// </summary>
    /// <param name="attributeName">The Facebook user profile attribute name.</param>
    /// <returns>A display name of the specified Facebook user profile attribute, if found; otherwise, an empty string.</returns>
    protected string GetFacebookUserAttributeDisplayName(string attributeName)
    {
        EntityAttributeModel attributeModel = UserProfileModel.GetAttributeModel(attributeName);
        if (attributeModel == null)
        {
            return String.Empty;
        }

        return ResHelper.LocalizeString(attributeModel.DisplayName);
    }


    /// <summary>
    /// Retrieves a display name of the specified CMS user field, and returns it.
    /// </summary>
    /// <param name="fieldName">The CMS user field name.</param>
    /// <returns>A display name of the specified CMS user field, if found; otherwise, an empty string.</returns>
    protected string GetUserFieldDisplayName(string fieldName)
    {
        FormFieldInfo field = UserFormInfo.GetFormField(fieldName);
        if (field == null)
        {
            return String.Empty;
        }

        return ResHelper.LocalizeString(field.GetPropertyValue(FormFieldPropertyEnum.FieldCaption, MacroContext.CurrentResolver));
    }


    /// <summary>
    /// Retrieves a display name of the specified CMS user settings field, and returns it.
    /// </summary>
    /// <param name="fieldName">The CMS user settings field name.</param>
    /// <returns>A display name of the specified CMS user settings field, if found; otherwise, an empty string.</returns>
    protected string GetUserSettingsFieldDisplayName(string fieldName)
    {
        FormFieldInfo field = UserSettingsFormInfo.GetFormField(fieldName);
        if (field == null)
        {
            return String.Empty;
        }

        return ResHelper.LocalizeString(field.GetPropertyValue(FormFieldPropertyEnum.FieldCaption, MacroContext.CurrentResolver));
    }

    #endregion

}