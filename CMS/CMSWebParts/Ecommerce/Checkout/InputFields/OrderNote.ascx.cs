using System;

using CMS.Base.Web.UI;
using CMS.Ecommerce.Web.UI;


/// <summary>
/// Order note Web part
/// </summary>
public partial class CMSWebParts_Ecommerce_Checkout_InputFields_OrderNote : CMSCheckoutWebPart
{
    #region "Page events"

    /// <summary>
    /// Control initialization after postback events.
    /// </summary>  
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        // Pre-load value if empty
        if (string.IsNullOrEmpty(txtOrderNode.Text))
        {
            txtOrderNode.Text = ShoppingCart.ShoppingCartNote;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Saves the data.
    /// </summary>
    /// <param name="e">The StepEventArgs instance containing the event data.</param>
    protected override void SaveStepData(object sender, StepEventArgs e)
    {
        base.SaveStepData(sender, e);

        ShoppingCart.ShoppingCartNote = txtOrderNode.Text;
    }

    #endregion
}