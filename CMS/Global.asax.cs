using System;
using System.Reflection;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;

/// <summary>
/// Application methods.
/// </summary>
public class Global : CMSHttpApplication
{
    #region "Methods"

    static Global()
    {
#if DEBUG
        // Set debug mode based on current web project build configuration
        SystemContext.IsWebProjectDebug = true;
#endif

        // Ensure initialization of dynamic modules after application pre-initialization. Must be called before the StartApplication method.
        ApplicationEvents.PreInitialized.Execute += EnsureDynamicModules;

        // Initialize CMS application. This method should not be called from custom code.
        InitApplication();
    }


    /// <summary>
    /// Ensures that modules from the App code (for web site) and CMSApp (for web application) assembly are registered.
    /// </summary>
    private static void EnsureDynamicModules(object sender, EventArgs e)
    {
        // Global.asax.cs is hosted in AppCode/CMSApp assembly
        var appCodeAssembly = Assembly.GetExecutingAssembly();

        // Ensures that registered modules are registered from AppCode/CMSApp assembly
        ModuleEntryManager.EnsureAppCodeModules(appCodeAssembly);
    }

    #endregion
}