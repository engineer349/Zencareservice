using System.ComponentModel.DataAnnotations;

namespace Zencareservice.Models
{
    public class OTPLoginModel
    {
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string ? OTPEmail { get; set; }
    }
}
