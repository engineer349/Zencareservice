using Newtonsoft.Json.Linq;

namespace Zencareservice.Models
{
	public class EmailVerifier
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public EmailVerifier(string apiKey)
		{
			_apiKey = apiKey;
			_httpClient = new HttpClient();
		}

		public async Task<bool> VerifyEmailAsync(string email)
		{
			try
			{
				var requestUri = $"https://api.kickbox.com/v2/verify?email={email}&apikey={_apiKey}";
				var response = await _httpClient.GetAsync(requestUri);
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var jsonResponse = JObject.Parse(content);
				Console.WriteLine(content);

				// Check if the email is deliverable
				return jsonResponse["result"]?.ToString() == "deliverable";
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error verifying email: {ex.Message}");
				return false;
			}
		}
	}
}
