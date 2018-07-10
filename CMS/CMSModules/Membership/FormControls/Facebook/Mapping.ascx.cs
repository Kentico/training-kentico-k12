using System;
using System.Collections;
using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.EventLog;
using CMS.ExternalAuthentication.Facebook;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;


/// <summary>
/// Displays the mapping between a Facebook user profile and a CMS user, and allows the user to edit it.
/// </summary>
/// <remarks>
/// This control is intended for the Settings section.
/// </remarks>
public partial class CMSModules_Membership_FormControls_Facebook_Mapping : FormEngineUserControl, ICallbackEventHandler
{

    #region "Private members"

    /// <summary>
    /// Indicates whether this control is enabled.
    /// </summary>
    private bool mEnabled = true;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the mapping in serialized form.
    /// </summary>
    public override object Value
    {
        get
        {
            return MappingHiddenField.Value;
        }
        set
        {
            MappingHiddenField.Value = value as string;
        }
    }


    /// <summary>
    /// Gets or sets the value indicating whether this control is enabled.
    /// </summary>
    public override bool Enabled
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
    /// Gets the client identifier of the control holding the setting value.
    /// </summary>
    public override string ValueElementID
    {
        get
        {
            return MappingHiddenField.ClientID;
        }
    }

    #endregion


    #region "Protected properties"

    /// <summary>
    /// Gets the identifier of a collection of parameters for the mapping editor.
    /// </summary>
    protected string ParametersId
    {
        get
        {
            string parametersId = ViewState["PID"] as string;
            if (String.IsNullOrEmpty(parametersId))
            {
                parametersId = Guid.NewGuid().ToString("N");
                ViewState["PID"] = parametersId;
            }

            return parametersId;
        }
    }

    #endregion

    #region "Life-cycle methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ScriptHelper.RegisterDialogScript(Page);
        ScriptHelper.RegisterJQuery(Page);
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        string baseUrl = UrlResolver.ResolveUrl("~/CMSModules/Membership/Pages/Facebook/MappingEditor.aspx");
        string url = String.Format("{0}?pid={1}", baseUrl, ParametersId);
        string script = String.Format("function Facebook_EditUserMapping (arg, context) {{ modalDialog('{0}', 'EditUserMapping', '850', '720', null); return false; }}", URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash(url)));
        ScriptHelper.RegisterClientScriptBlock(this, GetType(), "Facebook_EditUserMapping", script, true);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        InitializeControls();
    }

    #endregion


    #region "Private methods"

    private void InitializeControls()
    {
        try
        {
            string content = Value as string;
            if (!String.IsNullOrEmpty(content))
            {
                EntityMappingSerializer serializer = new EntityMappingSerializer();
                EntityMapping mapping = serializer.UnserializeEntityMapping(content);
                MappingControl.Mapping = mapping;
                MappingControl.Enabled = Enabled;
            }
            if (Enabled)
            {
                InitializeEditButton();
            }
            else
            {
                EditMappingButton.Visible = false;
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }


    private void InitializeEditButton()
    {
        EditMappingButton.OnClientClick = String.Format("{0}; return false;", Page.ClientScript.GetCallbackEventReference(this, null, "Facebook_EditUserMapping", null));
    }


    private Hashtable CreateParameters()
    {
        Hashtable parameters = new Hashtable();
        parameters["Mapping"] = Value;
        parameters["MappingHiddenFieldClientId"] = MappingHiddenField.ClientID;
        parameters["MappingPanelClientId"] = MappingPanel.ClientID;

        return parameters;
    }


    private void HandleError(Exception exception)
    {
        ErrorControl.Report(exception);
        EventLogProvider.LogException("Facebook integration", "MappingFormControl", exception);
    }

    #endregion


    #region ICallbackEventHandler Members
    
    string ICallbackEventHandler.GetCallbackResult()
    {
        return null;
    }

    
    void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
    {
        Hashtable parameters = WindowHelper.GetItem(ParametersId) as Hashtable;
        if (parameters == null)
        {
            parameters = CreateParameters();
            WindowHelper.Add(ParametersId, parameters);
        }
    }

    #endregion

}