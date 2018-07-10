using System;
using System.Web.UI.WebControls;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_View_ViewFile : CMSContentPage
{
    #region "Page events"

    protected override void OnPreInit(EventArgs e)
    {
        ((Panel)CurrentMaster.PanelBody.FindControl("pnlContent")).CssClass = "";
        menuElem.ShowSave = false;
        menuElem.HandleWorkflow = true;
        
        base.OnPreInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Update viewmode
        PortalContext.UpdateViewMode(ViewModeEnum.Edit);

        lblFileSize.Text = GetString("ViewFile.FileSize");

        if (Node != null)
        {
            // Get guid
            Guid guid = ValidationHelper.GetGuid(Node.GetValue("FileAttachment"), Guid.Empty);

            //Get latest version
            if (guid != Guid.Empty)
            {
                var atInfo = DocumentHelper.GetAttachment(Node, guid, false);
                if (atInfo != null)
                {
                    // If file exist, check filetype and create texts and links
                    lblFileSizeText.Text = atInfo.AttachmentSize.ToString();
                    lblFileNameText.Text = atInfo.AttachmentName;

                    // Get attachment URL
                    string attUrl;
                    if (Node.IsFile())
                    {
                        attUrl = "~/CMSPages/GetFile.aspx?nodeguid=" + Node.NodeGUID;
                    }
                    else
                    {
                        int versionHistoryId = Node.DocumentCheckedOutVersionHistoryID;
                        attUrl = DocumentHelper.GetAttachmentUrl(atInfo, versionHistoryId);
                    }
                    attUrl = URLHelper.UpdateParameterInUrl(attUrl, URLHelper.LanguageParameterName, CultureCode);
                    // Setup the display information
                    if (ImageHelper.IsImage(atInfo.AttachmentExtension))
                    {
                        if ((atInfo.AttachmentImageWidth != 0) && (atInfo.AttachmentImageHeight != 0))
                        {
                            // Image, show preview
                            plcSize.Visible = true;
                            lblSize.Text = GetString("ViewFile.Size");
                            lblSizeText.Text = atInfo.AttachmentImageWidth + "x" + atInfo.AttachmentImageHeight;
                        }

                        plcImage.Visible = true;
                        imgPreview.ImageUrl = attUrl + "&maxsidesize=600";

                        lnkView.NavigateUrl = attUrl;
                        lnkView.Text = GetString("ViewFile.OpenInFull");
                    }
                    else
                    {
                        // Document, open link
                        lnkView.Text = GetString("ViewFile.Open");
                        lnkView.NavigateUrl = attUrl;
                    }

                    // Register js synchronization script for split mode
                    if (PortalUIHelper.DisplaySplitMode)
                    {
                        RegisterSplitModeSync(true, false);
                    }
                }
            }
        }
    }

    #endregion
}