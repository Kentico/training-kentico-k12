using System;

using CMS.Base.Web.UI;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_MasterPage_PageEdit : CMSContentPage
{
    #region "Variables"

    private CurrentUserInfo mUser;

    #endregion


    #region "Methods"

    protected override void CreateChildControls()
    {
        // Enable split mode
        EnableSplitMode = true;

        mUser = MembershipContext.AuthenticatedUser;
        if (Node != null)
        {
            UIContext.EditedObject = Node;
            ucHierarchy.PreviewObjectName = Node.NodeAliasPath;
            ucHierarchy.AddContentParameter(new UILayoutValue("PreviewObject", Node));
            ucHierarchy.DefaultAliasPath = Node.NodeAliasPath;
            ucHierarchy.IgnoreSessionValues = true;
        }

        base.CreateChildControls();
    }

    protected override void OnInit(EventArgs e)
    {
        // Check UIProfile
        if (!mUser.IsAuthorizedPerUIElement("CMS.Design", "MasterPage"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "MasterPage");
        }

        // Check "Design" permission
        if (!mUser.IsAuthorizedPerResource("CMS.Design", "Design"))
        {
            RedirectToAccessDenied("CMS.Design", "Design");
        }

        ucHierarchy.RegisterEnvelopeClientID();
        base.OnInit(e);

        UIHelper.SetBreadcrumbsSuffix(GetString("content.ui.page"));
    }

    #endregion
}
