using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zencareservice.Models
{
    public class Login
    {

        public int LoginId { get; set; }

        [Required(ErrorMessage ="Pls Enter Username")]
        
        
        public string ?Username { get; set; }

        [Required(ErrorMessage = "Pls Enter Password")]
        public string? Password { get; set; }

        public string ?Email { get; set; }

        [Required(ErrorMessage ="Pls Enter OTP")]
        [MaxLength(5)]
        public string ?ResendOTP{get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string OTPEmail { get; set; }

        public string? CRPassword { get; set; }
    }
}
