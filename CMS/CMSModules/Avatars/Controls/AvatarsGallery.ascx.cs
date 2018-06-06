using System;
using System.Linq;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Avatars_Controls_AvatarsGallery : CMSAdminEditControl
{
    #region "Variables"

    private AvatarTypeEnum mAvatarType = AvatarTypeEnum.All;
    private const int maxSideSize = 100;

    public string avatarUrl = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Avatar type.
    /// </summary>
    public AvatarTypeEnum AvatarType
    {
        get
        {
            return mAvatarType;
        }
        set
        {
            mAvatarType = value;
        }
    }

    #endregion


    #region "Events and public methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        pgrAvatars.PagedControl = repAvatars;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterWOpenerScript(Page);

        // Get resource strings        
        Page.Title = GetString("avatars.gallery.title");
        lblInfo.ResourceString = "avat.noavatarsfound";

        // Resolve avatar url
        avatarUrl = ResolveUrl("~/CMSPages/GetAvatar.aspx?maxsidesize=" + maxSideSize + "&avatarguid=");
        avatarUrl = HTMLHelper.EncodeForHtmlAttribute(avatarUrl);
        // Get avatar type from querystring
        string avatarType = QueryHelper.GetString("avatartype", "all");
        AvatarType = AvatarInfoProvider.GetAvatarTypeEnum(avatarType);
        
        // Get all avatars form database
        var data = getAvatars();
        if (data.Any())
        {
            repAvatars.DataSource = data;
            repAvatars.DataBind();
            pnlAvatars.Visible = true;
        }
        else
        {
            lblInfo.Visible = true;
            repAvatars.Visible = false;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Get safe client ID from querystring
        string clientID = QueryHelper.GetControlClientId("clientid", string.Empty);

        // Javacript code to mark selected pictures and to fill hidden input value
        string avatarScript = ScriptHelper.GetScript(
            @"function markImage(id) {
                hidden = $cmsj('#" + hiddenAvatarGuid.ClientID + @"');
                if (hidden.val() != '') {
                    img = $cmsj('#' + hidden.val());
                    img.parent().toggleClass('avatar-list-item-selected');
                }
                img = $cmsj('#' + id);
                img.parent().toggleClass('avatar-list-item-selected');
                fillHiddenfield(id);
            }
            
            function fillHiddenfield(id)
            {
                hidden = $cmsj('#" + hiddenAvatarGuid.ClientID + @"');
                hidden.val(id);
            }");

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "avatarScript", avatarScript);

        // Javacript code which handles window closing and assignment of guid to parent window 
        string addToHiddenScript = ScriptHelper.GetScript(
            @"function CloseAndRefresh() {                 
                if((wopener!=null) && (wopener.UpdateForm!=null))
                { 
                    wopener.UpdateForm();
                }                
                CloseDialog();
              }

              function addToHidden() {
                 hidden = document.getElementById('" + hiddenAvatarGuid.ClientID + @"'); 
                 wopener." + clientID + "updateHidden(hidden.value, '" + clientID + @"');
                 CloseAndRefresh(); 
              }");

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "addToHiddenAndClose", addToHiddenScript);
    }


    /// <summary>
    ///  Creates URL for.
    /// </summary>
    /// <param name="param">URL paramater</param>
    /// <param name="number">Value</param>
    public string CreateUrl(string param, object number)
    {
        return HTMLHelper.EncodeForHtmlAttribute(URLHelper.UpdateParameterInUrl(RequestContext.CurrentURL, param, ValidationHelper.GetString(number, "1")));
    }


    /// <summary>
    /// Indicates if there are avatars to be displayed.
    /// </summary>
    public bool HasData()
    {
        return repAvatars.HasData();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Gets avatars from database.
    /// </summary>
    private ObjectQuery<AvatarInfo> getAvatars()
    {
        // Get the data
        int currentPage = QueryHelper.GetInteger("tpage", pgrAvatars.CurrentPage);

        int currentGroup = currentPage / pgrAvatars.GroupSize + 1;
        int topN = currentGroup * pgrAvatars.GroupSize * pgrAvatars.UniPager.PageSize + pgrAvatars.UniPager.PageSize;
        // Execute query
        switch (AvatarType)
        {
            case AvatarTypeEnum.Group:
                return AvatarInfoProvider.GetAvatars().Where("AvatarType IN ('group','all') AND AvatarIsCustom = 0").TopN(topN).Columns("AvatarName, AvatarGUID");

            case AvatarTypeEnum.User:
                return AvatarInfoProvider.GetAvatars().Where("AvatarType IN ('user','all') AND AvatarIsCustom = 0").TopN(topN).Columns("AvatarName, AvatarGUID");

            default:
                return AvatarInfoProvider.GetAvatars().Where("AvatarType = 'all' AND AvatarIsCustom = 0").TopN(topN).Columns("AvatarName, AvatarGUID");
        }
    }

    #endregion
}
