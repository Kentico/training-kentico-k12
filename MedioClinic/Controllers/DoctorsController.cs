using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.Doctors;
using MedioClinic.Models.Doctors;

namespace MedioClinic.Controllers
{
    public class DoctorsController : BaseController
    {
        private IDoctorsRepository DoctorsRepository { get; }
        private IDoctorSectionRepository DoctorSectionRepository { get; }

        public DoctorsController(
            IBusinessDependencies dependencies,
            IDoctorsRepository doctorsRepository,
            IDoctorSectionRepository doctorSectionRepository
            ) : base(dependencies)
        {
            DoctorsRepository = doctorsRepository;
            DoctorSectionRepository = doctorSectionRepository;
        }

        public ActionResult Index()
        {
            var doctorsSection = DoctorSectionRepository.GetDoctorSection();

            if (doctorsSection == null)
            {
                return HttpNotFound();
            }

            var model = GetPageViewModel(new DoctorsViewModel()
            {
                Doctors = DoctorsRepository.GetDoctors()
            }, doctorsSection.Header);

            return View(model);
        }

        [Route("Detail/{nodeId}/{nodeAlias}")]
        public ActionResult Detail(int nodeId, string nodeAlias)
        {
            var doctor = DoctorsRepository.GetDoctor(nodeId);

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