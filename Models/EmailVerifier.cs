using System;
using System.Threading.Tasks;
using NeverBounce;

namespace Zencareservice.Models
{
    public class EmailVerifier
    {
        private readonly NeverBounceApiClient _client;

        public EmailVerifier(string apiKey)
        {
            _client = new NeverBounceApiClient(apiKey);
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            try
            {
                var response = await _client.SingleCheck(email);
                return response.Result == NeverBounce.Models.SingleCheckResponseResult.Valid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying email: {ex.Message}");
                return false;
            }
        }
    }
}
