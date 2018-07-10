using System;
using System.Data;

using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;

public partial class CMSWebParts_Membership_Users_OnlineUsers : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Select only top n users.
    /// </summary>
    public int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectTopN"), 0);
        }
        set
        {
            SetValue("SelectTopN", value);
        }
    }


    /// <summary>
    /// Select only users localized in specified path.  
    /// </summary>
    public string Path
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Path"), "");
        }
        set
        {
            SetValue("Path", value);
        }
    }


    /// <summary>
    /// Gets or sets the transformation name.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), "");
        }
        set
        {
            SetValue("TransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets the additional info text.
    /// </summary>
    public string AdditionalInfoText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AdditionalInfoText"), "");
        }
        set
        {
            SetValue("AdditionalInfoText", value);
        }
    }


    /// <summary>
    /// Gets or sets the no users on-line text.
    /// </summary>
    public string NoUsersOnlineText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NoUsersOnlineText"), "");
        }
        set
        {
            SetValue("NoUsersOnlineText", value);
        }
    }


    /// <summary>
    /// Gets or sets columns to be retrieved from database.
    /// </summary>
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), "");
        }
        set
        {
            SetValue("Columns", value);
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// OnPreRender override method.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SetupControl();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Setups control properties.
    /// </summary>
    protected void SetupControl()
    {
        // Check StopProcessing property
        if (StopProcessing)
        {
            Visible = false;
        }
        else
        {
            if (!ObjectFactory<ILicenseService>.StaticSingleton().IsFeatureAvailable(FeatureEnum.OnlineUsers))
            {
                ShowLicenseLimitationError();

                return;
            }

            SetContext();

            DataSet users = null;
            bool transLoaded = false;

            // Load transformation
            if (!string.IsNullOrEmpty(TransformationName))
            {
                repUsers.ItemTemplate = TransformationHelper.LoadTransformation(this, TransformationName);
                transLoaded = true;
            }

            if ((transLoaded) || (!String.IsNullOrEmpty(Path)))
            {
                // Try to get data from cache
                using (var cs = new CachedSection<DataSet>(ref users, CacheMinutes, true, CacheItemName, "onlineusers", SiteContext.CurrentSiteName, SelectTopN, Columns, Path))
                {
                    if (cs.LoadData)
                    {
                        // Get the data
                        users = SessionManager.GetOnlineUsers(null, null, SelectTopN, Columns, MacroResolver.ResolveCurrentPath(Path), SiteContext.CurrentSiteName, false, false);

                        // Prepare the cache dependency
                        if (cs.Cached)
                        {
                            cs.CacheDependency = GetCacheDependency();
                        }

                        cs.Data = users;
                    }
                }

                // Data bind
                if (!DataHelper.DataSourceIsEmpty(users))
                {
                    // Set to repeater
                    repUsers.DataSource = users;
                    repUsers.DataBind();
                }
            }

            int authenticated = 0;
            int publicUsers = 0;

            string numbers = string.Empty;

            // Get or generate cache item name
            string cacheItemNameNumbers = CacheItemName;
            if (!string.IsNullOrEmpty(cacheItemNameNumbers))
            {
                cacheItemNameNumbers += "Number";
            }

            // Try to get data from cache
            using (var cs = new CachedSection<string>(ref numbers, CacheMinutes, true, cacheItemNameNumbers, "onlineusersnumber", SiteContext.CurrentSiteName, Path))
            {
                if (cs.LoadData)
                {
                    // Get the data
                    SessionManager.GetUsersNumber(CurrentSiteName, MacroResolver.ResolveCurrentPath(Path), false, false, out publicUsers, out authenticated);

                    // Save to the cache
                    if (cs.Cached)
                    {
                        cs.CacheDependency = GetCacheDependency();
                    }

                    cs.Data = publicUsers.ToString() + ";" + authenticated.ToString();
                }
                else if (!String.IsNullOrEmpty(numbers))
                {
                    // Retrieved from cache
                    string[] nums = numbers.Split(';');

                    publicUsers = ValidationHelper.GetInteger(nums[0], 0);
                    authenticated = ValidationHelper.GetInteger(nums[1], 0);
                }
            }

            // Check if at least one user is online
            if ((publicUsers + authenticated) == 0)
            {
                ltrAdditionaInfos.Text = NoUsersOnlineText;
            }
            else
            {
                ltrAdditionaInfos.Text = string.Format(AdditionalInfoText, publicUsers + authenticated, publicUsers, authenticated);
            }
        }

        ReleaseContext();
    }


    /// <summary>
    /// Clears the cached items.
    /// </summary>
    public override void ClearCache()
    {
        string useCacheItemName = DataHelper.GetNotEmpty(CacheItemName, CacheHelper.GetCacheItemName("onlineusers", SiteContext.CurrentSiteName, SelectTopN, Columns, Path));

        CacheHelper.ClearCache(useCacheItemName);

        // Get or generate cache item name for number
        string cacheItemNameNumbers = CacheItemName;
        if (!string.IsNullOrEmpty(cacheItemNameNumbers))
        {
            cacheItemNameNumbers += "Number";
        }

        string useCacheItemNameNumber = DataHelper.GetNotEmpty(cacheItemNameNumbers, CacheHelper.GetCacheItemName("onlineusersnumber", SiteContext.CurrentSiteName, Path));
        CacheHelper.ClearCache(useCacheItemNameNumber);
    }


    private void ShowLicenseLimitationError()
    {
        lblError.Text = string.Format(GetString("licenselimitation.featurenotavailable"), FeatureEnum.OnlineUsers);
        lblError.Visible = true;
        ltrAdditionaInfos.Visible = false;
        repUsers.Visible = false;
    }

    #endregion
}