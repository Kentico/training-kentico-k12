using System;

using CMS.Base.Web.UI;
using CMS.Community.Web.UI;
using CMS.Forums;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Groups_Tools_Forums_Posts_ForumPost_View : CMSGroupForumPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        postView.PostID = QueryHelper.GetInteger("postid", 0);
        postView.Reply = QueryHelper.GetInteger("reply", 0);
        postView.ForumID = QueryHelper.GetInteger("forumId", 0);
        postView.ListingPost = QueryHelper.GetString("listingpost", String.Empty);
        postView.IsLiveSite = false;

        // Register back to listing script
        if (!String.IsNullOrEmpty(postView.ListingPost))
        {
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "BackToListing", ScriptHelper.GetScript(
                "function BackToListing() { location.href = '" + ResolveUrl("~/CMSModules/Groups/Tools/Forums/Posts/ForumPost_Listing.aspx?postid=" + ScriptHelper.GetString(postView.ListingPost, false)) + "'; }\n"));
        }

        postView.OnCheckPermissions += postView_OnCheckPermissions;

        InitializeMasterPage();
    }


    protected void postView_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        int groupId = 0;
        ForumPostInfo fpi = ForumPostInfoProvider.GetForumPostInfo(postView.PostID);
        if (fpi != null)
        {
            ForumInfo fi = ForumInfoProvider.GetForumInfo(fpi.PostForumID);
            if (fi != null)
            {
                ForumGroupInfo fgi = ForumGroupInfoProvider.GetForumGroupInfo(fi.ForumGroupID);
                if (fgi != null)
                {
                    groupId = fgi.GroupGroupID;
                }
            }
        }

        CheckGroupPermissions(groupId, CMSAdminControl.PERMISSION_MANAGE);
    }


    /// <summary>
    /// Initializes MasterPage.
    /// </summary>
    protected void InitializeMasterPage()
    {
        Title = "Forum Post View";
        string listingParam = null;

        if (!String.IsNullOrEmpty(postView.ListingPost))
        {
            listingParam = "+ '&listingpost=" + HTMLHelper.HTMLEncode(postView.ListingPost) + "'";
        }

        // Register script
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "EditPost",
                                               ScriptHelper.GetScript("function EditPost(postId) { " +
                                                                      "if ( postId != 0 ) { parent.frames['posts_edit'].location.href = 'ForumPost_Edit.aspx?postid=' + postId" + listingParam + ";}}"));
    }
}
