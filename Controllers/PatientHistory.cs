using Microsoft.AspNetCore.Mvc;

namespace Zencareservice.Controllers
{
	public class PatientHistory : Controller
	{
		public string? Patfirstname { get; set; }
		public string? Patlastname { get; set; }
		public string? Patgender { get; set; }
		public int? Patage { get; set; }
		public string? PatPhoneno { get; set; }
		public string? Pataltphone { get; set; }
		public string? Patemail { get; set; }
		public string? Pataltemail { get; set; }
		public string? Pataddress1 { get; set; }
		public string? Pataddress2 { get; set; }
		public string? PatState { get; set; }
		public string? PatCity { get; set; }
		//step02
		//PresentingComplaint
		public string? PresentingComplaint { get; set; }
		//step03
		//History of Presenting Complaint
		public string? HPC { get; set; }
		//step04
		//Past Medical History
		public string? PMH { get; set; }
		//step05
		//Drug History
		public string? DrugHistory { get; set; }
		//step06
		//Social History
		public string? SocialHistory { get; set; }
		//step07
		//Reviewof Symptoms
		//https://www.medistudents.com/osce-skills/patient-history-taking
		public string? ROS { get; set; }

	}
}
