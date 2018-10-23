using System;

namespace Business.Dto.Doctors
{
    public class DoctorDto
    {
        public int NodeId { get; set; }
        public Guid NodeGuid { get; set; }
        public string NodeAlias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public string ImagePath { get; set; }
        public string Specialty { get; set; }
        public string Bio { get; set; }
        public int EmergencyShift { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool HasShiftToday => EmergencyShift == (int)DateTime.Now.DayOfWeek;
    }
}
