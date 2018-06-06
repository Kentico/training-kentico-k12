using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Dialogs_MessageUserSelector_Header : CMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Messaging);

        PageTitle.TitleText = GetString("Messaging.MessageUserSelector.HeaderCaption");
        if (!MembershipContext.AuthenticatedUser.IsPublic())
        {
            CurrentMaster.Tabs.Visible = true;
            bool communityModuleLoaded = ModuleManager.IsModuleLoaded(ModuleName.COMMUNITY);
            bool showOnlySearch = (QueryHelper.GetString("showtab", String.Empty).ToLowerCSafe() != "search");


            if (showOnlySearch)
            {
                // ContactList tab
                CurrentMaster.Tabs.AddTab(new UITabItem
                {
                    Text = GetString("Messaging.MessageUserSelector.ContactList"),
                    RedirectUrl = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/Dialogs/MessageUserSelector_ContactList.aspx") + "?hidid=" +
                                    QueryHelper.GetText("hidid", String.Empty) +
                                    "&mid=" +
                                    QueryHelper.GetText("mid", String.Empty),
                });

                // Show only if community module is present
                if (communityModuleLoaded && UIHelper.IsFriendsModuleEnabled(SiteContext.CurrentSiteName))
                {
                    // Friends tab
                    CurrentMaster.Tabs.AddTab(new UITabItem
                    {
                        Text = GetString("friends.friends"),
                        RedirectUrl = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/Dialogs/MessageUserSelector_FriendsList.aspx") + "?hidid=" +
                                        QueryHelper.GetText("hidid", String.Empty) +
                                        "&mid=" +
                                        QueryHelper.GetText("mid", String.Empty),
                    });
                }
            }

            // Search tab
            CurrentMaster.Tabs.AddTab(new UITabItem
            {
                Text = GetString("general.search"),
                RedirectUrl = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/Dialogs/MessageUserSelector_Search.aspx") + "?refresh=" +
                                QueryHelper.GetText("refresh", String.Empty) +
                                "&hidid=" +
                                QueryHelper.GetText("hidid", String.Empty) +
                                "&mid=" +
                                QueryHelper.GetText("mid", String.Empty),
            });

            CurrentMaster.Tabs.SelectedTab = 0;
            CurrentMaster.Tabs.UrlTarget = "MessageUserSelectorContent";
        }
    }
}