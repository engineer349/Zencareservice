using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Template;
using System.Data;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;

using Zencareservice.Models;
using Zencareservice.Repository;
using System;
using System.IO;
using System.Net;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;
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

namespace Zencareservice.Controllers
{

    public class AccountController : Controller
    {

        //private readonly TwilioService _twilioService;
        private readonly IDataProtector _dataProtector;

        //private readonly UserManager<Signup> _userManager;
        //private readonly SignInManager<Signup> _signInManager;

        //public AccountController(UserManager<Signup> userManager,
        //                              SignInManager<Signup> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}

        private int _generatedOtp;

        private int _regeneratedOtp; 

        private string? ResetEmail;

        private readonly DataAccess _dataaccess;

        private readonly SqlDataAccess _sqldataaccess;

        public AccountController(DataAccess dataaccess, SqlDataAccess sqldataaccess, IDataProtectionProvider dataProtectionProvider)
        {
            _dataaccess = dataaccess;
            _sqldataaccess = sqldataaccess;
            _dataProtector = dataProtectionProvider.CreateProtector("YourPurpose");
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
            string returnUrl = "/Account/UserRegsiter";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Register()
        {
            string returnUrl = "/Account/Register";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult PatientRegister()
        {
            string returnUrl = "/Account/PatientRegister";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Login()
        {
            string returnUrl = "/Account/Login";
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

        public IActionResult PatReset()
        {
            string returnUrl = "/Account/PatReset";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult ResetPassword()
        {
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

        public IActionResult OTPVerification()
        {
            string otp = TempData["OTP"] as string;

            if (string.IsNullOrEmpty(otp))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Store OTP in ViewBag to pass it to the view
            ViewBag.OTP = otp;

            HttpContext.Session.SetString("AccessDenied", DateTime.Now.AddMinutes(1).ToString());

            return RegistrationSuccess();
        }

        public IActionResult OTPLogin( )
        {
			string returnUrl = "/Account/OTPLogin";
			ViewData["ReturnUrl"] = returnUrl;
            return View();

		}
        public IActionResult VerifyOtp()
        {
            string gotp = Request.Cookies["OTP"];

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
        public IActionResult ValidateResetOtp( Login model)
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
                        return Json(new { success = false , redirectUrl = Url.Action("OTPVerification", "Account") });
                        
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

                    if(enteredOtp !="" )
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

									string UsrId = ds.Tables[0].Rows[0]["RId"].ToString();

									string UserName = ds.Tables[0].Rows[0]["Username"].ToString();

									TempData["Username"] = UserName;

									string Email = ds.Tables[0].Rows[0]["Email"].ToString();

									TempData["Email"] = Email;

									string Role = ds.Tables[0].Rows[0]["Role"].ToString();

									TempData["Role"] = Role;

									string Fname = ds.Tables[0].Rows[0]["Fname"].ToString();

									TempData["FirstName"] = Fname;

									string RCode = ds.Tables[0].Rows[0]["RCode"].ToString();

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

        private List<Signup> roles = new List<Signup>
        {
           
            new Signup { RoleId = "Doctor", RoleName = "Doctor" },
            new Signup {RoleId = "Staff", RoleName = "Staff"},
        };

        [HttpPost]
        public IActionResult ResetPassword(Signup Obj)
        {
			string ResetPassword = Obj.RPassword;
			string ConfirmResetPassword = Obj.CRPassword;

			if (TempData.TryGetValue("MyEmail", out var resetEmailObj) && resetEmailObj != null)
			{


				string email = resetEmailObj.ToString();


				if (!string.IsNullOrEmpty(ResetPassword) && !string.IsNullOrEmpty(ConfirmResetPassword))
				{
					DataAccess Obj_DataAccess = new DataAccess();
					DataSet ds = new DataSet();
					ds = Obj_DataAccess.ResetPassword(Obj, email);

				}
				ViewBag.Message = "UpdatedPassword";
				TempData["SwalMessage"] = "Successfully Updated";
				TempData["SwalType"] = "success";
				return RedirectToAction("Login", "Account");
			}
			else
			{

				return View();
			}
		}
        [HttpPost]
        public IActionResult PatReset(Signup Obj)
        {
            string ResetPassword = Obj.RPassword;
            string ConfirmResetPassword = Obj.CRPassword;

            if (TempData.TryGetValue("MyEmail", out var resetEmailObj) && resetEmailObj != null)
            {
                string email = resetEmailObj.ToString();


                if (!string.IsNullOrEmpty(ResetPassword) && !string.IsNullOrEmpty(ConfirmResetPassword))
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.ResetPassword(Obj, email);

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
        [HttpPost]
        public IActionResult ForgotPassword(Signup Obj)
        {
			string ResetEmail = Obj.Email;
			if (ResetEmail != null)
			{
				var gMail = IsEmailAccountValid("gmail-smtp-in.l.google.com", Obj.Email);

				if (gMail == true)
				{
					DataAccess Obj_DataAccess = new DataAccess();
					DataSet ds = new DataSet();
					ds = Obj_DataAccess.CheckEmail(Obj);
					if (ds.Tables[0].Rows.Count > 0)
					{
						string validemail = Obj.Email;
						TempData["MyEmail"] = validemail;
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
						}
						return RedirectToAction("OTPVerification", "Account");
					}
					else
					{
						ViewBag.Message = "InvalidEmail";
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
        public IActionResult PatForgot(Signup Obj)
        {
            string ResetEmail = Obj.Email;
            if (ResetEmail != null)
            {
                var gMail = IsEmailAccountValid("gmail-smtp-in.l.google.com", Obj.Email);

                if (gMail == true)
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet ds = new DataSet();
                    ds = Obj_DataAccess.CheckEmail(Obj);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string validemail = Obj.Email;
                        TempData["MyEmail"] = validemail;
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

                string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.Email, "fhshxafzjysuwxjw", "Autoverification", "Your Zencareservice  Account  OTP verification of Email is " + +_regeneratedOtp, "smtp.gmail.com", 587);

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

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            else
            {

                try
                {
                    // Normalize the domain
                    email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));

                    // Examines the domain part of the email and normalizes it.
                    string DomainMapper(Match match)
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        var idn = new IdnMapping();

                        // Pull out and process domain name (throws ArgumentException on invalid)
                        string domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                }
                catch (RegexMatchTimeoutException e)
                {
                    return false;
                }
                catch (ArgumentException e)
                {
                    return false;
                }

                try
                {
                    return Regex.IsMatch(email,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
        }

        private string Codegenerator()
        {
            Random random = new Random();
            int randomCode = random.Next(10000, 100000);
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
                using (TcpClient tClient = new TcpClient(smtpServer,25))
                using (NetworkStream netStream = tClient.GetStream())
                using (StreamReader reader = new StreamReader(netStream))
                {
                    string CRLF = "\r\n";
                    byte[] dataBuffer;
                    string responseString;

                    /* Perform HELO to SMTP Server and get Response */
                    dataBuffer = BytesFromString("HELO Hi" + CRLF);
                    netStream.Write(dataBuffer, 0, dataBuffer.Length);
                    responseString = reader.ReadLine();
                    // Check response for success

                    dataBuffer = BytesFromString($"MAIL FROM:<zencareservice.noreply@gmail.com>" + CRLF);
                    netStream.Write(dataBuffer, 0, dataBuffer.Length);
                    responseString = reader.ReadLine();
                    // Check response for success

                    /* Read Response of the RCPT TO Message to know from google if it exists or not */
                    dataBuffer = BytesFromString($"RCPT TO:<{emailAddress}>" + CRLF);
                    netStream.Write(dataBuffer, 0, dataBuffer.Length);
                    responseString = reader.ReadLine();
                    var responseCode = GetResponseCode(responseString);

                    if (responseCode == 550)
                    {
                        return false;
                    }

                    /* QUIT CONNECTION */
                    dataBuffer = BytesFromString("QUIT" + CRLF);  // Correct typo "QUITE"
                    netStream.Write(dataBuffer, 0, dataBuffer.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log exception details for debugging
                Console.WriteLine("Error checking email: " + ex.Message);
                return false;
            }
        }

       
        [HttpPost]
        public IActionResult PatientRegister(Signup Obj, string returnUrl)
        {

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
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
                                    string lname = Obj.Lastname;

                                    string password = Obj.Password;
                               
                                    TempData["Password"] = password;

                                    string confirmpassword = Obj.Confirmpassword;

                                    string username = Obj.Username;
									TempData["User"] = username;
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

        [HttpPost]
        public IActionResult Register(Signup Obj, string returnUrl)
        {

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
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

        [HttpPost]
        public IActionResult UserRegister(Signup Obj, string returnUrl, DateTime userDob)
        {
            

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {

                ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");

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



                                    //Console.WriteLine($"Gmail account is valid - {gMail.ToString()}");

                                    // var live = IsEmailAccountValid("live-com.olc.protection.outlook.com", "aa.aa@live.com");
                                    //Console.WriteLine($"Live account is valid - {live.ToString()}");


                                    string SelectedRoleId = Obj.RoleId;
                                    Obj.Age = userAge;
                                    
                                    if(Obj.RoleId == "Doctor")
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
                                    string fname = Obj.Firstname;
                                    string lname = Obj.Lastname;
                                    string password = Obj.Password;
                                    string confirmpassword = Obj.Confirmpassword;                                 
                                    string phoneno = Obj.Phonenumber;

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
                                    string fname = Obj.Firstname;
                                    string lname = Obj.Lastname;
                                    string password = Obj.Password;
                                    string confirmpassword = Obj.Confirmpassword;
                                    string username = Obj.Username;
                                    string phoneno = Obj.Phonenumber;

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
            string FName = Obj.Firstname;
            SendMail sendMail = new SendMail();
            SmtpClient client = new SmtpClient();

            string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.Email, "fhshxafzjysuwxjw", "Autoverification", $@"



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
                                <p>Thank you for using Zenhealthcareservice! </p>
                                <p>To ensure the security of your account, we have generated a One-Time Password (OTP) for you.</p>
                                <p class=""verification-code"">Your Verification Code:{_generatedOtp}</p>
                                <p class=""verification-instructions"">Please use the above code to verify your email address.</p>
        
                            </div>
                        </body>
                        </html>", "smtp.gmail.com", 587);
            return mail;

        }

		private string SendingOTPEmail(Login Obj)
		{
			string FName = Obj.OTPEmail;
			SendMail sendMail = new SendMail();
			SmtpClient client = new SmtpClient();

			string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.OTPEmail, "fhshxafzjysuwxjw", "2FAuthentication", $@"



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
                                <p>Thank you for using Zenhealthcareservice! </p>
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

			string mail = sendMail.EmailSend("zencareservice.noreply@gmail.com", Obj.Email, "fhshxafzjysuwxjw", "ResetEmailVerification", $@"



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
                                <p>Thank you for using Zenhealthcareservice! </p>
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



        [HttpPost]
		public async Task<IActionResult> Login(Login Obj)
        {
            
             string username = Obj.Username;

             string password = Obj.Password;

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
                        
                        string UsrId= ds.Tables[0].Rows[0]["RId"].ToString();

                        string UserName = ds.Tables[0].Rows[0]["Username"].ToString();

                        TempData["Username"] = Obj.Username;


                        string Email = ds.Tables[0].Rows[0]["Email"].ToString();

                        TempData["Email"] = Email;

                        string Role = ds.Tables[0].Rows[0]["Role"].ToString();

                        TempData["Role"] = Role;

                        string Fname = ds.Tables[0].Rows[0]["Fname"].ToString();

                        TempData["FirstName"] =  Fname;

                        string RCode = ds.Tables[0].Rows[0]["RCode"].ToString();

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

						var claims = new[]
					   {
						new Claim(ClaimTypes.Name, username),
						new Claim(ClaimTypes.Role, Role),
						new Claim("UserId", UsrId)
					};

						var identity = new ClaimsIdentity(claims, "login");

						var principal = new ClaimsPrincipal(identity);

						await HttpContext.SignInAsync(principal);


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

        [HttpPost]
        public IActionResult OTPLogin(Login Obj, string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {

                string email = Obj.OTPEmail;

                TempData["OTPuser"] = email;

                var gMail = IsEmailAccountValid("gmail-smtp-in.l.google.com", Obj.OTPEmail);

                if (gMail == true)
                {
                    DataAccess Obj_DataAccess = new DataAccess();
                    DataSet dse = new DataSet();
                    dse = Obj_DataAccess.CheckLoginEmail(Obj);


                    if (dse.Tables[0].Rows.Count != 0 && dse.Tables[1].Rows.Count != 0)
                    {

                        try
                        {
                            string validemail = Obj.OTPEmail;
                            TempData["MyEmail"] = validemail;

                            if (!String.IsNullOrEmpty(validemail))
                            {

                                string generatedCode = Codegenerator();

                                _generatedOtp = Convert.ToInt32(generatedCode);
                                CookieOptions options = new CookieOptions();
                                options.Expires = DateTime.Now.AddMinutes(5);
                                Response.Cookies.Append("OTP", generatedCode, options);

                                SendingOTPEmail(Obj);

                                 ViewBag.Message = "otpgenerated";
                                TempData["SwalMessage"] = "OTP sent";
                                TempData["SwalType"] = "success"; 
 
                                return RedirectToAction("VerifyOtp", "Account");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error checking email: " + ex.Message);
                            ViewBag.Message = "User is not created!";
                                ViewBag.Message = "notchecked";
                                TempData["SwalMessage"] = "Useraccount not checked";
                                TempData["SwalType"] = "success";
                            return View();
                        }
                    }
                }
                else
                {
                    TempData["Email"] = "InvalidUser";

                     ViewBag.Message = "Invalid";
                     TempData["SwalMessage"] = "Pls Enter Registered Email to Continue!";
                    TempData["SwalType"] = "warning";

                   
                }


            }
            else
            {
				ViewBag.Message = "Pls tryagain!";
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[ValidateAntiForgeryToken]
		public IActionResult Logout()
		{
          
            HttpContext.Session.Clear();
            // Clear authentication cookies
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    ViewBag.Message = "Logout";
                     TempData["SwalMessage"] = "User Logged out";
                    TempData["SwalType"] = "success";
			// Redirect to the home page or another appropriate page
			return RedirectToAction("Index", "Home");
		}
	}
}
