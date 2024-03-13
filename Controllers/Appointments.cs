using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Zencareservice.Models;
using Zencareservice.Repository;
using System.Text;
using Newtonsoft.Json;

namespace Zencareservice.Controllers
{
    public class Appointments : Controller
    {
        
        public void AptsDropdown()
        {
            string UsrId = Request.Cookies["UsrId"];
            TempData["UserId"] = UsrId;
            DataAccess Obj_DataAccess = new DataAccess();
            DataSet ds = new DataSet();
            ds = Obj_DataAccess.GetAppointments(UsrId);
            if (ds.Tables[2].Rows.Count < 1)
            {
                ViewBag.Message = "There are no doctors available to book your Appointment";

                //return RedirectToAction("PAptlist", "Appointments");
            }

            else
            {
                var dataRows = (ds.Tables.Count > 1) ? ds.Tables[2].AsEnumerable() : Enumerable.Empty<DataRow>();

               
                ViewBag.YourDataList = new SelectList(dataRows, "Gender", "Gender");


                var GenderList = new List<SelectListItem>();
                foreach (DataRow row in ds.Tables[5].Rows)
                {  
                    var gendername = new SelectListItem
                    {
                        Text = row["Gender"].ToString(),
                        Value = row["Gender"].ToString()
                    };

                GenderList.Add(gendername);
                ViewBag.Gendername = GenderList;
                ViewBag.SelectedValue = "Gender";

                // Add more options as needed
                };

                ViewBag.YourDataList = new SelectList(dataRows, "DId", "DFname");

                var DList = new List<SelectListItem>();

                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    var Doctorname = new SelectListItem
                    {
                        Text = row["DFname"].ToString(),
                        Value = row["DId"].ToString()
                    };

                    DList.Add(Doctorname);
                    ViewBag.DFname = DList;
                    ViewBag.SelectedValue = "Doctor FirstName";
                }


                foreach (DataRow row in ds.Tables[2].Rows)


               
                ViewBag.DataSet = ds.Tables[2];
                ViewBag.SelectedValue = "Tamil Nadu";
            }

        }


        public IActionResult Aptlist(Appts apt)
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
                ds = Obj_DataAccess.GetAppointmentList(UsrId, Role);

                if (Role == "Patient")
                {
                    List<Appts> AptList = new List<Appts>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        Appts apts = new Appts();
                        {
                            apts.RCode = row["RCode"].ToString();

                            apts.DoctorFirstName = row["DFname"].ToString();
                            //apts.DoctorLastName = row["DLname"].ToString();


                            apts.AptBookingDate = Convert.ToDateTime(row["AptBookingdate"]);
                            apts.ReasonType = row["Reasontype"].ToString();

                            object aptbooktime = row["AptTime"];
                            if (aptbooktime != DBNull.Value)
                            {
                                // Conversion is safe since the value is not DBNull
                                apts.AptBookingTime = (TimeSpan?)aptbooktime;
                            }

                            else
                            {
                                apts.AptBookingTime = DateTime.Now.TimeOfDay;
                            }


                            apts.DEmail = row["DEmail"].ToString();

                            apts.DContact = row["DContactno"].ToString();

                            apts.Aptbookconfirm = Convert.ToInt32(row["Aptbookingconfirm"]);
                            apts.Aptbookstatus = Convert.ToInt32(row["Aptbookingstatusconfirm"]);

                            if (row["Aptrescheduleconfirm"] != DBNull.Value)
                            {
                                apts.Aptreschedule = Convert.ToInt32(row["Aptrescheduleconfirm"]);
                            }
                            else
                            {
                                // Handle the case where the value is DBNull, e.g., assign a default value or take appropriate action.
                                // For example:
                                 apts.Aptreschedule = 0;
                            }
                            
                            if (apts.Aptbookconfirm == 1 && apts.Aptbookstatus == 1)
                            {
                                if (apts.Aptreschedule == 1 && apts.Aptbookstatus ==1)
                                {
                                    apts.Aptbookingstatusconfirm = "Rescheduled";

                                }

                                else
                                {
                                    apts.Aptbookingstatusconfirm = "BookingConfirmed";
                                }

                            }
                            else
                            {
                                apts.Aptbookingstatusconfirm = "Pending";
                            }


                        };
                        AptList.Add(apts);
                    }
                    apt.showlist = AptList;

                }
                else
                {
                    List<Appts> AptList = new List<Appts>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        Appts apts = new Appts();
                        {
                            apts.RCode = row["RCode"].ToString();
                            apts.DoctorFirstName = row["DFname"].ToString();
                            //apts.DoctorLastName = row["DLname"].ToString();
                       
                            apts.AptBookingDate = Convert.ToDateTime(row["AptBookingdate"]);


                            apts.ReasonType = row["Reasontype"].ToString();

                            object aptbooktime = row["AptTime"];
                            if (aptbooktime != DBNull.Value)
                            {
                                // Conversion is safe since the value is not DBNull
                                apts.AptBookingTime = (TimeSpan?)aptbooktime;
                            }

                            else
                            {
                                apts.AptBookingTime = DateTime.Now.TimeOfDay;
                            }

                            apts.DEmail = row["DEmail"].ToString();

                            apts.DContact = row["DContactno"].ToString();

                            apts.Aptbookconfirm = Convert.ToInt32(row["Aptbookingconfirm"]);
                            
                            apts.Aptreschedule = Convert.ToInt32(row["Aptrescheduleconfirm"]);

                            apts.Aptbookstatus = Convert.ToInt32(row["Aptbookingstatusconfirm"]);



                            if (apts.Aptbookconfirm == 1 && apts.Aptbookstatus == 1)
                            {
                                if(apts.Aptreschedule == 1 && apts.Aptbookstatus ==1)
                                {
                                    apts.Aptbookingstatusconfirm = "Rescheduled";

                                }

                                else
                                {
                                    apts.Aptbookingstatusconfirm = "BookingConfirmed";
                                }
                               

                            }
                            else
                            {
                                apts.Aptbookingstatusconfirm = "Pending";
                            }


                        };
                        AptList.Add(apts);
                    }
                    apt.showlist = AptList;
                }
            }
            

            return View(apt);


        }

        public IActionResult PAptlist(Appts apt)
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
                ds = Obj_DataAccess.GetAppointmentList(UsrId ,Role);



                List<Appts> AptList = new List<Appts>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    Appts apts = new Appts();
                    {
                        apts.RCode = row["RCode"].ToString();

                        
                        StringBuilder builder1 = new StringBuilder();

                        apts.PatientFirstName = row["PFname"].ToString();
                        apts.PatientLastName = row["PLname"].ToString();

                        string pfname = apts.PatientFirstName;
                        string plname = apts.PatientLastName;

                        builder1.Append(pfname);
                        builder1.Append(plname);

                        string patientname = builder1.ToString();
                        
                        apts.PatientFirstName = patientname;

                        apts.AptId = ((int)row["AptId"]);

                        apts.Patientphoneno = row["Patphone"].ToString();
                        apts.PatientEmail = row["PatientEmail"].ToString();

                        apts.AptBookingDate = Convert.ToDateTime(row["AptBookingdate"]);


                        object aptRescheduleTime = row["Aptrescheduletime"];

                        if (aptRescheduleTime != DBNull.Value)
                        {
                            // Conversion is safe since the value is not DBNull
                            apts.RescheduleAppointmentTime = (TimeSpan?)aptRescheduleTime;
                        }
                        else
                        {
                            // Handle the case when the value is DBNull (e.g., set a default value)
                            apts.RescheduleAppointmentTime = DateTime.Now.TimeOfDay;
                            // Or set to a default Nullable<TimeSpan> value
                        }
                        apts.Aptbookconfirm = Convert.ToInt32(row["Aptbookingconfirm"]);
                        apts.Aptbookstatus = Convert.ToInt32(row["Aptbookingstatusconfirm"]);
                        apts.Aptreschedule = Convert.ToInt32(row["Aptrescheduleconfirm"]);


                        if (apts.Aptbookconfirm == 1 && apts.Aptbookstatus == 1)
                        {
                            if (apts.Aptreschedule == 1 && apts.Aptbookstatus == 1)
                            {
                                apts.Aptbookingstatusconfirm = "Rescheduled";

                            }

                            else
                            {
                                apts.Aptbookingstatusconfirm = "BookingConfirmed";
                            }


                        }
                        else
                        {
                            apts.Aptbookingstatusconfirm = "Pending";
                        }

                        object aptRescheduleDate = row["Aptrescheduledate"];

                        if (aptRescheduleDate != DBNull.Value)
                        {
                            apts.RescheduleAppointmentDate = Convert.ToDateTime(aptRescheduleDate);
                        }
                        else
                        {
                            apts.RescheduleAppointmentDate = DateTime.Now; 
                        }
                        apts.ReasonType = row["Reasontype"].ToString();
                        apts.AptBookingTime = ((TimeSpan?)row["AptTime"]);



                       

                    };
                    AptList.Add(apts);
                }
                apt.showlist = AptList;

            }


            return View(apt);

        }


    

        public IActionResult CreateAppointment()
        {          

            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            if (string.IsNullOrEmpty(UsrId))
            {
                return RedirectToAction("PatientLogin", "Account");
            }

            else
            {
                AptsDropdown();
             }
              return View();
        
        }

        public IActionResult Aptstatus()
        {
            return View();
        }

       
        public IActionResult GetAppointment(string type)
        {
            string UsrId = Request.Cookies["UsrId"];
            DataSet ds = new DataSet();

            if (type == "self")
            {

                DataAccess Obj_DataAccess = new DataAccess();

                ds = Obj_DataAccess.SetSelfAppointment(UsrId);

            }

            ViewBag.Details = null;
            string details = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(details);
        }

       


        [HttpGet]
        public IActionResult Apptcrt(Appts apt)
        {

            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            string RCode = Request.Cookies["RCode"];

            if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(RCode))
            {
                return RedirectToAction("PatientLogin", "Account");
            }


            else
            {


                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();
                ds = Obj_DataAccess.GetAppointments(UsrId);

                if (ds.Tables[2].Rows.Count < 1)
                {
                    ViewBag.Message = "There are no doctors available to book your Appointment";

                    return RedirectToAction("PAptlist", "Appointments");
                }

                else
                {
                    var dataRows = (ds.Tables.Count > 1) ? ds.Tables[2].AsEnumerable() : Enumerable.Empty<DataRow>();

                    ViewBag.YourDataList = new SelectList(dataRows, "DId", "DFname");


             

                    var DList = new List<SelectListItem>();

                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        var Doctorname = new SelectListItem
                        {
                            Text = row["DFname"].ToString(),
                            Value = row["DId"].ToString()
                        };

                        DList.Add(Doctorname);
                        ViewBag.DFname = DList;
                        ViewBag.SelectedValue = "Doctor FirstName";
                    }

                    var GenderList = new List<SelectListItem>();

                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        var gendername = new SelectListItem
                        {
                            Text = row["Gender"].ToString(),
                            Value = row["Gender"].ToString()
                        };

                        GenderList.Add(gendername);
                        ViewBag.Gender = GenderList;
                        ViewBag.SelectedValue = "Gender Name";
                    }


                    foreach (DataRow row in ds.Tables[1].Rows)


                        ViewBag.GenderList = GenderList;
                    ViewBag.DataSet = ds.Tables[1];
                    ViewBag.SelectedValue = "Tamil Nadu";

                    string fname = ds.Tables[1].Rows[0]["Fname"].ToString();
                    string lname = ds.Tables[1].Rows[0]["Lname"].ToString();
                    string phoneno = ds.Tables[1].Rows[0]["Phoneno"].ToString();
                    string email = ds.Tables[1].Rows[0]["Email"].ToString();
                    string gender = ds.Tables[1].Rows[0]["Gender"].ToString();
                    string address1 = ds.Tables[1].Rows[0]["Addressline1"].ToString();
                    string patage = ds.Tables[1].Rows[0]["Age"].ToString();


                    apt = new Appts();
                    {
                        apt.PatientFirstName = fname;
                        apt.PatientLastName = lname;
                        apt.Patientphoneno = phoneno;
                        apt.PatientEmail = email;
                        apt.PatientAddress1 = address1;
                        apt.PatientGender = gender;
                        apt.PatientAge = patage;

                    }


                }
            }
            return View(apt);
        }

        [HttpPost]
        public IActionResult Aptedit( Appts Obj)
        {
           
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            if (string.IsNullOrEmpty(UsrId))
            {
                return RedirectToAction("PatientLogin", "Account");
            }
            else
            {
                int aptId = Convert.ToInt32(Obj.AptId);
                int reschedule = 1;
                DateTime rescheduledate = (DateTime)Obj.RescheduleAppointmentDate;
                TimeSpan rescheduletime = (TimeSpan)Obj.RescheduleAppointmentTime;

                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();
                ds = Obj_DataAccess.SetRescheduleAppointment(Obj);

            }

            return View();
        }

        public IActionResult Aptedit(Appts apt,int Id)
        {
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;           

            if (string.IsNullOrEmpty(UsrId))
            {
                return RedirectToAction("PatientLogin", "Account");
            }


            else
            {
                apt.Rescedule = true;
               
                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();
                ds = Obj_DataAccess.GetEditAppointments(UsrId,Id);

                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {

                        int aptid = Convert.ToInt32(ds.Tables[1].Rows[0]["AptId"].ToString());
                        string pfname = ds.Tables[1].Rows[0]["PFname"].ToString();
                        string plname = ds.Tables[1].Rows[0]["PLname"].ToString();
                        string dfname = ds.Tables[1].Rows[0]["DFname"].ToString();                       
                        string reasontype = ds.Tables[1].Rows[0]["Reasontype"].ToString();
                        string patphone = ds.Tables[1].Rows[0]["Patphone"].ToString();
                        string patemail = ds.Tables[1].Rows[0]["PatientEmail"].ToString();
                        DateTime aptbookingdate = Convert.ToDateTime( ds.Tables[1].Rows[0]["AptBookingdate"]);
                        TimeSpan aptbookingtime =(TimeSpan)ds.Tables[1].Rows[0]["AptTime"];

                       
                        apt = new Appts();
                        {
                            apt.AptId = aptid;
                            apt.PatientFirstName = pfname;
                            apt.PatientLastName = plname;
                            apt.DoctorFirstName = dfname;                          
                            apt.ReasonType = reasontype;
                            apt.AptBookingDate = aptbookingdate;
                            apt.AptBookingTime = aptbookingtime;
                            apt.Patientphoneno = patphone;
                            apt.PatientEmail = patemail;
                            int rescheduleValue = apt.Reschedule ? 1 : 0;

                        }


                    }


                }
                return View(apt);
            }

        }

        [HttpPost]
        public IActionResult GetCities(int stateId)
        {
            DataAccess Obj_DataAccess = new DataAccess();
            DataSet ddd = new DataSet();
            ddd = Obj_DataAccess.SetCity(stateId);

            string cities = JsonConvert.SerializeObject(ddd.Tables[0]);
            return Json(cities);
        }

        [HttpPost]
        public IActionResult ConfirmAppointment([FromBody] Appts Obj)
        {

            string aptId = Convert.ToString(Obj.AptId);


            DataAccess Obj_DataAccess = new DataAccess();
            DataSet ds = new DataSet();
            ds = Obj_DataAccess.SetConfirmAppointment(Obj);



            return RedirectToAction("PAptlist", "Appointments");

        }


        [HttpPost]
        public IActionResult CreateAppointment(Aptcrt Obj)
        {
                AptsDropdown();

                string UsrId = Request.Cookies["UsrId"];              

                if (string.IsNullOrEmpty(UsrId) )
                {
                    return RedirectToAction("PatientLogin", "Account");
                }
                else
                {
                   try
                    {
                      int dfname = Obj.DoctorFirstName;

                    if (ModelState.IsValid)
                        {
                            
                            Obj.UsrId = Convert.ToInt32(UsrId);
                            string pfname = Obj.PatientFirstName;
                            string plname = Obj.PatientLastName;
                            string pemail = Obj.PatientEmail;
                           
                            //string dlname = Obj.DoctorLastName;
                            string gender = Obj.PatientGender;

                            Obj.RCode = Convert.ToString(TempData["RCode"]);
                            DateTime aptbookdate = Obj.AptBookingDate;
                            TimeSpan aptbooktime = (TimeSpan)Obj.AptBookingTime;


                            DataAccess Obj_DataAccess = new DataAccess();
                            DataSet ds = new DataSet();
                            ds = Obj_DataAccess.CheckAppointmentList(Obj);

                            if (ds.Tables[0].Rows.Count == 0)
                            {


                                ds = Obj_DataAccess.SaveAppointment(Obj);

                                TempData["SwalMessage"] = "Your Appointment is saved.";
                                TempData["SwalType"] = "success";

                                return RedirectToAction("Aptlist", "Appointments");
                            }
                            else
                            {
                                TempData["SwalMessage"] = "Your Appointment is already booked.";
                                TempData["SwalType"] = "error";

                                return RedirectToAction("CreateAppointment", "Appointments");

                            }
                        }
                        else
                        {
                            return View(Obj);
                        }

                    }
                    catch (Exception ex)
                    {
						TempData["SwalMessage"] = "An error occurred while processing your request.";
						TempData["SwalType"] = "error";

						return RedirectToAction("Error", "Home");
					}

                  


				}


            }
           
            
    }

    }
