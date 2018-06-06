using System;

using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_AdminControls_Controls_UIControls_GeneralSelector : CMSAbstractUIWebpart
{
    #region "Variables"

    SpecialFieldsDefinition mSpecialFields;

    #endregion


    #region "Properties"

    /// <summary>
    /// Type of the selected objects.
    /// </summary>
    public String SelectorObjectType
    {
        get
        {
            return GetStringContextValue("SelectorObjectType", null);
        }
        set
        {
            SetValue("SelectorObjectType", value);
        }
    }


    /// <summary>
    /// Column name of the object which value should be returned by the selector. 
    /// If NULL, ID column is used.
    /// </summary>
    public String ReturnColumnName
    {
        get
        {
            return GetStringContextValue("ReturnColumnName", null);
        }
        set
        {
            SetValue("ReturnColumnName", value);
        }
    }


    /// <summary>
    /// Gets or sets site name. If set, only objects which belong to specified site are retrieved (if the object has SiteID column). If null or empty, all objects are retrieved.
    /// Use #currentsite or #current for CurrentSite and #global for global objects or #currentandglobal for both.
    /// </summary>
    public String ObjectSiteName
    {
        get
        {
            return GetStringContextValue("ObjectSiteName", null);
        }
        set
        {
            SetValue("ObjectSiteName", value);
        }
    }


    /// <summary>
    /// Format of the display name.
    /// </summary>
    public String DisplayNameFormat
    {
        get
        {
            // Disable resolve macros - macros will be resolved in UniSelector with proper context data.
            UIContext.ResolveMacros = false;
            String val = GetStringContextValue("DisplayNameFormat", null);
            UIContext.ResolveMacros = true;

            return val;
        }
        set
        {
            SetValue("DisplayNameFormat", value);
        }
    }


    /// <summary>
    /// Enables / disables the multiple selection mode.
    /// </summary>
    public SelectionModeEnum SelectionMode
    {
        get
        {
            return (SelectionModeEnum)GetIntContextValue("SelectionMode", 1);
        }
        set
        {
            SetValue("SelectionMode", (int)value);
        }
    }


    /// <summary>
    /// Additional columns to select.
    /// </summary>
    public String AdditionalColumns
    {
        get
        {
            return GetStringContextValue("AdditionalColumns", null);
        }
        set
        {
            SetValue("AdditionalColumns", value);
        }
    }


    /// <summary>
    /// Base order of the items. Applies to all Base multiple selection grid, 
    /// dropdownlist, single and multiple selection dialogs.
    /// </summary>
    public String OrderBy
    {
        get
        {
            return GetStringContextValue("OrderBy", null);
        }
        set
        {
            ViewState["OrderBy"] = value;
        }
    }


    /// <summary>
    /// Default number of items per page. Applies to all Base multiple selection grid, 
    /// dropdownlist, single and multiple selection dialogs. Default 25.
    /// </summary>
    public int ItemsPerPage
    {
        get
        {
            return GetIntContextValue("ItemsPerPage", UniSelector.DefaultItemsPerPage);
        }
        set
        {
            SetValue("ItemsPerPage", value);
        }
    }


    /// <summary>
    /// Replaces items which don't have any visible value to be displayed. Default is &amp;nbsp;.
    /// </summary>
    public String EmptyReplacement
    {
        get
        {
            return GetStringContextValue("EmptyReplacement", "&nbsp;");
        }
        set
        {
            SetValue("EmptyReplacement", value);
        }
    }


    /// <summary>
    /// Grid name (path to the XML) for the multiple selection grid.
    /// </summary>
    public String SelectorGridName
    {
        get
        {
            return GetStringContextValue("SelectorGridName", "~/CMSAdminControls/UI/UniSelector/ControlItemList.xml");
        }
        set
        {
            SetValue("SelectorGridName", value);
        }
    }


    /// <summary>
    /// Dialog grid name (path to the XML).
    /// </summary>
    public String DialogGridName
    {
        get
        {
            return GetStringContextValue("DialogGridName", null);
        }
        set
        {
            SetValue("DialogGridName", value);
        }
    }


    /// <summary>
    /// Additional columns to search in.
    /// </summary>
    public String AdditionalSearchColumns
    {
        get
        {
            return GetStringContextValue("AdditionalSearchColumns", null);
        }
        set
        {
            SetValue("AdditionalSearchColumns", value);
        }
    }


    /// <summary>
    /// Additional URL parameters added to dialogs URLs.
    /// Must start with '&amp;'.
    /// </summary>
    public String AdditionalUrlParameters
    {
        get
        {
            return GetStringContextValue("AdditionalUrlParameters", null);
        }
        set
        {
            SetValue("AdditionalUrlParameters", value);
        }
    }


    /// <summary>
    ///  Base where condition for the objects selection. Applies to all Base multiple selection grid, 
    ///  dropdownlist, single and multiple selection dialogs.
    /// </summary>
    public String WhereCondition
    {
        get
        {
            return GetStringContextValue("WhereCondition", null);
        }
        set
        {
            SetValue("WhereCondition", value);
        }
    }


    /// <summary>
    /// Column name where the enabled flag of the object is stored.
    /// </summary>
    public String EnabledColumnName
    {
        get
        {
            return GetStringContextValue("EnabledColumnName", null);
        }
        set
        {
            SetValue("EnabledColumnName", value);
        }
    }


    /// <summary>
    /// If there is a multi-selection enabled, the returned values are separated by this separator. 
    /// Default is semicolon ";".
    /// </summary>
    public char ValuesSeparator
    {
        get
        {
            return ValidationHelper.GetValue(GetStringContextValue("ValuesSeparator", null), ';');
        }
        set
        {
            SetValue("ValuesSeparator", value);
        }
    }


    /// <summary>
    ///  Specifies DDL value that will be pre-selected when DDL is loaded for the first time.
    /// </summary>
    public String SelectedValue
    {
        get
        {
            return GetStringContextValue("SelectedValue", null);
        }
        set
        {
            SetValue("SelectedValue", value);
        }
    }


    /// <summary>
    /// Specifies, whether the selector allows empty selection. If the dialog allows empty selection, 
    /// it displays the (none) field in the DDL variant and Clear button in the Textbox variant (default true).
    /// For multiple selection it returns empty string, otherwise it returns 0.
    /// </summary>
    public bool AllowEmpty
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowEmpty"), false);
        }
        set
        {
            SetValue("AllowEmpty", value);
        }
    }


    /// <summary>
    /// Specifies, whether the selector allows default item selection. If the dialog allows default selection, 
    /// it displays the (default) field in the DDL variant (default false).
    /// For multiple selection it returns empty string, otherwise it returns 0.
    /// </summary>
    public bool AllowDefault
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowDefault"), false);
        }
        set
        {
            SetValue("AllowDefault", value);
        }
    }


    /// <summary>
    /// Specifies, whether the selector allows selection of all items. If the dialog allows selection of all items, 
    /// it displays the (all) field in the DDL variant.
    /// When property is selected then Uniselector doesn’t load any data from DB, it just returns -1 value 
    /// and external code must handle data loading.
    /// </summary>
    public bool AllowAll
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowAll"), false);
        }
        set
        {
            SetValue("AllowAll", value);
        }
    }


    /// <summary>
    /// Specifies whether the selector should resolve localization macros.
    /// </summary>
    public bool LocalizeItems
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("LocalizeItems"), true);
        }
        set
        {
            SetValue("LocalizeItems", value);
        }
    }


    /// <summary>
    /// Path to the filter control (CMSAbstractBaseFilterControl), which will be used for advanced filtering of 
    /// the objects in the selection dialogs.
    /// </summary>
    public String FilterControl
    {
        get
        {
            return GetStringContextValue("FilterControl", null);
        }
        set
        {
            SetValue("FilterControl", value);
        }
    }


    /// <summary>
    /// If true, name filter is used (default true), it can be disabled when some FilterControl is used.
    /// </summary>
    public bool UseDefaultNameFilter
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseDefaultNameFilter"), true);
        }
        set
        {
            SetValue("UseDefaultNameFilter", value);
        }
    }


    /// <summary>
    /// If true, the textbox mode works with the return value and allows editing of the value.
    /// </summary>
    public bool AllowEditTextBox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowEditTextBox"), false);
        }
        set
        {
            SetValue("AllowEditTextBox", value);
        }
    }


    /// <summary>
    /// Array with the special field values that should be added to the DropDownList selection, 
    /// between the (none) item and the other items. 
    /// </summary>
    public SpecialFieldsDefinition SpecialFields
    {
        get
        {
            if (mSpecialFields == null)
            {
                // Get fields definition
                mSpecialFields = new SpecialFieldsDefinition(ResourcePrefix, selectorElem.FieldInfo, ContextResolver);
                mSpecialFields.SetUniqueIDs = true;
                mSpecialFields.LoadFromText(ValidationHelper.GetString(GetValue("SpecialFields"), String.Empty));
            }
            return mSpecialFields;
        }
        set
        {
            SetValue("SpecialFields", value);
        }
    }


    /// <summary>
    /// The number of maximum displayed total items in the dropdownlist selection (excluding the special and generic items). Default is 50. If exceeded, only MaxDisplayedItems is displayed.
    /// </summary>
    public int MaxDisplayedTotalItems
    {
        get
        {
            return GetIntContextValue("MaxDisplayedTotalItems", UniSelector.DefaultMaxDisplayedTotalItems);
        }
        set
        {
            SetValue("MaxDisplayedTotalItems", value);
        }
    }


    /// <summary>
    /// The number of maximum displayed items in the dropdownlist selection (excluding the special and generic items). Default is 25.
    /// </summary>
    public int MaxDisplayedItems
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxDisplayedItems"), UniSelector.DefaultMaxDisplayedItems);
        }
        set
        {
            SetValue("MaxDisplayedItems", value);
        }
    }


    /// <summary>
    /// Indicates whether global objects have suffix "(global)" in the grid.
    /// </summary>
    public bool AddGlobalObjectSuffix
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AddGlobalObjectSuffix"), false);
        }
        set
        {
            SetValue("AddGlobalObjectSuffix", value);
        }
    }


    /// <summary>
    /// Indicates whether global object names should be selected with prefix '.'
    /// </summary>
    public bool AddGlobalObjectNamePrefix
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AddGlobalObjectNamePrefix"), false);
        }
        set
        {
            SetValue("AddGlobalObjectNamePrefix", value);
        }
    }


    /// <summary>
    /// Gets or set the suffix which is added to global objects if AddGlobalObjectSuffix is true. Default is "(global)".
    /// </summary>
    public String GlobalObjectSuffix
    {
        get
        {
            return GetStringContextValue("GlobalObjectSuffix", null);
        }
        set
        {
            SetValue("GlobalObjectSuffix", value);
        }
    }


    /// <summary>
    /// Indicates whether to remove multiple commas (can happen when DisplayNameFormat is like {%column1%}, {%column2%}, {column3} and column2 is empty.
    /// </summary>
    public bool RemoveMultipleCommas
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("RemoveMultipleCommas"), false);
        }
        set
        {
            SetValue("RemoveMultipleCommas", value);
        }
    }


    /// <summary>
    /// Gets or sets identifiers of disabled items for selection dialog in multiple choice mode. Identifiers are separated by semicolon.
    /// </summary>
    public String DisabledItems
    {
        get
        {
            return GetStringContextValue("DisabledItems", null);
        }
        set
        {
            SetValue("DisabledItems", value);
        }
    }


    /// <summary>
    /// Path to the button image, if the path is specified, the button is displayed as a link button with the image.
    /// </summary>
    public String ButtonImage
    {
        get
        {
            return GetStringContextValue("ButtonImage", null);
        }
        set
        {
            SetValue("ButtonImage", value);
        }
    }


    /// <summary>
    /// Path to the new item page.
    /// </summary>
    public String NewItemPageUrl
    {
        get
        {
            return GetStringContextValue("NewItemPageUrl", null);
        }
        set
        {
            SetValue("NewItemPageUrl", value);
        }
    }


    /// <summary>
    /// URL of select item dialog.
    /// </summary>
    public String SelectItemPageUrl
    {
        get
        {
            return GetStringContextValue("SelectItemPageUrl", null);
        }
        set
        {
            SetValue("SelectItemPageUrl", value);
        }
    }


    /// <summary>
    /// Cache minutes (default 0 - caching not used).
    /// </summary>
    public override int CacheMinutes
    {
        get
        {
            return GetIntContextValue("CacheMinutes", 0);
        }
        set
        {
            SetValue("CacheMinutes", value);
        }
    }



    /// <summary>
    /// Dialog window name.
    /// </summary>
    public String DialogWindowName
    {
        get
        {
            return GetStringContextValue("DialogWindowName", "SelectionDialog");
        }
        set
        {
            SetValue("DialogWindowName", value);
        }
    }


    /// <summary>
    /// Dialog window width.
    /// </summary>
    public int DialogWindowWidth
    {
        get
        {
            return GetIntContextValue("DialogWindowWidth", 750);
        }
        set
        {
            SetValue("DialogWindowWidth", value);
        }
    }


    /// <summary>
    /// Dialog window height.
    /// </summary>
    public int DialogWindowHeight
    {
        get
        {
            return GetIntContextValue("DialogWindowHeight", 590);
        }
        set
        {
            SetValue("DialogWindowHeight", value);
        }
    }


    /// <summary>
    /// Confirmation message for the items removal. To disable confirmation, set this property to an empty string.
    /// </summary>
    public String RemoveConfirmation
    {
        get
        {
            return GetStringContextValue("RemoveConfirmation", null);
        }
        set
        {
            SetValue("RemoveConfirmation", value);
        }
    }


    /// <summary>
    /// Confirmation message for the items selection.
    /// </summary>
    public String SelectionConfirmation
    {
        get
        {
            return GetStringContextValue("SelectionConfirmation", null);
        }
        set
        {
            SetValue("SelectionConfirmation", value);
        }
    }


    /// <summary>
    /// Path to the edit item page.
    /// </summary>
    public String EditItemPageUrl
    {
        get
        {
            return GetStringContextValue("EditItemPageUrl", null);
        }
        set
        {
            SetValue("EditItemPageUrl", value);
        }
    }


    /// <summary>
    /// Indicates whether page postbacks after item change.
    /// </summary>
    public bool PostbackOnChange
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PostbackOnChange"), true);
        }
        set
        {
            SetValue("PostbackOnChange", value);
        }
    }


    /// <summary>
    /// Callback java-script method name.
    /// </summary>
    public String CallbackMethod
    {
        get
        {
            return GetStringContextValue("CallbackMethod", null);
        }
        set
        {
            SetValue("CallbackMethod", value);
        }
    }


    /// <summary>
    /// Client java-script code before event 'onchange'.
    /// </summary>
    public String OnBeforeClientChanged
    {
        get
        {
            return GetStringContextValue("OnBeforeClientChanged", null);
        }
        set
        {
            SetValue("OnBeforeClientChanged", value);
        }
    }


    /// <summary>
    /// Client java-script code after event 'onchange'.
    /// </summary>
    public String OnAfterClientChanged
    {
        get
        {
            return GetStringContextValue("OnAfterClientChanged", null);
        }
        set
        {
            SetValue("OnAfterClientChanged", value);
        }
    }


    /// <summary>
    /// Name under selected value will be stored in UI Context
    /// </summary>
    public String ContextName
    {
        get
        {
            return GetStringContextValue("ContextName", "SelectorValue");
        }
        set
        {
            SetValue("ContextName", false);
        }
    }


    /// <summary>
    /// Selector label
    /// </summary>
    public String SelectorLabel
    {
        get
        {
            return GetStringContextValue("SelectorLabel");
        }
        set
        {
            SetValue("SelectorLabel", false);
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
            selectorElem.ObjectType = SelectorObjectType;
            selectorElem.ReturnColumnName = ReturnColumnName;
            selectorElem.ObjectSiteName = ObjectSiteName;
            selectorElem.DisplayNameFormat = DisplayNameFormat;
            selectorElem.SelectionMode = SelectionMode;
            selectorElem.AdditionalColumns = AdditionalColumns;
            selectorElem.WhereCondition = WhereCondition;
            selectorElem.EnabledColumnName = EnabledColumnName;
            selectorElem.ValuesSeparator = ValuesSeparator;
            selectorElem.AllowEditTextBox = AllowEditTextBox;

            selectorElem.AllowEmpty = AllowEmpty;
            selectorElem.AllowDefault = AllowDefault;
            selectorElem.AllowAll = AllowAll;
            selectorElem.SpecialFields = SpecialFields;

            selectorElem.ResourcePrefix = ResourcePrefix;
            selectorElem.LocalizeItems = LocalizeItems;
            selectorElem.MaxDisplayedTotalItems = MaxDisplayedTotalItems;
            selectorElem.MaxDisplayedItems = MaxDisplayedItems;
            selectorElem.ItemsPerPage = ItemsPerPage;
            selectorElem.AddGlobalObjectNamePrefix = AddGlobalObjectNamePrefix;
            selectorElem.AddGlobalObjectSuffix = AddGlobalObjectSuffix;
            selectorElem.GlobalObjectSuffix = GlobalObjectSuffix;
            selectorElem.AddGlobalObjectNamePrefix = AddGlobalObjectNamePrefix;
            selectorElem.EmptyReplacement = EmptyReplacement;
            selectorElem.RemoveMultipleCommas = RemoveMultipleCommas;
            selectorElem.DisabledItems = DisabledItems;
            selectorElem.AdditionalUrlParameters = AdditionalUrlParameters;
            selectorElem.RemoveConfirmation = RemoveConfirmation;
            selectorElem.EmptyReplacement = ButtonImage;

            selectorElem.NewItemPageUrl = NewItemPageUrl;
            selectorElem.EditItemPageUrl = EditItemPageUrl;
            selectorElem.SelectItemPageUrl = SelectItemPageUrl;
            selectorElem.FilterControl = FilterControl;
            selectorElem.DialogWindowName = DialogWindowName;
            selectorElem.DialogWindowHeight = DialogWindowHeight;
            selectorElem.DialogWindowWidth = DialogWindowWidth;
            selectorElem.GridName = SelectorGridName;
            selectorElem.DialogGridName = DialogGridName;

            selectorElem.CallbackMethod = CallbackMethod;
            selectorElem.OnBeforeClientChanged = OnBeforeClientChanged;
            selectorElem.OnAfterClientChanged = OnAfterClientChanged;

            if (PostbackOnChange)
            {
                selectorElem.DropDownSingleSelect.AutoPostBack = true;
            }

            lblText.Text = GetString(SelectorLabel);
            lblText.Visible = !String.IsNullOrEmpty(SelectorLabel);
        }
    }


    protected override void OnInit(EventArgs e)
    {
        selectorElem.ContextResolver.SetNamedSourceData("UIContext", UIContext);
        base.OnInit(e);
    }


    protected override void OnLoad(EventArgs e)
    {
        if (!RequestHelper.IsPostBack())
        {
            // Select selector value from context
            if (UIContext[ContextName] != null)
            {
                selectorElem.Value = UIContext[ContextName];
            }
            else if (!String.IsNullOrEmpty(SelectedValue))
            {
                selectorElem.Value = SelectedValue;
            }

            // Reload data for first item selection
            selectorElem.Reload(false);
        }

        UIContext[ContextName] = selectorElem.Value;

        base.OnLoad(e);
    }

    #endregion
}