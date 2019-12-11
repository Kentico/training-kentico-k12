using System;

using CMS.Base;

using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_TemplateSelection : CMSContentPage
{
    #region "Properties"

    /// <summary>
    /// Local messages placeholder to ensure correct positioning
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
        set
        {
            plcMess = value;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        if (RequiresDialog)
        {
            // Use simple master page if dialog mode required
            Page.MasterPageFile = ResolveUrl("~/CMSMasterPages/UI/Dialogs/ModalSimplePage.master");
        }
    }

    
    protected override void OnInit(EventArgs e)
    {
        int classId = QueryHelper.GetInteger("classId", 0);
        
        // Load the root category of the selector
        DataClassInfo dci = DataClassInfoProvider.GetDataClassInfo(classId);
        if (dci != null)
        {
            selTemplate.RootCategoryID = dci.ClassPageTemplateCategoryID;
            if (!RequestHelper.IsPostBack() && (dci.ClassDefaultPageTemplateID > 0))
            {
                PageTemplateInfo pageTemplate = PageTemplateInfoProvider.GetPageTemplateInfo(dci.ClassDefaultPageTemplateID);
                if ((pageTemplate != null) && PageTemplateInfoProvider.IsPageTemplateOnSite(pageTemplate.PageTemplateId, SiteContext.CurrentSiteID))
                {
                    bool setDefaultTemplate = true;

                    // If different template categories are used
                    if (pageTemplate.CategoryID != dci.ClassPageTemplateCategoryID)
                    {
                        // Get categories info
                        PageTemplateCategoryInfo templateCategory = PageTemplateCategoryInfoProvider.GetPageTemplateCategoryInfo(pageTemplate.CategoryID);
                        PageTemplateCategoryInfo rootCategory = PageTemplateCategoryInfoProvider.GetPageTemplateCategoryInfo(dci.ClassPageTemplateCategoryID);

                        // Check if selected root category is same as one of parent page template categories or has no parent category
                        if ((rootCategory.ParentId != 0) && !templateCategory.CategoryPath.Contains("/" + rootCategory.CategoryName + "/"))
                        {
                            setDefaultTemplate = false;
                        }
                    }

                    if (setDefaultTemplate)
                    {
                        selTemplate.SetDefaultTemplate(dci.ClassDefaultPageTemplateID);
                    }
                }
            }

            // Display footer only in dialog mode
            pnlFooterContent.Visible = RequiresDialog;

            var titleText = String.Format(GetString("content.newdocument"), dci.ClassDisplayName);
            
            // Set dialog title
            if (RequiresDialog)
            {
                SetTitle(HTMLHelper.HTMLEncode(titleText));
            }
            else
            {
                titleElem.TitleText = HTMLHelper.HTMLEncode(titleText);
                EnsureDocumentBreadcrumbs(titleElem.Breadcrumbs, action: titleElem.TitleText);
            }
        }

        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        DocumentManager.Mode = FormModeEnum.Insert;
        DocumentManager.ParentNodeID = QueryHelper.GetInteger("parentnodeid", 0);
        DocumentManager.NewNodeCultureCode = QueryHelper.GetString("parentculture", null);
        DocumentManager.NewNodeClassID = QueryHelper.GetInteger("classid", 0);

        // Add the continue action
        menuElem.ShowSave = false;
        menuElem.AddExtraAction(new HeaderAction
        {
            Text = GetString("General.Continue"),
            Tooltip = GetString("General.Continue"),
            CommandName = "Continue"
        });

        menuElem.HeaderActions.ActionPerformed += Menu_ActionPerformed;

        if (DocumentManager.ParentNode != null)
        {
            selTemplate.DocumentID = DocumentManager.ParentNode.DocumentID;
            selTemplate.ParentNodeID = DocumentManager.ParentNodeID;
        }
    }


    protected void Menu_ActionPerformed(object sender, CommandEventArgs e)
    {
        if (e.CommandName.EqualsCSafe("Continue", true))
        {
            string errorMessage = null;

            // Ensure the page template
            PageTemplateInfo pti = selTemplate.EnsureTemplate(null, Guid.Empty, ref errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ShowError(errorMessage);
            }
            else
            {
                // Get the template ID
                int templateId = 0;
                if (pti != null)
                {
                    templateId = pti.PageTemplateId;
                }

                URLHelper.Redirect("~/CMSModules/Content/CMSDesk/Edit/edit.aspx" + RequestContext.CurrentQueryString + "&templateidentifier=" + templateId);
            }

        }
    }

    #endregion
}