using System;

using CMS.Core;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSWebParts_BizForms_bizform : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the form name of BizForm.
    /// </summary>
    public string BizFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("BizFormName"), "");
        }
        set
        {
            SetValue("BizFormName", value);
        }
    }


    /// <summary>
    /// Gets or sets the alternative form full name (ClassName.AlternativeFormName).
    /// </summary>
    public string AlternativeFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternativeFormName"), "");
        }
        set
        {
            SetValue("AlternativeFormName", value);
        }
    }


    /// <summary>
    /// Gets or sets the site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SiteName"), "");
        }
        set
        {
            SetValue("SiteName", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the WebPart use colon behind label.
    /// </summary>
    public bool UseColonBehindLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseColonBehindLabel"), true);
        }
        set
        {
            SetValue("UseColonBehindLabel", value);
        }
    }


    /// <summary>
    /// Gets or sets the message which is displayed after validation failed.
    /// </summary>
    public string ValidationErrorMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ValidationErrorMessage"), "");
        }
        set
        {
            SetValue("ValidationErrorMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion track name used after successful registration.
    /// </summary>
    public string TrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TrackConversionName"), "");
        }
        set
        {
            if (value.Length > 400)
            {
                value = value.Substring(0, 400);
            }
            SetValue("TrackConversionName", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion value used after successful registration.
    /// </summary>
    public double ConversionValue
    {
        get
        {
            return ValidationHelper.GetDoubleSystem(GetValue("ConversionValue"), 0);
        }
        set
        {
            SetValue("ConversionValue", value);
        }
    }


    /// <summary>
    /// Gets or sets the default form layout.
    /// </summary>
    public string DefaultFormLayout
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DefaultFormLayout"), FormLayoutEnum.SingleTable.ToString());
        }
        set
        {
            SetValue("DefaultFormLayout", value);
        }
    }

    #endregion


    #region "Methods"

    protected override void OnLoad(EventArgs e)
    {
        viewBiz.OnAfterSave += viewBiz_OnAfterSave;
        base.OnLoad(e);
    }


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();        
    }


    /// <summary>
    /// Reloads data for partial caching.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
            viewBiz.StopProcessing = true;
        }
        else
        {
            // Set BizForm properties
            viewBiz.FormName = BizFormName;
            viewBiz.SiteName = SiteName;
            viewBiz.UseColonBehindLabel = UseColonBehindLabel;
            viewBiz.AlternativeFormFullName = AlternativeFormName;
            viewBiz.ValidationErrorMessage = ValidationErrorMessage;
            viewBiz.DefaultFormLayout = DefaultFormLayout.ToEnum<FormLayoutEnum>();

            // Set the live site context
            if (viewBiz != null)
            {
                viewBiz.ControlContext.ContextName = CMS.Base.Web.UI.ControlContext.LIVE_SITE;
            }
        }
    }


    private void viewBiz_OnAfterSave(object sender, EventArgs e)
    {
        if (TrackConversionName != String.Empty)
        {
            string siteName = SiteContext.CurrentSiteName;

            if (AnalyticsHelper.AnalyticsEnabled(siteName) && Service.Resolve<IAnalyticsConsentProvider>().HasConsentForLogging() && !AnalyticsHelper.IsIPExcluded(siteName, RequestContext.UserHostAddress))
            {
                HitLogProvider.LogConversions(SiteContext.CurrentSiteName, LocalizationContext.PreferredCultureCode, TrackConversionName, 0, ConversionValue);
            }
        }
    }

    #endregion
}