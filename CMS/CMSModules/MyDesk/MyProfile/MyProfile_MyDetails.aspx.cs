using System;

using CMS.UIControls;


[UIElement("CMS", "MyProfile.Details")]
public partial class CMSModules_MyDesk_MyProfile_MyProfile_MyDetails : CMSContentManagementPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Set non live site mode here
        ucMyDetails.IsLiveSite = false;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Title = GetString("MyAccount.MyDetails");
    }
}