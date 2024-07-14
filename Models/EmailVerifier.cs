using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json.Linq;

namespace Zencareservice.Models
{
    public class EmailVerifier
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public EmailVerifier(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            var client = _httpClientFactory.CreateClient();
            var requestUri = $"https://api.zerobounce.net/v2/validate?api_key={_apiKey}&email={email}";

            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error verifying email.");
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(content);

            var status = jsonResponse["status"]?.ToString();
            return status == "valid";
        }
    }

    }
