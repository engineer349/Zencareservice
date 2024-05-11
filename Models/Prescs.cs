namespace Zencareservice.Models
{
    public class Prescs
    {
        public List<Prescs> showlist1 { get; set; } = new List<Prescs>();

		public List<Prescs> showlist { get; set; } = new List<Prescs>();

		public string? AppointmentCode { get; set; }
        public string? DoctorFirstName { get; set; }

        public string ? PatBloodgroup { get; set; }

        public string ? PatWeight { get; set; }

        public string ? DoctorEmail { get; set; }

        public string? DoctorLastName { get; set; }

        public string? PatientFirstName { get; set; }

        public string? PatientLastName { get; set; }

        public string? PatientEmail { get; set; }

        public string? PatientAge { get; set; }

        public string? Patientphoneno { get; set; }

        public string? PatientGender { get; set; }

        public string? PatientDiagnosis { get; set; }

        public string? PatientWeight { get; set; }
        public string? PrescriptionDetail { get; set; }

        public int SlNo { get; set; }
        public string Prescription { get; set; }
        public int Dosage { get; set; }
        public int NoOfDays { get; set; }

        public string prscode { get; set; }
        public string aptcode { get; set; }
    
        public DateTime Prscdate { get; set; }
    }




}
