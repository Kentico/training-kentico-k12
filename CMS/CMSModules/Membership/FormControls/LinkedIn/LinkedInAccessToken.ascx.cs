using System;

using CMS.Base.Web.UI;
using CMS.ExternalAuthentication;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;


public partial class CMSModules_Membership_FormControls_LinkedIn_LinkedInAccessToken : FormEngineUserControl
{
    #region "Properties"

    /// <summary>
    /// Gets or sets enabled state.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return txtToken.Enabled;
        }
        set
        {
            txtToken.Enabled = value;
            btnSelect.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets the value of access token.
    /// </summary>
    public override object Value
    {
        get
        {
            return txtToken.Text;
        }
        set
        {
            txtToken.Text = value.ToString();
        }
    }


    /// <summary>
    /// Gets ClientID of the control from which the Value is retrieved or 
    /// null if such a control can't be specified.
    /// </summary>
    public override string ValueElementID
    {
        get
        {
            return txtToken.ClientID;
        }
    }


    /// <summary>
    /// Gets or sets the name of the client function to be used to get the API key value.
    /// </summary>
    public string APIKeyValueAccessFunctionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("APIKeyValueAccessFunctionName"), null);
        }
        set
        {
            SetValue("APIKeyValueAccessFunctionName", value);
        }
    }


    /// <summary>
    /// Gets or sets the name of the client function to be used to get the application secret value.
    /// </summary>
    public string AppSecretValueAccessFunctionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AppSecretValueAccessFunctionName"), null);
        }
        set
        {
            SetValue("AppSecretValueAccessFunctionName", value);
        }
    }

    #endregion


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        lblError.Text = GetString("socialnetworking.linkedin.apisettingsmissing");

        // Register the dialog script
        ScriptHelper.RegisterDialogScript(Page);

        // Build script with modal dialog opener and set textbox value functions
        const string scriptStringFormat = @"
function tagSelect(id, apikeycid, apisecretcid) {{
    var txtToken = $cmsj('#' + id);
    var apiKey = {2}();
    var apiSecret = {3}();
    if (txtToken != null) {{
        if ((apiKey.length == 0) || (apiSecret.length == 0))
        {{
            var label = $cmsj('#{1}');
            label.removeClass('hidden');

            return;
        }}

        modalDialog('{0}?txtToken=' + id + '&apiKey=' + apiKey + '&apiSecret=' + apiSecret, 'LinkedInAccessToken', '550', '300', null, null, null, true);
    }}
}}
function setAccessTokenToTextBox(textBoxId, accessTokenString) {{
    if (textBoxId != '') {{
        var textbox = document.getElementById(textBoxId);
        if (textbox != null) {{
            textbox.value = accessTokenString;
        }}
    }}
}}";

        string scriptString = string.Format(scriptStringFormat,
                                            URLHelper.GetAbsoluteUrl(LinkedInProvider.ACCESS_TOKEN_PAGE),
                                            lblError.ClientID,
                                            APIKeyValueAccessFunctionName,
                                            AppSecretValueAccessFunctionName);

        // Register tag script 
        ScriptHelper.RegisterStartupScript(this, typeof(string), "accessTokenScript", scriptString, true);

        btnSelect.OnClientClick = "tagSelect('" + txtToken.ClientID + "'); return false;";
    }
}