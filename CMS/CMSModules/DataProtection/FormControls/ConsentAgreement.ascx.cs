using System;

using CMS.FormEngine.Web.UI;
using CMS.Core;
using CMS.DataProtection;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.Localization;
using CMS.Helpers;

public partial class CMSModules_DataProtection_FormControls_ConsentAgreement : FormEngineUserControl
{
    private string SelectedConsent => FieldInfo?.Settings["Consent"] as string;


    public override object Value
    {
        get
        {
            return string.IsNullOrEmpty(hdnValueHolder.Value) ? null : hdnValueHolder.Value;
        }
        set
        {
            hdnValueHolder.Value = value?.ToString();
        }
    }


    public override bool Enabled
    {
        get
        {
            return chkConsent.Enabled;
        }
        set
        {
            base.Enabled = value;
            chkConsent.Enabled = value;
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (Form != null)
        {
            Form.OnBeforeSave += Form_OnBeforeSave;
        }

        if (FieldInfo != null)
        {
            litConsentText.Text = GetConsentReferenceMarkup();
        }
    }


    private void Form_OnBeforeSave(object sender, EventArgs e)
    {
        var info = Form.Data as BaseInfo;
        if (info == null)
        {
            return;
        }

        var service = Service.Resolve<IFormConsentAgreementService>();
        var consent = ConsentInfoProvider.GetConsentInfo(SelectedConsent);
        var contact = ContactManagementContext.GetCurrentContact();
        ConsentAgreementInfo agreement;

        if (chkConsent.Checked)
        {
            agreement = service.Agree(contact, consent, info);
        }
        else
        {
            agreement = service.Revoke(contact, consent, info);
        }

        StoreAgreementGuidInData(info, agreement);
    }


    private void StoreAgreementGuidInData(BaseInfo info, ConsentAgreementInfo agreement)
    {
        if (FieldInfo == null)
        {
            return;
        }

        var agreementGuid = agreement.ConsentAgreementGuid;
        info.SetValue(FieldInfo.Name, agreementGuid);
        Value = agreementGuid;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        chkConsent.Text = GetConsentText();

        if (Value == null)
        {
            return;
        }

        chkConsent.Checked = IsConsentAgreed();
    }


    private bool IsConsentAgreed()
    {
        var agreementGuid = ValidationHelper.GetGuid(Value, Guid.Empty);
        var isRevoked = ConsentAgreementInfoProvider.GetConsentAgreements()
                                                    .WithGuid(agreementGuid)
                                                    .Column("ConsentAgreementRevoked")
                                                    .GetScalarResult(true);
        return !isRevoked;
    }


    private string GetConsentText()
    {
        var consent = ConsentInfoProvider.GetConsentInfo(SelectedConsent);
        var currentCulture = LocalizationContext.CurrentCulture.CultureCode;

        return consent != null ? consent.GetConsentText(currentCulture).ShortText : string.Empty;
    }


    private string GetConsentReferenceMarkup()
    {
        var consentReferenceMarkupFieldName = "ConsentReferenceMarkup";

        if (FieldInfo.SettingIsMacro(consentReferenceMarkupFieldName))
        {
            return ContextResolver.ResolveMacros(FieldInfo.SettingsMacroTable[consentReferenceMarkupFieldName] as string);
        }

        return FieldInfo.Settings[consentReferenceMarkupFieldName] as string;
    }
}