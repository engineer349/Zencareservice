using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using Zencareservice.Models;
using Zencareservice.Repository;

namespace Zencareservice.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {

        public IActionResult Report()
        {
            string ?UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            string Role = Request.Cookies["Role"];

            if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(Role))
            {
                return RedirectToAction("PatientLogin", "Account");
            }



            else
            {
                Reportdropdown();

            }
            return View();
        }


        public IActionResult Getprscdetails(Prescs prsc, string type)
        {
            DataAccess Obj_DataAccess = new DataAccess();
            DataSet ds = new DataSet();
            ds = Obj_DataAccess.SetDetails(type);
            List<Prescs> PrescList = new List<Prescs>();

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                Prescs prescs = new Prescs();
                {
                    prescs.SlNo = Convert.ToInt32(row["Slno"]);
                    prescs.Prescription = row["Prscitem"].ToString();
                    prescs.Dosage = Convert.ToInt32( row["PrscDosage"]);
                    prsc.NoOfDays = Convert.ToInt32(row["Prscdays"]);

                };
                PrescList.Add(prescs);
            }
             prsc.showlist1= PrescList;
            return View(prsc);

        }

        public IActionResult Getdetails(string type, Prescs prsc)
        {
            DataSet ds = new DataSet();
            DataAccess Obj_DataAccess = new DataAccess();
            ds = Obj_DataAccess.SetDetails(type);         

            string prescdetails = JsonConvert.SerializeObject(ds.Tables[0]);

            return Json(prescdetails);
        }

        public IActionResult Prescdetails( string type, Prescs presc)
        {
            DataSet ds = new DataSet();

            DataAccess Obj_DataAccess = new DataAccess();

            ds = Obj_DataAccess.SetDetails(type);

            List<Prescs> PrescList = new List<Prescs>();


            foreach (DataRow row in ds.Tables[1].Rows)
            {
                Prescs prsc = new Prescs();
                {

                    prsc.Prescription = row["Prscitem"].ToString();

                    prsc.SlNo = Convert.ToInt32(row["Slno"]);

                    prsc.Dosage = Convert.ToInt32(row["PrscDosage"]);

                    prsc.NoOfDays = Convert.ToInt32(row["Prscdays"]);
                };


                PrescList.Add(presc);

                presc.showlist1 = PrescList;

            }
            return View(presc);
        }

        public void Reportdropdown()
        {

            string? UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            DataAccess Obj_DataAccess = new DataAccess();

            DataSet ds = new DataSet();

            ds = Obj_DataAccess.GetPrescriptions(UsrId);


            var dataRows = (ds.Tables.Count > 1) ? ds.Tables[2].AsEnumerable() : Enumerable.Empty<DataRow>();

            ViewBag.YourDataList = new SelectList(dataRows, "AptCode", "AptCode");


            var AptcodeList = new List<SelectListItem>();

            foreach (DataRow row in ds.Tables[3].Rows)
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

        [Authorize(Roles = "Admin, Patient, Doctor")]
        public IActionResult Dashboard(Dashboard Obj)
        {
            string ?userId = Request.Cookies["UsrId"];
            string ?usrName = Request.Cookies["UsrName"];
            string? roleName = Request.Cookies["Role"];

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(usrName) || string.IsNullOrEmpty(roleName))
            {
                return RedirectToAction("Login", "Account");
            }

            Obj.UsrId = Convert.ToInt32(userId);
            Obj.Role = roleName;

            DataAccess objDataAccess = new DataAccess();
            DataSet ds = new DataSet();

            try
            {
                ds = objDataAccess.GetDashboardvalues(Obj);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    ViewBag.Appointments = row["AptCount"].ToString();
                    ViewBag.Users = row["RegisterCount"].ToString();
                    ViewBag.AppointmentStatus = row["AptstatusCount"].ToString();
                    ViewBag.Prescriptions = row["PrescriptionCount"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                // Log the exception if necessary
                throw ex;
            }

            return View();
        }


    }
}
