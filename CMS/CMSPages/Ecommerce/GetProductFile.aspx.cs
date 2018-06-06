using System;

using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Base;
using CMS.UIControls;
using CMS.Routing.Web;

public partial class CMSPages_Ecommerce_GetProductFile : AbstractCMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set default error message
        var errorMessageResString = "getproductfile.existerror";

        // Get order item SKU file
        var token = QueryHelper.GetGuid("token", Guid.Empty);
        var orderedFile = OrderItemSKUFileInfoProvider.GetOrderItemSKUFileInfo(token);
        if (orderedFile != null)
        {
            // Get parent order item
            var orderItem = OrderItemInfoProvider.GetOrderItemInfo(orderedFile.OrderItemID);
            if (orderItem != null)
            {
                // If download is not expired
                if ((orderItem.OrderItemValidTo.CompareTo(DateTimeHelper.ZERO_TIME) == 0) || (orderItem.OrderItemValidTo.CompareTo(DateTime.Now) > 0))
                {
                    // Get SKU file
                    var skuFile = SKUFileInfoProvider.GetSKUFileInfo(orderedFile.FileID);

                    if (skuFile != null)
                    {
                        // Decide how to process the file based on file type
                        switch (skuFile.FileType.ToLowerCSafe())
                        {
                            case "metafile":
                                // Set parameters to current context
                                Context.Items["fileguid"] = skuFile.FileMetaFileGUID;
                                Context.Items["disposition"] = "attachment";

                                // Handle request using metafile handler
                                Response.Clear();
                                var handler = new GetMetaFileHandler();
                                handler.ProcessRequest(Context);
                                Response.End();

                                return;
                        }
                    }
                }
                else
                {
                    // Set error message
                    errorMessageResString = "getproductfile.expirederror";
                }
            }
        }

        // Perform server side redirect to error page
        Response.Clear();
        Server.Transfer(AdministrationUrlHelper.GetErrorPageUrl("getproductfile.error", errorMessageResString));
        Response.End();
    }
}