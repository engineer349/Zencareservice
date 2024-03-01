using Zencareservice.Controllers;

namespace Zencareservice.Models
{
    public class Report
    {

        //public List<Appts> Appointments { get; set; }
        //public List<Prescs> Prescriptions { get; set; }

        //// Constructor to initialize lists
        //public Report()
        //{
        //    Appointments = new List<Appts>();
        //    Prescriptions = new List<Prescs>();
        //}

        //// Method to add an appointment to the list
        //public void AddAppointment(Appts appointment)
        //{
        //    Appointments.Add(appointment);
        //}

        //// Method to add a prescription to the list
        //public void AddPrescription(Prescs prescription)
        //{
        //    Prescriptions.Add(prescription);
        //}

        //// Method to get all appointments for a specific patient by their email
        //public List<Appts> GetAppointmentsForPatient(string patientEmail)
        //{
        //    return Appointments.Where(appt => appt.PatientEmail == patientEmail).ToList();
        //}

        //// Method to get all prescriptions for a specific patient by their email
        //public List<Prescs> GetPrescriptionsForPatient(string patientEmail)
        //{
        //    return Prescriptions.Where(presc => presc.PatientEmail == patientEmail).ToList();
        //}

        //// Method to get all appointments for a specific doctor by their email
        //public List<Appts> GetAppointmentsForDoctor(string doctorEmail)
        //{
        //    return Appointments.Where(appt => appt.DEmail == doctorEmail).ToList();
        //}

        //// Method to get all prescriptions for a specific doctor by their email
        //public List<Prescs> GetPrescriptionsForDoctor(string doctorEmail)
        //{
        //    return Prescriptions.Where(presc => presc.DoctorEmail == doctorEmail).ToList();
          //}
        public List<Appts> showlist { get; set; } = new List<Appts>();


        public List<Prescs> showlist1 { get; set; } = new List<Prescs>();

    }
}