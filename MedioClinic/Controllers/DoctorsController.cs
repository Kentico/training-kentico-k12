using System;
using System.Web.Mvc;
using System.Web.UI;
using Business.DI;
using Business.Repository.Doctor;
using CMS.Helpers;
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
            var doctorSection = DoctorSectionRepository.GetDoctorSection();

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
            Dependencies.CacheDependencyService.GetAndSetPageDependency(nodeGuid);

            var model = GetPageViewModel(new DoctorDetailViewModel()
            {
                Doctor = doctor
            }, doctor.FullName);

            return View(model);
        }
    }
}