using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Zencareservice.Models
{
    public class OtpService
    {
        private readonly string _connectionString;

        public OtpService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string SaveOtpToDatabase(string email, string otp)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO OtpCodes (Email, Otp, ExpiryTime) VALUES (@Email, @Otp, @ExpiryTime)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Otp", otp);
                    command.Parameters.AddWithValue("@ExpiryTime", DateTime.Now.AddMinutes(5)); // OTP valid for 5 minutes
                    command.ExecuteNonQuery();
                }
            }
            return otp;
        }

        public bool ValidateOtp(string OTPEmail, string OTP)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM OtpCodes WHERE Email = @Email AND Otp = @OTP AND ExpiryTime > @CurrentTime";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", OTPEmail);
                    command.Parameters.AddWithValue("@OTP", OTP);
                    command.Parameters.AddWithValue("@CurrentTime", DateTime.Now);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        public async Task SendOtpByEmail(string email, string otp)
        {
       
            
            var client = new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("zencareservice.noreply@gmail.com", "noiauctkdrybniwx"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("zencareservice.noreply@gmail.com"),
                Subject = "Your OTP Code",
                Body = $"Your OTP code is {otp}",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
           
        }
    }
}
