using System;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Content_Controls_Dialogs_YouTube_YouTubeSizes : CMSUserControl
{
    #region "Variables"

    private int mMaxSideSize = 60;
    private string mOnSelectedItemClick = "";

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the maximal size of the rectangle in preview.
    /// </summary>
    public int MaxSideSize
    {
        get
        {
            return mMaxSideSize;
        }
        set
        {
            mMaxSideSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the JavaScript code which is prcessed when some size is clicked.
    /// </summary>
    public string OnSelectedItemClick
    {
        get
        {
            return mOnSelectedItemClick;
        }
        set
        {
            mOnSelectedItemClick = value;
        }
    }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public int SelectedWidth
    {
        get
        {
            return ValidationHelper.GetInteger(hdnWidth.Value, 0);
        }
        set
        {
            hdnWidth.Value = value.ToString();
        }
    }


    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public int SelectedHeight
    {
        get
        {
            return ValidationHelper.GetInteger(hdnHeight.Value, 0);
        }
        set
        {
            hdnHeight.Value = value.ToString();
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // Register scripts
        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "YouTubeSizes", ScriptHelper.GetScript(
            "function setSizes(width, height) { \n" +
            "  var widthElem = document.getElementById('" + hdnWidth.ClientID + "'); \n" +
            "  var heightElem = document.getElementById('" + hdnHeight.ClientID + "'); \n" +
            "  if ((widthElem != null) && (heightElem != null)) { \n" +
            "    widthElem.value = width; \n" +
            "    heightElem.value = height; \n" + OnSelectedItemClick + "; \n" +
            "  } \n" +
            "} \n"));
    }


    #region "Public methods"

    /// <summary>
    /// Loads the sizes previews.
    /// </summary>
    /// <param name="sizes">Array with sizes (two items per box)</param>
    public void LoadSizes(int[] sizes)
    {
        plcSizes.Controls.Clear();
        if (sizes.Length > 1)
        {
            int max = sizes[0];
            foreach (int size in sizes)
            {
                if (size > max)
                {
                    max = size;
                }
            }

            var radioList = new Panel
            {
                CssClass = "radio-list-vertical"
            };

            plcSizes.Controls.Add(radioList);

            for (int i = 0; i < sizes.Length; i += 2)
            {
                var radio = new CMSRadioButton();
                radio.Text = sizes[i] + " x " + sizes[i + 1];
                radio.Attributes.Add("onclick", "setSizes('" + sizes[i] + "', '" + sizes[i + 1] + "');");
                radio.GroupName = "size";

                if ((sizes[i] == SelectedWidth) && (sizes[i + 1] == SelectedHeight))
                {
                    radio.InputAttributes.Add("checked", "checked");
                }

                radioList.Controls.Add(radio);
            }
        }
    }

    #endregion
}
