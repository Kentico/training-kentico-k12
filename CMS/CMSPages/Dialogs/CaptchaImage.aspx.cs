using System;
using System.Drawing.Imaging;
using System.Web;
using System.Web.UI;

using CMS.Helpers;
using CMS.UIControls;

#pragma warning disable BH3502 // Page must inherit from right class.
public partial class CMSPages_Dialogs_CaptchaImage : Page
#pragma warning restore BH3502 // Page must inherit from right class.
{
    #region "Constants"

    private const int MAXSIDESIZE = 2000;

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        string captcha = QueryHelper.GetString("captcha", "");
        int width = QueryHelper.GetInteger("width", 80);
        if (width > MAXSIDESIZE)
        {
            width = MAXSIDESIZE;
        }
        int height = QueryHelper.GetInteger("height", 20);
        if (height > MAXSIDESIZE)
        {
            height = MAXSIDESIZE;
        }

        if (WindowHelper.GetItem("CaptchaImageText" + captcha) != null)
        {
            bool useWarp = QueryHelper.GetBoolean("useWarp", true);

            // Create a CAPTCHA image using the text stored in the Session object.
            CaptchaImage ci = new CaptchaImage(WindowHelper.GetItem("CaptchaImageText" + captcha).ToString(), width, height, null, useWarp);

            // Change the response headers to output a JPEG image.
            Response.Clear();
            Response.ContentType = "image/jpeg";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();

            RequestHelper.EndResponse();
        }
    }

    #endregion
}