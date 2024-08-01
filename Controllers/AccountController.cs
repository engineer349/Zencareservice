using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Template;
using System.Data;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Google.Apis.Auth;
using System.Threading.Tasks;
using Zencareservice.Models;
using Zencareservice.Repository;
using System;
using System.IO;
using System.Net;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

using System.Globalization;
using System.CodeDom.Compiler;
using System.Xml.Linq;
using System.Net.Sockets;
using Zencareservice.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using Microsoft.SqlServer.Server;
using System.Security.Claims;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using NuGet.Protocol;

namespace Zencareservice.Controllers
{

    public class AccountController : Controller
    {


        private readonly IDataProtector _dataProtector;
        private readonly string _connectionString;

        private readonly OtpService _otpService;

        private int _generatedOtp;

        private int _regeneratedOtp;

       

        private readonly DataAccess _dataaccess;

        private readonly SqlDataAccess _sqldataaccess;

        private readonly EmailVerifier _emailVerifier;

       

        public AccountController(EmailVerifier emailVerifier, DataAccess dataaccess, SqlDataAccess sqldataaccess, IDataProtectionProvider dataProtectionProvider, IConfiguration configuration, OtpService otpService)
        {
            
            _emailVerifier = emailVerifier;
            _dataaccess = dataaccess;
            _sqldataaccess = sqldataaccess;
            _dataProtector = dataProtectionProvider.CreateProtector("MyCookieProtection");
            _otpService = otpService;
            _connectionString = configuration.GetConnectionString("ZencareserviceConnection");
        }




        public IActionResult Index()
        {

            ViewBag.Message = "Your Details are successfully saved!";

            return View("RegistrationSuccess", "Account");
        }

        public IActionResult AdminRegister()
        {
            string returnUrl = "/Account/AdminRegister";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult UserRegister()
        {
            string returnUrl = "/Account/UserRegister";
            ViewData["ReturnUrl"] = returnUrl;
            var roles = RolesDropdown();
            if (roles == null || !roles.Any())
            {
                ViewBag.Message = "No roles found in the database.";
            }

            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult DoctorRegister()
        {
            string returnUrl = "/Account/DoctorRegister";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        
        [HttpGet("/Account/PatientRegister/")]
        public IActionResult PatientRegister()
        {
            string returnUrl = "/Account/PatientRegister";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        public IActionResult PatientLogin()
        {
            string returnUrl = "/Account/PatientLogin";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        public IActionResult PatForgot()
        {
            string returnUrl = "/Account/PatForgot";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        public IActionResult PatReset()
        {
            string returnUrl = "/Account/PatReset";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        public IActionResult ResetPassword()
        {
            string returnUrl = "/Account/ResetPassword";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public IActionResult ForgotPassword()
        {

            return View();
        }

        //public IActionResult OTPVerification()
        //{
        //    string otpEmail = TempData["OTPEmail"] as string;

        //    if (string.IsNullOrEmpty(otpEmail))
        //    {
        //        return RedirectToAction("AccessDenied", "Account");
        //    }

        //    // Store OTPEmail in ViewBag to pass it to the view
        //    ViewBag.OTPEmail = otpEmail;

        //    // Optionally, store OTPEmail in TempData again if it needs to persist for the next request
        //    TempData["OTPEmail"] = otpEmail;

        //    return View(new OtpVerificationModel { OTPEmail = otpEmail });
        //}


        public IActionResult OTPLogin()
        {
            string returnUrl = "/Account/OTPLogin";
            ViewData["ReturnUrl"] = returnUrl;
            return View();

        }
        public IActionResult VerifyOtp()
        {
            string ?gotp = Request.Cookies["OTP"];

            ViewBag.Message = gotp;

            if (string.IsNullOrEmpty(gotp))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            HttpContext.Session.SetString("AccessDenied", DateTime.Now.AddMinutes(1).ToString());

            return RegistrationSuccess();
        }

        public IActionResult UserAccount()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ValidateResetOtp(Login model)
        {
            string username, password;
            string enteredOtp = model.ResendOTP;
            if (TempData.TryGetValue("GOTP", out var gotp))
            {
                ViewBag.Message = gotp;
                string _genotp = ViewBag.Message.ToString();
                TempData["ROTP"] = _genotp;
                if (enteredOtp != null)
                {
                    if (Convert.ToInt64(enteredOtp) == Convert.ToInt64(_genotp))
                    {
                        ViewBag.Message = "VerificationOTP";
                        TempData["SwalMessage"] = "OTP Verified";
                        TempData["SwalType"] = "success";
                        return Json(new { success = true, redirectUrl = Url.Action("PatReset", "Account") });

                    }

                    else
                    {
                        ViewBag.Message = "OTPFailed";
                        TempData["SwalMessage"] = "OTP Failed";
                        TempData["SwalType"] = "error";
                        return Json(new { success = false, redirectUrl = Url.Action("OTPVerification", "Account") });

                    }


                }
            }
            else if (TempData.TryGetValue("REGOTP", out var regotp))
            {
                ViewBag.Message = regotp;
                string _regenotp = ViewBag.Message.ToString();
                TempData["OTP"] = _regenotp;
                if (enteredOtp != null)
                {
                    if (Convert.ToInt64(enteredOtp) == Convert.ToInt64(_regenotp))
                    {
                        ViewBag.Message = "VerificationOTP";
                        TempData["SwalMessage"] = "OTP Verified";
                        TempData["SwalType"] = "success";


                        return RedirectToAction("PatReset", "Account");

                    }

                    else
                    {
                        ViewBag.Message = " Please Enter OTP!";
                        return View("OTPVerification", "Account");
                    }


                }

                return RedirectToAction("VerifyOtp", "Account");
            }

            ViewBag.Message = "InvalidOTP";
            TempData["SwalMessage"] = "OTP Input not valid";
            TempData["SwalType"] = "warning";
            return RedirectToAction("OTPVerification", "Account");
        }

        [HttpPost]
        public IActionResult ValidateOtp(Signup model)
        {
            string username;

            string password;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(model.numeric1);
            stringBuilder.Append(model.numeric2);
            stringBuilder.Append(model.numeric3);
            stringBuilder.Append(model.numeric4);
            stringBuilder.Append(model.numeric5);


            string enteredOtp = stringBuilder.ToString();

            if (TempData.TryGetValue("GOTP", out var gotp))
            {
                ViewBag.Message = gotp;

                string _genotp = ViewBag.Message.ToString();

                TempData["OTP"] = _genotp;

                if (enteredOtp != "")
                {
                    if (Convert.ToInt64(enteredOtp) == Convert.ToInt64(_genotp))
                    {

                        switch (Convert.ToInt64(enteredOtp) == Convert.ToInt64(_genotp))
                        {
                            case true:
                                username = TempData["User"] as string;
                                password = TempData["Password"] as string;
                                model.Password = password;
                                break;
                            case false:
                                username = TempData["User"] as string;
                                password = TempData["Password"] as string;
                                model.Password = password;
                                break;
                        }
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {

                            model.Username = username;
                            model.Password = password;
                            DataAccess Obj_DataAccess = new DataAccess();
                            DataSet ds = new DataSet();
                            ds = Obj_DataAccess.SaveOTPLogin(username, password);

                            int Status;

                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                Status = Convert.ToInt32(ds.Tables[0].Rows[0]["LStatus"]);

                                if (Status == 1)
                                {

                                    string ?UsrId = ds.Tables[0].Rows[0]["RId"].ToString();

                                    string ?UserName = ds.Tables[0].Rows[0]["Username"].ToString();

                                    TempData["Username"] = UserName;

                                    string ? Email = ds.Tables[0].Rows[0]["Email"].ToString();

                                    TempData["Email"] = Email;

                                    string Role = ds.Tables[0].Rows[0]["Role"].ToString();

                                    TempData["Role"] = Role;

                                    string? Fname = ds.Tables[0].Rows[0]["Fname"].ToString();

                                    TempData["FirstName"] = Fname;

                                    string ?RCode = ds.Tables[0].Rows[0]["RCode"].ToString();

                                    TempData["RCode"] = RCode;

                                    var userData = new Signup
                                    {
                                        UserId = UsrId,
                                        Rcode = RCode,
                                        Email = Email,
                                        RoleName = Role,
                                        Firstname = Fname
                                    };

                                    var userDataJson = JsonConvert.SerializeObject(userData);

                                    var protectedData = _dataProtector.Protect(userDataJson);


                                    var cookieOptions = new CookieOptions
                                    {
                                        Expires = DateTime.Now.AddDays(1),
                                        HttpOnly = true,
                                    };

                                    Response.Cookies.Append("EncryptCookie", protectedData, new CookieOptions
                                    {
                                        HttpOnly = true,
                                        Secure = true,
                                        SameSite = SameSiteMode.None,
                                        Expires = DateTimeOffset.Now.AddDays(1)

                                    });
                                    Response.Cookies.Append("MyCookie", "CookieValue", cookieOptions);

                                    Response.Cookies.Append("UserId", UsrId);
                                    CookieOptions options = new CookieOptions();
                                    options.Expires = DateTime.Now.AddMinutes(5);

                                    Response.Cookies.Append("UsrName", UserName, options);
                                    CookieOptions options1 = new CookieOptions();
                                    options1.Expires = DateTime.Now.AddMinutes(5);

                                    Response.Cookies.Append("Role", Role, options);
                                    CookieOptions options2 = new CookieOptions();
                                    options2.Expires = DateTime.Now.AddMinutes(5);

                                    Response.Cookies.Append("RCode", RCode, options);
                                    CookieOptions options3 = new CookieOptions();
                                    options3.Expires = DateTime.Now.AddMinutes(5);

                                    Response.Cookies.Append("UsrId", UsrId, options);
                                    // Get the cookie value
                                    HttpContext.Session.SetString("UsrId", UsrId);

                                    // Now, you can use 'storedUsrId' as needed

                                    HttpContext.Session.SetString("FirstName", Fname);

                                    string jsonString = JsonConvert.SerializeObject(ds.Tables[1]);


                                    HttpContext.Session.SetString("MenuList", jsonString);

                                    ViewBag.Message = "OTPLoginsuccessful";
                                    TempData["SwalMessage"] = "Login Succesfull";
                                    TempData["SwalType"] = "success";

                                    return RedirectToAction("Dashboard", "Report");

                                }

                                else
                                {
                                    return RedirectToAction("Login", "Account");
                                }

                            }
                            else
                            {

                                return RedirectToAction("VerifyOtp", "Account");
                            }

                        }

                        return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        ViewBag.Message = "PINMISMATCH";
                        TempData["SwalMessage"] = "Login Failed";
                        TempData["SwalType"] = "error";
                        return RedirectToAction("VerifyOtp", "Account");
                    }

                }



            }
            ViewBag.Message = "PINMISMATCH";
            TempData["SwalMessage"] = "Login Failed";
            TempData["SwalType"] = "error";
            return RedirectToAction("VerifyOtp", "Account");
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        [HttpPost]
        public IActionResult ResetPassword(Signup Obj)
        {
            string ResetPassword = Obj.RPassword;
            string ConfirmResetPassword = Obj.CRPassword;

            if (TempData.TryGetValue("MyEmail", out var resetEmailObj) && resetEmailObj != null)
            {
                string? email = resetEmailObj.ToString();


                if (!string.IsNullOrEmpty(ResetPassword) && !string.IsNullOrEmpty(ConfirmResetPassword))
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.ResetPassword(Obj, email);

                    HttpContext.Session.Clear();

                    // Delete cookies
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    ViewBag.Message = "UpdatedPassword";
                    TempData["SwalMessage"] = "Successfully Updated";
                    TempData["SwalType"] = "success";
                    return RedirectToAction("PatientLogin", "Account");
                }
                else
                {
                    return View();
                }

            }
            else
            {

                return View();
            }

        }


        [Authorize(Roles = "Admin, Patient, Doctor")]
        [HttpPost]
        public IActionResult PatReset(Signup Obj)
        {
            string ResetPassword = Obj.RPassword;
            string ConfirmResetPassword = Obj.CRPassword;

            if (TempData.TryGetValue("MyEmail", out var resetEmailObj) && resetEmailObj != null)
            {
                string ?email = resetEmailObj.ToString();


                if (!string.IsNullOrEmpty(ResetPassword) && !string.IsNullOrEmpty(ConfirmResetPassword))
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.ResetPassword(Obj, email);

                    HttpContext.Session.Clear();

                    // Delete cookies
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
             
                    ViewBag.Message = "UpdatedPassword";
                    TempData["SwalMessage"] = "Successfully Updated";
                    TempData["SwalType"] = "success";
                    return RedirectToAction("PatientLogin", "Account");
                }
                else
                {
                    return View();
                }

            }
            else
            {

                return View();
            }

        }

       

      

        public IActionResult ResendEmail(Signup Obj)
        {
            string regeneratedCode = Codegenerator();
            _regeneratedOtp = Convert.ToInt32(regeneratedCode);
            TempData["REGOTP"] = _regeneratedOtp;
            if (TempData.TryGetValue("MyEmail", out var myEmail))
            {

                ViewBag.Message = myEmail;
                Obj.Email = Convert.ToString(ViewBag.Message);

                SendMail sendMail = new SendMail();
                SmtpClient client = new SmtpClient();

                string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.Email, "noiauctkdrybniwx", "Autoverification", "Your Zencareservice  Account  OTP verification of Email is " + +_regeneratedOtp, "smtp.gmail.com", 587);

                return RedirectToAction("OTPVerification", "Account");
            }
            else
            {
                //ViewBag.Message = "Your Account may be validated!";
                //return RedirectToAction("PatientLogin", "Account");
            }
            ViewBag.Message = "Your Account may be validated!";
            return Json(new { redirectToLogin = true });


        }

        private bool IsDateOfBirthValid(DateTime dob)
        {

            return dob.AddYears(18) <= DateTime.Now && dob > DateTime.Now.AddYears(-100); // Assuming a reasonable upper limit for age

        }

        private int CalculateAge(DateTime dob)
        {
            // Calculate age based on the difference between the current date and the date of birth
            int age = DateTime.Now.Year - dob.Year;

            return age;
        }

      

        private string Codegenerator()
        {
            Random random = new Random();
            int randomCode = random.Next(100000, 999999);
            _generatedOtp = randomCode;
            TempData["GOTP"] = _generatedOtp;

            return _generatedOtp.ToString();

        }

        private static byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private static int GetResponseCode(string responseString)
        {
            // Handle potential invalid responses with exception or default value
            if (responseString.Length < 3)
            {
                throw new Exception("Invalid response received from server");
            }
            return int.Parse(responseString.Substring(0, 3));
        }

  
        private static bool IsEmailAccountValid(string smtpServer, string emailAddress)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect(smtpServer, 587);
                    using (var netStream = client.GetStream())
                    using (var reader = new StreamReader(netStream))
                    {
                        netStream.ReadTimeout = 10000;
                        netStream.WriteTimeout = 10000;  //Set timeout to 10 seconds

                        string CRLF = "\r\n";
                        byte[] dataBuffer;
                        string responseString;

                        dataBuffer = BytesFromString("EHLO Hi" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        responseString = reader.ReadLine();

                        dataBuffer = BytesFromString("MAIL FROM:<zencareservice.noreply@gmail.com>" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        responseString = reader.ReadLine();

                        dataBuffer = BytesFromString($"RCPT TO:<{emailAddress}>" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);
                        responseString = reader.ReadLine();
                        int responseCode = GetResponseCode(responseString);

                        dataBuffer = BytesFromString("QUIT" + CRLF);
                        netStream.Write(dataBuffer, 0, dataBuffer.Length);

                        return responseCode != 550;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking email: " + ex.Message);
                return false;
            }
        }



        [HttpPost]
        public async Task<IActionResult> PatientRegister(Signup Obj, string returnUrl)
        {

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {

                if (IsDateOfBirthValid(Obj.Dob))
                {

                    int userAge = CalculateAge(Obj.Dob);


                    bool agreeToTerms = true;


                    if (agreeToTerms == true)
                    {

                        bool isEmailValid = await _emailVerifier.VerifyEmailAsync(Obj.Email);

                        if (isEmailValid)
                        {
                            DataAccess Obj_DataAccess = new DataAccess();
                            DataSet dse = new DataSet();
                            dse = Obj_DataAccess.CheckEmail(Obj);

                            if (dse.Tables[0].Rows.Count == 0 && dse.Tables[1].Rows.Count == 0)
                            {

                                try
                                {
                                    string validemail = Obj.Email;
                                    TempData["MyEmail"] = validemail;



                                    //Console.WriteLine($"Gmail account is valid - {gMail.ToString()}");

                                    // var live = IsEmailAccountValid("live-com.olc.protection.outlook.com", "aa.aa@live.com");
                                    //Console.WriteLine($"Live account is valid - {live.ToString()}");


                                    Obj.RoleId = "Patient";
                                    Obj.RCategory = "Patient";

                                    Obj.Age = userAge;

                                    int agreeterms = Convert.ToInt32(Obj.agreeterm);

                                    string fname = Obj.Firstname;
                                    string ?lname = Obj.Lastname;

                                    string ?password = Obj.Password;

                                    TempData["Password"] = password;

                                    string ?confirmpassword = Obj.Confirmpassword;

                                    string ?username = Obj.Username;
                                    TempData["User"] = username;
                                    string ?phoneno = Obj.Phonenumber;


                                    DateTime Dob = Obj.Dob;
                                    Obj.Status = 1;

                                    if (!String.IsNullOrEmpty(validemail))
                                    {

                                        string generatedCode = Codegenerator();

                                        _generatedOtp = Convert.ToInt32(generatedCode);
                                        CookieOptions options = new CookieOptions();
                                        options.Expires = DateTime.Now.AddMinutes(5);
                                        Response.Cookies.Append("OTP", generatedCode, options);

                                    }

                                    SendingEmail(Obj);

                                    //ValidateOtp(Obj);

                                    DataAccess Obj_DataAccess2 = new DataAccess();
                                    DataSet ds = new DataSet();
                                    ds = Obj_DataAccess2.SaveRegister(Obj);

                                    ViewBag.Message = "success";
                                    TempData["SwalMessage"] = "User AccountCreated!";
                                    TempData["SwalType"] = "success";

                                    return RedirectToAction("VerifyOtp", "Account");




                                }


                                catch (Exception ex)
                                {
                                    string msg = ex.Message.ToString();
                                    ViewBag.Message = msg;
                                }
                            }
                            else
                            {
                                ViewBag.Message = "UserAlreadyExists";
                                TempData["SwalMessage"] = "UserAlreadyExists";
                                TempData["SwalType"] = "warning";
                            }
                        }
                        else
                        {


                            ViewBag.Message = "InvalidEmailaddress";

                            TempData["Email"] = "InvalidUser";
                            TempData["SwalMessage"] = "InvalidEmailaddress";
                            TempData["SwalType"] = "warning";

                            return View();



                        }
                    }

                    else
                    {
                        ModelState.AddModelError(nameof(Signup.agreeterm), "Pls  agree to terms of service and condition.");
                    }

                }
                else
                {
                    ViewBag.Message = "Legal";
                    TempData["SwalMessage"] = "Age Restriction";
                    TempData["SwalType"] = "warning";
                    ModelState.AddModelError(nameof(Signup.Dob), "User must be at least 18 years old.");
                }
            }

            else
            {
                ViewBag.Message = "PleaseTryAgain";
                TempData["SwalMessage"] = "Pls tryagain";
                TempData["SwalType"] = "warning";
                return RedirectToAction("Index", "Home");
            }

            return View();


        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegister(Signup Obj, string returnUrl)
        {

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {


                if (IsDateOfBirthValid(Obj.Dob))
                {

                    int userAge = CalculateAge(Obj.Dob);

                    bool agreeToTerms = true;

                    if (agreeToTerms == true)
                    {

                        bool isEmailValid = await _emailVerifier.VerifyEmailAsync(Obj.Email);

                        if (isEmailValid)
                        {
                            DataAccess Obj_DataAccess = new DataAccess();
                            DataSet dse = new DataSet();
                            dse = Obj_DataAccess.CheckEmail(Obj);

                            if (dse.Tables[0].Rows.Count == 0 && dse.Tables[1].Rows.Count == 0)
                            {

                                try
                                {
                                    string validemail = Obj.Email;
                                    TempData["MyEmail"] = validemail;



                                    //Console.WriteLine($"Gmail account is valid - {gMail.ToString()}");

                                    // var live = IsEmailAccountValid("live-com.olc.protection.outlook.com", "aa.aa@live.com");
                                    //Console.WriteLine($"Live account is valid - {live.ToString()}");


                                    Obj.RoleId = "Doctor";
                                    Obj.RCategory = "Employee";
                                    Obj.Username = "";
                                    Obj.Age = userAge;
                                    int agreeterms = Convert.ToInt32(Obj.agreeterm);
                                    string fname = Obj.Firstname;
                                    string lname = Obj.Lastname;
                                    string password = Obj.Password;
                                    TempData["Password"] = password;
                                    string confirmpassword = Obj.Confirmpassword;
                                    string phoneno = Obj.Phonenumber;
                                    DateTime Dob = Obj.Dob;
                                    Obj.Status = 1;

                                    if (!String.IsNullOrEmpty(validemail))
                                    {

                                        string generatedCode = Codegenerator();

                                        _generatedOtp = Convert.ToInt32(generatedCode);
                                        CookieOptions options = new CookieOptions();
                                        options.Expires = DateTime.Now.AddMinutes(5);
                                        Response.Cookies.Append("OTP", generatedCode, options);

                                    }

                                    SendingEmail(Obj);

                                    //ValidateOtp(Obj);

                                    DataAccess Obj_DataAccess2 = new DataAccess();
                                    DataSet ds = new DataSet();
                                    ds = Obj_DataAccess2.SaveRegister(Obj);

                                    ViewBag.Message = "success";
                                    TempData["SwalMessage"] = "User AccountCreated!";
                                    TempData["SwalType"] = "success";

                                    return RedirectToAction("VerifyOtp", "Account");




                                }


                                catch (Exception ex)
                                {
                                    string msg = ex.Message.ToString();
                                    ViewBag.Message = msg;
                                }
                            }
                            else
                            {
                                ViewBag.Message = "UserAlreadyExists";
                                TempData["SwalMessage"] = "UserAlreadyExists";
                                TempData["SwalType"] = "warning";
                            }
                        }
                        else
                        {


                            ViewBag.Message = "InvalidEmailaddress";

                            TempData["Email"] = "InvalidUser";
                            TempData["SwalMessage"] = "InvalidEmailaddress";
                            TempData["SwalType"] = "warning";

                            return View();



                        }
                    }

                    else
                    {
                        ModelState.AddModelError(nameof(Signup.agreeterm), "Pls  agree to terms of service and condition.");
                    }

                }
                else
                {
                    ViewBag.Message = "Legal";
                    TempData["SwalMessage"] = "Age Restriction";
                    TempData["SwalType"] = "warning";
                    ModelState.AddModelError(nameof(Signup.Dob), "User must be at least 18 years old.");
                }
            }

            else
            {
                ViewBag.Message = "PleaseTryAgain";
                TempData["SwalMessage"] = "Pls tryagain";
                TempData["SwalType"] = "warning";
                return RedirectToAction("Index", "Home");
            }


            return View();


        }

        private List<Signup> RolesDropdown()
        {
            DataAccess Obj_DataAccess = new DataAccess();
            DataSet ds = Obj_DataAccess.RolesList();

            var roles = new List<Signup>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                roles.Add(new Signup
                {
                    RoleId = row["RoleId"].ToString(),
                    RoleName = row["RoleName"].ToString()
                });
            }

            return roles;
        }

        [HttpPost]
        public IActionResult UserRegister(Signup Obj, string returnUrl, DateTime userDob)
        {

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                RolesDropdown();
                var roles = RolesDropdown();
                ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");


                if (IsDateOfBirthValid(Obj.Dob))
                {
                    int userAge = CalculateAge(Obj.Dob);
                    bool agreeToTerms = true;

                    if (agreeToTerms == true)
                    {


                        var gMail = IsEmailAccountValid("gmail-smtp-in.l.google.com", Obj.Email);

                        if (gMail == true)
                        {
                            DataAccess Obj_DataAccess = new DataAccess();
                            DataSet dse = new DataSet();
                            dse = Obj_DataAccess.CheckEmail(Obj);

                            if (dse.Tables[0].Rows.Count == 0)
                            {

                                try
                                {
                                    string validemail = Obj.Email;
                                    TempData["MyEmail"] = validemail;



                                    //Console.WriteLine($"Gmail account is valid - {gMail.ToString()}");

                                    // var live = IsEmailAccountValid("live-com.olc.protection.outlook.com", "aa.aa@live.com");
                                    //Console.WriteLine($"Live account is valid - {live.ToString()}");


                                    string SelectedRoleId = Obj.RoleId;
                                    Obj.Age = userAge;

                                    if (Obj.RoleId == "Doctor")
                                    {
                                        Obj.RCategory = "Employee";
                                        Obj.Username = "Employee";
                                    }
                                    else if (Obj.RoleId == "Staff")
                                    {
                                        Obj.RCategory = "Employee";
                                        Obj.Username = "Employee";
                                    }
                                    else
                                    {
                                        Obj.RCategory = "Employee";
                                        Obj.Username = "Employee";
                                    }
                                    int agreeterms = Convert.ToInt32(Obj.agreeterm);
                                    string ?fname = Obj.Firstname;
                                    string ?lname = Obj.Lastname;
                                    string ?password = Obj.Password;
                                    string ?confirmpassword = Obj.Confirmpassword;
                                    string ?phoneno = Obj.Phonenumber;

                                    DateTime Dob = Obj.Dob;
                                    Obj.Status = 1;

                                    if (!String.IsNullOrEmpty(validemail))
                                    {

                                        string generatedCode = Codegenerator();

                                        _generatedOtp = Convert.ToInt32(generatedCode);
                                        CookieOptions options = new CookieOptions();
                                        options.Expires = DateTime.Now.AddMinutes(2);
                                        Response.Cookies.Append("OTP", generatedCode, options);

                                    }

                                    SendingEmail(Obj);


                                    DataAccess Obj_DataAccess2 = new DataAccess();
                                    DataSet ds = new DataSet();
                                    ds = Obj_DataAccess2.SaveRegister(Obj);
                                    ViewBag.Message = "success";
                                    TempData["SwalMessage"] = "User AccountCreated!";
                                    TempData["SwalType"] = "success";

                                    return RedirectToAction("VerifyOtp", "Account");




                                }


                                catch (Exception ex)
                                {
                                    string msg = ex.Message.ToString();
                                    ViewBag.Message = msg;
                                }
                            }
                            else
                            {
                                ViewBag.Message = "UserAlreadyExists";
                                TempData["SwalMessage"] = "UserAlready Exsits";
                                TempData["SwalType"] = "error";

                            }
                        }
                        else
                        {


                            ViewBag.Message = "InvalidEmailaddress!";
                            TempData["Email"] = "InvalidUser";
                            TempData["SwalMessage"] = "InvalidEmailaddress";
                            TempData["SwalType"] = "warning";


                        }

                    }
                    else
                    {
                        ModelState.AddModelError(nameof(Signup.agreeterm), "Pls  agree to terms of service and condition.");
                    }

                }

                else
                {
                    ViewBag.Message = "Legal";
                    TempData["SwalMessage"] = "Age Restriction";
                    TempData["SwalType"] = "warning";
                    ModelState.AddModelError(nameof(Signup.Dob), "User must be at least 18 years old.");
                }
            }

            else
            {
                ViewBag.Message = "PleaseTryAgain";
                TempData["SwalMessage"] = "Pls tryagain";
                TempData["SwalType"] = "warning";
                return RedirectToAction("Index", "Home");

            }

            return View();

        }

        [HttpPost]
        public IActionResult AdminRegister(Signup Obj)
        {


            // Perform additional validation
            if (IsDateOfBirthValid(Obj.Dob))
            {

                int userAge = CalculateAge(Obj.Dob);

                bool agreeToTerms = true;

                if (agreeToTerms == true)
                {

                    var gMail = IsEmailAccountValid("gmail-smtp-in.l.google.com", Obj.Email);

                    if (gMail == true)
                    {
                        DataAccess Obj_DataAccess = new DataAccess();
                        DataSet dse = new DataSet();
                        dse = Obj_DataAccess.CheckEmail(Obj);

                        if (dse.Tables[0].Rows.Count == 0)
                        {

                            try
                            {
                                string validemail = Obj.Email;
                                TempData["MyEmail"] = validemail;

                                //string SelectedRoleId = "Admin";

                                Obj.RoleId = "Admin";

                                if (Obj.RoleId == "Admin")
                                {
                                    Obj.RCategory = "Admin";
                                }
                                else if (Obj.RoleId == "Doctor")
                                {
                                    Obj.RCategory = "Employee";
                                }
                                else
                                {
                                    Obj.RCategory = "Patient";
                                }


                                Obj.Age = userAge;
                                int agreeterms = Convert.ToInt32(Obj.agreeterm);
                                string ?fname = Obj.Firstname;
                                string ?lname = Obj.Lastname;
                                string ?password = Obj.Password;
                                string ?confirmpassword = Obj.Confirmpassword;
                                string ?username = Obj.Username;
                                string ?phoneno = Obj.Phonenumber;

                                DateTime Dob = Obj.Dob;
                                Obj.Status = 1;

                                if (!String.IsNullOrEmpty(validemail))
                                {

                                    string generatedCode = Codegenerator();

                                    _generatedOtp = Convert.ToInt32(generatedCode);
                                    CookieOptions options = new CookieOptions();
                                    options.Expires = DateTime.Now.AddMinutes(2);
                                    Response.Cookies.Append("OTP", generatedCode, options);

                                }

                                SendingEmail(Obj);


                                DataAccess Obj_DataAccess2 = new DataAccess();
                                DataSet ds = new DataSet();
                                ds = Obj_DataAccess2.SaveRegister(Obj);
                                ViewBag.Message = "success";
                                TempData["SwalMessage"] = "User AccountCreated!";
                                TempData["SwalType"] = "success";

                                return RedirectToAction("VerifyOtp", "Account");


                            }


                            catch (Exception ex)
                            {
                                string msg = ex.Message.ToString();
                                ViewBag.Message = msg;
                                TempData["SwalMessage"] = msg;
                                TempData["SwalType"] = "error";


                            }
                        }
                        else
                        {
                            ViewBag.Message = "UserAlready Exsits";
                            TempData["SwalMessage"] = "User Exists!";
                            TempData["SwalType"] = "warning!";


                        }
                    }
                    else
                    {
                        TempData["Email"] = "InvalidUser";
                        ViewBag.Message = "Invalid Emailaddress UserAccount can't create.Pls Enter a valid email address!";
                        return View();

                    }

                }
                else
                {
                    ModelState.AddModelError(nameof(Signup.agreeterm), "Pls  agree to terms of service and condition.");
                }

            }





            return View();


        }

        private string SendingEmail(Signup Obj)
        {
            string ?FName = Obj.Firstname;
            SendMail sendMail = new SendMail();
            SmtpClient client = new SmtpClient();

            string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.Email, "noiauctkdrybniwx", "Autoverification", $@"



                        <!DOCTYPE html>

                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Email Verification</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    margin: 0;
                                    padding: 0;
                                    display: flex;
                                    flex-direction: column;
                                    justify-content: center;
                                    align-items: center;
                                    height: 100vh;
                                    background-color: #f4f4f4;
                                }}

                                .logo {{
                                    max-width: 200px;
                                    height: auto;
                                    margin-bottom: 20px;
                                }}

                                .verification-container {{
                                    background-color: #fff;
                                    padding: 20px;
                                    border-radius: 5px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                    text-align: center;
                                }}

                                .verification-code {{
                                    font-size: 24px;
                                    font-weight: bold;
                                    color: #007bff;
                                    margin-bottom: 20px;
                                }}

                                .verification-instructions {{
                                    color: #555;
                                    margin-bottom: 20px;
                                }}

                                .verification-btn {{
                                    padding: 10px;
                                    background-color: #007bff;
                                    color: #fff;
                                    border: none;
                                    border-radius: 5px;
                                    cursor: pointer;
                                }}
                            </style>
                        </head>
                        <body>
                            <img src=""~/images/zencare-logo1.png"" alt=""Your Logo"" class=""logo"">
                            <div class=""verification-container"">
        
                                <h2>Hi {FName},</p>
                                <p>Thank you for using Zencareserviceservice! </p>
                                <p>To ensure the security of your account, we have generated a One-Time Password (OTP) for you.</p>
                                <p class=""verification-code"">Your Verification Code:{_generatedOtp}</p>
                                <p class=""verification-instructions"">Please use the above code to verify your email address.</p>
        
                            </div>
                        </body>
                        </html>", "smtp.gmail.com", 587);
            return mail;

        }

        private string SendingOTPEmail(OTPLoginModel Obj)
        {
            string ?FName = Obj.OTPEmail;
            SendMail sendMail = new SendMail();
            SmtpClient client = new SmtpClient();

            string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.OTPEmail, "noiauctkdrybniwx", "2FAuthentication", $@"



                        <!DOCTYPE html>

                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>OTP Verification</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    margin: 0;
                                    padding: 0;
                                    display: flex;
                                    flex-direction: column;
                                    justify-content: center;
                                    align-items: center;
                                    height: 100vh;
                                    background-color: #f4f4f4;
                                }}

                                .logo {{
                                    max-width: 200px;
                                    height: auto;
                                    margin-bottom: 20px;
                                }}

                                .verification-container {{
                                    background-color: #fff;
                                    padding: 20px;
                                    border-radius: 5px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                    text-align: center;
                                }}

                                .verification-code {{
                                    font-size: 24px;
                                    font-weight: bold;
                                    color: #007bff;
                                    margin-bottom: 20px;
                                }}

                                .verification-instructions {{
                                    color: #555;
                                    margin-bottom: 20px;
                                }}

                                .verification-btn {{
                                    padding: 10px;
                                    background-color: #007bff;
                                    color: #fff;
                                    border: none;
                                    border-radius: 5px;
                                    cursor: pointer;
                                }}
                            </style>
                        </head>
                        <body>
                            <img src=""~/images/zencare-logo1.png"" alt=""Your Logo"" class=""logo"">
                            <div class=""verification-container"">

                                <h2>Hi {FName},</p>
                                <p>Thank you for using Zencareserviceservice! </p>
                                <p>To ensure the security of your account, we have generated a One-Time Password (OTP) for you.</p>
                                <p class=""verification-code"">Your Verification Code:{_generatedOtp}</p>
                                <p class=""verification-instructions"">Please use the above code to verify your email address.</p>

                            </div>
                        </body>
                        </html>", "smtp.gmail.com", 587);
            return mail;

        }

        private string SendingResetEmail(Signup Obj)
        {

            SendMail sendMail = new SendMail();
            SmtpClient client = new SmtpClient();

            string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.Email, "noiauctkdrybniwx", "ResetEmailVerification", $@"



                        <!DOCTYPE html>

                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Email Verification</title>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    margin: 0;
                                    padding: 0;
                                    display: flex;
                                    flex-direction: column;
                                    justify-content: center;
                                    align-items: center;
                                    height: 100vh;
                                    background-color: #f4f4f4;
                                }}

                                .logo {{
                                    max-width: 200px;
                                    height: auto;
                                    margin-bottom: 20px;
                                }}

                                .verification-container {{
                                    background-color: #fff;
                                    padding: 20px;
                                    border-radius: 5px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                    text-align: center;
                                }}

                                .verification-code {{
                                    font-size: 24px;
                                    font-weight: bold;
                                    color: #007bff;
                                    margin-bottom: 20px;
                                }}

                                .verification-instructions {{
                                    color: #555;
                                    margin-bottom: 20px;
                                }}

                                .verification-btn {{
                                    padding: 10px;
                                    background-color: #007bff;
                                    color: #fff;
                                    border: none;
                                    border-radius: 5px;
                                    cursor: pointer;
                                }}
                            </style>
                        </head>
                        <body>
                            <img src=""~/images/zencare-logo1.png"" alt=""Your Logo"" class=""logo"">
                            <div class=""verification-container"">
        
                                <h2>Hi ,</p>
                                <p>Thank you for using Zencareserviceservice! </p>
                                <p>To ensure the security of your account, we have generated a One-Time Password (OTP) for you.</p>
                                <p class=""verification-code"">Your Verification Code:{_generatedOtp}</p>
                                <p class=""verification-instructions"">Please use the above code to verify your email address.</p>
        
                            </div>
                        </body>
                        </html>", "smtp.gmail.com", 587);
            return mail;

        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        public IActionResult GetDecryptedCookie()
        {
            var encryptedData = Request.Cookies["YourCookieName"];
            var userDataJson = _dataProtector.Unprotect(encryptedData);

            // Deserialize UserData from the JSON string
            var userData = JsonConvert.DeserializeObject<Signup>(userDataJson);

            // Use the
            //userData.UserId, 
            //userData.UserName, 
            //userData.Role, 
            //userData.RCode 

            return View();
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login Obj)
        {

            string ?username = Obj.Username;

            string ?password = Obj.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {


                DataAccess Obj_DataAccess = new DataAccess();
                DataSet ds = new DataSet();
                ds = Obj_DataAccess.SaveLogin(Obj);

                int Status;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    Status = Convert.ToInt32(ds.Tables[0].Rows[0]["LStatus"]);
                    if (Status == 1)
                    {

                        string ?UsrId = ds.Tables[0].Rows[0]["RId"].ToString();

                        string ?UserName = ds.Tables[0].Rows[0]["Username"].ToString();

                        TempData["Username"] = Obj.Username;


                        string ?Email = ds.Tables[0].Rows[0]["Email"].ToString();

                        TempData["Email"] = Email;

                        string ?Role = ds.Tables[0].Rows[0]["Role"].ToString();

                        TempData["Role"] = Role;

                        string ?Fname = ds.Tables[0].Rows[0]["Fname"].ToString();

                        TempData["FirstName"] = Fname;

                        string ?RCode = ds.Tables[0].Rows[0]["RCode"].ToString();

                        TempData["RCode"] = RCode;



                        var userData = new Signup
                        {
                            UserId = UsrId,
                            Rcode = RCode,
                            Email = Email,
                            RoleName = Role,
                            Firstname = Fname
                        };

                        var userDataJson = JsonConvert.SerializeObject(userData);

                        var protectedData = _dataProtector.Protect(userDataJson);


                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(1), // Set the expiration date
                            HttpOnly = true, // Makes the cookie accessible only to the server-side code
                        };

                        Response.Cookies.Append("EncryptCookie", protectedData, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTimeOffset.Now.AddDays(1)

                        });
                        Response.Cookies.Append("MyCookie", "CookieValue", cookieOptions);

                        Response.Cookies.Append("UserId", UsrId);
                        CookieOptions options = new CookieOptions();
                        options.Expires = DateTime.Now.AddMinutes(5);

                        Response.Cookies.Append("UsrName", UserName, options);
                        CookieOptions options1 = new CookieOptions();
                        options.Expires = DateTime.Now.AddMinutes(5);

                        Response.Cookies.Append("Role", Role, options);
                        CookieOptions options2 = new CookieOptions();
                        options.Expires = DateTime.Now.AddMinutes(5);

                        Response.Cookies.Append("RCode", RCode, options);
                        CookieOptions options3 = new CookieOptions();
                        options.Expires = DateTime.Now.AddMinutes(5);

                        Response.Cookies.Append("UsrId", UsrId, options);
                        // Get the cookie value
                        HttpContext.Session.SetString("UsrId", UsrId);

                        var claims = new List<Claim>
                       {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, Role),
                        new Claim("UserId", UsrId)
                    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            // Allow refresh
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        };


                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        HttpContext.Session.SetString("FirstName", Fname);

                        string jsonString = JsonConvert.SerializeObject(ds.Tables[1]);


                        HttpContext.Session.SetString("MenuList", jsonString);


                        ViewBag.Message = "Login";
                        TempData["SwalMessage"] = "Login Succesfull";
                        TempData["SwalType"] = "success";


                        return RedirectToAction("Dashboard", "Report");

                    }
                }
            }
            else
            {
                ViewBag.Message = "Invalid";
                TempData["SwalMessage"] = "Invalid Username or Password!";
                TempData["SwalType"] = "error";
                return RedirectToAction("Login", "Account");

            }
            ViewBag.Message = "Invalid";
            TempData["SwalMessage"] = "Invalid Username or Password!";
            TempData["SwalType"] = "error";
            return RedirectToAction("PatientLogin", "Account");

        }

        [HttpGet("/Account/GoogleLogin")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleSignInCallback") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleSignInCallback", Name = "GoogleSignInCallback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleSignInCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Authentication failed.");

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var emailClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
                return BadRequest("Invalid login attempt.");

            var email = emailClaim.Value;
            var userDataResult = await GetUserDataByEmailAsync(email);

            if (userDataResult == null || userDataResult.UserData == null)
                return RedirectToAction("ShowRegisterAlert");

            var userDataJson = JsonConvert.SerializeObject(userDataResult.UserData);
            var protectedData = _dataProtector.Protect(userDataJson);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("EncryptCookie", protectedData, cookieOptions);
            Response.Cookies.Append("UsrId", userDataResult.UserData.UserId, cookieOptions);
            Response.Cookies.Append("UsrName", userDataResult.UserData.Firstname, cookieOptions);
            Response.Cookies.Append("Role", userDataResult.UserData.RoleName, cookieOptions);
            Response.Cookies.Append("RCode", userDataResult.UserData.Rcode, cookieOptions);

            HttpContext.Session.SetString("MenuList", userDataResult.JsonString) ;
            HttpContext.Session.SetString("UsrId", userDataResult.UserData.UserId);
            HttpContext.Session.SetString("FirstName", userDataResult.UserData.Firstname);

            var identity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, userDataResult.UserData.Firstname),
        new Claim(ClaimTypes.Role, userDataResult.UserData.RoleName),
        new Claim("UsrId", userDataResult.UserData.UserId)
    }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            TempData["SwalMessage"] = "Login Successful";
            TempData["SwalType"] = "success";

            return RedirectToAction("Dashboard", "Report");
        }



        public async Task<UserDataResult?> GetUserDataByEmailAsync(string email)
        {
            return await Task.Run(() =>
            {
                DataAccess obj_DataAccess = new DataAccess();
                DataSet ds = obj_DataAccess.SaveGLogin(email);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    if (Convert.ToInt32(row["LStatus"]) == 1)
                    {
                        var userData = new Signup
                        {
                            UserId = row["RId"].ToString(),
                            Firstname = row["Fname"].ToString(),
                            RoleName = row["Role"].ToString(),
                            Rcode = row["RCode"].ToString(),
                            Email = email
                        };
                        string jsonString = JsonConvert.SerializeObject(ds.Tables[1]);

                        return new UserDataResult
                        {
                            UserData = userData,
                            JsonString = jsonString
                        };
                    }
                }
                return null;
            });
        }



        public class UserDataResult
        {
            public Signup? UserData { get; set; }
            public string? JsonString { get; set; }
        }


        private async Task<string> DecodeIdTokenAndGetEmailAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return payload.Email;
            }
            catch (Exception ex)
            {
                // Handle validation errors
                return null;
            }
        }

     

        public IActionResult ShowRegisterAlert()
        {
            TempData["SwalMessage"] = "Please register";
            TempData["SwalType"] = "warning";
            return RedirectToAction("Register", "Account");
        }

        [HttpPost]
        public IActionResult PatForgot(Signup Obj)
        {
            string ResetEmail = Obj.Email;
            if (ResetEmail != null)
            {


                var gMail = true;

                if (gMail == true)
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.CheckEmail(Obj);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string validemail = Obj.Email;

                        TempData["OTPEmail"] = validemail;
                        TempData["Source"] = "PatForgot";
                        TempData["Title"] = "PatForgot";

                        if (!String.IsNullOrEmpty(validemail))
                        {
                            string generatedCode = Codegenerator();
                            _generatedOtp = Convert.ToInt32(generatedCode);
                            // Store OTP in TempData for verification
                            TempData["OTP"] = generatedCode;
                            SendingResetEmail(Obj);
                            TempData.Keep("OTP");

                            ViewBag.Message = "VerificationOTP";
                            TempData["SwalMessage"] = "OTP sent";
                            TempData["SwalType"] = "success";
                            DataAccess Obj_DataAccess1 = new DataAccess();
                            DataSet dse = Obj_DataAccess1.SaveOTP(Obj.Email, generatedCode);

                        }
                        return RedirectToAction("OTPVerification", "Account");
                    }
                    else
                    {
                        ViewBag.Message = "InvalidEmail";
                        TempData["SwalMessage"] = "Invalid";
                        TempData["SwalType"] = "error";
                        return RedirectToAction("PatientRegister", "Account");
                    }
                }
                else
                {
                    ViewBag.Message = "Please enter your registered email to continue!";
                    return RedirectToAction("PatForgot", "Account");
                }
            }
            else
            {
                ViewBag.Message = "Please enter your registered email to continue!";
                return RedirectToAction("PatForgot", "Account");
            }
        }

        [HttpPost]
        public IActionResult DocForgot(Signup Obj)
        {
            string ResetEmail = Obj.Email;
            if (ResetEmail != null)
            {


                var gMail = true;

                if (gMail == true)
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.CheckEmail(Obj);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string validemail = Obj.Email;

                        TempData["OTPEmail"] = validemail;
                        TempData["Source"] = "DocForgot";
                        TempData["Title"] = "DocForgot";

                        if (!String.IsNullOrEmpty(validemail))
                        {
                            string generatedCode = Codegenerator();
                            _generatedOtp = Convert.ToInt32(generatedCode);
                            // Store OTP in TempData for verification
                            TempData["OTP"] = generatedCode;
                            SendingResetEmail(Obj);
                            TempData.Keep("OTP");

                            ViewBag.Message = "VerificationOTP";
                            TempData["SwalMessage"] = "OTP sent";
                            TempData["SwalType"] = "success";
                            DataAccess Obj_DataAccess1 = new DataAccess();
                            DataSet dse = Obj_DataAccess1.SaveOTP(Obj.Email, generatedCode);

                        }
                        return RedirectToAction("OTPVerification", "Account");
                    }
                    else
                    {
                        ViewBag.Message = "InvalidEmail";
                        TempData["SwalMessage"] = "Invalid";
                        TempData["SwalType"] = "error";
                        return RedirectToAction("Register", "Account");
                    }
                }
                else
                {
                    ViewBag.Message = "Please enter your registered email to continue!";
                    return RedirectToAction("ForgotPassword", "Account");
                }
            }
            else
            {
                ViewBag.Message = "Please enter your registered email to continue!";
                return RedirectToAction("ForgotPassword", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> OTPLogin(OTPLoginModel Obj, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                string ?email = Obj.OTPEmail;

                

                bool isEmailValid = await _emailVerifier.VerifyEmailAsync(email);
                if (isEmailValid)
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet dse = Obj_DataAccess.CheckLoginEmail(Obj);

                    if (dse.Tables[0].Rows.Count != 0 && dse.Tables[1].Rows.Count != 0)
                    {
                        try
                        {
                            string validemail = Obj.OTPEmail;
                            TempData["MyEmail"] = validemail;

                            if (!string.IsNullOrEmpty(validemail))
                            {
                                string generatedCode = Codegenerator();

                                if (int.TryParse(generatedCode, out int generatedOtp))
                                {
                                    _generatedOtp = generatedOtp; 
                                    SendingOTPEmail(Obj);

                                    ViewBag.Message = "otpgenerated";
                                    TempData["SwalMessage"] = "OTP sent";
                                    TempData["SwalType"] = "success";

                                    DataAccess Obj_DataAccess1 = new DataAccess();
                                    DataSet ds = Obj_DataAccess1.SaveOTP(Obj.OTPEmail, generatedCode);

                                    TempData["OTPEmail"] = email;
                                    TempData["Source"] = "OTPLogin";                              
                                    TempData["Title"] = "OTPLogin";

                                    return RedirectToAction("OTPVerification", "Account");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error checking email: " + ex.Message);
                            ViewBag.Message = "Error";
                            TempData["SwalMessage"] = "User account not checked";
                            TempData["SwalType"] = "warning";
                            return View();
                        }
                    }
                }
                else
                {
                    TempData["Email"] = "InvalidUser";
                    ViewBag.Message = "Invalid";
                    TempData["SwalMessage"] = "Please enter registered email to continue!";
                    TempData["SwalType"] = "warning";
                }
            }
            else
            {
                ViewBag.Message = "Please try again!";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        [HttpGet]
        public IActionResult AccountOTPLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ClearCookies()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> RequestOtp(OTPLoginModel model)
        {
            if (ModelState.IsValid)
            {

                string otp = _otpService.GenerateOtp();
                await _otpService.SendOtpByEmail(model.OTPEmail, otp);
                _otpService.SaveOtpToDatabase(model.OTPEmail, otp);

                return RedirectToAction("OTPVerification", new { email = model.OTPEmail });
            }

            return View("AccountOTPLogin", model);
        }

        [HttpGet]
        public IActionResult OTPVerification()
        {
            // Retrieve the title from TempData
            ViewBag.Title = TempData["Title"] as string;

            // Retrieve email and source if needed
            string email = TempData["OTPEmail"] as string;
            string source = TempData["Source"] as string;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Create the model and pass the retrieved data
            var model = new OtpVerificationModel
            {
                OTPEmail = email,
                Source = source
            };

            // Return the view with the model
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationModel model)
        {
            string source = model.Source ?? "OTPLogin";
            
            if (model == null || string.IsNullOrEmpty(model.Source))
            {
                return Json(new { success = false, message = "Invalid data received." });
            }

            bool isValid = _otpService.ValidateOtp(model.OTPEmail, model.OTP);

            if (isValid)
            {
                DataAccess objDataAccess = new DataAccess();
                DataSet ds = objDataAccess.OTPLogging(model);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int status = Convert.ToInt32(ds.Tables[0].Rows[0]["LStatus"]);
                    if (status == 1)
                    {
                        foreach (var cookie in Request.Cookies.Keys)
                        {
                            Response.Cookies.Delete(cookie);
                        }

                        var userData = ExtractUserData(ds);
                        SetTempData(userData);
                        SetCookies(userData);
                        await SetUserClaims(userData, ds);

                        TempData["SwalMessage"] = "Login Successful";
                        TempData["SwalType"] = "success";
                        string redirectUrl;
                        switch (model.Source)
                        {
                            case "PatForgot":
                                TempData["MyEmail"] = model.OTPEmail;
                                redirectUrl = Url.Action("PatReset", "Account");
                                break;
                            case "OTPLogin":
                                redirectUrl = Url.Action("Dashboard", "Report");
                                break;
                            default:
                                redirectUrl = Url.Action("Index", "Home");
                                break;
                        }

                        return Json(new { success = true, redirectUrl });


                      

                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid OTP." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "OTP logging failed." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid OTP." });
            }
        }







        private void SetTempData(Signup userData)
        {
            TempData["Username"] = userData.UserName;
            TempData["Email"] = userData.Email;
            TempData["Role"] = userData.RoleName;
            TempData["FirstName"] = userData.Firstname;
            TempData["RCode"] = userData.Rcode;
        }

        private void SetCookies(Signup userData)
        {
            var userDataJson = JsonConvert.SerializeObject(userData);
            var protectedData = _dataProtector.Protect(userDataJson);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("EncryptCookie", protectedData, cookieOptions);
            Response.Cookies.Append("UsrId", userData.UserId, cookieOptions);
            Response.Cookies.Append("UsrName", userData.UserName, cookieOptions);
            Response.Cookies.Append("Role", userData.RoleName, cookieOptions);
        }

        private async Task SetUserClaims(Signup userData, DataSet ds)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, userData.UserName),
        new Claim(ClaimTypes.Role, userData.RoleName),
        new Claim("UserId", userData.UserId)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            HttpContext.Session.SetString("FirstName", userData.Firstname);
            string jsonString = JsonConvert.SerializeObject(ds.Tables[1]);
            HttpContext.Session.SetString("MenuList", jsonString);
        }

        private Signup ExtractUserData(DataSet ds)
        {
            return new Signup
            {
                UserId = ds.Tables[0].Rows[0]["RId"].ToString(),
                UserName = ds.Tables[0].Rows[0]["Username"].ToString(),
                Email = ds.Tables[0].Rows[0]["Email"].ToString(),
                RoleName = ds.Tables[0].Rows[0]["Role"].ToString(),
                Firstname = ds.Tables[0].Rows[0]["Fname"].ToString(),
                Rcode = ds.Tables[0].Rows[0]["RCode"].ToString()
            };
        }







                [ValidateAntiForgeryToken]
                public IActionResult Logout()
                {
                    HttpContext.Session.Clear();
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    ViewBag.Message = "Logout";
                    TempData["SwalMessage"] = "User Logged out";
                    TempData["SwalType"] = "success";
                    // Redirect to the home page or another appropriate page
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    
