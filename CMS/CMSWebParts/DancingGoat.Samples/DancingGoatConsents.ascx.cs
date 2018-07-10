using System;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.ContactManagement;
using CMS.Core;
using CMS.DataProtection;
using CMS.Helpers;
using CMS.Membership;
using CMS.Localization;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_DancingGoat_Samples_DancingGoatConsents : CMSAbstractWebPart
{
    private ICurrentUserContactProvider mCurrentUserContactProvider;
    private IConsentAgreementService mConsentAgreementService;
    private ContactInfo mCurrentContact;


    private ICurrentUserContactProvider CurrentUserContactProvider => mCurrentUserContactProvider ?? (mCurrentUserContactProvider = Service.Resolve<ICurrentUserContactProvider>());

    private IConsentAgreementService ConsentAgreementService => mConsentAgreementService ?? (mConsentAgreementService = Service.Resolve<IConsentAgreementService>());

    private ContactInfo CurrentContact
    {
        get
        {
            if (mCurrentContact == null)
            {
                // Try to get contact from cookie
                mCurrentContact = ContactManagementContext.CurrentContact;

                // If contact is not found, get the contact for current user regardless of the cookie level set
                if (mCurrentContact == null)
                {
                    mCurrentContact = CurrentUserContactProvider.GetContactForCurrentUser(MembershipContext.AuthenticatedUser);
                }
            }

            return mCurrentContact;
        }
    }


    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    protected void SetupControl()
    {
        if (CurrentContact == null)
        {
            return;
        }

        var consents = ConsentAgreementService.GetAgreedConsents(CurrentContact);
        var currentCulture = LocalizationContext.CurrentCulture.CultureCode;

        var dataSource = consents.Select(consent => new
        {
            ConsentID = consent.Id,
            ConsentDisplayName = consent.DisplayName,
            ConsentShortText = consent.GetConsentText(currentCulture).ShortText
        });
        rptConsents.DataSource = dataSource;
        rptConsents.DataBind();

        lblNoData.Visible = !consents.Any();
    }


    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        if (PortalContext.ViewMode.IsLiveSite())
        {
            Button btn = (Button)sender;
            int consentId = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            ConsentInfo consent = ConsentInfoProvider.GetConsentInfo(consentId);

            if ((consent != null) && (CurrentContact != null))
            {
                ConsentAgreementService.Revoke(CurrentContact, consent);
                SetupControl();
                lblInfo.Visible = true;
            }
        }
    }
}
