using System;

using CMS.Base;
using CMS.UIControls;
using CMS.Base.Web.UI;
using CMS.Helpers;

public partial class CMSPages_PortalTemplate : PortalPage
{
    #region "Properties"

    /// <summary>
    /// Document manager
    /// </summary>
    public override ICMSDocumentManager DocumentManager
    {
        get
        {
            // Enable document manager
            docMan.Visible = true;
            docMan.StopProcessing = false;
            return docMan;
        }
    }


    /// <summary>
    /// Returns XHTML namespace if current page has XHTML DocType. Otherwise it returns empty string.
    /// </summary>
    protected string XHtmlNameSpace
    {
        get
        {
            return DocumentBase.IsHTML5 ? String.Empty : HTMLHelper.DEFAULT_XMLNS_ATTRIBUTE;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Init the header tags
        tags.Text = HeaderTags;
    }

    #endregion
}