using System;

using CMS.Base.Web.UI.ActionsConfig;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineMarketing.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


[Security(Resource = "CMS.ABTest", UIElements = "ABTestListing")]
[UIElement("CMS.ABTest", "ABTestListing")]
public partial class CMSModules_OnlineMarketing_Pages_Content_ABTesting_ABTest_List : CMSABTestPage
{
    /// <summary>
    /// Smart tip identifiers. If this smart tip is collapsed, this ID is stored in DB.
    /// </summary>
    private const string SMART_TIP_IDENTIFIER = "howtovideo|abtest|listing";
    private const string SMART_TIP_IDENTIFIER_MVC = "howtovideo|abtest|mvclisting";


    /// <summary>
    /// If true, the items are edited in dialog
    /// </summary>
    private bool EditInDialog
    {
        get
        {
            return listElem.Grid.EditInDialog;
        }
        set
        {
            listElem.Grid.EditInDialog = value;
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ucDisabledModule.TestSettingKeys = SiteContext.CurrentSite.SiteIsContentOnly ? "CMSABTestingEnabled" : "CMSAnalyticsEnabled;CMSABTestingEnabled";

        EditInDialog = QueryHelper.GetBoolean("editindialog", false);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ucDisabledModule.ParentPanel = pnlDisabled;

        InitHeaderActions();
        InitTitle();
        InitSmartTip();
    }


    /// <summary>
    /// Initializes header actions.
    /// </summary>
    private void InitHeaderActions()
    {
        if (!SiteContext.CurrentSite.SiteIsContentOnly && MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.ABTest", "New"))
        {
            string url = UIContextHelper.GetElementUrl("CMS.ABTest", "New", EditInDialog);

            // Get the alias path of the current node, if in content
            if (Node != null)
            {
                listElem.NodeID = Node.NodeID;
                listElem.ShowOriginalPageColumn = false;
                string aliasPath = Node.NodeAliasPath;
                listElem.AliasPath = aliasPath;

                url = URLHelper.AddParameterToUrl(url, "NodeID", Node.NodeID.ToString());
                url = URLHelper.AddParameterToUrl(url, "AliasPath", aliasPath);
            }

            url = ResolveUrl(url);

            // Set header action
            var action = new HeaderAction
            {
                ResourceName = "CMS.ABTest",
                Permission = "Manage",
                Text = GetString("abtesting.abtest.new"),
                RedirectUrl = url,
                OpenInDialog = EditInDialog
            };

            CurrentMaster.HeaderActions.AddAction(action);
        }
    }


    /// <summary>
    /// Sets title if not in content.
    /// </summary>
    private void InitTitle()
    {
        if (NodeID <= 0)
        {
            SetTitle(GetString("analytics_codename.abtests"));
        }
    }


    /// <summary>
    /// Initialize the smart tip with the how to video.
    /// Shows how to video.
    /// </summary>
    private void InitSmartTip()
    {
        tipHowToListing.ExpandedHeader = GetString("abtesting.howto.howtosetupabtest.title");

        if (SiteContext.CurrentSite.SiteIsContentOnly)
        {
            tipHowToListing.CollapsedStateIdentifier = SMART_TIP_IDENTIFIER_MVC;
            tipHowToListing.Content = String.Format(GetString("abtesting.howto.howtosetupabtestmvc.text"), DocumentationHelper.GetDocumentationTopicUrl("ab_testing_mvc"));
        }
        else
        {
            var linkBuilder = new MagnificPopupYouTubeLinkBuilder();
            var linkID = Guid.NewGuid().ToString();
            var link = linkBuilder.GetLink("2wU7rNzC95w", linkID, GetString("abtesting.howto.howtosetupabtest.link"));

            new MagnificPopupYouTubeJavaScriptRegistrator().RegisterMagnificPopupElement(this, linkID);

            tipHowToListing.CollapsedStateIdentifier = SMART_TIP_IDENTIFIER;
            tipHowToListing.Content = String.Format("{0} {1}", GetString("abtesting.howto.howtosetupabtest.text"), link);
        }
    }
}