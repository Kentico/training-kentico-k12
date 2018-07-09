using System;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.ExternalAuthentication.Facebook;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;


/// <summary>
/// Displays an editor of a mapping between a CMS user field and a Facebook user attribute.
/// </summary>
public partial class CMSModules_Membership_Controls_Facebook_MappingEditorItem : AbstractUserControl
{

    #region "Public properties"

    /// <summary>
    /// Gets or sets the current mapping item.
    /// </summary>
    public EntityMappingItem SourceMappingItem { get; set; }


    /// <summary>
    /// Gets or sets the Facebook user profile model that is the source of the mapping.
    /// </summary>
    public EntityModel UserProfileModel { get; set; }


    /// <summary>
    /// Gets or sets the CMS user field info that is the target of the mapping.
    /// </summary>
    public FormFieldInfo FieldInfo { get; set; }


    /// <summary>
    /// Gets or sets the CMS user field scope display name.
    /// </summary>
    public string FieldScopeDisplayName { get; set; }


    /// <summary>
    /// Gets or sets the CMS user field scope.
    /// </summary>
    public string FieldScope { get; set; }


    /// <summary>
    /// Gets or sets the factory that creates instances of entity attribute value converters.
    /// </summary>
    public IEntityAttributeValueConverterFactory AttributeValueConverterFactory { get; set; }

    #endregion


    #region "Life-cycle methods"

    protected override void OnPreRender(EventArgs e)
    {
        if (AttributeDropDownList.Items.Count == 1)
        {
            string message = GetString("fb.nofieldmappingavailable");
            MessageControl.InnerHtml = HTMLHelper.HTMLEncode(message);
            AttributeDropDownList.Visible = false;
        }
        else
        {
            MessageControl.Visible = false;
        }
        base.OnPreRender(e);
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Initializes this instance of mapping editor item.
    /// </summary>
    public void Initialize()
    {
        InitializeControl();
    }


    /// <summary>
    /// Updates the mapping depending on user selection.
    /// </summary>
    /// <param name="mapping">The mapping to update.</param>
    public void UpdateMapping(EntityMapping mapping)
    {
        string value = AttributeDropDownList.SelectedValue;
        if (!String.IsNullOrEmpty(value))
        {
            string[] tokens = value.Split('-');
            if (tokens[0] == "Attribute" || tokens[0] == "RestrictedAttribute")
            {
                string attributeName = tokens[1];
                EntityAttributeModel attributeModel = UserProfileModel.GetAttributeModel(attributeName);
                if (attributeModel != null)
                {
                    mapping.Add(attributeModel, FieldInfo, FieldScope);
                }
            }
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Initializes inner controls.
    /// </summary>
    private void InitializeControl()
    {
        FieldLabel.Text = ResHelper.LocalizeString(FieldInfo.GetPropertyValue(FormFieldPropertyEnum.FieldCaption, MacroContext.CurrentResolver));
        AttributeDropDownList.Items.Add(new ListItem());
        foreach (EntityAttributeModel attributeModel in UserProfileModel.Items)
        {
            EntityAttributeValueConverterBase attributeValueConverter = AttributeValueConverterFactory.CreateConverter(attributeModel);
            if (attributeValueConverter != null && attributeValueConverter.IsCompatibleWithFormField(FieldInfo))
            {
                ListItem item = new ListItem
                {
                    Value = String.Format("{0}-{1}", GetAttributeTypeName(attributeModel), attributeModel.Name),
                    Text = ResHelper.LocalizeString(attributeModel.DisplayName)
                };
                AttributeDropDownList.Items.Add(item);
            }
        }
        if (SourceMappingItem != null)
        {
            EntityAttributeModel attributeModel = UserProfileModel.GetAttributeModel(SourceMappingItem.AttributeName);
            if (attributeModel != null)
            {
                AttributeDropDownList.SelectedValue = String.Format("{0}-{1}", GetAttributeTypeName(attributeModel), SourceMappingItem.AttributeName);
            }
        }
    }


    /// <summary>
    /// Gets the security type name of the specified attribute, and returns it.
    /// </summary>
    /// <param name="attributeModel">The attribute model.</param>
    /// <returns>The security type name of the specified attribute.</returns>
    private string GetAttributeTypeName(EntityAttributeModel attributeModel)
    {
        return (String.IsNullOrEmpty(attributeModel.FacebookPermissionScopeName) ? "Attribute" : "RestrictedAttribute");
    }

    #endregion

}