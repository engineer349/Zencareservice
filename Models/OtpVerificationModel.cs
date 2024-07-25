using System.ComponentModel.DataAnnotations;

namespace Zencareservice.Models
{
    public class OtpVerificationModel
    {
        [Required]
        [EmailAddress]
        public string ? OTPEmail { get; set; }

        [Required]
        [MaxLength(6)]
        public string ? OTP { get; set; }

       
        [MaxLength(6)]
        public string? ResendOTP { get; set; }
    }
}
