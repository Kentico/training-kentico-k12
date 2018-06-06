using System;

using CMS.Membership;
using CMS.OnlineMarketing;
using CMS.OnlineMarketing.Web.UI;
using CMS.UIControls;

// Edited object
[EditedObject(MVTVariantInfo.OBJECT_TYPE, "variantid")]
// Breadcrumbs
[Breadcrumbs()]
[Breadcrumb(0, "mvtvariant.list", "~/CMSModules/OnlineMarketing/Pages/Content/MVTVariant/List.aspx?nodeid={?nodeid?}", null)]
[Breadcrumb(1, Text = "{%EditedObject.DisplayName%}", ExistingObject = true)]
[Breadcrumb(1, ResourceString = "mvtvariant.new", NewObject = true)]
// Context help
[Help("mvtvariant_edit")]
public partial class CMSModules_OnlineMarketing_Pages_Content_MVTVariant_Edit : CMSMVTestPage
{
    #region "Page events"

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check UI Permissions
        if ((MembershipContext.AuthenticatedUser == null) || (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Content", "OnlineMarketing.MVTVariants")))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "OnlineMarketing.MVTVariants");
        }

        // Set disabled module info
        ucDisabledModule.ParentPanel = pnlDisabled;
    }

    #endregion
}