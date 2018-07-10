using System;

using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;


public partial class CMSModules_REST_FormControls_GenerateAuthHash : FormEngineUserControl
{
    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return null;
        }
        set
        {
        }
    }


    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    public override bool IsValid()
    {
        return true;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterDialogScript(Page);
        lnkGenerate.OnClientClick = "modalDialog('" + UrlResolver.ResolveUrl("~/CMSModules/REST/FormControls/GenerateHash.aspx") + "' , 'GenerateAuthHash', 800, 350); return false;";
    }
}
