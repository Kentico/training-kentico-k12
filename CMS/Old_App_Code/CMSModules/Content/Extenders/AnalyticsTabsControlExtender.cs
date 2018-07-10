using CMS;
using CMS.DocumentEngine;
using CMS.UIControls;

[assembly: RegisterCustomClass("AnalyticsTabsControlExtender", typeof(AnalyticsTabsControlExtender))]

/// <summary>
/// Analytics tabs control extender
/// </summary>
public class AnalyticsTabsControlExtender : UITabsExtender
{
    #region "Variables"

    private TreeNode node;

    #endregion


    #region "Methods"

    /// <summary>
    /// Initialization of tabs.
    /// </summary>
    public override void OnInitTabs()
    {
        var page = (CMSPage)Control.Page;
        if (page == null)
        {
            return;
        }

        node = page.DocumentManager.Node;
        if (node == null)
        {
            return;
        }

        Control.OnTabCreated += OnTabCreated;
    }


    /// <summary>
    /// Fires when a tab is created and hides ui elements which should not be visible for content only classes.
    /// </summary>
    private void OnTabCreated(object sender, TabCreatedEventArgs e)
    {
        if (e.Tab == null)
        {
            return;
        }

        if (DocumentUIHelper.IsElementHiddenForNode(e.UIElement, node))
        {
            e.Tab = null;
        }
    }

    #endregion
}