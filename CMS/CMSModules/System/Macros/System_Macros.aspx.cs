using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;

using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.UIControls;

public partial class CMSModules_System_Macros_System_Macros : GlobalAdminPage
{
    private const string EVENTLOG_SOURCE_REFRESHSECURITYPARAMS = "Macros - Refresh security parameters";

    private readonly NameValueCollection processedObjects = new NameValueCollection();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        InitForm();
        InitAsyncDialog();
    }


    #region "Async log"

    /// <summary>
    /// Inits the async dialog.
    /// </summary>
    private void InitAsyncDialog()
    {
        ctlAsyncLog.TitleText = GetString("macros.refreshsecurityparams.title");

        ctlAsyncLog.OnCancel += ctlAsyncLog_OnCancel;
        ctlAsyncLog.OnFinished += ctlAsyncLog_OnFinished;
    }


    private void ctlAsyncLog_OnCancel(object sender, EventArgs args)
    {
        EventLogProvider.LogEvent(EventType.INFORMATION, ctlAsyncLog.Parameter, "CANCELLED");

        pnlAsyncLog.Visible = false;

        ShowConfirmation(GetString("general.actioncanceled"));
    }


    private void ctlAsyncLog_OnFinished(object sender, EventArgs args)
    {
        EventLogProvider.LogEvent(EventType.INFORMATION, ctlAsyncLog.Parameter, "FINISHED");

        pnlAsyncLog.Visible = false;

        ShowConfirmation(GetString("general.actionfinished"));
    }


    /// <summary>
    /// Runs the specified action asynchronously.
    /// </summary>
    /// <param name="actionName">Action name</param>
    /// <param name="action">Action</param>
    private void RunAsync(string actionName, AsyncAction action)
    {
        // Set action name as process parameter
        ctlAsyncLog.Parameter = actionName;

        // Log async action start
        EventLogProvider.LogEvent(EventType.INFORMATION, actionName, "STARTED");

        // Run async action
        ctlAsyncLog.RunAsync(action, WindowsIdentity.GetCurrent());
    }

    #endregion


    #region "Refresh security params"

    /// <summary>
    /// Inits the "Refresh security parameters" form.
    /// </summary>
    private void InitForm()
    {
        // Init old salt text box
        if (chkRefreshAll.Checked)
        {
            txtOldSalt.Enabled = false;
            txtOldSalt.Text = GetString("macros.refreshsecurityparams.refreshalldescription");
        }
        else
        {
            txtOldSalt.Enabled = true;
        }

        chkRefreshAll.CheckedChanged += (sender, args) =>
        {
            // Clear the textbox after enabling it
            if (!chkRefreshAll.Checked)
            {
                txtOldSalt.Text = null;
            }
        };

        // Init new salt text box
        if (chkUseCurrentSalt.Checked)
        {
            txtNewSalt.Enabled = false;

            var customSalt = SettingsHelper.AppSettings[ValidationHelper.APP_SETTINGS_HASH_STRING_SALT];

            var resString = string.IsNullOrEmpty(customSalt) ? "macros.refreshsecurityparams.currentsaltisconnectionstring" : "macros.refreshsecurityparams.currentsaltiscustomvalue";

            txtNewSalt.Text = GetString(resString);
        }
        else
        {
            txtNewSalt.Enabled = true;
        }

        chkUseCurrentSalt.CheckedChanged += chkUseCurrentSalt_CheckedChanged;

        // Init submit button
        btnRefreshSecurityParams.Text = GetString("macros.refreshsecurityparams");
        btnRefreshSecurityParams.Click += (sender, args) =>
        {
            var oldSaltInput = txtOldSalt.Text.Trim();
            var newSaltInput = txtNewSalt.Text.Trim();

            if (!chkRefreshAll.Checked && string.IsNullOrEmpty(oldSaltInput))
            {
                ShowError(GetString("macros.refreshsecurityparams.oldsaltempty"));
                return;
            }

            if (!chkUseCurrentSalt.Checked && string.IsNullOrEmpty(newSaltInput))
            {
                ShowError(GetString("macros.refreshsecurityparams.newsaltempty"));
                return;
            }

            pnlAsyncLog.Visible = true;
            var objectTypes = Functions.GetObjectTypesWithMacros();

            RunAsync(EVENTLOG_SOURCE_REFRESHSECURITYPARAMS, p => RefreshSecurityParams(objectTypes, oldSaltInput, newSaltInput));
        };
    }


    private void chkUseCurrentSalt_CheckedChanged(object sender, EventArgs args)
    {
        // Clear the textbox after enabling it
        if (!chkUseCurrentSalt.Checked)
        {
            txtNewSalt.Text = null;
        }
    }


    private void AddLog(string logText)
    {
        ctlAsyncLog.AddLog(logText);
    }


    /// <summary>
    /// Refreshes the security parameters in macros for all the objects of the specified object types.
    /// Signs all the macros with the current user if the old salt is not specified.
    /// </summary>
    /// <param name="objectTypes">Object types</param>
    /// <param name="oldSalt">Old salt </param>
    /// <param name="newSalt">New salt</param>
    private void RefreshSecurityParams(IEnumerable<string> objectTypes, string oldSalt, string newSalt)
    {
        var oldSaltSpecified = !string.IsNullOrEmpty(oldSalt) && !chkRefreshAll.Checked;
        var newSaltSpecified = !string.IsNullOrEmpty(newSalt) && !chkUseCurrentSalt.Checked;

        processedObjects.Clear();

        using (var context = new CMSActionContext())
        {
            context.LogEvents = false;
            context.LogSynchronization = false;
            var processingString = GetString("macros.refreshsecurityparams.processing");

            foreach (var objectType in objectTypes)
            {
                var niceObjectType = GetNiceObjectTypeName(objectType);

                AddLog(string.Format(processingString, niceObjectType));

                try
                {
                    var infos = new InfoObjectCollection(objectType);

                    var csi = infos.TypeInfo.ClassStructureInfo;
                    var orderByIndex = FindOrderByIndex(csi);
                    if (orderByIndex != null)
                    {
                        infos.OrderByColumns = orderByIndex.GetOrderBy();
                    }

                    infos.PageSize = 1000;

                    // Skip object types derived from general data class object type to avoid duplicities
                    if ((infos.TypeInfo.OriginalObjectType == DataClassInfo.OBJECT_TYPE) && (infos.TypeInfo.ObjectType != DataClassInfo.OBJECT_TYPE))
                    {
                        continue;
                    }

                    foreach (var info in infos)
                    {
                        try
                        {
                            bool refreshed;
                            if (oldSaltSpecified)
                            {
                                refreshed = MacroSecurityProcessor.RefreshSecurityParameters(info, oldSalt, newSaltSpecified ? newSalt : ValidationHelper.HashStringSalt, true);
                            }
                            else
                            {
                                var identityOption = MacroIdentityOption.FromUserInfo(MembershipContext.AuthenticatedUser);
                                if (chkRefreshAll.Checked && newSaltSpecified)
                                {
                                    // Do not check integrity, but use new salt
                                    refreshed = MacroSecurityProcessor.RefreshSecurityParameters(info, identityOption, true, newSalt);
                                }
                                else
                                {
                                    // Do not check integrity, sign everything with current user
                                    refreshed = MacroSecurityProcessor.RefreshSecurityParameters(info, identityOption, true);
                                }
                            }

                            if (refreshed)
                            {
                                var objectName = HTMLHelper.HTMLEncode(ResHelper.LocalizeString(info.Generalized.ObjectDisplayName));
                                processedObjects.Add(niceObjectType, objectName);
                            }
                        }
                        catch (Exception ex)
                        {
                            string message = "Signing " + TypeHelper.GetNiceObjectTypeName(info.TypeInfo.ObjectType) + " " + info.Generalized.ObjectDisplayName + " failed: " + ex.Message;

                            using (var exceptionContext = new CMSActionContext())
                            {
                                exceptionContext.LogEvents = true;

                                EventLogProvider.LogEvent(EventType.ERROR, "Import", "MACROSECURITY", message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddLog(ex.Message);

                    using (var exceptionContext = new CMSActionContext())
                    {
                        exceptionContext.LogEvents = true;

                        EventLogProvider.LogException(EVENTLOG_SOURCE_REFRESHSECURITYPARAMS, "ERROR", ex);
                    }
                }
            }
        }

        EventLogProvider.LogEvent(EventType.INFORMATION, EVENTLOG_SOURCE_REFRESHSECURITYPARAMS, "PROCESSEDOBJECTS", GetProcessedObjectsForEventLog());
    }


    /// <summary>
    /// Finds suitable index for order by statement.
    /// </summary>
    private Index FindOrderByIndex(ClassStructureInfo classStructureInfo)
    {
        var indexes = classStructureInfo.GetTableIndexes();
        if (indexes == null)
        {
            return null;
        }

        // Clustered index has the best performance for paging but when not unique, stable result sets are not guaranteed over individual paging queries
        var clusteredIndex = indexes.GetClusteredIndex();
        if ((clusteredIndex != null) && clusteredIndex.IsUnique)
        {
            return clusteredIndex;
        }

        // Fall back to primary key index and then any index which is better than paging over non-indexed columns
        return indexes.GetPrimaryKeyIndex() ?? indexes.GetIndexes().FirstOrDefault();
    }


    private static string GetNiceObjectTypeName(string objectType)
    {
        var objectTypeResourceKey = TypeHelper.GetObjectTypeResourceKey(objectType);

        var niceObjectType = GetString(objectTypeResourceKey);
        if (niceObjectType.Equals(objectTypeResourceKey, StringComparison.OrdinalIgnoreCase))
        {
            if (objectType.StartsWith("bizformitem.bizform.", StringComparison.OrdinalIgnoreCase))
            {
                DataClassInfo dci = DataClassInfoProvider.GetDataClassInfo(objectType.Substring("bizformitem.".Length));
                if (dci != null)
                {
                    niceObjectType = "on-line form " + ResHelper.LocalizeString(dci.ClassDisplayName);
                }
            }
            else
            {
                niceObjectType = objectType;
            }
        }
        return niceObjectType;
    }


    /// <summary>
    /// Gets the list of processed objects formatted for use in the event log.
    /// </summary>
    private string GetProcessedObjectsForEventLog()
    {
        return processedObjects.AllKeys.SelectMany(processedObjects.GetValues, (k, v) => string.Format("{0} '{1}'", k, v)).Join("<br />");
    }

    #endregion
}
