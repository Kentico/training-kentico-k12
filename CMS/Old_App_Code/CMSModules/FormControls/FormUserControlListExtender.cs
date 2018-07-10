using System;

using CMS;
using CMS.Base;
using CMS.Core;
using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.UIControls;

[assembly: RegisterCustomClass("FormUserControlListExtender", typeof(FormUserControlListExtender))]

/// <summary>
/// Form User Control unigrid extender
/// </summary>
public class FormUserControlListExtender : ControlExtender<UniGrid>
{
    public override void OnInit()
    {
        Control.OnExternalDataBound += OnExternalDataBound;
    }


    /// <summary>
    /// Handles external databound event of the unigrid.
    /// </summary>
    private object OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        if (sourceName.EqualsCSafe("controltype", true))
        {
            if ((parameter != null) && (parameter != DBNull.Value))
            {
                return CoreServices.Localization.GetString("formusercontroltypeenum." + FormUserControlInfoProvider.GetTypeEnum(ValidationHelper.GetInteger(parameter, 0)));
            }
        }

        return null;
    }
}