using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.Doctor;
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

        [Route("Detail/{nodeId}/{nodeAlias}")]
        public ActionResult Detail(int nodeId, string nodeAlias)
        {
            var doctor = DoctorRepository.GetDoctor(nodeId);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(new DoctorDetailViewModel()
            {
                Doctor = doctor
            }, doctor.FullName);

            return View(model);
        }
    }
}