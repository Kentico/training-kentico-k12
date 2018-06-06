using System;

using CMS.CMSImportExport;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_ImportExport_Controls_Import_cms_pagetemplate : ImportExportControl
{
    /// <summary>
    /// True if import into existing site.
    /// </summary>
    protected bool ExistingSite
    {
        get
        {
            if (Settings != null)
            {
                return ((SiteImportSettings)Settings).ExistingSite;
            }
            return true;
        }
    }


    /// <summary>
    /// True if the data should be imported.
    /// </summary>
    protected bool ImportScopes
    {
        get
        {
            return chkObject.Checked && Visible;
        }
    }


    /// <summary>
    /// True if the web part/zone.widget variants should be imported.
    /// </summary>
    protected bool ImportVariants
    {
        get
        {
            return chkVariants.Checked && Visible;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        chkObject.Text = GetString("CMSImport_PageTemplates.ImportSitePageTemplatesScopes");
        chkVariants.Text = GetString("CMSImport_PageTemplates.ImportSitePageTemplatesVariants");
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        Visible = ((SiteImportSettings)Settings).SiteIsIncluded;
    }


    /// <summary>
    /// Gets settings.
    /// </summary>
    public override void SaveSettings()
    {
        Settings.SetSettings(ImportExportHelper.SETTINGS_PAGETEMPLATE_SCOPES, ImportScopes);
        Settings.SetSettings(ImportExportHelper.SETTINGS_PAGETEMPLATE_VARIANTS, ImportVariants);
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        chkObject.Checked = ValidationHelper.GetBoolean(Settings.GetSettings(ImportExportHelper.SETTINGS_PAGETEMPLATE_SCOPES), !ExistingSite && Visible);
        chkVariants.Checked = ValidationHelper.GetBoolean(Settings.GetSettings(ImportExportHelper.SETTINGS_PAGETEMPLATE_VARIANTS), !ExistingSite && Visible);
    }
}