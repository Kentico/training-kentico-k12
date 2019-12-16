using System;
using System.Collections.Generic;
using System.Web.Http;

using Business.Dto.Airport;
using Business.Repository.Airport;

namespace MedioClinic.Controllers.PageTemplates
{
    public class AirportsController : ApiController
    {
        public IAirportRepository AirportRepository { get; }

        public AirportsController()
        {
        }

        public AirportsController(IAirportRepository airportRepository)
        {
            AirportRepository = airportRepository ?? throw new ArgumentNullException(nameof(airportRepository));
        }

        public IHttpActionResult GetAirports(string searchPhrase)
        {
            IEnumerable<AirportDto> dtos = null;

            if (!string.IsNullOrWhiteSpace(searchPhrase) && searchPhrase.Length > 1)
            {
                dtos = AirportRepository
                    .GetAirportDtos(searchPhrase);
            }

            return Ok(dtos);
        }
    }
}