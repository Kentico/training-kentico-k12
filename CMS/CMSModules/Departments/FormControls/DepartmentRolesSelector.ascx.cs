using System;
using System.Collections.Generic;
using System.Data;

using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;


public partial class CMSModules_Departments_FormControls_DepartmentRolesSelector : ReadOnlyFormEngineUserControl
{
    #region "Variables"

    private bool mAllowEmpty = true;
    private NodePermissionsEnum mPermission = NodePermissionsEnum.Read;
    private TreeNode mEditedNode;

    #endregion


    #region "Properties"

    /// <summary>
    /// Indicates if role selector allow empty selection.
    /// </summary>
    public bool AllowEmpty
    {
        get
        {
            return mAllowEmpty;
        }
        set
        {
            mAllowEmpty = value;
        }
    }


    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }
        set
        {
            EnsureChildControls();
            base.Enabled = value;
            usRoles.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets if live iste property.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            EnsureChildControls();
            return base.IsLiveSite;
        }
        set
        {
            EnsureChildControls();
            base.IsLiveSite = value;
            usRoles.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Indicates if node should inherit parent permissions or not.
    /// </summary>
    public bool InheritParentPermissions
    {
        get;
        set;
    }


    /// <summary>
    /// Required document permission.
    /// </summary>
    public NodePermissionsEnum Permission
    {
        get
        {
            return mPermission;
        }
        set
        {
            mPermission = value;
        }
    }


    public override object ValueForValidation
    {
        get
        {
            return usRoles.Value;
        }
    }


    /// <summary>
    /// Gets document TreeNode.
    /// </summary>
    private TreeNode EditedNode
    {
        get
        {
            return mEditedNode ?? (mEditedNode = Form.EditedObject as TreeNode);
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Page_Load event.
    /// </summary>    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Form == null)
        {
            return;
        }

        // Check if node updated and role permissions should be loaded
        if (!usRoles.HasValue && (Form.Mode != FormModeEnum.Insert))
        {
            // Check if node has own ACL
            if (EditedNode.NodeIsACLOwner)
            {
                DataSet dsRoles = AclItemInfoProvider.GetAllowedRoles(ValidationHelper.GetInteger(EditedNode.GetValue("NodeACLID"), 0), Permission, "RoleID");
                if (!DataHelper.DataSourceIsEmpty(dsRoles))
                {
                    IList<string> roles = DataHelper.GetStringValues(dsRoles.Tables[0], "RoleID");
                    usRoles.Value = TextHelper.Join(";", roles);
                }
            }
        }

        // Set after save operation
        Form.OnAfterSave += AddRoles;

        // Initialize UniSelector
        Reload(false);
    }


    /// <summary>
    /// Reloads the selector's data.
    /// </summary>
    /// <param name="forceReload">Indicates whether data should be forcibly reloaded</param>
    public void Reload(bool forceReload)
    {
        // Set allow empty
        usRoles.AllowEmpty = AllowEmpty;

        // Set uniselector properties
        usRoles.ReturnColumnName = "RoleID";
        usRoles.WhereCondition = "(SiteID = " + SiteContext.CurrentSiteID + ") AND (RoleGroupID IS NULL)";

        if (forceReload)
        {
            usRoles.Reload(true);
        }
    }


    /// <summary>
    /// Creates child controls and loads update panle container if it is required.
    /// </summary>
    protected override void CreateChildControls()
    {
        // If selector is not defined load update panel container
        if (usRoles == null)
        {
            pnlUpdate.LoadContainer();
        }
        // Call base method
        base.CreateChildControls();
    }


    /// <summary>
    /// After node created, solver role permissions.
    /// </summary>
    private void AddRoles(object sender, EventArgs e)
    {
        string roleIds = ";" + usRoles.Value + ";";

        // Check if ACL should inherit from parent
        if (InheritParentPermissions)
        {
            AclInfoProvider.EnsureOwnAcl(EditedNode);
        }
        else
        {
            // If node has already own ACL don't leave permissions, otherwise break inheritance
            if (!EditedNode.NodeIsACLOwner)
            {
                AclInfoProvider.BreakInheritance(EditedNode, false);
            }
        }

        // Get original ACLItems
        DataSet ds = AclItemInfoProvider.GetACLItemsAndOperators(EditedNode.NodeID)
                                            .WhereStartsWith("Operator", "R")
                                            .WhereEquals("ACLID", EditedNode.NodeACLID)
                                            .Columns("Operator", "Allowed", "Denied");

        // Change original values
        if (!DataHelper.DataSourceIsEmpty(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string op = DataHelper.GetNotEmpty(dr["Operator"], "R");
                int allowed = ValidationHelper.GetInteger(dr["Allowed"], 0);
                int denied = ValidationHelper.GetInteger(dr["Denied"], 0);
                int aclRoleId = ValidationHelper.GetInteger(op.Substring(1), 0);

                if (aclRoleId != 0)
                {
                    // Check if read permission should be set or removed
                    if (roleIds.Contains(";" + aclRoleId + ";"))
                    {
                        // Remove role from processed role and adjust permissions in database
                        roleIds = roleIds.Replace(";" + aclRoleId + ";", ";");
                        allowed |= 1;
                    }
                    else
                    {
                        allowed &= 126;
                    }

                    RoleInfo ri = RoleInfoProvider.GetRoleInfo(aclRoleId);
                    AclItemInfoProvider.SetRolePermissions(EditedNode, allowed, denied, ri);
                }
            }
        }


        if (roleIds.Trim(';') == "")
        {
            return;
        }

        // Create ACL items for new roles
        string[] roles = roleIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string role in roles)
        {
            RoleInfo ri = RoleInfoProvider.GetRoleInfo(int.Parse(role));
            AclItemInfoProvider.SetRolePermissions(EditedNode, 1, 0, ri);
        }
    }


    /// <summary>
    /// Returns true if entered data is valid. If data is invalid, it returns false and displays an error message.
    /// </summary>
    public override bool IsValid()
    {
        if ((FieldInfo != null) && !FieldInfo.AllowEmpty && String.IsNullOrEmpty(usRoles.Value.ToString()))
        {
            ValidationError = ResHelper.GetString("BasicForm.ErrorEmptyValue");
            return false;
        }
        return true;
    }

    #endregion
}