
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Web;
using Zencareservice.Models;
using Zencareservice.Repository;

namespace Zencareservice.Controllers
{
	[Authorize]
	public class Prescriptions : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "Doctor")]
		public IActionResult Prescrt()
		{
			string UsrId = Request.Cookies["UsrId"] ?? string.Empty;

			TempData["UserId"] = UsrId;

			if (string.IsNullOrEmpty(UsrId))
			{
				return RedirectToAction("PatientLogin", "Account");
			}

			else
			{
				var hasAppointmentCodes = Prescdropdown();

				if (!hasAppointmentCodes)
				{
					ViewBag.Message = "Invalid";
					TempData["SwalMessage"] = "No New Appointments";
					TempData["SwalType"] = "warning";
					return RedirectToAction("NoAppointments", "Home");
				}

			}
			return View();
		}

		[Authorize(Roles = "Doctor")]
		public IActionResult Prscedit(Prescs psc, string Id)
		{
			string UsrId = Request.Cookies["UsrId"] ?? string.Empty;

			string decodedId = HttpUtility.UrlDecode(Id);

			TempData["UserId"] = UsrId;

			if (string.IsNullOrEmpty(UsrId))
			{
				return RedirectToAction("PatientLogin", "Account");
			}

			DataAccess Obj_DataAccess = new DataAccess();
			DataSet ds = Obj_DataAccess.GetEditPrescriptions(UsrId, decodedId);

			if (ds != null && ds.Tables.Count > 0)
			{
				DataRow headerRow = ds.Tables[1].Rows[0];
				psc.PatientFirstName = headerRow["Pfname"].ToString();
				psc.PatientLastName = headerRow["Plname"].ToString();
				psc.Patientphoneno = headerRow["Pphoneno"].ToString();
				psc.PatientEmail = headerRow["Pemail"].ToString();
				psc.PatientAge = headerRow["Page"].ToString();
				psc.PatBloodgroup = headerRow["PBloodgroup"].ToString();
				psc.PatWeight = headerRow["Pweight"].ToString();
				psc.DoctorFirstName = headerRow["dfname"].ToString();
				psc.AptCode = headerRow["Aptcode"].ToString();
				psc.PrsCode = headerRow["PrsCode"].ToString();
				psc.Status = headerRow["Status"].ToString();
				// Populate medication items
				psc.showlist1 = new List<Prescs>();
				foreach (DataRow row in ds.Tables[2].Rows)
				{
					var item = new Prescs
					{
						SlNo = Convert.ToInt32(row["Slno"]),
						Prescription = row["PrscItem"].ToString() ?? string.Empty,
						Dosage = Convert.ToInt32(row["PrscDosage"]),
						NoOfDays = Convert.ToInt32(row["Prscdays"])
					};
					psc.showlist1.Add(item);
				}

				return View(psc);
			}
			else
			{
				return View();
			}
		}

		//[HttpPost]
		//public IActionResult Prescedit([FromBody] Prescs[] medications)
		//{
		//    string UsrId = Request.Cookies["UsrId"];
		//    TempData["UserId"] = UsrId;

		//    if (string.IsNullOrEmpty(UsrId))
		//    {
		//        return RedirectToAction("PatientLogin", "Account");
		//    }
		//    else
		//    {
		//        try
		//        {
		//            foreach (var medication in medications)
		//            {
		//                int slno = medication.SlNo;
		//                string prescription = medication.Prescription;
		//                int dosage = medication.Dosage;
		//                int noofdays = medication.NoOfDays;
		//                string aptcode = medication.AptCode;
		//                string prscode = medication.PrsCode;

		//                // Save medication
		//                DataAccess Obj_DataAccess = new DataAccess();
		//                DataSet ds = Obj_DataAccess.SaveUpdatePrescription(slno, prescription, dosage, noofdays, aptcode, prscode);
		//            }

		//            return Json(new { success = true });
		//        }
		//        catch (Exception ex)
		//        {
		//            return Json(new { success = false, message = ex.Message });
		//        }
		//    }
		//}

		[Authorize(Roles = "Doctor")]
		[HttpPost]
		public IActionResult Prescedit([FromBody] Prescs[] medications)
		{
			string UsrId = Request.Cookies["UsrId"] ?? string.Empty;
			TempData["UserId"] = UsrId;

			if (string.IsNullOrEmpty(UsrId))
			{
				return RedirectToAction("PatientLogin", "Account");
			}
			else
			{
				if (medications == null || medications.Length == 0)
				{
					return Json(new { success = false, message = "No medications provided" });
				}

				try
				{
					DataAccess Obj_DataAccess = new DataAccess();

					List<Prescs> medicationsList = medications.ToList();


					DataSet ds = Obj_DataAccess.SaveUpdateItemPrescription(medicationsList);

					TempData["SwalMessage"] = "Your Prescription is Updated.";
					TempData["SwalType"] = "success";

					return Json(new { success = true });


				}
				catch (Exception ex)
				{
					// Log the exception here
					return Json(new { success = false, message = ex.Message });
				}
			}
		}

		public bool Prescdropdown()
		{
			string UsrId = Request.Cookies["UsrId"] ?? string.Empty;
			TempData["UserId"] = UsrId;

			DataAccess Obj_DataAccess = new DataAccess();
			DataSet ds = new DataSet();

			try
			{
				ds = Obj_DataAccess.GetPrescriptions(UsrId);

				// Check if AptCode data exists and is not empty
				if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count == 0)
				{
					return false;
				}

				var dataRows = ds.Tables[2].AsEnumerable();

				ViewBag.YourDataList = new SelectList(dataRows, "AptCode", "AptCode");

				var AptcodeList = new List<SelectListItem>();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					var presccategory = new SelectListItem
					{
						Text = row["AptCode"].ToString(),
						Value = row["AptCode"].ToString()
					};

					AptcodeList.Add(presccategory);
				}

				ViewBag.AptCode = AptcodeList;
				ViewBag.SelectedValue = "Doctor FirstName";

				var BloodgroupList = new List<SelectListItem>();
				foreach (DataRow row in ds.Tables[2].Rows)
				{
					var bloodgroupcategory = new SelectListItem
					{
						Text = row["Bloodgroup"].ToString(),
						Value = row["Bloodgroup"].ToString()
					};

					BloodgroupList.Add(bloodgroupcategory);
				}

				ViewBag.Bloodgroup = BloodgroupList;
				ViewBag.SelectedValue = "Bloodgroup";
				ViewBag.DataSet = ds.Tables[2];
				ViewBag.SelectedValue = "Tamil Nadu";
			}
			catch (Exception ex)
			{
				// Log the exception (ex) if necessary
				return false;
			}

			return true;
		}
		public IActionResult GetPrescription(string type)
		{


			DataSet ds = new DataSet();

			DataAccess Obj_DataAccess = new DataAccess();

			ds = Obj_DataAccess.SetPrescription(type);

			ViewBag.PrescriptionDetails = null;
			string prescdetails = JsonConvert.SerializeObject(ds.Tables[0]);
			return Json(prescdetails);
		}
		[Authorize(Roles = "Doctor")]
		[HttpPost]
		public IActionResult SaveItemPrescription([FromBody] Prescs[] medications)
		{
			foreach (var medication in medications)
			{

				int slno = medication.SlNo;
				TempData["slno"] = slno;
				string prescription = medication.Prescription;
				int dosage = medication.Dosage;
				int noofdays = medication.NoOfDays;
				string? aptcode = medication.AptCode;

				DataAccess Obj_DataAccess = new DataAccess();
				DataSet ds = Obj_DataAccess.SaveItemPrescription(slno, prescription, dosage, noofdays, aptcode);
			}


			return RedirectToAction("Dashboard", "Report");
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost]
		public IActionResult SaveHeadPrescription(Prescs Obj)
		{
			//if (TempData["aptcode"] != null && TempData["aptcode"].ToString() == Obj.AppointmentCode)
			//{

			//    return RedirectToAction("Index", "Home"); 
			//}

			// Store the appointment code in TempData to check for duplicates in subsequent requests
			TempData["aptcode"] = Obj.AppointmentCode;

			string aptcode = Obj.AppointmentCode;

			TempData["aptcode"] = aptcode;

			//string pfname = Obj.PatientFirstName;
			//string plname = Obj.PatientLastName;
			//string patphoneno = Obj.Patientphoneno;
			//string patemail = Obj.PatientEmail;
			//string patage = Obj.PatientAge;
			//string patbloodgroup = Obj.PatBloodgroup;
			//string patweight = Obj.PatWeight;
			//string dfname = Obj.DoctorFirstName;



			DataSet ds = new DataSet();
			DataAccess Obj_DataAccess = new DataAccess();
			ds = Obj_DataAccess.SaveHeadPrescription(Obj);


			return RedirectToAction("Dashboard", "Report");
		}


		[Authorize(Roles = "Doctor")]
		[HttpPost]
		public IActionResult UpdateHeadPrescription(Prescs Obj)
		{
			//if (TempData["aptcode"] != null && TempData["aptcode"].ToString() == Obj.AppointmentCode)
			//{

			//    return RedirectToAction("Index", "Home"); 
			//}

			// Store the appointment code in TempData to check for duplicates in subsequent requests
			TempData["aptcode"] = Obj.AptCode;

			string? aptcode = Obj.AptCode;

			TempData["aptcode"] = aptcode;

			string pfname = Obj.PatientFirstName ?? string.Empty;
			string plname = Obj.PatientLastName ?? string.Empty;
			string patphoneno = Obj.Patientphoneno ?? string.Empty;
			string patemail = Obj.PatientEmail ?? string.Empty;
			string patage = Obj.PatientAge ?? string.Empty;
			string patbloodgroup = Obj.PatBloodgroup ?? string.Empty;
			string patweight = Obj.PatWeight ?? string.Empty;
			string dfname = Obj.DoctorFirstName ?? string.Empty;
			string Appointmentcode = Obj.AptCode ?? string.Empty;
			string PrsCode = Obj.PrsCode ?? string.Empty;


			DataSet ds = new DataSet();
			DataAccess Obj_DataAccess = new DataAccess();
			ds = Obj_DataAccess.UpdateHeadPrescription(Obj);


			return RedirectToAction("Dashboard", "Report");
		}

		[Authorize(Roles = "Doctor")]
		public IActionResult Prescedit()
		{
			return View();
		}
		[Authorize(Roles = "Patient,Doctor")]
		public IActionResult Presclist(Prescs prescs)
		{
			string UsrId = Request.Cookies["UsrId"] ?? string.Empty;

			TempData["UserId"] = UsrId;

			string Role = Request.Cookies["Role"] ?? string.Empty;

			if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(Role))
			{
				return RedirectToAction("PatientLogin", "Account");
			}
			else
			{
				DataAccess Obj_DataAccess = new DataAccess();
				DataSet ds = new DataSet();
				ds = Obj_DataAccess.GetPrescriptionList(UsrId, Role);


				List<Prescs> PrescsList = new List<Prescs>();

				foreach (DataRow row in ds.Tables[0].Rows)
				{
					Prescs presc = new Prescs();
					{
						presc.PatientFirstName = row["Pfname"].ToString();

						presc.Patientphoneno = row["Pphoneno"].ToString();

						presc.AppointmentCode = row["AptCode"].ToString();

						presc.PrsCode = row["PrsCode"].ToString();

						presc.Prscdate = Convert.ToDateTime(row["Prscreatedate"]);

						presc.Status = row["Status"].ToString();

					};

					PrescsList.Add(presc);

					prescs.showlist1 = PrescsList;
				}
			}

			return View(prescs);
		}
		[Authorize(Roles = "Patient")]
		public IActionResult DPresclist(Prescs prescs)
		{
			string UsrId = Request.Cookies["UsrId"] ?? string.Empty;

			TempData["UserId"] = UsrId;

			string Role = Request.Cookies["Role"] ?? string.Empty;

			if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(Role))
			{
				return RedirectToAction("PatientLogin", "Account");
			}
			else
			{
				DataAccess Obj_DataAccess = new DataAccess();
				DataSet ds = new DataSet();
				ds = Obj_DataAccess.GetPrescriptionList(UsrId, Role);


				List<Prescs> PrescsList = new List<Prescs>();

				foreach (DataRow row in ds.Tables[1].Rows)
				{
					Prescs presc = new Prescs();
					{
						presc.DoctorFirstName = row["Dfname"].ToString();

						presc.Doctorcontact = row["Dphoneno"].ToString();

						presc.AppointmentCode = row["Aptcode"].ToString();

						presc.PrsCode = row["PrsCode"].ToString();

						presc.Prscdate = Convert.ToDateTime(row["Prscreatedate"]);

						presc.Status = row["Status"].ToString();

					};

					PrescsList.Add(presc);

					prescs.showlist1 = PrescsList;
				}
			}

			return View(prescs);
		}
	}
}