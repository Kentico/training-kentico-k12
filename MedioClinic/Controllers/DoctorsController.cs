using System;
using System.Web.Mvc;
using System.Web.UI;
using Business.DependencyInjection;
using Business.Dto.Doctors;
using Business.Repository.Doctor;
using Business.Services.Cache;
using CMS.DocumentEngine.Types.MedioClinic;
using MedioClinic.Models.Doctors;

namespace MedioClinic.Controllers
{
    public class DoctorsController : BaseController
    {
        private IDoctorRepository DoctorRepository { get; }
        private IDoctorSectionRepository DoctorSectionRepository { get; }

        public DoctorsController(
            IBusinessDependencies dependencies,
            IDoctorRepository doctorRepository,
            IDoctorSectionRepository doctorSectionRepository
            ) : base(dependencies)
        {
            DoctorRepository = doctorRepository;
            DoctorSectionRepository = doctorSectionRepository;
        }

        public ActionResult Index()
        {
            var doctorSection = Dependencies.CacheService.Cache(
                () => DoctorSectionRepository.GetDoctorSection(), // Gets data for the DoctorSection if there isn't any cached data (data was invalidated or cache expired)
                60, // Sets caching of data to 60 minutes
                $"{nameof(DoctorsController)}|{nameof(Index)}|{nameof(DoctorSectionDto)}", // cached data identifier
                Dependencies.CacheService.GetNodesCacheDependencyKey(Doctor.CLASS_NAME, CacheDependencyType.All) // cache dependencies
                );

            if (doctorSection == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(new DoctorsViewModel()
            {
                Doctors = DoctorRepository.GetDoctors(),
                DoctorSection = doctorSection
            }, doctorSection.Header);

            return View(model);
        }

        [OutputCache(Duration = 3600, VaryByParam = "nodeGuid", Location = OutputCacheLocation.Server)]
        [Route("Detail/{nodeGuid}/{nodeAlias}")]
        public ActionResult Detail(Guid nodeGuid, string nodeAlias)
        {
            var doctor = DoctorRepository.GetDoctor(nodeGuid);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            // Sets cache dependency on single page based on NodeGUID
            // System clears the cache when given doctor is deleted or edited in Kentico
            Dependencies.CacheService.SetOutputCacheDependency(nodeGuid);

            var model = GetPageViewModel(new DoctorDetailViewModel()
            {
                Doctor = doctor
            }, doctor.FullName);

            return View(model);
        }
    }
}