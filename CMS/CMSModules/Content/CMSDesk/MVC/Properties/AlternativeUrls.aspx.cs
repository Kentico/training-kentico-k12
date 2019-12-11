using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Internal;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;

[UIElement(RESOURCE_NAME, ELEMENT_NAME, CheckPermissions = false)]
public partial class CMSModules_Content_CMSDesk_MVC_Properties_AlternativeUrls : CMSPropertiesPage
{
    private const string RESOURCE_NAME = "CMS.Content";
    private const string ELEMENT_NAME = "Properties.AlternativeURLs";
    private const string PERMISSION_NAME = "ManageAlternativeURLs";

    private int mEditedUrlId;
    private bool? mCanManageAlternativeURLs;
    private string mSitePresentationUrl;
    private readonly Dictionary<int, CMSGridActionButton> mOpenUrlActions = new Dictionary<int, CMSGridActionButton>();


    protected bool CanManageAlternativeURLs
    {
        get
        {
            if (!mCanManageAlternativeURLs.HasValue)
            {
                mCanManageAlternativeURLs =
                    MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(RESOURCE_NAME, PERMISSION_NAME);
            }

            return mCanManageAlternativeURLs.Value;
        }
    }


    private string SitePresentationUrl
    {
        get
        {
            return mSitePresentationUrl ?? (mSitePresentationUrl = SiteContext.CurrentSite.SitePresentationURL.TrimEnd('/') + "/");
        }
    }


    protected override void OnInit(EventArgs e)
    {
        DocumentManager.RegisterEvents = false;
        DocumentManager.UseDocumentHelper = false;
        DocumentManager.RegisterSaveChangesScript = false;
        DocumentManager.CheckPermissions = false;

        base.OnInit(e);

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement(RESOURCE_NAME, ELEMENT_NAME))
        {
            RedirectToUIElementAccessDenied(RESOURCE_NAME, ELEMENT_NAME);
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        lblAltUrl.AssociatedControlClientID = txtAltUrl.TextBox.ClientID;
        txtAltUrl.PlaceholderText = SitePresentationUrl;

        gridUrls.WhereCondition = $"AlternativeUrlSiteID = {SiteContext.CurrentSiteID} AND AlternativeUrlDocumentID = {EditedDocument.DocumentID}";

        CheckPermissions();
    }


    private void CheckPermissions()
    {
        if (CanManageAlternativeURLs)
        {
            pnlCannotManageAlternativeUrl.Visible = false;
            return;
        }

        pnlAddAlternativeUrl.Visible = false;

        var errorMessage = ResHelper.GetString("cmsdesk.notauthorizedtoeditdocument");
        pnlCannotManageAlternativeUrl.Label.Text = String.Format(errorMessage, DocumentManager.Node.NodeAliasPath);

        // Remove extra grid action buttons
        var actionsToRemove = gridUrls.GridActions.Actions
            .Where(t => !t.Name.Equals("openurl", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        foreach (var action in actionsToRemove)
        {
            gridUrls.GridActions.Actions.Remove(action);
        }
    }


    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (!CanManageAlternativeURLs)
        {
            RedirectToAccessDenied(RESOURCE_NAME, PERMISSION_NAME);
        }

        var alternativeUrl = GetAlternativeUrlInfo(txtAltUrl.Text);

        // Save alternative URL or display error message if error occurred
        var error = SaveData(alternativeUrl);
        if (String.IsNullOrEmpty(error))
        {
            ShowChangesSaved();

            txtAltUrl.Text = String.Empty;
            gridUrls.ReloadData();
        }
        else
        {
            ShowError(error);
        }
    }


    protected void gridUrls_OnAction(string actionName, object actionArgument)
    {
        if (!CanManageAlternativeURLs)
        {
            RedirectToAccessDenied(RESOURCE_NAME, PERMISSION_NAME);
        }

        int urlId = ValidationHelper.GetInteger(actionArgument, 0);

        switch (actionName.ToLowerInvariant())
        {
            case "edit":
                mEditedUrlId = urlId;
                break;

            case "delete":
                AlternativeUrlInfoProvider.DeleteAlternativeUrlInfo(urlId);
                DocumentManager.SaveDocument();
                break;
        }
    }


    protected object gridUrls_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerInvariant())
        {
            case "openurl":
                {
                    var row = (DataRowView)((GridViewRow)parameter).DataItem;
                    var altUrlId = DataHelper.GetIntValue(row.Row, "AlternativeUrlID");

                    mOpenUrlActions[altUrlId] = (CMSGridActionButton)sender;
                }
                break;

            case "url":
                {
                    var row = (DataRowView)parameter;
                    var altUrl = DataHelper.GetStringValue(row.Row, "AlternativeUrlUrl");
                    var altUrlId = DataHelper.GetIntValue(row.Row, "AlternativeUrlID");

                    var completeUrl = $"{SitePresentationUrl}{altUrl}";

                    SetOpenUrlAction(altUrlId, completeUrl);

                    if (!CanManageAlternativeURLs)
                    {
                        return HTMLHelper.HTMLEncode(completeUrl);
                    }

                    var inlineUrl = new InlineEditingTextBox
                    {
                        Text = altUrl,
                        FormattedText = completeUrl,
                        MaxLength = 450,
                        AdditionalCssClass = "inline-editing-textbox-wide",
                        EnableEditingOnTextClick = false
                    };

                    inlineUrl.Update += (s, e) => UpdateAlternativeUrlFromInline(inlineUrl, altUrlId);

                    if (altUrlId == mEditedUrlId)
                    {
                        inlineUrl.Load += (s, e) => inlineUrl.SwitchToEditMode();
                    }

                    return inlineUrl;
                }
        }

        return parameter;
    }

    /// <summary>
    /// EventHandler called when <paramref name="inlineUrl"/> was edited by user
    /// </summary>
    private void UpdateAlternativeUrlFromInline(InlineEditingTextBox inlineUrl, int altUrlId)
    {
        var alternativeUrl = GetAlternativeUrlInfo(inlineUrl.Text, altUrlId);
        if (alternativeUrl == null)
        {
            return;
        }

        var error = SaveData(alternativeUrl);
        if (!string.IsNullOrEmpty(error))
        {
            inlineUrl.ErrorText = error;
            ShowError(error);

            return;
        }

        var completeUrl = $"{SitePresentationUrl}{alternativeUrl.AlternativeUrlUrl}";

        SetOpenUrlAction(altUrlId, completeUrl);

        inlineUrl.FormattedText = completeUrl;

        ShowChangesSaved();

        // Force redraw of full grid to make sure sorting is correct after inline edit
        gridUrls.ReloadData();
    }

    /// <summary>
    /// Returns <see cref="String.Empty"/> when <paramref name="alternativeUrl"/> was saved.
    /// If error occurs during saving the <paramref name="alternativeUrl"/>, corresponding error message is returned.
    /// </summary>
    private string SaveData(AlternativeUrlInfo alternativeUrl)
    {
        if (String.IsNullOrWhiteSpace(alternativeUrl.AlternativeUrlUrl.NormalizedUrl))
        {
            return GetString("general.requiresvalue");
        }

        try
        {
            AlternativeUrlInfoProvider.SetInfoCheckForConflictingPage(alternativeUrl);
        }
        catch (InvalidAlternativeUrlException ex) when (ex.ConflictingPage != null)
        {
            return String.Format(GetString("alternativeurl.isinconflictwithpage"), alternativeUrl.AlternativeUrlUrl, AlternativeUrlHelper.GetDocumentIdentification(ex.ConflictingPage.DocumentNamePath, ex.ConflictingPage.DocumentCulture));
        }
        catch (InvalidAlternativeUrlException ex) when (ex.ConflictingAlternativeUrl != null)
        {
            var page = DocumentHelper.GetDocument(ex.ConflictingAlternativeUrl.AlternativeUrlDocumentID, new TreeProvider());
            return String.Format(GetString("alternativeurl.isinconflictwithalternativeurl"), alternativeUrl.AlternativeUrlUrl, AlternativeUrlHelper.GetDocumentIdentification(page.DocumentNamePath, page.DocumentCulture));
        }
        catch (InvalidAlternativeUrlException ex) when (!String.IsNullOrEmpty(ex.ExcludedUrl) || ex.AlternativeUrl != null)
        {
            var errorMessage = SettingsKeyInfoProvider.GetValue("CMSAlternativeURLsErrorMessage", SiteContext.CurrentSiteID);
            return String.IsNullOrEmpty(errorMessage) ?
                GetString("settingskey.cmsalternativeurlsfallbackerrormessage") :
                ResHelper.LocalizeString(errorMessage);
        }
        catch (InvalidAlternativeUrlException)
        {
            return String.Format(GetString("general.mustbeunique"), TypeHelper.GetNiceObjectTypeName(AlternativeUrlInfo.OBJECT_TYPE));
        }
        catch (RegexMatchTimeoutException)
        {
            return String.Format(GetString("settingskey.cmsalternativeurlstimeouterrormessage"), alternativeUrl.AlternativeUrlUrl);
        }

        DocumentManager.SaveDocument();

        return String.Empty;
    }


    private AlternativeUrlInfo GetAlternativeUrlInfo(string url, int altUrlId = 0)
    {
        AlternativeUrlInfo alternativeUrl;

        if (altUrlId > 0)
        {
            alternativeUrl = AlternativeUrlInfoProvider.GetAlternativeUrlInfo(altUrlId);
            if (alternativeUrl == null)
            {
                return null;
            }
        }
        else
        {
            alternativeUrl = new AlternativeUrlInfo
            {
                AlternativeUrlDocumentID = EditedDocument.DocumentID,
                AlternativeUrlSiteID = SiteContext.CurrentSiteID
            };
        }

        alternativeUrl.AlternativeUrlUrl = AlternativeUrlHelper.NormalizeAlternativeUrl(url);

        return alternativeUrl;
    }

    /// <summary>
    /// Sets 'onclick' JS action on grid action button to open new window
    /// </summary>
    /// <param name="altUrlId">Alternative Url ID</param>
    /// <param name="url">Un-Escaped plain-text full Url</param>
    private void SetOpenUrlAction(int altUrlId, string url)
    {
        if (mOpenUrlActions.TryGetValue(altUrlId, out CMSGridActionButton button))
        {
            var escapedUrl = Uri.EscapeUriString(url);
            button.OnClientClick = $"window.open(\"{escapedUrl}\"); return false;";
        }
    }
}