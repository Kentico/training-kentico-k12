using System;

using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.UIControls;


public partial class CMSModules_Modules_Pages_Class_Code : GlobalAdminPage
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        var classId = QueryHelper.GetInteger("classId", 0);
        var dataClass = DataClassInfoProvider.GetDataClassInfo(classId);

        if (dataClass == null)
        {
            return;
        }

        if (UseBindingTemplates(dataClass))
        {
            plcNormalInfo.Visible = false;
            chkUseIdHashtable.Visible = false;

            txtInfoCode.Height = 600;
            txtInfoProviderCode.Height = 600;
        }

        // Setup the Macro source
        drpDisplayNameColumn.MacroSource = drpCodeNameColumn.MacroSource =
            drpGuidColumn.MacroSource = drpLastModifiedColumn.MacroSource =
            drpBinaryColumn.MacroSource = drpSiteIdColumn.MacroSource = "Flatten(\";(none)\", GetClassFields(\"" + dataClass.ClassName + "\"))";

        if (!RequestHelper.IsPostBack())
        {
            // Load code generation settings from data class
            var settings = dataClass.ClassCodeGenerationSettingsInfo;

            txtNamespace.Text = String.IsNullOrEmpty(settings.NameSpace) ? ProviderHelper.GetCodeName(PredefinedObjectType.RESOURCE, dataClass.ClassResourceID) : settings.NameSpace;

            if (string.IsNullOrEmpty(settings.ObjectType))
            {
                txtObjectType.Text = dataClass.ClassName.ToLowerInvariant();
            }
            else
            {
                txtObjectType.Text = settings.ObjectType;
            }
            drpDisplayNameColumn.Text = settings.DisplayNameColumn;
            drpCodeNameColumn.Text = settings.CodeNameColumn;
            drpGuidColumn.Text = settings.GuidColumn;
            drpLastModifiedColumn.Text = settings.LastModifiedColumn;
            drpBinaryColumn.Text = settings.BinaryColumn;
            drpSiteIdColumn.Text = settings.SiteIdColumn;

            chkUseIdHashtable.Checked = settings.UseIdHashtable;
            chkUseNameHashtable.Checked = settings.UseNameHashtable;
            chkUseGuidHashtable.Checked = settings.UseGuidHashtable;

            var moduleCodeName = ProviderHelper.GetCodeName(PredefinedObjectType.RESOURCE, dataClass.ClassResourceID);
            var savePath = "~/" + (SystemContext.IsWebApplicationProject ? "Old_App_Code" : "App_Code") + "/CMSModules/" + TrimCmsPrefix(moduleCodeName) + "";
            ucSaveFsSelector.Value = savePath;
            if (Directory.Exists(savePath))
            {
                ucSaveFsSelector.DefaultPath = savePath;
            }

            GenerateCode(dataClass);
        }

        btnGenerateCode.Click += (sender, args) =>
        {
            SaveCodeGenerationSettings(dataClass);
            GenerateCode(dataClass);
        };

        if (SystemContext.IsPrecompiledWebsite)
        {
            var message = GetString("classes.code.codesaveerror");
            ShowInformation(message);

            ucSaveFsSelector.Enabled = false;
            btnSaveCode.Enabled = false;
        }
        else
        {
            btnSaveCode.Click += (sender, args) => SaveCode();
            btnSaveCode.OnClientClick = "if (!confirm('" + GetString("codegenerators.saveconfirmation") + "')) { return false }";
        }
    }


    private string TrimCmsPrefix(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.StartsWithCSafe("cms.", true))
        {
            return name.Substring(4);
        }
        return name;
    }


    private void SaveCodeGenerationSettings(DataClassInfo dataClass)
    {
        dataClass.ClassCodeGenerationSettingsInfo = new ClassCodeGenerationSettings()
        {
            NameSpace = txtNamespace.Text,
            ObjectType = txtObjectType.Text.Trim(),
            DisplayNameColumn = drpDisplayNameColumn.Text,
            CodeNameColumn = drpCodeNameColumn.Text.Trim(),
            GuidColumn = drpGuidColumn.Text.Trim(),
            LastModifiedColumn = drpLastModifiedColumn.Text.Trim(),
            BinaryColumn = drpBinaryColumn.Text.Trim(),
            SiteIdColumn = drpSiteIdColumn.Text.Trim(),

            UseIdHashtable = chkUseIdHashtable.Checked,
            UseNameHashtable = chkUseNameHashtable.Checked,
            UseGuidHashtable = chkUseGuidHashtable.Checked,
        };

        DataClassInfoProvider.SetDataClassInfo(dataClass);
    }


    private void GenerateCode(DataClassInfo dataClass)
    {
        if (UseBindingTemplates(dataClass))
        {
            var infoTemplate = DataEngineCodeTemplateGenerator.GetBindingInfoCodeTemplate(dataClass);
            txtInfoCode.Text = infoTemplate.TransformText();
            hdnInfoClassName.Value = infoTemplate.InfoClassName;

            var infoProviderTemplate = DataEngineCodeTemplateGenerator.GetBindingInfoProviderCodeTemplate(dataClass);
            txtInfoProviderCode.Text = infoProviderTemplate.TransformText();
            hdnInfoProviderClassName.Value = infoProviderTemplate.InfoProviderClassName;
        }
        else
        {
            var infoTemplate = DataEngineCodeTemplateGenerator.GetInfoCodeTemplate(dataClass);
            txtInfoCode.Text = infoTemplate.TransformText();
            hdnInfoClassName.Value = infoTemplate.InfoClassName;

            var infoProviderTemplate = DataEngineCodeTemplateGenerator.GetInfoProviderCodeTemplate(dataClass);
            txtInfoProviderCode.Text = infoProviderTemplate.TransformText();
            hdnInfoProviderClassName.Value = infoProviderTemplate.InfoProviderClassName;
        }
    }


    private void SaveCode()
    {
        var savePath = ValidationHelper.GetString(ucSaveFsSelector.Value, "~") + "/";

        if (SystemContext.IsPrecompiledWebsite)
        {
            return;
        }

        try
        {
            var infoPath = savePath + hdnInfoClassName.Value + ".cs";
            WriteFile(infoPath, txtInfoCode.Text);

            var infoProviderPath = savePath + hdnInfoProviderClassName.Value + ".cs";
            WriteFile(infoProviderPath, txtInfoProviderCode.Text);

            var message = GetString("classes.code.filessaved");
            message = string.Format(message, infoPath, infoProviderPath);
            ShowConfirmation(message);
        }
        catch (Exception exception)
        {
            CoreServices.EventLog.LogException("Classes", "SAVECODE", exception);

            var message = GetString("classes.code.filessaveerror");
            ShowError(message);
        }
    }


    private static bool UseBindingTemplates(DataClassInfo dc)
    {
        // Use binding generator when there are exactly two binding references in the object
        int bindingReferences = 0;
        FormInfo fi = new FormInfo(dc.ClassFormDefinition);
        foreach (var field in fi.GetFields<FormFieldInfo>())
        {
           if (!string.IsNullOrEmpty(field.ReferenceToObjectType) && (field.ReferenceType == ObjectDependencyEnum.Binding))
           {
               bindingReferences++;
           }
        }

        return bindingReferences == 2;
    }


    private void WriteFile(string path, string content)
    {
        var physicalPath = URLHelper.GetPhysicalPath(path);
        DirectoryHelper.EnsureDiskPath(physicalPath, SystemContext.WebApplicationPhysicalPath);
        using (var file = File.CreateText(physicalPath))
        {
            file.Write(content);
        }
    }
}