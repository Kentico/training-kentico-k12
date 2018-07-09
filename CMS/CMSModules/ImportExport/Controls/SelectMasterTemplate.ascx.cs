using System;
using System.Data;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.Search;
using CMS.UIControls;


public partial class CMSModules_ImportExport_Controls_SelectMasterTemplate : CMSUserControl
{
    /// <summary>
    /// Template ID.
    /// </summary>
    public int MasterTemplateId
    {
        get
        {
            return ucSelector.SelectedId;
        }
    }


    /// <summary>
    /// Site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return ValidationHelper.GetString(ViewState["SiteName"], "");
        }
        set
        {
            ViewState["SiteName"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing)
        {
            ReloadData();
        }
    }


    public void ReloadData()
    {
        // Load the data
        DataSet templates = PageTemplateInfoProvider.GetTemplates()
                                                    .WhereTrue("PageTemplateIsReusable")
                                                    .WhereTrue("PageTemplateShowAsMasterTemplate")
                                                    .OrderBy("PageTemplateDisplayName")
                                                    .Columns("PageTemplateID", "PageTemplateDisplayName", "PageTemplateDescription", "PageTemplateThumbnailGUID", "PageTemplateIconClass");

        ucSelector.DataSource = templates;
        ucSelector.IDColumn = "PageTemplateID";
        
        ucSelector.DisplayNameColumn = "PageTemplateDisplayName";
        ucSelector.DescriptionColumn = "PageTemplateDescription";
        ucSelector.ThumbnailGUIDColumn = "PageTemplateThumbnailGUID";
        ucSelector.IconClassColumn = "PageTemplateIconClass";

        ucSelector.ObjectType = PageTemplateInfo.OBJECT_TYPE;
        ucSelector.DataBind();

        if (ucSelector.SelectedId == 0)
        {
            if (!DataHelper.DataSourceIsEmpty(templates))
            {
                int firstTemplateId = ValidationHelper.GetInteger(templates.Tables[0].Rows[0]["PageTemplateID"], 0);
                ucSelector.SelectedId = firstTemplateId;
            }
        }
    }


    /// <summary>
    /// Apply control settings.
    /// </summary>
    public bool ApplySettings()
    {
        if (MasterTemplateId <= 0)
        {
            lblError.Text = GetString("TemplateSelection.SelectTemplate");
            return false;
        }
        else
        {
            // Update all culture versions
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            DataSet ds = tree.SelectNodes(SiteName, "/", TreeProvider.ALL_CULTURES, false, SystemDocumentTypes.Root, null, null, -1, false);
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // Update the document
                    TreeNode node = TreeNode.New(SystemDocumentTypes.Root, dr, tree);

                    node.SetDefaultPageTemplateID(MasterTemplateId);

                    node.Update();

                    // Update search index for node
                    if (DocumentHelper.IsSearchTaskCreationAllowed(node))
                    {
                        SearchTaskInfoProvider.CreateTask(SearchTaskTypeEnum.Update, TreeNode.OBJECT_TYPE, SearchFieldsConstants.ID, node.GetSearchID(), node.DocumentID);
                    }
                }
            }
        }

        return true;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        lblError.Visible = (lblError.Text != "");
    }
}