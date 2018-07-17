using UI.Models.Clinic;

namespace Kentico.Repository.Clinic
{
    public interface IClinicRepository : IRepository
    {
        ClinicDto GetClinic();
    }
}
