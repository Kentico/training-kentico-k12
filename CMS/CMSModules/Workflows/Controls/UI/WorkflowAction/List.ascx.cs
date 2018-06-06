using System;

using CMS.Base;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Workflows_Controls_UI_WorkflowAction_List : CMSAdminListControl
{

    #region "Properties"


    /// <summary>
    /// Current object type.
    /// </summary>
    public string ObjectType
    {
        get
        {
            return gridElem.ObjectType;
        }
        set
        {
            gridElem.ObjectType = value;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Init Unigrid
        gridElem.OnAction += uniGrid_OnAction;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
        gridElem.OnBeforeDataReload += gridElem_OnBeforeDataReload;
    }


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that throws event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void uniGrid_OnAction(string actionName, object actionArgument)
    {
        int objectId = ValidationHelper.GetInteger(actionArgument, 0);

        if (actionName == "edit")
        {
            URLHelper.Redirect(UIContextHelper.GetElementUrl("CMS.OnlineMarketing", "ActionProperties", false, objectId));
        }
    }


    private void gridElem_OnBeforeDataReload()
    {
        // Hide allowed objects column when not in development mode
        gridElem.NamedColumns["scope"].Visible = SystemContext.DevelopmentMode;
    }


    private object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "enabled":
                return UniGridFunctions.ColoredSpanYesNo(parameter, true);

            case "scope":
                {
                    string types = ValidationHelper.GetString(parameter, "");
                    var typeList = TypeHelper.GetTypes(types);
                    if (typeList.Count == 0)
                    {
                        return GetString("general.selectall");
                    }
                    string list = "";

                    typeList.ForEach(s => list += ResHelper.GetString(ResHelper.GetAPIString(TypeHelper.GetTasksResourceKey(s), s)) + ", ");

                    return list.Trim().TrimEnd(',');
                }
        }

        return parameter;
    }

    #endregion
}