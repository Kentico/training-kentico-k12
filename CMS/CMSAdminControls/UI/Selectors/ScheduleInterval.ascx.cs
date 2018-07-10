using System;
using System.Drawing;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Scheduler;
using CMS.UIControls;


public partial class CMSAdminControls_UI_Selectors_ScheduleInterval : BaseSchedulingControl
{
    #region "Public properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Control providing amount of time units
    /// </summary>
    public override CMSTextBox Quantity
    {
        get
        {
            return txtEvery;
        }
    }


    /// <summary>
    /// Control providing window of opportunity start hours
    /// </summary>
    public override CMSTextBox FromHours
    {
        get
        {
            return txtFromHours;
        }
    }


    /// <summary>
    /// Control providing window of opportunity start minutes
    /// </summary>
    public override CMSTextBox FromMinutes
    {
        get
        {
            return txtFromMinutes;
        }
    }


    /// <summary>
    /// Control providing window of opportunity end hours
    /// </summary>
    public override CMSTextBox ToHours
    {
        get
        {
            return txtToHours;
        }
    }


    /// <summary>
    /// Control providing window of opportunity end minutes
    /// </summary>
    public override CMSTextBox ToMinutes
    {
        get
        {
            return txtToMinutes;
        }
    }


    /// <summary>
    /// Control providing start time
    /// </summary>
    public override DateTimePicker StartTime
    {
        get
        {
            return dateTimePicker;
        }
    }


    /// <summary>
    /// Control providing time unit used for scheduling
    /// </summary>
    public override CMSDropDownList Period
    {
        get
        {
            return drpPeriod;
        }
    }


    /// <summary>
    /// Control providing allowed weekdays
    /// </summary>
    public override CMSCheckBoxList WeekDays
    {
        get
        {
            return chkWeek;
        }
    }


    /// <summary>
    /// Control providing allowed weekend days
    /// </summary>
    public override CMSCheckBoxList WeekEnd
    {
        get
        {
            return chkWeekEnd;
        }
    }


    /// <summary>
    /// Control to select date mode for month
    /// </summary>
    public override CMSRadioButton MonthModeDate
    {
        get
        {
            return radMonthDate;
        }
    }


    /// <summary>
    /// Control providing day in the month
    /// </summary>
    public override CMSDropDownList MonthDate
    {
        get
        {
            return drpMonthDate;
        }
    }


    /// <summary>
    /// Control to select specification mode for month
    /// </summary>
    public override CMSRadioButton MonthModeSpecification
    {
        get
        {
            return radMonthSpecification;
        }
    }


    /// <summary>
    /// Control providing order of the desired day in the month
    /// </summary>
    public override CMSDropDownList MonthOrder
    {
        get
        {
            return drpMonthOrder;
        }
    }


    /// <summary>
    /// Control providing desired day of week in the month
    /// </summary>
    public override CMSDropDownList MonthDay
    {
        get
        {
            return drpMonthDay;
        }
    }


    /// <summary>
    /// If true, start time text-box is displayed
    /// </summary>
    public bool DisplayStartTime
    {
        get
        {
            return pnlStartTime.Visible;
        }
        set
        {
            pnlStartTime.Visible = value;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        EnsureChildControls();
        if (!RequestHelper.IsPostBack())
        {
            ReloadData();
        }
    }


    protected override void CreateChildControls()
    {
        base.CreateChildControls();

        // Control initialization
        StartTime.DateTimeTextBox.ForeColor = Color.Black;
        Quantity.ForeColor = Color.Black;
        FromHours.ForeColor = Color.Black;
        FromMinutes.ForeColor = Color.Black;
        ToHours.ForeColor = Color.Black;
        ToMinutes.ForeColor = Color.Black;

        ValidatorInit();
        if (!RequestHelper.IsPostBack())
        {
            ControlInit();
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!WeekListChecked)
        {
            // Initialize week and weekend selectors
            WeekDays.ClearSelection();
            foreach (ListItem li in WeekDays.Items)
            {
                li.Selected = true;
            }
            WeekEnd.ClearSelection();
            foreach (ListItem li in WeekEnd.Items)
            {
                li.Selected = true;
            }
        }
    }


    /// <summary>
    /// Reloads form with values from the schedule interval.
    /// </summary>
    public void ReloadData()
    {
        EnsureChildControls();

        TaskInterval ti = TaskInterval;
        if (ti == null)
        {
            ti = new TaskInterval();
            ti.Every = 1;
            ti.Period = SchedulingHelper.PERIOD_DAY;
            ti.StartTime = DateTime.Now;
        }

        // Set period type
        Period.SelectedValue = ti.Period;
        OnPeriodChangeInit();

        // Start time
        if (StartTime != null)
        {
            StartTime.SelectedDateTime = ti.StartTime;
        }

        switch (Period.SelectedValue)
        {
            case SchedulingHelper.PERIOD_SECOND:
            case SchedulingHelper.PERIOD_MINUTE:
            case SchedulingHelper.PERIOD_HOUR:
                Quantity.Text = ti.Every.ToString();
                FromHours.Text = ti.BetweenStart.TimeOfDay.Hours >= 10 ? ti.BetweenStart.TimeOfDay.Hours.ToString() : "0" + ti.BetweenStart.TimeOfDay.Hours.ToString();
                FromMinutes.Text = ti.BetweenStart.TimeOfDay.Minutes >= 10 ? ti.BetweenStart.TimeOfDay.Minutes.ToString() : "0" + ti.BetweenStart.TimeOfDay.Minutes.ToString();
                ToHours.Text = ti.BetweenEnd.TimeOfDay.Hours >= 10 ? ti.BetweenEnd.TimeOfDay.Hours.ToString() : "0" + ti.BetweenEnd.TimeOfDay.Hours.ToString();
                ToMinutes.Text = ti.BetweenEnd.TimeOfDay.Minutes >= 10 ? ti.BetweenEnd.TimeOfDay.Minutes.ToString() : "0" + ti.BetweenEnd.TimeOfDay.Minutes.ToString();

                SetDays(WeekDays, ti.Days);
                SetDays(WeekEnd, ti.Days);
                break;

            case SchedulingHelper.PERIOD_DAY:
                Quantity.Text = ti.Every.ToString();

                SetDays(WeekDays, ti.Days);
                SetDays(WeekEnd, ti.Days);
                break;

            case SchedulingHelper.PERIOD_WEEK:
            case SchedulingHelper.PERIOD_YEAR:
                Quantity.Text = ti.Every.ToString();
                break;

            case SchedulingHelper.PERIOD_MONTH:
                if (string.IsNullOrEmpty(ti.Day))
                {
                    MonthDate.SelectedValue = ti.Order.ToLowerCSafe();
                    MonthModeDate.Checked = true;
                    MonthModeSelectionChanged(true);
                }
                else
                {
                    MonthOrder.SelectedValue = ti.Order.ToLowerCSafe();
                    MonthModeSpecification.Checked = true;
                    MonthModeSelectionChanged(false);
                    MonthDay.SelectedValue = ti.Day;
                }
                break;
        }

        WeekListChecked = (TaskInterval != null);
    }


    /// <summary>
    /// Initialization of validators.
    /// </summary>
    protected void ValidatorInit()
    {
        string error = GetString("ScheduleInterval.WrongFormat");
        string empty = GetString("ScheduleInterval.ErrorEmpty");
        // 'Every' panel validators
        rfvEvery.ErrorMessage = empty;
        rvEvery.MinimumValue = QUANTITY_MINIMUM.ToString();
        rvEvery.MaximumValue = QUANTITY_MAXIMUM.ToString();
        rvEvery.ErrorMessage = error;
        // 'Between' panel validators
        rfvFromHours.ErrorMessage = empty;
        rvFromHours.MinimumValue = HOURS_MINIMUM.ToString();
        rvFromHours.MaximumValue = HOURS_MAXIMUM.ToString();
        rvFromHours.ErrorMessage = error;
        rfvFromMinutes.ErrorMessage = empty;
        rvFromMinutes.MinimumValue = MINUTES_MINIMUM.ToString();
        rvFromMinutes.MaximumValue = MINUTES_MAXIMUM.ToString();
        rvFromMinutes.ErrorMessage = error;
        rfvToHours.ErrorMessage = empty;
        rvToHours.MinimumValue = HOURS_MINIMUM.ToString();
        rvToHours.MaximumValue = HOURS_MAXIMUM.ToString();
        rvToHours.ErrorMessage = error;
        rfvToMinutes.ErrorMessage = empty;
        rvToMinutes.MinimumValue = MINUTES_MINIMUM.ToString();
        rvToMinutes.MaximumValue = MINUTES_MAXIMUM.ToString();
        rvToMinutes.ErrorMessage = error;
        rfvInterval.ErrorMessage = String.Format("{0} {1}.", GetString("BasicForm.ErrorInvalidDateTime"), DateTime.Now);
    }


    /// <summary>
    /// Initializes dialog according to selected period.
    /// </summary>
    protected override void OnPeriodChangeInit()
    {
        switch (drpPeriod.SelectedValue)
        {
            case SchedulingHelper.PERIOD_SECOND: // Second
                pnlEvery.Visible = true;
                pnlBetween.Visible = true;
                pnlDays.Visible = true;
                pnlMonth.Visible = false;
                lblEveryPeriod.Text = GetString("ScheduleInterval.Period.second");
                break;

            case SchedulingHelper.PERIOD_MINUTE: // Minute
                pnlEvery.Visible = true;
                pnlBetween.Visible = true;
                pnlDays.Visible = true;
                pnlMonth.Visible = false;
                lblEveryPeriod.Text = GetString("ScheduleInterval.Period.Minute");
                break;

            case SchedulingHelper.PERIOD_HOUR: // Hour
                pnlEvery.Visible = true;
                pnlBetween.Visible = true;
                pnlDays.Visible = true;
                pnlMonth.Visible = false;
                lblEveryPeriod.Text = GetString("ScheduleInterval.Period.Hour");
                break;

            case SchedulingHelper.PERIOD_DAY: // Day
                pnlEvery.Visible = true;
                pnlBetween.Visible = false;
                pnlDays.Visible = true;
                pnlMonth.Visible = false;
                lblEveryPeriod.Text = GetString("ScheduleInterval.Period.Day");
                break;

            case SchedulingHelper.PERIOD_WEEK: // Week
                pnlEvery.Visible = true;
                pnlBetween.Visible = false;
                pnlDays.Visible = false;
                pnlMonth.Visible = false;
                lblEveryPeriod.Text = GetString("ScheduleInterval.Period.Week");
                break;

            case SchedulingHelper.PERIOD_YEAR: // Year
                pnlEvery.Visible = true;
                pnlBetween.Visible = false;
                pnlDays.Visible = false;
                pnlMonth.Visible = false;
                lblEveryPeriod.Text = GetString("ScheduleInterval.Period.Year");
                break;

            case SchedulingHelper.PERIOD_MONTH: // Month
                pnlEvery.Visible = false;
                pnlBetween.Visible = false;
                pnlDays.Visible = false;
                pnlMonth.Visible = true;
                break;

            case SchedulingHelper.PERIOD_ONCE: // Once
                pnlEvery.Visible = false;
                pnlBetween.Visible = false;
                pnlDays.Visible = false;
                pnlMonth.Visible = false;
                break;
        }

        // Set default values to text boxes and check-box lists
        StartTime.SelectedDateTime = DateTime.Now;

        Quantity.Text = QUANTITY_MINIMUM.ToString();
        FromHours.Text = HOURS_MINIMUM.ToString();
        FromMinutes.Text = MINUTES_MINIMUM.ToString();
        ToHours.Text = HOURS_MAXIMUM.ToString();
        ToMinutes.Text = MINUTES_MAXIMUM.ToString();

        MonthDate.SelectedIndex = 0;
        MonthOrder.SelectedIndex = 0;
        MonthDay.SelectedIndex = 0;
    }


    /// <summary>
    /// On selected index changed.
    /// </summary>
    protected void DrpPeriod_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        OnPeriodChangeInit();
    }


    /// <summary>
    /// On 1st radio button change.
    /// </summary>
    protected void radMonthDate_CheckedChanged(object sender, EventArgs e)
    {
        MonthModeSelectionChanged(true);
    }


    /// <summary>
    /// On 2nd radio button change.
    /// </summary>
    protected void radMonthSpecification_CheckedChanged(object sender, EventArgs e)
    {
        MonthModeSelectionChanged(false);
    }

    #endregion
}