using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_CMSPages_MessageUserSelector_Header : LivePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Add styles
        RegisterDialogCSSLink();

        PageTitle.TitleText = GetString("Messaging.MessageUserSelector.HeaderCaption");
        if (!MembershipContext.AuthenticatedUser.IsPublic())
        {
            int selectedTab = 2;
            CurrentMaster.Tabs.Visible = true;

            if (QueryHelper.GetString("showtab", String.Empty).ToLowerCSafe() != "search")
            {
                selectedTab = 0;
                // ContactList tab
                CurrentMaster.Tabs.AddTab(new UITabItem()
                {
                    Text = GetString("Messaging.MessageUserSelector.ContactList"),
                    RedirectUrl = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/CMSPages/MessageUserSelector_ContactList.aspx") + "?hidid=" +
                             QueryHelper.GetText("hidid", String.Empty) +
                             "&mid=" +
                             QueryHelper.GetText("mid", String.Empty),
                });

                // Show only if community module is present
                if (ModuleManager.IsModuleLoaded(ModuleName.COMMUNITY) && UIHelper.IsFriendsModuleEnabled(SiteContext.CurrentSiteName))
                {
                    // Friends tab
                    CurrentMaster.Tabs.AddTab(new UITabItem()
                    {
                        Text = GetString("friends.friends"),
                        RedirectUrl = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/CMSPages/MessageUserSelector_FriendsList.aspx") + "?hidid=" +
                                     QueryHelper.GetText("hidid", String.Empty) +
                                     "&mid=" +
                                     QueryHelper.GetText("mid", String.Empty),
                    });
                }
            }

            // Search tab
            CurrentMaster.Tabs.AddTab(new UITabItem()
            {
                Text = GetString("general.search"),
                RedirectUrl = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/CMSPages/MessageUserSelector_Search.aspx") + "?refresh=" +
                         QueryHelper.GetText("refresh", String.Empty) +
                         "&hidid=" +
                         QueryHelper.GetText("hidid", String.Empty) +
                         "&mid=" +
                         QueryHelper.GetText("mid", String.Empty),
            });

            CurrentMaster.Tabs.SelectedTab = selectedTab;
            CurrentMaster.Tabs.UrlTarget = "MessageUserSelectorContent";
        }
    }
}