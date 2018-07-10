using System;

using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Widgets_LiveDialogs_WidgetProperties_Properties_Frameset : CMSWidgetPropertiesLivePage
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        rowsFrameset.Attributes.Add("rows", "*," + FooterFrameHeight);
        frameContent.Attributes.Add("src", "widgetproperties_properties.aspx" + RequestContext.CurrentQueryString);
        frameButtons.Attributes.Add("src", "widgetproperties_buttons.aspx" + RequestContext.CurrentQueryString);
    }
}