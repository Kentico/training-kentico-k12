using System;

using CMS.DocumentEngine;

namespace Business.Dto.LandingPage
{
    public class EventLandingPageDto : LandingPageDto
    {
        public DateTime EventDate { get; set; }

        public static Func<DocumentQuery<CMS.DocumentEngine.Types.MedioClinic.EventLandingPage>, DocumentQuery<CMS.DocumentEngine.Types.MedioClinic.EventLandingPage>> QueryModifier =
            (originalQuery) =>
                    originalQuery.AddColumns("EventDate");

        public static Func<CMS.DocumentEngine.Types.MedioClinic.EventLandingPage, EventLandingPageDto, EventLandingPageDto> Selector =
            (landingPage, landingPageDto) =>
                {
                    landingPageDto.EventDate = landingPage.EventDate;

                    return landingPageDto;
                };
    }
}
