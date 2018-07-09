using System;

using CMS.CMSImportExport;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_ImportExport_Controls_Import_ma_automationprocess : ImportExportControl
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
    protected bool Import
    {
        get
        {
            return chkObject.Checked && Visible;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        chkObject.Text = GetString("CMSImport_Processes.ImportWorkFlowTriggers");
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
        Settings.SetSettings(ImportExportHelper.SETTINGS_WORKFLOW_TRIGGERS, Import);
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        chkObject.Checked = ValidationHelper.GetBoolean(Settings.GetSettings(ImportExportHelper.SETTINGS_WORKFLOW_TRIGGERS), !ExistingSite && Visible);
    }
}