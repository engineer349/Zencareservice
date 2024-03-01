using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;
using Zencareservice.Models;

namespace Zencareservice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult PHoto()
        {
            return View();
        }


        public IActionResult CapturePhoto()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Index(string user)
        {
            user = "Hi ZencareUser!";

            if (!string.IsNullOrEmpty(user))
            {
                ViewBag.JavaScriptFunction = string.Format("ShowGreetings('{0}');", user);
            }
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }


        public IActionResult SendEmail()
        {

            return View();

        }

        [HttpPost]
        public IActionResult SendEmail(string name, string email, string subject, string message)
        {
            try
            {
                // Set up SMTP client
                using (SmtpClient client = new SmtpClient("smtp.yourprovider.com"))
                {
                    // Set credentials if needed
                    client.Credentials = new NetworkCredential("your-email@yourprovider.com", "your-password");

                    // Create the email message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("your-email@yourprovider.com");
                    mailMessage.To.Add("zenhealthcareservice@gmail.com"); // Change this to the recipient's email address
                    mailMessage.Subject = subject;
                    mailMessage.Body = $"Name: {name}\nEmail: {email}\n\n{message}";

                    // Send the email
                    client.Send(mailMessage);
                }

                // Message sent successfully
                ViewBag.Message = "Your message has been sent. Thank you!";
                return View();
            }
            catch (Exception ex)
            {
                // Error occurred while sending email
                ViewBag.Error = $"Error: {ex.Message}";
                return View();
            }
        }







        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Detail = exceptionDetails?.Error.Message,
                Status = 500  //Internal Server Error
                              //Add other properties as needed
            };
            return View("Error", problemDetails);

        }
    }
}
