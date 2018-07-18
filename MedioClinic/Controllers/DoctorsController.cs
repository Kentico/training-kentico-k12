using System.Web.Mvc;
using Kentico.DI;
using Kentico.Repository.Doctors;
using MedioClinic.Models.Doctors;

namespace MedioClinic.Controllers
{
    public class DoctorsController : BaseController
    {
        private IDoctorsRepository DoctorsRepository { get; }

        public DoctorsController(
            IBusinessDependencies dependencies,
            IDoctorsRepository doctorsRepository
            ) : base(dependencies)
        {
            DoctorsRepository = doctorsRepository;
        }

        public ActionResult Index()
        {
            var model = GetPageViewModel(new DoctorsViewModel()
            {
                Doctors = DoctorsRepository.GetDoctors()
            },"Doctors");

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