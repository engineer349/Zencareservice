using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Data;
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

        public IActionResult SaveItemPrescription( [FromBody] Prescs[] medications)
        {


            foreach (var medication in medications)
            {
                //   // Save medication to the database using your data access logic
                //    // MedicationViewModel properties: prescription, dos/age, noofdays
                int slno = medication.SlNo;
                TempData["slno"] = slno;
                string prescription = medication.Prescription;
                int dosage = medication.Dosage;
                int noofdays = medication.NoOfDays;
                string aptcode = medication.aptcode;
               
                DataSet ds = new DataSet();
                DataAccess Obj_DataAccess = new DataAccess();
                ds = Obj_DataAccess.SaveItemPrescription(slno,prescription,dosage,noofdays,aptcode);

            }
            
           
            return RedirectToAction("Dashboard", "Report");
        }


        [HttpPost]
        public async Task<IActionResult>  SaveHeadPrescription(Prescs Obj)
        {
            // Here, you would process and save the medication data to the database
            // The MedicationViewModel corresponds to the structure of your Medication data.
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

        public IActionResult Presclist() 
        {
            return View();
        }
    }
}
