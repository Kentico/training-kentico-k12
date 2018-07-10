using System;
using System.Data;
using System.Xml;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


/// <summary>
/// Checkout definition update event handler.
/// </summary>
public delegate void OnCheckoutProcessDefinitionUpdateEventHandler(string action);


public partial class CMSModules_Ecommerce_FormControls_CheckoutProcess : FormEngineUserControl
{
    #region "Variables"

    private string mImageFolder = "";
    private CheckoutProcessInfo mCheckoutProcess;
    private int mStepOrder = 1;
    private string breadcrumbsText;

    #endregion


    #region "Events"

    /// <summary>
    /// Occurs when mode (listing/edit) is changed.
    /// </summary>
    /// <param name="isListingMode">Indicates if control is in listing mode</param>
    public delegate void ModeChangedHandler(bool isListingMode);

    public event ModeChangedHandler OnModeChanged;

    #endregion


    #region "Private properties"

    private CheckoutProcessInfo CheckoutProcess
    {
        get
        {
            if (mCheckoutProcess == null)
            {
                mCheckoutProcess = new CheckoutProcessInfo();
                mCheckoutProcess.LoadXmlDefinition(CheckoutProcessXml);
            }
            return mCheckoutProcess;
        }
    }


    /// <summary>
    /// Checkout process XML - shopping cart steps definition in XML format.
    /// </summary>
    private string CheckoutProcessXml
    {
        get
        {
            return hdnCheckoutProcessXml.Value;
        }
        set
        {
            hdnCheckoutProcessXml.Value = value;
        }
    }


    /// <summary>
    /// Checkout process type.
    /// </summary>
    private CheckoutProcessEnum CheckoutProcessType
    {
        get
        {
            object obj = ViewState["CheckoutProcessType"];
            if (obj != null)
            {
                return (CheckoutProcessEnum)(obj);
            }

            ViewState["CheckoutProcessType"] = 0;
            return (CheckoutProcessEnum)ViewState["CheckoutProcessType"];
        }
        set
        {
            ViewState["CheckoutProcessType"] = value;
        }
    }


    /// <summary>
    /// Original step name.
    /// </summary>
    private string OriginalStepName
    {
        get
        {
            return Convert.ToString(ViewState["OriginalStepName"]);
        }
        set
        {
            ViewState["OriginalStepName"] = value;
        }
    }


    /// <summary>
    /// Indicates if control is in listing mode or in edit/insert mode.
    /// </summary>
    private bool ListingMode
    {
        set
        {
            plcList.Visible = value;
            plcEdit.Visible = !value;

            if (OnModeChanged != null)
            {
                OnModeChanged(value);
            }
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Checkout definition update event handler.
    /// </summary>
    public event OnCheckoutProcessDefinitionUpdateEventHandler OnCheckoutProcessDefinitionUpdate;


    /// <summary>
    /// Header actions control
    /// </summary>
    public override HeaderActions HeaderActions
    {
        get
        {
            return headerActions;
        }
    }



    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            plcMessNew.IsLiveSite = value;
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Image folder.
    /// </summary>
    public string ImageFolder
    {
        get
        {
            return mImageFolder;
        }
        set
        {
            mImageFolder = value;
        }
    }


    /// <summary>
    /// Information.
    /// </summary>
    public string Information
    {
        set
        {
            ShowConfirmation(value);
        }
    }


    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(CheckoutProcessXml);
                if (xml.DocumentElement.SelectNodes("step").Count == 0)
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }

            return CheckoutProcessXml;
        }
        set
        {
            CheckoutProcessXml = Convert.ToString(value);
        }
    }


    /// <summary>
    /// Indicates whether checkout process types should be visible and editable to user. 
    /// FALSE - Default value. Step is created without relation to any of the default checkout process. 
    /// Use this option when control is used to generate custom shopping cart webpart checkout process. 
    /// TRUE - User can choose from the default checkout process the step will be included in.
    /// </summary>
    public bool EnableDefaultCheckoutProcessTypes
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["EnableDefaultCheckoutProcessTypes"], false);
        }
        set
        {
            ViewState["EnableDefaultCheckoutProcessTypes"] = value;
        }
    }


    /// <summary>
    /// Gets ClientID of the hidden field which stores the actual value of the control.
    /// </summary>
    public override string ValueElementID
    {
        get
        {
            return hdnCheckoutProcessXml.ClientID;
        }
    }

    #endregion


    #region "Page Events"

    protected override void OnInit(EventArgs e)
    {
        // Attach Event Handlers for uniGrid 
        ugSteps.OnExternalDataBound += ugSteps_OnExternalDataBound;
        ugSteps.OnAction += ugSteps_OnAction;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (EnableDefaultCheckoutProcessTypes)
        {

            // Register javascript to confirm generate default checkout process
            string script = "function ConfirmDefaultProcess() {return confirm(" + ScriptHelper.GetLocalizedString("CheckoutProcess.ConfirmDefaultProcess") + ");}";
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "ConfirmDefaultProcess", ScriptHelper.GetScript(script));
        }

        // Initialize validators
        rfvStepCaption.ErrorMessage = GetString("CheckoutProcess.ErrorStepCaptionEmpty");
        rfvStepControlPath.ErrorMessage = GetString("CheckoutProcess.ErrorStepControlPathEmpty");
        rfvStepName.ErrorMessage = GetString("CheckoutProcess.ErrorStepNameEmpty");

        // Disable sorting of steps
        ugSteps.GridView.AllowSorting = false;

        if (!RequestHelper.IsPostBack())
        {
            ListingMode = true;
            ReloadData();
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Hide default checkout process types - for live site usage
        if (!EnableDefaultCheckoutProcessTypes)
        {
            // Step list
            ugSteps.NamedColumns["LiveSite"].Visible = false;
            ugSteps.NamedColumns["CMSDeskCustomer"].Visible = false;
            ugSteps.NamedColumns["CMSDeskOrder"].Visible = false;
            ugSteps.NamedColumns["CMSDeskOrderItems"].Visible = false;
        }

        // Initialize breadcrumb items
        breadcrumbs.AddBreadcrumb(new BreadcrumbItem
        {
            Text = GetString("CheckoutProcess.lnkList"),
            RedirectUrl = "~/CMSModules/Ecommerce/Pages/Tools/Configuration/StoreSettings/StoreSettings_CheckoutProcess.aspx"
        });
        breadcrumbs.AddBreadcrumb(new BreadcrumbItem
        {
            Text = breadcrumbsText,
        });
    }

    #endregion


    #region "Event Handlers"

    /// <summary>
    /// Handles data bound of unigrid
    /// </summary>
    protected object ugSteps_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        // Formatting columns
        switch (sourceName.ToLowerCSafe())
        {
            case "steporder":
                return mStepOrder++;
        }
        return parameter;
    }


    /// <summary>
    /// Handles unigrid actions.
    /// </summary>
    protected void ugSteps_OnAction(string actionName, object actionArgument)
    {
        // Name of checkout process step
        string stepName = ValidationHelper.GetString(actionArgument, "");

        switch (actionName.ToLowerCSafe())
        {
            case "up":
                // Move node up in xml
                CheckoutProcess.MoveCheckoutProcessStepNodeUp(stepName);

                // Update xml definition in view state
                CheckoutProcessXml = CheckoutProcess.GetXmlDefinition();

                RaiseDefinitionUpdate("moveup");

                ReloadData();
                break;

            case "down":
                // Move node down in xml definition
                CheckoutProcess.MoveCheckoutProcessStepNodeDown(stepName);

                // Update xml definition in viewstate
                CheckoutProcessXml = CheckoutProcess.GetXmlDefinition();

                RaiseDefinitionUpdate("movedown");

                ReloadData();
                break;

            case "edit":
                ListingMode = false;
                plcDefaultTypes.Visible = EnableDefaultCheckoutProcessTypes;

                // Load step data to the form
                CheckoutProcessStepInfo stepObj = CheckoutProcess.GetCheckoutProcessStepInfo(stepName);
                if (stepObj != null)
                {
                    breadcrumbsText = ResHelper.LocalizeString(stepObj.Caption);

                    txtStepCaption.Text = stepObj.Caption;
                    txtStepControlPath.Text = stepObj.ControlPath;
                    txtStepImageUrl.Text = stepObj.Icon;
                    txtStepName.Text = stepObj.Name;
                    chkLiveSite.Checked = stepObj.ShowOnLiveSite;
                    chkCMSDeskOrder.Checked = stepObj.ShowInCMSDeskOrder;
                    chkCMSDeskCustomer.Checked = stepObj.ShowInCMSDeskCustomer;
                    chkCMSDeskOrderItems.Checked = stepObj.ShowInCMSDeskOrderItems;

                    // Save original step name
                    OriginalStepName = stepObj.Name;
                }
                break;

            case "delete":
                // Remove node from xml
                CheckoutProcess.RemoveCheckoutProcessStepNode(stepName);
                // Update xml definition in view state
                CheckoutProcessXml = CheckoutProcess.GetXmlDefinition();

                RaiseDefinitionUpdate("delete");

                ReloadData();
                break;
        }
    }


    /// <summary>
    /// LnkNewStep click event handler.
    /// </summary>
    protected void lnkNewStep_Click(object sender, EventArgs e)
    {
        NewStep();
    }


    /// <summary>
    /// BtnDefaultProcess click event handler.
    /// </summary>
    protected void btnDefaultProcess_Click(object sender, EventArgs e)
    {
        GenerateDefaultProcess();
    }


    /// <summary>
    /// BtnOk click event handler.
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        breadcrumbsText = GetString("CheckoutProcess.NewStep");

        string errorMessage = ValidateForm();

        if (errorMessage == "")
        {
            CheckoutProcessStepInfo stepObj = CheckoutProcess.GetCheckoutProcessStepInfo(txtStepName.Text.Trim());
            if ((stepObj == null) || (stepObj.Name.ToLowerCSafe() == OriginalStepName.ToLowerCSafe()))
            {
                if (stepObj == null)
                {
                    stepObj = new CheckoutProcessStepInfo();
                }

                // Save localization
                txtStepCaption.Save();

                // Get step data from form
                stepObj.Caption = txtStepCaption.Text.Trim();
                stepObj.Name = txtStepName.Text.Trim();
                stepObj.ControlPath = txtStepControlPath.Text.Trim();
                stepObj.Icon = txtStepImageUrl.Text.Trim();
                stepObj.ShowInCMSDeskCustomer = chkCMSDeskCustomer.Checked;
                stepObj.ShowInCMSDeskOrder = chkCMSDeskOrder.Checked;
                stepObj.ShowOnLiveSite = chkLiveSite.Checked;
                stepObj.ShowInCMSDeskOrderItems = chkCMSDeskOrderItems.Checked;

                if ((OriginalStepName != "") && (OriginalStepName.ToLowerCSafe() != txtStepName.Text.ToLowerCSafe()))
                {
                    // Replace node
                    CheckoutProcess.ReplaceCheckoutProcessStepNode(stepObj, OriginalStepName);
                }
                else
                {
                    // Update or insert node
                    CheckoutProcess.SetCheckoutProcessStepNode(stepObj);
                }

                // Update Xml definition in viewstate
                CheckoutProcessXml = CheckoutProcess.GetXmlDefinition();

                if (OnCheckoutProcessDefinitionUpdate != null)
                {
                    OnCheckoutProcessDefinitionUpdate("update");
                }

                breadcrumbsText = ResHelper.LocalizeString(stepObj.Caption);

                ListingMode = true;
                ReloadData();
                ugSteps.ReloadData();
            }
            else
            {
                errorMessage = GetString("CheckoutProcess.ErrorStepNameNotUnique");
            }
        }

        // Show error message
        if (errorMessage != "")
        {
            plcMessNew.ShowError(errorMessage);

            // If error during editing, set original caption to breadcrumbs
            if (!string.IsNullOrEmpty(OriginalStepName))
            {
                CheckoutProcessStepInfo stepObj = CheckoutProcess.GetCheckoutProcessStepInfo(OriginalStepName);
                if (stepObj != null)
                {
                    breadcrumbsText = ResHelper.LocalizeString(stepObj.Caption);
                }
            }
        }
    }


    /// <summary>
    /// Validates form input data and returns error message if some error occurs.
    /// </summary>
    private string ValidateForm()
    {
        return new Validator().NotEmpty(txtStepCaption.Text.Trim(), rfvStepCaption.ErrorMessage).
            NotEmpty(txtStepName.Text.Trim(), rfvStepName.ErrorMessage).
            NotEmpty(txtStepControlPath.Text.Trim(), rfvStepControlPath.ErrorMessage).
            IsCodeName(txtStepName.Text.Trim(), GetString("General.ErrorCodeNameInIdentifierFormat")).Result;
    }


    /// <summary>
    /// Reloads data in unigrid.
    /// </summary>
    public void ReloadData()
    {
        // Load xml definition from viewstate
        CheckoutProcess.LoadXmlDefinition(CheckoutProcessXml);

        // Create data source for unigrid
        DataSet ds = new DataSet();
        ds.Tables.Add(CheckoutProcess.GetDataTableFromXmlDefinition(CheckoutProcessType));

        // Fill unigrid
        ugSteps.DataSource = ds;
        ugSteps.DataBind();
    }


    /// <summary>
    /// Check validity of the control.
    /// </summary>
    public override bool IsValid()
    {
        if ((string)Value == "")
        {
            ValidationError = GetString("CheckoutProcess.ErrorProcessEmpty");
            return false;
        }

        ValidationError = "";
        return true;
    }


    /// <summary>
    /// Opens new step form.
    /// </summary>
    public void NewStep()
    {
        ListingMode = false;
        plcDefaultTypes.Visible = EnableDefaultCheckoutProcessTypes;
        breadcrumbsText = GetString("CheckoutProcess.NewStep");

        // Set default values
        txtStepCaption.Text = "";
        txtStepControlPath.Text = "";
        txtStepImageUrl.Text = "";
        txtStepName.Text = "";
        chkCMSDeskCustomer.Checked = false;
        chkCMSDeskOrder.Checked = false;
        chkLiveSite.Checked = false;
        chkCMSDeskOrderItems.Checked = false;

        // Clear original step name
        OriginalStepName = "";
    }


    /// <summary>
    /// Generates default process.
    /// </summary>
    public void GenerateDefaultProcess()
    {
        RaiseDefinitionUpdate("defaultprocess");
        ugSteps.ReloadData();
    }


    /// <summary>
    /// Generates process from global.
    /// </summary>
    public void GenerateFromGlobalProcess()
    {
        RaiseDefinitionUpdate("fromglobalprocess");
        ugSteps.ReloadData();
    }


    /// <summary>
    /// Raises OnCheckoutProcessDefinitionUpdate event with given update reason name.
    /// </summary>
    /// <param name="reason">Update reason name constant.</param>
    protected void RaiseDefinitionUpdate(string reason)
    {
        if (OnCheckoutProcessDefinitionUpdate != null)
        {
            OnCheckoutProcessDefinitionUpdate(reason);
        }
    }

    #endregion
}