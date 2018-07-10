using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.EventLog;
using CMS.ExternalAuthentication.Facebook;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


/// <summary>
/// Displays an editor of mapping between a Facebook user profile and CMS user.
/// </summary>
public partial class CMSModules_Membership_Pages_Facebook_MappingEditor : CMSModalPage
{
    #region "Private members"

    private IFormInfoProvider mFormInfoProvider;
    private FormInfo mUserFormInfo;
    private FormInfo mUserSettingsFormInfo;
    private IEntityAttributeValueConverterFactory mAttributeValueConverterFactory;
    private EntityModel mUserProfileModel;
    private EntityMapping mSourceMapping;
    private string mSourceMappingHiddenFieldClientId;
    private string mSourceMappingPanelClientId;
    private List<CMSModules_Membership_Controls_Facebook_MappingEditorItem> mMappingEditorItems;

    #endregion


    #region "Protected properties"

    /// <summary>
    /// Gets the object that provides a form info for the specified CMS object type.
    /// </summary>
    protected IFormInfoProvider FormInfoProvider
    {
        get
        {
            return mFormInfoProvider ?? (mFormInfoProvider = new FormInfoProvider());
        }
    }


    /// <summary>
    /// Gets the form info of the CMS user object.
    /// </summary>
    protected FormInfo UserFormInfo
    {
        get
        {
            return mUserFormInfo ?? (mUserFormInfo = FormInfoProvider.GetFormInfo(UserInfo.TYPEINFO));
        }
    }


    /// <summary>
    /// Gets the form info for the CMS user settings object.
    /// </summary>
    protected FormInfo UserSettingsFormInfo
    {
        get
        {
            return mUserSettingsFormInfo ?? (mUserSettingsFormInfo = FormInfoProvider.GetFormInfo(UserSettingsInfo.TYPEINFO));
        }
    }


    /// <summary>
    /// Gets the object that converts values of Facebook entity attributes to values compatible with CMS objects fields.
    /// </summary>
    protected IEntityAttributeValueConverterFactory AttributeValueConverterFactory
    {
        get
        {
            return mAttributeValueConverterFactory ?? (mAttributeValueConverterFactory = new EntityAttributeValueConverterFactory());
        }
    }


    /// <summary>
    /// Gets the entity model of the Facebook user.
    /// </summary>
    protected EntityModel UserProfileModel
    {
        get
        {
            return mUserProfileModel ?? (mUserProfileModel = FacebookMappingHelper.GetUserProfileModel());
        }
    }


    /// <summary>
    /// Gets the collection of MappingEditorItem controls.
    /// </summary>
    protected List<CMSModules_Membership_Controls_Facebook_MappingEditorItem> MappingEditorItems
    {
        get
        {
            return mMappingEditorItems ?? (mMappingEditorItems = new List<CMSModules_Membership_Controls_Facebook_MappingEditorItem>());
        }
    }


    /// <summary>
    /// Gets the identifier of the HTML element with the serialized mapping from the parent page.
    /// </summary>
    protected string SourceMappingHiddenFieldClientId
    {
        get
        {
            return mSourceMappingHiddenFieldClientId;
        }
    }


    /// <summary>
    /// Gets the identifier of the HTML element with the mapping view from the parent page.
    /// </summary>
    protected string SourceMappingPanelClientId
    {
        get
        {
            return mSourceMappingPanelClientId;
        }
    }


    /// <summary>
    /// Gets the mapping to edit.
    /// </summary>
    protected EntityMapping SourceMapping
    {
        get
        {
            return mSourceMapping;
        }
    }

    #endregion


    #region "Life-cycle methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ScriptHelper.RegisterWOpenerScript(Page);
        ScriptHelper.RegisterJQuery(Page);
        PageTitle.TitleText = GetString("fb.mapping.editor.title");
        ShowWarning(GetString("fb.mapping.editor.introduction"));
        ShowInformation(GetString("fb.mapping.editor.instruction"));
        UserMappingItemRepeater.ItemDataBound += UserMappingItemRepeater_ItemDataBound;
        UserSettingsMappingItemRepeater.ItemDataBound += UserSettingsMappingItemRepeater_ItemDataBound;
        Save += (s, ea) => Confirm();
        try
        {
            RestoreParameters();
            UserMappingItemRepeater.DataSource = UserFormInfo.GetFields(true, true);
            UserMappingItemRepeater.DataBind();
            UserSettingsMappingItemRepeater.DataSource = UserSettingsFormInfo.GetFields(true, true);
            UserSettingsMappingItemRepeater.DataBind();
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }


    protected void UserMappingItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        FormFieldInfo fieldInfo = e.Item.DataItem as FormFieldInfo;
        CMSModules_Membership_Controls_Facebook_MappingEditorItem control = e.Item.FindControl("MappingEditorItemControl") as CMSModules_Membership_Controls_Facebook_MappingEditorItem;
        control.SourceMappingItem = SourceMapping.Items.SingleOrDefault(x => x.FieldName == fieldInfo.Name);
        control.UserProfileModel = UserProfileModel;
        control.FieldInfo = fieldInfo;
        control.AttributeValueConverterFactory = AttributeValueConverterFactory;
        control.Initialize();
        MappingEditorItems.Add(control);
    }


    protected void UserSettingsMappingItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        FormFieldInfo fieldInfo = e.Item.DataItem as FormFieldInfo;
        CMSModules_Membership_Controls_Facebook_MappingEditorItem control = e.Item.FindControl("MappingEditorItemControl") as CMSModules_Membership_Controls_Facebook_MappingEditorItem;
        control.SourceMappingItem = SourceMapping.Items.SingleOrDefault(x => x.FieldName == String.Format("UserSettings.{0}", fieldInfo.Name));
        control.UserProfileModel = UserProfileModel;
        control.FieldInfo = fieldInfo;
        control.FieldScopeDisplayName = GetString("objecttype.cms_usersettings");
        control.FieldScope = "UserSettings";
        control.AttributeValueConverterFactory = AttributeValueConverterFactory;
        control.Initialize();
        MappingEditorItems.Add(control);
    }


    protected void Confirm()
    {
        try
        {
            EntityMapping mapping = GetTargetMapping();
            EntityMappingSerializer serializer = new EntityMappingSerializer();
            MappingHiddenField.Value = serializer.SerializeEntityMapping(mapping);
            MappingControl.Mapping = mapping;
            string parametersIdentifier = QueryHelper.GetString("pid", null);
            Hashtable parameters = WindowHelper.GetItem(parametersIdentifier) as Hashtable;
            parameters["Mapping"] = MappingHiddenField.Value;
            WindowHelper.Add(parametersIdentifier, parameters);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

    #endregion


    #region "Private methods"

    private EntityMapping GetTargetMapping()
    {
        EntityMapping mapping = new EntityMapping();
        foreach (CMSModules_Membership_Controls_Facebook_MappingEditorItem control in MappingEditorItems)
        {
            control.UpdateMapping(mapping);
        }

        return mapping;
    }


    private void RestoreParameters()
    {
        // Validate parameters
        if (!QueryHelper.ValidateHash("hash"))
        {
            throw new Exception("[FacebookMappingEditorPage.RestoreParameters]: Invalid query hash.");
        }
        Hashtable parameters = WindowHelper.GetItem(QueryHelper.GetString("pid", null)) as Hashtable;
        if (parameters == null)
        {
            throw new Exception("[FacebookMappingEditorPage.RestoreParameters]: The dialog page parameters are missing, the session might have been lost.");
        }

        // Restore parameters
        mSourceMappingHiddenFieldClientId = ValidationHelper.GetString(parameters["MappingHiddenFieldClientId"], null);
        mSourceMappingPanelClientId = ValidationHelper.GetString(parameters["MappingPanelClientId"], null);

        // Restore mapping
        string content = ValidationHelper.GetString(parameters["Mapping"], null);
        if (String.IsNullOrEmpty(content))
        {
            mSourceMapping = new EntityMapping();
        }
        else
        {
            EntityMappingSerializer serializer = new EntityMappingSerializer();
            mSourceMapping = serializer.UnserializeEntityMapping(content);
        }
    }


    private void HandleError(Exception exception)
    {
        ErrorControl.Report(exception);
        EventLogProvider.LogException("Facebook integration", "MappingEditorPage", exception);
    }

    #endregion

}
