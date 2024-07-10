using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Zencareservice.Repository;
using Zencareservice.Models;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;

namespace Zencareservice.Controllers
{
    [Authorize]
    public class Profiles : Controller
    {

  
        public IActionResult Personaldetails(Personaldetails personal, int Id)
        {
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            string RCode = Request.Cookies["RCode"];
        

            if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(RCode))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();  
                
                ds = Obj_DataAccess.GetProfile(Id);

                var dataRows = (ds.Tables.Count > 1) ? ds.Tables[1].AsEnumerable() : Enumerable.Empty<DataRow>();
                ViewBag.YourDataList = new SelectList(dataRows, "Id", "State");
                ViewBag.YourDataList = new SelectList(dataRows, "specialistid", "specialistname");
                ViewBag.YourDataList = new SelectList(dataRows, "Gender", "Gender");


                var GenderList = new List<SelectListItem>();

                foreach (DataRow row in ds.Tables[3].Rows)
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
                var StateList = new List<SelectListItem>();


                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    var StateItem = new SelectListItem
                    {
                        Text = row["State"].ToString(),
                        Value = row["Id"].ToString()
                    };

                    StateList.Add(StateItem);
                }

                var doctorspecialist = new List<SelectListItem>();


                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    var specialistitem = new SelectListItem
                    {
                        Text = row["specialistname"].ToString(),
                        Value = row["specialistid"].ToString()
                    };

                    doctorspecialist.Add(specialistitem);
                }



                foreach (DataRow row in ds.Tables[1].Rows)
               
                ViewBag.specialistname = doctorspecialist;
                ViewBag.State = StateList;
                ViewBag.GenderList = GenderList;              
                ViewBag.DataSet = ds.Tables[1];
                ViewBag.SelectedValue = "Tamil Nadu";

                string fname = ds.Tables[0].Rows[0]["Fname"].ToString();
                string lname = ds.Tables[0].Rows[0]["Lname"].ToString();              
                string phoneno = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                string email = ds.Tables[0].Rows[0]["Email"].ToString();
                string gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                string address1 = ds.Tables[0].Rows[0]["Addressline1"].ToString();
                string address2 = ds.Tables[0].Rows[0]["Addressline2"].ToString();
                string altaddress = ds.Tables[0].Rows[0]["AltAddress"].ToString();
                string altphoneno = ds.Tables[0].Rows[0]["Aphoneno"].ToString();
                string uniqueid = ds.Tables[0].Rows[0]["UniqueId"].ToString();
                string zipcode = ds.Tables[0].Rows[0]["Zipcode"].ToString();
                string state = ds.Tables[0].Rows[0]["State"].ToString();
                int sid = Convert.ToInt32( ds.Tables[0].Rows[0]["SId"]);
                //int sid = 0;
                string city = ds.Tables[0].Rows[0]["City"].ToString();
                string country = ds.Tables[0].Rows[0]["Country"].ToString();
                string Role = ds.Tables[0].Rows[0]["Role"].ToString();
                


                TempData["Role"] = Role;

				if(!String.IsNullOrEmpty(city))
                {
                    ViewBag.Citymsg = city;
					DataAccess Obj_DataAccess2 = new DataAccess();
					DataSet ddd = new DataSet();
					ddd = Obj_DataAccess2.SetCity(sid);

					ViewBag.YourDataList = new SelectList(dataRows, "CId", "City");
					var CityList = new List<SelectListItem>();


					foreach (DataRow row in ddd.Tables[0].Rows)
					{
						var CityItem = new SelectListItem
						{
							Text = row["City"].ToString(),
							Value = row["CId"].ToString()
						};

						CityList.Add(CityItem);
					}
					ViewBag.City = CityList;
				}


				personal = new Personaldetails();
                {
                    personal.Firstname = fname;
                    personal.Lastname = lname;
                    personal.Phoneno = phoneno;
                    personal.Email = email;
                    personal.Address1 = address1;
                    personal.Gender = gender;
                    personal.Address2 = address2;
                    personal.AltPhoneno = altphoneno;
                    personal.Uniqueid = uniqueid;
                    personal.Zipcode = zipcode;
                    personal.AltAddress = altaddress;
                    personal.Country = country;
                    personal.State = state;
                    personal.City = city;
                    personal.Role = Role; 
                    personal.Country = country;
                   

                   

                }
            }

            return View(personal);
        }




        [HttpPost]
        public IActionResult GetCities(int stateId)
        {
            DataAccess Obj_DataAccess = new DataAccess();
            DataSet ddd=new DataSet();
            ddd= Obj_DataAccess.SetCity(stateId);
            ViewBag.Citymsg = null;
            string cities = JsonConvert.SerializeObject(ddd.Tables[0]);
            return Json(cities);
        }

        public IActionResult Profile(Personaldetails pers, int Id)
        {

            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            string RCode = Request.Cookies["RCode"];
        

            if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(RCode))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();  
                
                ds = Obj_DataAccess.GetProfile(Id);

                var dataRows = (ds.Tables.Count > 1) ? ds.Tables[1].AsEnumerable() : Enumerable.Empty<DataRow>();
                ViewBag.YourDataList = new SelectList(dataRows, "Id", "State");
                ViewBag.YourDataList = new SelectList(dataRows, "specialistid", "specialistname");
                ViewBag.YourDataList = new SelectList(dataRows, "Gender", "Gender");


                var GenderList = new List<SelectListItem>();

                foreach (DataRow row in ds.Tables[3].Rows)
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
                var StateList = new List<SelectListItem>();


                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    var StateItem = new SelectListItem
                    {
                        Text = row["State"].ToString(),
                        Value = row["Id"].ToString()
                    };

                    StateList.Add(StateItem);
                }

                var doctorspecialist = new List<SelectListItem>();


                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    var specialistitem = new SelectListItem
                    {
                        Text = row["specialistname"].ToString(),
                        Value = row["specialistid"].ToString()
                    };

                    doctorspecialist.Add(specialistitem);
                }



                foreach (DataRow row in ds.Tables[1].Rows)
               
                ViewBag.specialistname = doctorspecialist;
                ViewBag.State = StateList;
                ViewBag.GenderList = GenderList;              
                ViewBag.DataSet = ds.Tables[1];
                ViewBag.SelectedValue = "Tamil Nadu";

                string fname = ds.Tables[0].Rows[0]["Fname"].ToString();
                string lname = ds.Tables[0].Rows[0]["Lname"].ToString();              
                string phoneno = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                string email = ds.Tables[0].Rows[0]["Email"].ToString();
                string gender = ds.Tables[0].Rows[0]["Gender"].ToString();
                string address1 = ds.Tables[0].Rows[0]["Addressline1"].ToString();
                string address2 = ds.Tables[0].Rows[0]["Addressline2"].ToString();
                string altaddress = ds.Tables[0].Rows[0]["AltAddress"].ToString();
                string altphoneno = ds.Tables[0].Rows[0]["Aphoneno"].ToString();
                string uniqueid = ds.Tables[0].Rows[0]["UniqueId"].ToString();
                string zipcode = ds.Tables[0].Rows[0]["Zipcode"].ToString();
                string state = ds.Tables[0].Rows[0]["State"].ToString();
                int sid = Convert.ToInt32( ds.Tables[0].Rows[0]["SId"]);
                //int sid = 0;
                string city = ds.Tables[0].Rows[0]["City"].ToString();
                string country = ds.Tables[0].Rows[0]["Country"].ToString();
                string Role = ds.Tables[0].Rows[0]["Role"].ToString();
                


                TempData["Role"] = Role;

				if(!String.IsNullOrEmpty(city))
                {
                    ViewBag.Citymsg = city;
					DataAccess Obj_DataAccess2 = new DataAccess();
					DataSet ddd = new DataSet();
					ddd = Obj_DataAccess2.SetCity(sid);

					ViewBag.YourDataList = new SelectList(dataRows, "CId", "City");
					var CityList = new List<SelectListItem>();


					foreach (DataRow row in ddd.Tables[0].Rows)
					{
						var CityItem = new SelectListItem
						{
							Text = row["City"].ToString(),
							Value = row["CId"].ToString()
						};

						CityList.Add(CityItem);
					}
					ViewBag.City = CityList;
				}


				pers = new Personaldetails();
                {
                    pers.Firstname = fname;
                    pers.Lastname = lname;
                    pers.Phoneno = phoneno;
                    pers.Email = email;
                    pers.Address1 = address1;
                    pers.Gender = gender;
                    pers.Address2 = address2;
                    pers.AltPhoneno = altphoneno;
                    pers.Uniqueid = uniqueid;
                    pers.Zipcode = zipcode;
                    pers.AltAddress = altaddress;
                    pers.Country = country;
                    pers.State = state;
                    pers.City = city;
                    pers.Role = Role; 
                    pers.Country = country;
                   

                   

                }
            }

            return View(pers);
        }


       [HttpPost]
        public IActionResult UpdateProfile(Personaldetails Obj)
        {
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            //string RCode = Request.Cookies["RCode"];


            if (string.IsNullOrEmpty(UsrId))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {


                try
                {

                    // if (file != null && file.Length > 0)
                    // {
                    //    using (var memoryStream = new MemoryStream())
                    //     {
                    //       file.CopyTo(memoryStream);
                    //         Obj.ImageData = memoryStream.ToArray();

                    // }
                    // }
                    // if (File != null && File.Length > 0)
                    // { 
                    //    using (var memoryStream = new MemoryStream())
                    //    {
                    //         File.CopyTo(memoryStream);
                    //       Obj.ProfData = memoryStream.ToArray();

                    //     }
                    // }

                    Obj.UsrId = Convert.ToInt32(TempData["UserId"]);
                    Obj.Role = Convert.ToString(TempData["Role"]);

                    if (Obj.Role == "Patient")
                    {
                       string phistory = Obj.PatientHistory;
                       string dhistory = Obj.Diagnosisdetails;
                        string other = Obj.Otherdetails;
                    }
                    else if (Obj.Role == "Doctor")
                    {
                        int docexperience = Obj.DExp;
                       string docspecialist = Obj.specialistid;
                       string docqualification = Obj.DQualification;
                     

                    }
                    else
                    {
                        string empqualification = Obj.EmpQualification;
                       string emphistory = Obj.Emphistory;
                       int empexp = Obj.EmpExp;
                     }
                    Obj.UsrId = Convert.ToInt32(UsrId);
                    string Gender = Obj.Gender;
                    // byte[] photo = Obj.ImageData;
                    // byte[] profphoto = Obj.ProfData;
                    string Altcontact = Obj.AltPhoneno;
                    string Addressline1 = Obj.Address1;
                    string Addressline2 = Obj.Address2;
                    string Altaddress = Obj.AltAddress;
                    string State = Obj.State;
                    string City = Obj.City;
                    string Country = Obj.Country;
                    string Uniqueid = Obj.Uniqueid;
                    string zipcode = Obj.Zipcode;
                    string Email = Obj.Email;
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.UpdateProfile(Obj);

                }

                catch (Exception ex)
                {
                    throw ex;
                }

                if(Obj.Role== "Admin")
                {
                    return RedirectToAction("Userlist", "Profiles");
                }

                else
                {
                    return RedirectToAction("Dashboard", "Report");

                }

            }
        }


		[Authorize(Roles = "Admin")]
		public IActionResult Userlist(Signup user)
        {
            string UsrId = Request.Cookies["UsrId"];

            TempData["UserId"] = UsrId;

            string Role = Request.Cookies["Role"];

            if (string.IsNullOrEmpty(UsrId) || string.IsNullOrEmpty(Role))
            {
                return RedirectToAction("Login", "Account");
            }

            else
            {
                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();
                ds = Obj_DataAccess.GetUserList(Role);



                List<Signup> UserList = new List<Signup>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    Signup reg = new Signup();
                    {
                        reg.Rcode = row["RCode"].ToString();
                        reg.Firstname = row["Fname"].ToString();
                        reg.Lastname = row["Lname"].ToString();
                        reg.Email = row["Email"].ToString();
                        reg.Phonenumber = row["Phoneno"].ToString();
                        reg.RId = Convert.ToInt32(row["RId"]);

                    };
                    UserList.Add(reg);
                }
                user.showlist = UserList;

                return View(user);

            }



        }


    }
}
