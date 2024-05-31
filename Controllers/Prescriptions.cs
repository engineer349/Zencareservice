using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Entity;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Web;
using Zencareservice.Models;
using Zencareservice.Repository;

namespace Zencareservice.Controllers
{
    public class Prescriptions : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Prescrt(Prescs prc)
        {
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            if (string.IsNullOrEmpty(UsrId))
            {
                return RedirectToAction("PatientLogin", "Account");
            }

            else
            {
                Prescdropdown();

            }
            return View();
        }

        public async Task<IActionResult> Prscedit(Prescs psc, string Id)
        {
            string UsrId = Request.Cookies["UsrId"];

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
                // Populate prescription details
                DataRow headerRow = ds.Tables[1].Rows[0];
                psc.PatientFirstName = headerRow["Pfname"].ToString();
                psc.PatientLastName = headerRow["Plname"].ToString();
                psc.Patientphoneno = headerRow["Pphoneno"].ToString();
                psc.PatientEmail = headerRow["Pemail"].ToString();
                psc.PatientAge = headerRow["Page"].ToString();
                psc.PatBloodgroup = headerRow["PBloodgroup"].ToString();
                psc.PatWeight = headerRow["Pweight"].ToString();
                psc.DoctorFirstName = headerRow["dfname"].ToString();
                psc.AppointmentCode = headerRow["Aptcode"].ToString();

                // Populate medication items
                psc.showlist1 = new List<Prescs>();
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    var item = new Prescs
                    {
                        SlNo = Convert.ToInt32(row["Slno"]),
                        Prescription = row["PrscItem"].ToString(),
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
        [HttpPost]
        public IActionResult Prescedit(Prescs[] medications, string id)
        {

            string UsrId = Request.Cookies["UsrId"];
      
             string decodedId = HttpUtility.UrlDecode(id);

            //string decodedId = Request.Query["id"].ToString();

            TempData["UserId"] = UsrId;

            if (string.IsNullOrEmpty(UsrId))
            {
                return RedirectToAction("PatientLogin", "Account");
            }
            else
            {
                try
                {

                    foreach (var medication in medications)
                    {

                        int slno = medication.SlNo;
                        TempData["slno"] = slno;
                        string prescription = medication.Prescription;
                        int dosage = medication.Dosage;
                        int noofdays = medication.NoOfDays;
                        string aptcode = medication.aptcode;

                        DataAccess Obj_DataAccess = new DataAccess();
                        DataSet ds = Obj_DataAccess.SaveUpdatePrescription(slno, prescription, dosage, noofdays, aptcode, decodedId);
                    }

                    return RedirectToAction("Dashboard", "Report");
                }
                catch(Exception ex)
                {
                    return RedirectToAction(ex.Message);
                }
            }



        }
        public void Prescdropdown()
        {

            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            DataAccess Obj_DataAccess = new DataAccess();

            DataSet ds = new DataSet();

            ds = Obj_DataAccess.GetPrescriptions(UsrId);


            var dataRows = (ds.Tables.Count > 1) ? ds.Tables[2].AsEnumerable() : Enumerable.Empty<DataRow>();

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
                ViewBag.AptCode = AptcodeList;
                ViewBag.SelectedValue = "Doctor FirstName";


            }


            ViewBag.YourDataList = new SelectList(dataRows, "Bloodgroup", "Bloodgroup");

            var BloodgroupList = new List<SelectListItem>();


            foreach (DataRow row in ds.Tables[2].Rows)
            {
                var bloodgroupcategory = new SelectListItem
                {
                    Text = row["Bloodgroup"].ToString(),
                    Value = row["Bloodgroup"].ToString()
                };
                BloodgroupList.Add(bloodgroupcategory);
                ViewBag.Bloodgroup = BloodgroupList;
                ViewBag.SelectedValue = "Bloodgroup";
            }

            foreach (DataRow row in ds.Tables[2].Rows)

                ViewBag.DataSet = ds.Tables[2];
            ViewBag.SelectedValue = "Tamil Nadu";

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
                string aptcode = medication.aptcode;
 
                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = Obj_DataAccess.SaveItemPrescription(slno, prescription, dosage, noofdays, aptcode);
            }
       

            return RedirectToAction("Dashboard", "Report");
        }


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

            string pfname = Obj.PatientFirstName;
            string plname = Obj.PatientLastName;
            string patphoneno = Obj.Patientphoneno;
            string patemail = Obj.PatientEmail;
            string patage = Obj.PatientAge;
            string patbloodgroup = Obj.PatBloodgroup;
            string patweight = Obj.PatWeight;
            string dfname = Obj.DoctorFirstName;



            DataSet ds = new DataSet();
            DataAccess Obj_DataAccess = new DataAccess();
            ds = Obj_DataAccess.SaveHeadPrescription(Obj);


            return RedirectToAction("Dashboard", "Report");
        }
        public IActionResult Prescedit()
        {

            return View();
        }
        public IActionResult Presclist(Prescs prescs)
        {
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            string Role = Request.Cookies["Role"];

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

                            presc.prscode = row["PrsCode"].ToString();

                            presc.Prscdate = Convert.ToDateTime(row["Prscreatedate"]);

                        };

                        PrescsList.Add(presc);
                    
                    prescs.showlist1 = PrescsList;
                }
            }
            
			return View(prescs);
		}
    }
}