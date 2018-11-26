using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineMarketing;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;

using TreeNode = CMS.DocumentEngine.TreeNode;

public partial class CMSModules_OnlineMarketing_Controls_Content_CombinationPanel : CMSAbstractPortalUserControl, ICallbackEventHandler
{
    #region "Variables"

    private const string ITEM_ENABLED_CLASS = "DropDownItemEnabled";
    private const string ITEM_DISABLED_CLASS = "DropDownItemDisabled";
    private string callbackValue = string.Empty;
    private string cookieTestName = string.Empty;
    private ViewModeEnum viewMode = ViewModeEnum.Unknown;
    private CurrentUserInfo currentUser;

    /// <summary>
    /// Indicates whether processing should be stopped.
    /// </summary>
    private bool stopProcessing;

    #endregion


    #region "Methods"

    /// <summary>
    /// Init event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        combinationSelector.UniSelector.UseUniSelectorAutocomplete = false;
        currentUser = MembershipContext.AuthenticatedUser;

        // Set the selected combination (from cookie by default)
        MVTestInfo mvTestInfo = MVTestInfoProvider.GetRunningTest(DocumentContext.CurrentAliasPath, SiteContext.CurrentSiteID, DocumentContext.CurrentDocumentCulture.CultureCode);

        // Get the cookie name
        if (mvTestInfo != null)
        {
            // Get a cookie name for the mvt test
            cookieTestName = CookieName.GetMVTCookieName(mvTestInfo.MVTestName);
        }
        else
        {
            // Get a template cookie name (used just in CMSDesk when no test is running)
            cookieTestName = CookieName.GetNoMVTCookieName(DocumentContext.CurrentDocument.GetUsedPageTemplateId());
        }

        // Move cookies expiration to next 30 days
        var cookieMVTTest = CookieHelper.GetExistingCookie(cookieTestName);
        if (cookieMVTTest != null)
        {
            CookieHelper.SetValue(cookieMVTTest.Name, cookieMVTTest.Value, cookieMVTTest.Path, DateTime.Now.AddDays(30), false);
        }

        base.OnInit(e);

        viewMode = PortalContext.ViewMode;

        // Check permissions
        if ((currentUser == null)
            || (!currentUser.IsAuthorizedPerResource("CMS.Design", "Design") && PortalContext.IsDesignMode(viewMode))
            || (!currentUser.IsAuthorizedPerResource("CMS.MVTest", "Read")))
        {
            stopProcessing = true;
        }
    }


    /// <summary>
    /// Handles the PreRender event of the Page control.
    /// </summary>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (stopProcessing)
        {
            Visible = false;
            return;
        }

        // Show the warning panel when there is a running MVT test
        if ((viewMode != ViewModeEnum.Preview)
            && MVTestInfoProvider.ContainsRunningMVTest(DocumentContext.CurrentAliasPath, SiteContext.CurrentSiteID, DocumentContext.CurrentPageInfo.DocumentCulture))
        {
            DocumentManager.MessagesPlaceHolder.ShowWarning(ResHelper.GetString("mvtest.runningtestwarning", MembershipContext.AuthenticatedUser.PreferredUICultureCode));
        }

        if (RequestHelper.IsPostBack())
        {
            // Reload the combination panel because one of the combination could have been removed
            ReloadData(true);
        }

        // Set the OnChange attribute => Save the variant slider configuration into a cookie and raise a postback
        combinationSelector.UniSelector.OnBeforeClientChanged = "SaveCombinationPanelSelection(); " + Page.ClientScript.GetPostBackEventReference(this, "combinationchanged") + ";";


        MVTCombinationInfo ci = null;

        if (DocumentContext.CurrentDocument != null)
        {
            int templateId = DocumentContext.CurrentDocument.GetUsedPageTemplateId();

            // Get the combination name from cookie
            string combinationName = CookieHelper.GetValue(cookieTestName);
            if (string.IsNullOrEmpty(combinationName))
            {
                // CombinationName is not defined, use the default combination for the page template
                ci = MVTCombinationInfoProvider.GetDefaultCombinationInfo(templateId);
            }
            else
            {
                // Keep current instance node
                TreeNode tn = DocumentContext.CurrentDocument;

                // Use the defined combination 
                ci = MVTCombinationInfoProvider.GetMVTCombinationInfo(tn.NodeAliasPath, combinationName, SiteContext.CurrentSiteName, tn.DocumentCulture);

                if (ci == null)
                {
                    // Combination not found (can happen after deleting a variant), use the default combination for the page template
                    ci = MVTCombinationInfoProvider.GetDefaultCombinationInfo(templateId);
                }
            }
        }

        // Show the combination panel only if there are any combinations for the document
        pnlMvtCombination.Enabled = combinationSelector.HasData;

        if (ci != null)
        {
            int combinationId = ci.MVTCombinationID;

            // Setup the combination panel values
            combinationSelector.DropDownSelect.SelectedValue = ci.MVTCombinationName;
            chkEnabled.Checked = ci.MVTCombinationEnabled;
            txtCustomName.Text = ResHelper.LocalizeString(ci.MVTCombinationCustomName, currentUser.PreferredUICultureCode);

            // Create javascript variables of the combination panel. Used when changing combination by the variation slider/arrows
            StringBuilder combinationJSList = new StringBuilder();
            combinationJSList.Append("var mvtCPselector = document.getElementById('" + combinationSelector.DropDownSelect.ClientID + "');");
            combinationJSList.Append("var mvtCPenabled = document.getElementById('" + chkEnabled.ClientID + "');");
            combinationJSList.Append("var mvtCPcustomName = document.getElementById('" + txtCustomName.ClientID + "');");
            combinationJSList.Append("var mvtCPcurrentCombinationName = document.getElementById('" + hdnCurrentCombination.ClientID + "');");

            // Generate the JS configuration array for the Edit and Design view modes only (modes where the variant sliders can be used)
            if (viewMode.IsOneOf(ViewModeEnum.Edit, ViewModeEnum.EditDisabled) || PortalContext.IsDesignMode(viewMode))
            {
                // Get variants for the selected combination
                DataSet dsSelectedCombinationVariants = MVTVariantInfoProvider.GetMVTVariants(ci.MVTCombinationPageTemplateID, combinationId);


                #region "Generate javascript arrays used for changing the selected combination according to the selected variants"

                // List of compulsory combination variants (used for the combination JS array only).
                // For example:
                // Selected combination contains WidgetVariantID3 + ZoneVariantID5 and the user is in the Edit mode.
                // Therefore compulsory combination variants will be only containing the zone variant ID 5-> that means: only combinations with this compulsory variant will be proceed
                // This ensures a correct behavior of the combination panel when changing the variant sliders.
                combinationJSList.Append("var compulsoryCombinationVariants = [");
                int compulsoryCombinationVariantsCounter = 0;

                // Fill the array 'compulsoryCombinationVariants'
                if (!DataHelper.DataSourceIsEmpty(dsSelectedCombinationVariants))
                {
                    // Edit mode
                    if ((viewMode.IsEdit()) || (viewMode == ViewModeEnum.EditDisabled))
                    {
                        foreach (DataRow row in dsSelectedCombinationVariants.Tables[0].Rows)
                        {
                            // Process web part and zones only
                            if (ValidationHelper.GetInteger(row["MVTVariantDocumentID"], 0) == 0)
                            {
                                if (compulsoryCombinationVariantsCounter > 0)
                                {
                                    combinationJSList.Append(",");
                                }
                                // Add the web part/zone to the JS array
                                combinationJSList.Append(ValidationHelper.GetInteger(row["MVTVariantID"], 0));
                                compulsoryCombinationVariantsCounter++;
                            }
                        }
                    }
                    // Design mode
                    else if (PortalContext.IsDesignMode(viewMode))
                    {
                        foreach (DataRow row in dsSelectedCombinationVariants.Tables[0].Rows)
                        {
                            // Process widgets only
                            if (ValidationHelper.GetInteger(row["MVTVariantDocumentID"], 0) > 0)
                            {
                                if (compulsoryCombinationVariantsCounter > 0)
                                {
                                    combinationJSList.Append(",");
                                }
                                // Add the widget to the JS array
                                combinationJSList.Append(ValidationHelper.GetInteger(row["MVTVariantID"], 0));
                                compulsoryCombinationVariantsCounter++;
                            }
                        }
                    }
                }

                combinationJSList.Append("];");

                // combinationsArray - array containing configuration of each combination displayed in the combination panel.
                // This array is used after the user changes a variant slider and the new combination (selected by the combination panel) is to be calculated.
                combinationJSList.Append("var combinationsArray = [");
                int combinationCount = 0;

                foreach (ListItem item in combinationSelector.DropDownSelect.Items)
                {
                    // Get the combination object
                    MVTCombinationInfo cObj = MVTCombinationInfoProvider.GetMVTCombinationInfo(combinationSelector.PageTemplateID, item.Value);

                    if (cObj != null)
                    {
                        DataSet cVariants = MVTVariantInfoProvider.GetMVTVariants(cObj.MVTCombinationPageTemplateID, cObj.MVTCombinationID);
                        if (combinationCount > 0)
                        {
                            combinationJSList.Append(",");
                        }

                        combinationJSList.Append("['");
                        combinationJSList.Append(cObj.MVTCombinationName);
                        combinationJSList.Append("',");
                        combinationJSList.Append(cObj.MVTCombinationEnabled.ToString().ToLowerCSafe());
                        combinationJSList.Append(",");
                        combinationJSList.Append(ScriptHelper.GetString(ResHelper.LocalizeString(cObj.MVTCombinationCustomName, currentUser.PreferredUICultureCode)));

                        // Generate the unique variant IDs code (format: 155_158_180) - must be ordered by variantID
                        combinationJSList.Append(",'");
                        if (!DataHelper.DataSourceIsEmpty(cVariants))
                        {
                            int variantCount = 0;
                            foreach (DataRow row in cVariants.Tables[0].Rows)
                            {
                                if (variantCount > 0)
                                {
                                    combinationJSList.Append("_");
                                }

                                combinationJSList.Append(ValidationHelper.GetString(row["MVTVariantID"], "0"));
                                variantCount++;
                            }
                        }

                        combinationJSList.Append("']");
                        combinationCount++;
                    }
                }

                combinationJSList.Append("];");

                #endregion


                // Choose the correct variant from all rendered variants for a current web part (used in Content->Design/Edit page)
                if (!DataHelper.DataSourceIsEmpty(dsSelectedCombinationVariants))
                {
                    combinationJSList.Append("function SetCombinationVariants() {");
                    bool variantWasSet = false;

                    // Process all the variants of the selected combination
                    foreach (DataRow row in dsSelectedCombinationVariants.Tables[0].Rows)
                    {
                        bool itemIsZoneVariant = string.IsNullOrEmpty(ValidationHelper.GetString(row["MVTVariantInstanceGUID"], string.Empty));
                        string itemIdentifier;
                        if (itemIsZoneVariant)
                        {
                            // Zone
                            itemIdentifier = "Variant_Zone_" + HTMLHelper.HTMLEncode(ValidationHelper.GetString(row["MVTVariantZoneID"], string.Empty));
                        }
                        else
                        {
                            // Web part/widget
                            itemIdentifier = "Variant_WP_" + ValidationHelper.GetGuid(row["MVTVariantInstanceGUID"], Guid.Empty).ToString("N");
                        }

                        // Set the appropriate variant
                        int itemVariantId = ValidationHelper.GetInteger(row["MVTVariantID"], 0);
                        combinationJSList.Append("SetVariant('" + itemIdentifier + "', " + itemVariantId + ");");
                        variantWasSet = true;
                    }

                    if (variantWasSet)
                    {
                        // Refresh the combination panel if any variant was set manually
                        combinationJSList.Append("UpdateCombinationPanel();");
                    }

                    combinationJSList.Append("}");
                }
            }

            // Save the current combination id in javascript
            combinationJSList.Append("mvtCPcurrentCombinationName.value = '" + ci.MVTCombinationName + "';");

            // Register the JS arrays and current variants
            ScriptHelper.RegisterStartupScript(this, typeof(string), "combinationJSList", ScriptHelper.GetScript(combinationJSList.ToString()));
        }

        // Display the "set as result" button when there any MVT variants in the page
        if ((currentUser != null)
            && (currentUser.IsAuthorizedPerResource("CMS.Design", "Design")))
        {
            plcUseCombination.Visible = combinationSelector.DropDownSelect.Items.Count > 1;
        }
    }


    /// <summary>
    /// Creates child controls.
    /// </summary>
    protected override void CreateChildControls()
    {
        base.CreateChildControls();

        if (!stopProcessing)
        {
            SetupControl();
        }
    }


    /// <summary>
    /// Reloads the data.
    /// </summary>
    /// <param name="forceLoad">If set to <c>true</c>, reload the control even if the control has been already reloaded</param>
    protected void ReloadData(bool forceLoad)
    {
        combinationSelector.ReloadData(forceLoad);
    }


    /// <summary>
    /// Sets the current combination as default 
    /// </summary>
    protected void btnUseCombination_Click(object sender, EventArgs e)
    {
        if ((DocumentContext.CurrentPageInfo != null))
        {
            // Keep current page template instance
            PageInfo pi = DocumentContext.CurrentPageInfo;

            // Get combination from cookies
            string combinationName = CookieHelper.GetValue(cookieTestName);

            // Get the page template id
            int templateId = GetPageTemplateId(pi);

            // Get combination info
            MVTCombinationInfo currentCombination = MVTCombinationInfoProvider.GetMVTCombinationInfo(templateId, combinationName);

            // Get default combination info
            MVTCombinationInfo defaultCombination = MVTCombinationInfoProvider.GetDefaultCombinationInfo(templateId);

            // Check whether default and current combination exists
            if ((currentCombination != null) && (defaultCombination != null))
            {
                // Do not save default combination
                if (currentCombination.MVTCombinationID != defaultCombination.MVTCombinationID)
                {
                    // Copy the combination custom name
                    defaultCombination.MVTCombinationCustomName = currentCombination.MVTCombinationCustomName;
                    defaultCombination.Update();

                    // Tree provider instance
                    TreeProvider tp = new TreeProvider(currentUser);

                    // Documents - use the Preview view mode to ensure that only chosen variant will be rendered (Design mode renders all web part/zone variants and does not combine the instance with the variants)
                    PageTemplateInstance instance = MVTestInfoProvider.CombineWithMVT(pi, pi.UsedPageTemplateInfo.TemplateInstance, currentCombination.MVTCombinationID, ViewModeEnum.Preview);
                    CMSPortalManager.SaveTemplateChanges(pi, instance, WidgetZoneTypeEnum.None, ViewModeEnum.Design, tp);

                    // Widgets - use the Preview view mode to ensure that only chosen variant will be rendered (Edit mode renders all widget variants and does not combine the instance with the variants)
                    instance = MVTestInfoProvider.CombineWithMVT(pi, pi.DocumentTemplateInstance, currentCombination.MVTCombinationID, ViewModeEnum.Preview);
                    CMSPortalManager.SaveTemplateChanges(pi, instance, WidgetZoneTypeEnum.Editor, ViewModeEnum.Edit, tp);

                    // Save document after combining widgets as it is by default saved in the session and changes are not properly reflected
                    DocumentManager.SaveDocument();
                }

                // Remove all variants
                MVTVariantInfoProvider.GetMVTVariants().WhereEquals("MVTVariantPageTemplateID", templateId).ForEachObject(v => v.Delete());

                // Clear cached template info
                pi.UsedPageTemplateInfo.Update();

                // Redirect to the same page to update the current view
                URLHelper.Redirect(RequestContext.CurrentURL);
            }
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Setups this control.
    /// </summary>
    private void SetupControl()
    {
        // Check permissions
        if ((currentUser == null)
            || (!currentUser.IsAuthorizedPerResource("CMS.MVTest", "Manage")))
        {
            plcEditCombination.Visible = false;
        }

        // Set callback events
        btnChange.OnClientClick = Page.ClientScript.GetCallbackEventReference(this, "MVTSetCustomName()", "MVTReceiveServerData", null) + ";return false;";
        chkEnabled.Attributes.Add("onclick", Page.ClientScript.GetCallbackEventReference(this, "MVTSetEnabled()", "MVTReceiveServerData", null) + ";return true;");

        btnUseCombination.OnClientClick = "if (!confirm(" + ScriptHelper.GetLocalizedString("mvt.usecombinationconfirm") + ")) { return false; }";
        btnUseCombination.Click += btnUseCombination_Click;

        if ((DocumentContext.CurrentPageInfo != null) && (DocumentContext.CurrentPageInfo.UsedPageTemplateInfo != null))
        {
            // Get the current document ID
            int documentId = 0;
            if (DocumentContext.CurrentDocument != null)
            {
                documentId = DocumentContext.CurrentDocument.DocumentID;
            }

            // Display the combinations only for the current document
            combinationSelector.DocumentID = documentId;
            combinationSelector.PageTemplateID = DocumentContext.CurrentPageInfo.UsedPageTemplateInfo.PageTemplateId;
        }

        // Setup the localized strings
        string prefferedUICode = currentUser.PreferredUICultureCode;
        lblCombination.Text = ResHelper.GetString("mvtcombination.name", prefferedUICode);
        chkEnabled.Text = ResHelper.GetString("general.enabled", prefferedUICode);
        lblCustomName.Text = ResHelper.GetString("mvtcombination.customName", prefferedUICode);
        btnChange.Text = ResHelper.GetString("general.change", prefferedUICode);
        lblSaved.Text = ResHelper.GetString("mvtvariant.customnamesaved", prefferedUICode);
        btnUseCombination.Text = ResHelper.GetString("mvt.usecombination", prefferedUICode);

        // Hide label "Saved"
        lblSaved.Style.Add("display", "none");

        // Setup the combination uniselector
        combinationSelector.UniSelector.OrderBy = "MVTCombinationName";
        combinationSelector.UniSelector.AllowEmpty = false;

        // Ensure a full postback for the uniselector (this is set manually due to the Update panel in the uniselector)
        ScriptManager scr = ScriptManager.GetCurrent(Page);
        scr.RegisterPostBackControl(combinationSelector.DropDownSelect);

        // Register page javascript
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
            function MVTSetCustomName() {
                var ddlCombinations = document.getElementById('", combinationSelector.DropDownSelect.ClientID, @"');
                var combinationId = ddlCombinations.options[ddlCombinations.selectedIndex].value;
                var txtCustomName = document.getElementById('", txtCustomName.ClientID, @"');
                return combinationId + ';cname;' + txtCustomName.value;
            }

            function MVTSetEnabled() {
                var ddlCombinations = document.getElementById('", combinationSelector.DropDownSelect.ClientID, @"');
                var combinationId = ddlCombinations.options[ddlCombinations.selectedIndex].value;
                var chkEnabled = document.getElementById('", chkEnabled.ClientID, @"');
                return combinationId + ';enabled;' + chkEnabled.checked;
            }

            function MVTReceiveServerData(value) {
                var ddlCombinations = document.getElementById('", combinationSelector.DropDownSelect.ClientID, @"');
                var listitem = ddlCombinations.options[ddlCombinations.selectedIndex];
                if (value.length > 0) {
                    /* Custom name changed */
                    listitem.text = value;

                    /* Update the combinationsArray array */
                    combinationsArray[ddlCombinations.selectedIndex][2] = document.getElementById('", txtCustomName.ClientID, @"').value;

                    document.getElementById('", lblSaved.ClientID, @"').style.display = 'inline-block';
                    setTimeout(""document.getElementById('", lblSaved.ClientID, @"').style.display = 'none'"", 4000);
                }
                else {
                    /* Enabled changed */
                    var chkEnabled = document.getElementById('", chkEnabled.ClientID, @"');
                    if (listitem != null) {
                        if (chkEnabled.checked) {
                            listitem.className = '", ITEM_ENABLED_CLASS, @"';
                        }
                        else {
                            listitem.className = '", ITEM_DISABLED_CLASS, @"';
                        }
                    }

                    /* Update the combinationsArray array */
                    combinationsArray[ddlCombinations.selectedIndex][1] = chkEnabled.checked;
                }
            }

            function SaveCombinationPanelSelection() {
                var ddlCombinations = document.getElementById('", combinationSelector.DropDownSelect.ClientID, @"');
                var listitem = ddlCombinations.options[ddlCombinations.selectedIndex];
                var combinationName = listitem.value;
                if (combinationName.length > 0) {
                    $cmsj.cookie('", cookieTestName, @"', combinationName, { expires: 7, path: '/' });
                }
                else {
                    $cmsj.cookie('", cookieTestName, @"', null);
                }
            }
        ");

        ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "MVTCombinationPanel", sb.ToString(), true);

        ReloadData(false);
    }


    /// <summary>
    /// Gets the page template id of the given page info (or template id of the parent page info if inherited).
    /// </summary>
    /// <param name="pi">The PageInfo</param>
    private int GetPageTemplateId(PageInfo pi)
    {
        int templateId = 0;

        if (pi != null)
        {
            if (!pi.NodeInheritPageTemplate)
            {
                templateId = pi.GetUsedPageTemplateId();
            }
            else if (pi.UsedPageTemplateInfo != null)
            {
                templateId = pi.UsedPageTemplateInfo.PageTemplateId;
            }
        }

        return templateId;
    }

    #endregion


    #region "Callback handling"

    /// <summary>
    /// Callback event handler.
    /// </summary>
    /// <param name="argument">Callback argument</param>
    public void RaiseCallbackEvent(string argument)
    {
        // Check permissions
        if ((currentUser == null)
            || (!currentUser.IsAuthorizedPerResource("CMS.MVTest", "Manage"))
            || stopProcessing)
        {
            return;
        }

        // Get arguments
        if (!string.IsNullOrEmpty(argument))
        {
            string[] args = argument.Split(new[] { ';' }, 3);
            if (args.Length == 3)
            {
                string combinationName = ValidationHelper.GetString(args[0], string.Empty);
                string action = args[1].ToLowerCSafe();
                string newValue = args[2];

                // Get the combination info
                MVTCombinationInfo mvtcInfo = MVTCombinationInfoProvider.GetMVTCombinationInfo(combinationSelector.PageTemplateID, combinationName);
                if (mvtcInfo != null)
                {
                    switch (action)
                    {
                        case "cname":
                            // Custom name changed
                            mvtcInfo.MVTCombinationCustomName = newValue;
                            if (string.IsNullOrEmpty(newValue))
                            {
                                newValue = mvtcInfo.MVTCombinationName;
                            }
                            // return the new value (when newValue=="", then return combination code name)
                            callbackValue = newValue;
                            break;

                        case "enabled":
                            // combination Enabled changed
                            mvtcInfo.MVTCombinationEnabledOriginal = mvtcInfo.MVTCombinationEnabled;
                            mvtcInfo.MVTCombinationEnabled = ValidationHelper.GetBoolean(newValue, true);
                            callbackValue = string.Empty;
                            break;

                        default:
                            return;
                    }

                    MVTCombinationInfoProvider.SetMVTCombinationInfo(mvtcInfo);

                    // Synchronize widget variants if enabling combination
                    if ((mvtcInfo.MVTCombinationDocumentID > 0)
                        || (!mvtcInfo.MVTCombinationEnabledOriginal && mvtcInfo.MVTCombinationEnabled
                           ))
                    {
                        // Log synchronization
                        TreeProvider tree = new TreeProvider(currentUser);
                        TreeNode node = tree.SelectSingleDocument(mvtcInfo.MVTCombinationDocumentID);

                        if (node != null)
                        {
                            DocumentSynchronizationHelper.LogDocumentChange(node, TaskTypeEnum.UpdateDocument, tree);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Callback result retrieving handler.
    /// </summary>
    public string GetCallbackResult()
    {
        // Return combination custom name or combination code name
        return callbackValue;
    }

    #endregion
}
