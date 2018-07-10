using System;
using System.Data;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.IO;
using CMS.Modules;
using CMS.UIControls;

// New document type action
[Action(0, "DocumentType_List.NewDoctype", "DocumentType_New.aspx")]
public partial class CMSModules_DocumentTypes_Pages_Development_DocumentType_List : GlobalAdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterExportScript();

        // Unigrid initialization
        uniGrid.OnExternalDataBound += uniGrid_OnExternalDataBound;
        uniGrid.OnAction += uniGrid_OnAction;
        uniGrid.ZeroRowsText = GetString("general.nodatafound");
    }


    /// <summary>
    /// Handles the UniGrid's OnExternalDataBound event.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="sourceName">Source name</param>
    /// <param name="parameter">Parameter</param>
    protected object uniGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        if (sourceName == "DataType")
        {
            var dataClass = parameter as DataRowView;
            if (dataClass == null)
            {
                return string.Empty;
            }

            var isContainer = !ValidationHelper.GetBoolean(dataClass.Row["ClassIsCoupledClass"], false);
            if (isContainer)
            {
                return GetString("documenttype.type.container");
            }

            var isContentOnly = ValidationHelper.GetBoolean(dataClass.Row["ClassIsContentOnly"], false);
            return GetString(isContentOnly ? "documenttype.type.contentonly" : "documenttype.type.page");
        }

        return parameter;
    }


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that throws event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void uniGrid_OnAction(string actionName, object actionArgument)
    {
        if (actionName == "edit")
        {
            URLHelper.Redirect(GetEditUrl(actionArgument.ToString()));
        }
        else if (actionName == "delete")
        {
            int classId = ValidationHelper.GetInteger(actionArgument, 0);
            var dci = DataClassInfoProvider.GetDataClassInfo(classId);
            if (dci == null)
            {
                return;
            }

            // Delete dataclass and its dependencies
            try
            {
                string className = dci.ClassName;
                DataClassInfoProvider.DeleteDataClassInfo(dci);

                // Delete icons
                string iconFile = UIHelper.GetDocumentTypeIconPath(this, className, "", false);
                string iconLargeFile = UIHelper.GetDocumentTypeIconPath(this, className, "48x48", false);
                iconFile = Server.MapPath(iconFile);
                iconLargeFile = Server.MapPath(iconLargeFile);

                if (File.Exists(iconFile))
                {
                    File.Delete(iconFile);
                }
                // Ensure that ".gif" file will be deleted
                iconFile = iconFile.Replace(".png", ".gif");

                if (File.Exists(iconFile))
                {
                    File.Delete(iconFile);
                }

                if (File.Exists(iconLargeFile))
                {
                    File.Delete(iconLargeFile);
                }
                // Ensure that ".gif" file will be deleted
                iconLargeFile = iconLargeFile.Replace(".png", ".gif");
                if (File.Exists(iconLargeFile))
                {
                    File.Delete(iconLargeFile);
                }
            }
            catch (CheckDependenciesException)
            {
                var description = uniGrid.GetCheckDependenciesDescription(dci);
                ShowError(GetString("unigrid.deletedisabledwithoutenable"), description);
            }
            catch (Exception ex)
            {
                LogAndShowError("Development", "DeleteDocType", ex);
            }
        }
    }

    #region "Private methods"

    /// <summary>
    /// Creates URL for editing.
    /// </summary>
    /// <param name="documentTypeId">Document type identifier</param>
    private String GetEditUrl(string documentTypeId)
    {
        UIElementInfo uiChild = UIElementInfoProvider.GetUIElementInfo("CMS.DocumentEngine", "EditDocumentType");
        if (uiChild != null)
        {
            return URLHelper.AppendQuery(UIContextHelper.GetElementUrl(uiChild, UIContext), "displaytitle=false&objectid=" + documentTypeId);
        }

        return String.Empty;
    }

    #endregion
}