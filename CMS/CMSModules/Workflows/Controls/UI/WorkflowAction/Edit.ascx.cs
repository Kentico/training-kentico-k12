using System;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.PortalEngine.Web.UI;
using CMS.UIControls;
using CMS.WorkflowEngine;


public partial class CMSModules_Workflows_Controls_UI_WorkflowAction_Edit : CMSAdminEditControl
{
    #region "Properties"

    /// <summary>
    /// Forced allowed objects for action.
    /// </summary>
    public string AllowedObjects
    {
        get;
        set;
    }


    /// <summary>
    /// Current object type.
    /// </summary>
    public string ObjectType
    {
        get
        {
            return formElem.ObjectType;
        }
        set
        {
            formElem.ObjectType = value;
        }
    }


    /// <summary>
    /// Edited workflow action.
    /// </summary>
    public WorkflowActionInfo CurrentAction
    {
        get
        {
            return (WorkflowActionInfo)formElem.EditedObject;
        }
    }


    /// <summary>
    /// UIForm control
    /// </summary>
    public UIForm EditForm
    {
        get
        {
            return formElem;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        ucThumbnail.SetValue("IconCssFieldName", "ActionThumbnailClass");
        ucThumbnail.SetValue("Category", ObjectAttachmentsCategories.THUMBNAIL);

        ucIcon.SetValue("IconCssFieldName", "ActionIconClass");
        ucIcon.SetValue("Category", ObjectAttachmentsCategories.ICON);

        assemblyElem.BaseClassNames = (formElem.ObjectType == WorkflowActionInfo.OBJECT_TYPE_AUTOMATION) ? "CMS.Automation.AutomationAction, CMS.Automation" : "CMS.DocumentEngine.DocumentWorkflowAction, CMS.DocumentEngine";

        pnlImages.Visible = (formElem.Mode != FormModeEnum.Insert);

        if (!String.IsNullOrEmpty(AllowedObjects))
        {
            // Ensure correct allowed objects format
            CurrentAction.ActionAllowedObjects = ";" + AllowedObjects.Trim(';') + ";";
        }
    }

    #endregion
}
