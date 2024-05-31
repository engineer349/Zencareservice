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
    public class ReportController : Controller
    {
    
        public IActionResult Report()
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

        public async Task<IActionResult> Prescdetails( string type, Prescs presc)
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

            string UsrId = Request.Cookies["UsrId"];

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

        public IActionResult Dashboard(Dashboard Obj)
        {
            string UserId = Request.Cookies["UsrId"];
            string UsrName = Request.Cookies["UsrName"];
            string RoleName = Request.Cookies["Role"];

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(UsrName) || string.IsNullOrEmpty(RoleName))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Obj.UsrId = Convert.ToInt32(UserId);
                Obj.Role = RoleName;

                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();

                try
                {
                    ds = Obj_DataAccess.GetDashboardvalues(Obj);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        ViewBag.Appointments = row["AptCount"].ToString();
                        ViewBag.Users = row["RegisterCount"].ToString();
                        ViewBag.AppointmentStatus = row["AptstatusCount"].ToString();
                        ViewBag.Prescriptions = row["PrescriptionCount"].ToString();
                    }
                    else
                    {
                        // Handle case when there are no rows returned from the database
                        // You can set default values or display an error message
                        ViewBag.Appointments = "0";
                        ViewBag.Users = "0";
                        ViewBag.AppointmentStatus = "0";
                        ViewBag.Prescriptions = "0";
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately, such as logging it
                    ViewBag.ErrorMessage = "An error occurred while retrieving dashboard data.";
                    // Optionally, you can also throw the exception to be handled at a higher level
                    // throw ex;
                }
            }
            return View();
        }


    }
}
