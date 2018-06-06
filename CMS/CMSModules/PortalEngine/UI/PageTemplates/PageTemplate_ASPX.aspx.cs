using System;

using CMS.Base;
using CMS.EventLog;
using CMS.Helpers;
using CMS.IO;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_PageTemplates_PageTemplate_ASPX : GlobalAdminPage
{
    protected int templateId = 0;
    private string fileName = "";
    private string selectedSite = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentMaster.DisplaySiteSelectorPanel = true;

        // Get page template id from url
        templateId = QueryHelper.GetInteger("templateid", 0);
        PageTemplateInfo pti = PageTemplateInfoProvider.GetPageTemplateInfo(templateId);
        EditedObject = pti;

        string templateName = txtName.Text.Trim();
        if (templateName == "")
        {
            templateName = radMaster.Checked ? "MainMenu" : "Template";

            if (pti != null)
            {
                templateName = ValidationHelper.GetIdentifier(pti.DisplayName);
            }

            txtName.Text = templateName;
        }

        // Set site selector        
        siteSelector.AllowAll = false;
        siteSelector.UseCodeNameForSelection = true;

        if (!RequestHelper.IsPostBack())
        {
            selectedSite = SiteContext.CurrentSiteName;
            siteSelector.Value = selectedSite;
        }
        else
        {
            selectedSite = ValidationHelper.GetString(siteSelector.Value, String.Empty);
        }

        string className = "CMSTemplates_" + selectedSite + "_" + templateName;
        fileName = templateName;

        lblCodeInfo.Text = GetString("pagetemplate_aspx.info");
        lblCodeBehindInfo.Text = GetString("pagetemplate_aspx.codebehindinfo");

        lblName.Text = GetString("pagetemplate_aspx.name");
        lblMaster.Text = GetString("pagetemplate_aspx.mastername");
        btnSave.Text = GetString("pagetemplate_aspx.save");
        btnRefresh.Text = GetString("pagetemplate_aspx.refresh");

        radSlave.Text = GetString("pagetemplate_aspx.slave");
        radMaster.Text = GetString("pagetemplate_aspx.master");
        radTemplate.Text = GetString("pagetemplate_aspx.template");
        radTemplateOnly.Text = GetString("pagetemplate_aspx.templateonly");

        plcMasterTemplate.Visible = radSlave.Checked;

        if (pti != null)
        {
            string codeBehind = "";
            string registerCode = "";
            string code = PortalHelper.GetPageTemplateASPXCode(pti, out registerCode);

            if (radTemplateOnly.Checked)
            {
                code = registerCode + code;
            }
            else if (radSlave.Checked)
            {
                fileName += ".aspx";

                codeBehind = File.ReadAllText(Server.MapPath("~/CMSModules/PortalEngine/UI/PageTemplates/ASPX/ChildTemplate.aspx.cs"));
                codeBehind = codeBehind.Replace("PageTemplates_ChildTemplate", className);

                string pageCode = File.ReadAllText(Server.MapPath("~/CMSModules/PortalEngine/UI/PageTemplates/ASPX/ChildTemplate.aspx"));
                pageCode = pageCode.Replace(" Inherits=\"PageTemplates_ChildTemplate\"", " Inherits=\"" + className + "\"");
                pageCode = pageCode.Replace(" CodeFile=\"ChildTemplate.aspx.cs\"", " CodeFile=\"" + fileName + ".cs\"");

                string master = "";
                if (txtMaster.Text.Trim() != "")
                {
                    master = " MasterPageFile=\"" + txtMaster.Text.Trim() + ".master\"";
                }
                pageCode = pageCode.Replace(" MasterPageFile=\"Template.master\"", master);

                pageCode = pageCode.Replace("<%--REGISTER--%>", registerCode);

                pageCode = pageCode.Replace("<%--CONTENT--%>", code);

                code = pageCode;
            }
            else if (radMaster.Checked)
            {
                fileName += ".master";

                codeBehind = File.ReadAllText(Server.MapPath("~/CMSModules/PortalEngine/UI/PageTemplates/ASPX/Template.master.cs"));
                codeBehind = codeBehind.Replace("PageTemplates_MasterTemplate", className);

                string pageCode = File.ReadAllText(Server.MapPath("~/CMSModules/PortalEngine/UI/PageTemplates/ASPX/Template.master"));
                pageCode = pageCode.Replace(" Inherits=\"PageTemplates_MasterTemplate\"", " Inherits=\"" + className + "\"");
                pageCode = pageCode.Replace(" CodeFile=\"Template.master.cs\"", " CodeFile=\"" + fileName + ".cs\"");

                pageCode = pageCode.Replace("<%@ Register Assembly=\"CMS.DocumentEngine.Web.UI\" Namespace=\"CMS.DocumentEngine.Web.UI\" TagPrefix=\"cc1\" %>", "");
                pageCode = pageCode.Replace("<%--REGISTER--%>", registerCode);

                code = code.Replace("<%--CONTENT--%>", "<asp:ContentPlaceHolder ID=\"plcMain\" runat=\"server\"></asp:ContentPlaceHolder>");

                pageCode = pageCode.Replace("<%--CONTENT--%>", code);

                code = pageCode;
            }
            else if (radTemplate.Checked)
            {
                fileName += ".aspx";

                codeBehind = File.ReadAllText(Server.MapPath("~/CMSModules/PortalEngine/UI/PageTemplates/ASPX/Template.aspx.cs"));
                codeBehind = codeBehind.Replace("PageTemplates_Template", className);

                string pageCode = File.ReadAllText(Server.MapPath("~/CMSModules/PortalEngine/UI/PageTemplates/ASPX/Template.aspx"));
                pageCode = pageCode.Replace("Inherits=\"PageTemplates_Template\"", "Inherits=\"" + className + "\"");
                pageCode = pageCode.Replace(" CodeFile=\"Template.aspx.cs\"", " CodeFile=\"" + fileName + ".cs\"");

                pageCode = pageCode.Replace("<%@ Register Assembly=\"CMS.DocumentEngine.Web.UI\" Namespace=\"CMS.DocumentEngine.Web.UI\" TagPrefix=\"cc1\" %>", "");
                pageCode = pageCode.Replace("<%--REGISTER--%>", registerCode);

                pageCode = pageCode.Replace("<%--CONTENT--%>", code);

                code = pageCode;
            }

            txtCode.Text = HTMLHelper.ReformatHTML(code);
            txtCodeBehind.Text = codeBehind;
        }

        ShowInformation(String.Format(GetString("pagetemplate_aspx.saveinfo"), "~/CMSTemplates/" + selectedSite + "/" + fileName));
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string path = "~/CMSTemplates/" + selectedSite + "/" + fileName;
            path = Server.MapPath(path);

            string directory = path.Substring(0, path.LastIndexOfCSafe('\\'));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (txtCode.Text.Trim() != "")
            {
                File.WriteAllText(path, txtCode.Text);
            }

            if (txtCodeBehind.Text.Trim() != "")
            {
                File.WriteAllText(path + ".cs", txtCodeBehind.Text);
            }

            ShowInformation(String.Format(GetString("pagetemplate_aspx.saved"), path));

        }
        catch (Exception ex)
        {
            ShowError(ex.Message, null, EventLogProvider.GetExceptionLogMessage(ex));
        }
    }
}