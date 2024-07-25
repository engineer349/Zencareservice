using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zencareservice.Models
{
    public class Login
    {

    

        [Required(ErrorMessage ="Pls Enter Username")]
        
        
        public string ?Username { get; set; }

        [Required(ErrorMessage = "Pls Enter Password")]
        public string? Password { get; set; }

        public string ?Email { get; set; }

        [Required(ErrorMessage = "Pls Enter OTP")]
        [MaxLength(5)]
        public string? ResendOTP { get; set; }



        public string? CRPassword { get; set; }
    }
}
