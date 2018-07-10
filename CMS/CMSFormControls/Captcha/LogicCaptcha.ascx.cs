using System;
using System.Collections;
using System.Web.Caching;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;


public partial class CMSFormControls_Captcha_LogicCaptcha : FormEngineUserControl
{
    #region "Constants"

    private const string CHOICE_MATH_OPERATION = "mathoperation";
    private const string CHOICE_EQUAL = "equal";
    private const string CHOICE_COMPARE = "compare";
    private const string CHOICE_GREATER = "greater";
    private const string CHOICE_SMALLER = "smaller";
    private const string CHOICE_PLUS = "plus";
    private const string CHOICE_MINUS = "minus";
    private const string CHOICE_MULT = "mult";
    private const string CHOICE_DIV = "div";
    private const string CHOICE_STATEMENT = "s";
    private const string CHOICE_IMAGE = "i";

    #endregion


    #region "Variables"

    private bool useRadioButtons = false;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Indicates if should be used text or image CAPTCHA (False - Text, True - Image).
    /// </summary>
    public bool UseTextOrImageCaptcha
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseTextOrImageCaptcha"), false);
        }
        set
        {
            SetValue("UseTextOrImageCaptcha", value);
        }
    }


    /// <summary>
    /// Sets or gets whether math operations are used or not. 
    /// </summary>
    public bool UseMathOperations
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseMathOperations"), true);
        }
        set
        {
            SetValue("UseMathOperations", value);
        }
    }


    /// <summary>
    /// Sets or gets whether operations multiple and divide are allowed in math operations.
    /// </summary>
    public bool UseMultAndDiv
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseMultAndDiv"), false);
        }
        set
        {
            SetValue("UseMultAndDiv", value);
        }
    }


    /// <summary>
    /// Sets or gets whether compare operations are used or not.
    /// </summary>
    public bool UseCompareOperations
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseCompareOperations"), true);
        }
        set
        {
            SetValue("UseCompareOperations", value);
        }
    }


    /// <summary>
    /// Sets or gets whether statements are used or not.
    /// </summary>
    public bool UseStatements
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseStatements"), false);
        }
        set
        {
            SetValue("UseStatements", value);
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
            base.Enabled = value;
            txtAnswer.Enabled = value;
        }
    }


    /// <summary>
    /// Sets or gets whether convert operands to text.
    /// </summary>
    public bool ConvertOperandsToText
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ConvertOperandsToText"), true);
        }
        set
        {
            SetValue("ConvertOperandsToText", value);
        }
    }


    /// <summary>
    /// Sets or gets whether convert operators to text.
    /// </summary>
    public bool ConvertOperatorToText
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ConvertOperatorToText"), false);
        }
        set
        {
            SetValue("ConvertOperatorToText", value);
        }
    }


    /// <summary>
    /// Gets or sets field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return txtAnswer.Text;
        }
        set
        {
            txtAnswer.Text = ValidationHelper.GetString(value, string.Empty);
        }
    }


    /// <summary>
    /// Statement question.
    /// </summary>
    public string StatementQuestions
    {
        get
        {
            return ValidationHelper.GetString(GetValue("StatementQuestions"), string.Empty);
        }
        set
        {
            SetValue("StatementQuestions", value);
        }
    }


    /// <summary>
    /// Image question.
    /// </summary>
    public string ImageQuestions
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ImageQuestions"), string.Empty);
        }
        set
        {
            SetValue("ImageQuestions", value);
        }
    }


    /// <summary>
    /// Indicates whether the info label should be displayed.
    /// </summary>
    public bool ShowInfoLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowInfoLabel"), false);
        }
        set
        {
            SetValue("ShowInfoLabel", value);
        }
    }


    /// <summary>
    /// Width of the CAPTCHA image.
    /// </summary>
    public int ImageWidth
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ImageWidth"), 80);
        }
        set
        {
            SetValue("ImageWidth", value);
        }
    }


    /// <summary>
    /// Height of the CAPTCHA image.
    /// </summary>
    public int ImageHeight
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ImageHeight"), 20);
        }
        set
        {
            SetValue("ImageHeight", value);
        }
    }


    /// <summary>
    /// Indicates whether the after text should be displayed.
    /// </summary>
    public bool ShowAfterText
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowAfterText"), false);
        }
        set
        {
            SetValue("ShowAfterText", value);
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Page load.
    /// </summary>    
    protected void Page_Load(object sender, EventArgs e)
    {
        // Show info label
        if (ShowInfoLabel)
        {
            if (Form == null)
            {
                // If inside form, the label tag is provided by form label, otherwise it has own label
                lblSecurityCode.AssociatedControlClientID = txtAnswer.ClientID;
            }
            lblSecurityCode.Text = GetString("SecurityCode.lblSecurityCodeLogic");
            lblSecurityCode.Visible = true;
        }

        // Show after text
        plcAfterText.Visible = ShowAfterText;
        if (plcAfterText.Visible)
        {
            if (UseTextOrImageCaptcha)
            {
                lblAfterText.Text = GetString("SecurityCode.AfterText");
            }
            else
            {
                lblAfterText.Text = GetString("LogicCaptcha.Text.AfterText");
            }
        }

        useRadioButtons = (UseStatements && !string.IsNullOrEmpty(StatementQuestions) && !UseTextOrImageCaptcha && !UseMathOperations && !UseMultAndDiv);
    }


    /// <summary>
    ///  Page prerender.
    /// </summary>    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Regenerate CAPTCHA if possible
        if (!RequestHelper.IsAsyncPostback() || ControlsHelper.IsInUpdatePanel(this))
        {
            // Regenerate security code
            GenerateNew();

            // Update update panel if needed
            if (ControlsHelper.IsInUpdatePanel(this))
            {
                ControlsHelper.UpdateCurrentPanel(this);
            }
        }

        txtAnswer.Visible = !useRadioButtons;
        rdAnswerNo.Visible = useRadioButtons;
        rdAnswerYes.Visible = useRadioButtons;

        // Hide image code if it is not generated
        imgSecurityCode.Visible = !string.IsNullOrEmpty(imgSecurityCode.ImageUrl);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Returns true if user control is valid.
    /// </summary>
    public override bool IsValid()
    {
        bool isValid = false;
        string captcha = ValidationHelper.GetString(WindowHelper.GetItem("CaptchaImageText" + ClientID), string.Empty);

        if (!string.IsNullOrEmpty(captcha))
        {
            // Use radio buttons for answer
            if (useRadioButtons)
            {
                // Only bool answer (Yes/No)
                if (ValidationHelper.IsBoolean(captcha))
                {
                    // Captcha value from session
                    bool catpchaValue = ValidationHelper.GetBoolean(captcha, false);
                    // Compare answer
                    if ((catpchaValue && rdAnswerYes.Checked) || (!catpchaValue && rdAnswerNo.Checked))
                    {
                        return true;
                    }

                    return false;
                }
            }
            // User's value in textbox
            else
            {
                string value = txtAnswer.Text.Trim();

                isValid = (value == captcha);

                // Extension so user can write 1 or true for true and 0 or false for false.
                if (!isValid && ValidationHelper.IsBoolean(value) && ValidationHelper.IsBoolean(captcha)
                    && IsQuestionBoolean(ValidationHelper.GetString(SessionHelper.GetValue("CaptchaOperation" + ClientID), string.Empty)))
                {
                    isValid = (ValidationHelper.GetBoolean(value, false) == ValidationHelper.GetBoolean(captcha, false));
                }
            }
        }

        if (!isValid)
        {
            ValidationError = GetString("captcha.notcorrect");
        }

        return isValid;
    }


    /// <summary>
    /// Indicates if string is for boolean operation.
    /// </summary>
    /// <param name="choice">Boolean operation</param>
    private bool IsQuestionBoolean(string choice)
    {
        if (!string.IsNullOrEmpty(choice))
        {
            choice = choice.ToLowerCSafe();
            return ((choice == CHOICE_SMALLER) || (choice == CHOICE_GREATER) || (choice == CHOICE_EQUAL) || (choice == CHOICE_STATEMENT));
        }

        return false;
    }


    /// <summary>
    /// Returns a string of six random digits.
    /// </summary>    
    private string GenerateRandomCode()
    {
        Random random = new Random(ClientID.GetHashCode() + (int)DateTime.Now.Ticks);

        string s = string.Empty;

        for (int i = 0; i < 6; i++)
        {
            s = String.Concat(s, random.Next(10).ToString());
        }

        return s;
    }


    /// <summary>
    /// Generates new CAPTCHA.
    /// </summary>
    public void GenerateNew()
    {
        txtAnswer.Text = string.Empty;
        CreateQuestion();
    }


    /// <summary>
    /// Creates CAPTCHA question.
    /// </summary>   
    private void CreateQuestion()
    {
        // Possible operations: + - * / < > =, plus same number of statements, plus same number of text and image possibilities
        string[] questions = new string[28];
        int length = 0;

        // If use text CAPTCHA
        if (!UseTextOrImageCaptcha)
        {
            // Add basic math operations
            if (UseMathOperations)
            {
                questions[length] = CHOICE_PLUS;
                length++;
                questions[length] = CHOICE_MINUS;
                length++;
            }

            // Add multiplication and dividing
            if (UseMultAndDiv)
            {
                questions[length] = CHOICE_MULT;
                length++;
                questions[length] = CHOICE_DIV;
                length++;
            }

            // Add compare operations
            if (UseCompareOperations)
            {
                questions[length] = CHOICE_SMALLER;
                length++;
                questions[length] = CHOICE_GREATER;
                length++;
                questions[length] = CHOICE_EQUAL;
                length++;
            }

            // Add statements
            if (UseStatements)
            {
                if (length == 0)
                {
                    questions[0] = CHOICE_STATEMENT;
                    length++;
                }
                else
                {
                    int countStatements = length * 2;
                    // Add same number of statements as number of operations
                    for (int i = length; i != countStatements; i++)
                    {
                        questions[i] = CHOICE_STATEMENT;
                        length++;
                    }
                }
            }
        }
        // Use image CAPTCHA
        else
        {
            if (length == 0)
            {
                questions[0] = CHOICE_IMAGE;
                length++;
            }
            else
            {
                int countImages = length * 2;
                for (int i = length; i != countImages; i++)
                {
                    questions[i] = CHOICE_IMAGE;
                    length++;
                }
            }
        }

        // Choose captcha
        Random random = new Random(ClientID.GetHashCode() + (int)DateTime.Now.Ticks);
        int num = random.Next(length);

        string choice = questions[num];

        // Prepare operands
        int op1 = random.Next(1, 10);
        int op2 = random.Next(1, 10);
        int result = 0;
        string chosenOperator = string.Empty;
        string captchaQuestion = string.Empty;
        string catpchaChoiceString = "error";

        // Generate chosen CAPTCHA
        switch (choice)
        {
            // Statements
            case CHOICE_STATEMENT:
                result = GetStatement(out captchaQuestion, random);
                if (result == -1)
                {
                    return;
                }
                break;

            // Image CAPTCHA
            case CHOICE_IMAGE:
                result = GetImageQuestion(out captchaQuestion, random);
                break;

            // Math and compare operations
            case CHOICE_PLUS:
                result = op1 + op2;
                chosenOperator = "+";
                catpchaChoiceString = CHOICE_MATH_OPERATION;
                break;

            case CHOICE_MINUS:
                result = op1 - op2;
                chosenOperator = "-";
                catpchaChoiceString = CHOICE_MATH_OPERATION;
                break;

            case CHOICE_MULT:
                result = op1 * op2;
                chosenOperator = "*";
                catpchaChoiceString = CHOICE_MATH_OPERATION;
                break;

            case CHOICE_DIV:
                bool correct = true;
                while (correct)
                {
                    // Swap numbers
                    if (op1 < op2)
                    {
                        int swap = op1;
                        op1 = op2;
                        op2 = swap;
                    }
                    // Check rest of division
                    if ((op1 % op2) == 0)
                    {
                        result = op1 / op2;
                        correct = false;
                    }
                    // Generate new numbers
                    else
                    {
                        op1 = random.Next(1, 10);
                        op2 = random.Next(1, 10);
                    }
                }

                chosenOperator = "/";
                catpchaChoiceString = CHOICE_MATH_OPERATION;
                break;

            case CHOICE_EQUAL:
                result = (op1 == op2) ? 1 : 0;
                chosenOperator = "=";
                catpchaChoiceString = CHOICE_EQUAL;
                break;

            case CHOICE_SMALLER:
                result = (op1 < op2) ? 1 : 0;
                chosenOperator = "<";
                catpchaChoiceString = CHOICE_COMPARE;
                break;

            case CHOICE_GREATER:
                result = (op1 > op2) ? 1 : 0;
                chosenOperator = ">";
                catpchaChoiceString = CHOICE_COMPARE;
                break;

            default:
                break;
        }

        // Build CAPTCHA question
        if ((choice != CHOICE_STATEMENT) && (choice != CHOICE_IMAGE))
        {
            // Handle operands
            string strOp1 = op1.ToString();
            string strOp2 = op2.ToString();

            if (ConvertOperandsToText)
            {
                strOp1 = GetString("captcha." + strOp1);
                strOp2 = GetString("captcha." + strOp2);
            }

            // Handle operator
            if (ConvertOperatorToText)
            {
                chosenOperator = GetString("captcha." + choice);
            }

            captchaQuestion = String.Format(GetString("captcha.question." + catpchaChoiceString), chosenOperator, strOp1, strOp2);
        }

        // Store in session
        WindowHelper.Add("CaptchaImageText" + ClientID, result);
        SessionHelper.SetValue("CaptchaOperation" + ClientID, choice);

        lblQuestion.Text = GetString(captchaQuestion);

        // Image
        plcImage.Visible = (choice == CHOICE_IMAGE);
    }


    /// <summary>
    /// Gets random statement.
    /// <param name="statement">Output statement</param>
    /// <param name="random">Random generator</param>    
    /// </summary>
    private int GetStatement(out string statement, Random random)
    {
        statement = string.Empty;

        // Get random question
        string[] line = SelectLine(random, StatementQuestions);
        if ((line != null) && (line.Length == 2))
        {
            statement = line[0] + " " + GetString("captcha.statement");
            return ValidationHelper.GetInteger(line[1].TrimEnd('\r'), 0);
        }

        return -1;
    }


    /// <summary>
    /// Generates OCR CAPTCHA and get some question.
    /// </summary>
    /// <param name="question">Image question</param>
    /// <param name="random">Random generator</param>
    private int GetImageQuestion(out string question, Random random)
    {
        question = string.Empty;

        // Get random line from statements
        string[] line = SelectLine(random, ImageQuestions);
        if (line == null)
        {
            return -1;
        }

        string result = "0";

        // Handle chosen line
        if (line.Length == 2)
        {
            question = line[0];
            string statement = ValidationHelper.GetString(line[1].TrimEnd('\r'), string.Empty);

            // Generate random code
            string code = GenerateRandomCode();

            // Generate image            
            imgSecurityCode.ImageUrl = ResolveUrl(string.Format("~/CMSPages/Dialogs/CaptchaImage.aspx?hash={0}&captcha={1}&width={2}&height={3}", Guid.NewGuid(), ClientID + "Code", ImageWidth, ImageHeight));
            WindowHelper.Add("CaptchaImageText" + ClientID + "Code", code);

            // Get result
            switch (statement)
            {
                case "first":
                    result = code[0].ToString();
                    break;

                case "last":
                    result = code[5].ToString();
                    break;

                case "number":
                    // There is allways 6 digits in security code
                    result = "6";
                    break;

                case "firstandlast":
                    result = code[0].ToString() + code[5].ToString();
                    break;

                case "second":
                    result = code[1].ToString();
                    break;

                case "third":
                    result = code[2].ToString();
                    break;

                case "fourth":
                    result = code[3].ToString();
                    break;

                case "fith":
                    result = code[4].ToString();
                    break;

                case "firstandsecond":
                    result = code[0].ToString() + code[1].ToString();
                    break;

                case "max":
                    int max = 0;
                    for (int i = 0; i != code.Length; i++)
                    {
                        int current = ValidationHelper.GetInteger(code[i].ToString(), 0);
                        if (current > max)
                        {
                            max = current;
                        }
                    }
                    result = max.ToString();
                    break;

                case "min":
                    int min = 9;
                    for (int i = 0; i != code.Length; i++)
                    {
                        int current = ValidationHelper.GetInteger(code[i].ToString(), 9);
                        if (current < min)
                        {
                            min = current;
                        }
                    }
                    result = min.ToString();
                    break;

                default:
                    break;
            }
        }

        return ValidationHelper.GetInteger(result, 0);
    }


    /// <summary>
    /// Selects line randomly.
    /// </summary>
    /// <param name="random">Random generator</param>
    /// <param name="questions">Questions</param>
    /// <returns>Lines</returns>
    private string[] SelectLine(Random random, string questions)
    {
        ArrayList lines = null;

        // Get current culture
        string culture = ValidationHelper.GetString(ViewState["CultureCode"], LocalizationContext.PreferredCultureCode);
        string sitename = SiteContext.CurrentSiteName;

        // Get or generate cache item name
        string useCacheItemName = CacheHelper.BaseCacheKey + "|" + RequestContext.CurrentURL + "|" + ClientID + "|" + culture + "|" + UseTextOrImageCaptcha;

        // Try to get data from cache 
        if (CacheHelper.CacheMinutes(sitename) == 0)
        {
            // No caching
            CacheHelper.Remove(useCacheItemName);
            lines = GetLines(random, culture, questions);
        }
        else
        {
            object value = null;

            // Try to retrieve data from the cache
            if (!CacheHelper.TryGetItem(useCacheItemName, out value))
            {
                lines = GetLines(random, culture, questions);
                if ((lines != null) && (lines.Count > 0))
                {
                    CacheHelper.Add(useCacheItemName, lines, null, DateTime.Now.AddMinutes(CacheHelper.CacheMinutes(sitename)), Cache.NoSlidingExpiration);
                }
            }
            else
            {
                lines = (ArrayList)CacheHelper.GetItem(useCacheItemName);
            }
        }

        if ((lines != null) && (lines.Count > 0))
        {
            // Split selected line a return result.
            int selectedLine = random.Next(lines.Count);
            return lines[selectedLine].ToString().Split(';');
        }

        return null;
    }


    /// <summary>
    /// Gets all lines from specified file.
    /// </summary>
    /// <param name="path">Path to file</param>
    /// <param name="random">Random generator</param>
    /// <param name="cultureCode">Culture code</param>
    /// <param name="questions">Questions</param>
    private ArrayList GetLines(Random random, string cultureCode, string questions)
    {
        ArrayList lines = new ArrayList();

        if (string.IsNullOrEmpty(questions))
        {
            return null;
        }

        // Select one line, split it and return result
        string[] fileLines = questions.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        // Add all lines into array list
        foreach (string line in fileLines)
        {
            lines.Add(line);
        }

        return lines;
    }

    #endregion
}