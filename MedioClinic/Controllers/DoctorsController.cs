using System;
using System.Web.Mvc;
using System.Web.UI;
using Business.DI;
using Business.Dto.Doctors;
using Business.Repository.Doctor;
using Business.Services.Cache;
using CMS.DocumentEngine.Types.Training;
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
                () => DoctorSectionRepository.GetDoctorSection(), // function to get data
                60, // how many minutes data should be cached
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

            // Sets cache dependency on single page based on NodeGuid
            // This example makes the system clear the cache when given doctor is deleted or edited in Kentico
            Dependencies.CacheService.SetOutputCacheDependency(nodeGuid);

            var model = GetPageViewModel(new DoctorDetailViewModel()
            {
                Doctor = doctor
            }, doctor.FullName);

            return View(model);
        }
    }
}