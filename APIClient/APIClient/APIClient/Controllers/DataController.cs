using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using APIClient.Helper;

namespace APIClient.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private IConfigurationRoot _config;
        private string BASE_URL;

        private string RSA_PRIVATE_KEY;

        public DataController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            BASE_URL = _config["DataService:rest_api_server_base_url"];
        }
       

        

        [HttpPost(Name = "dataService")]
        public async Task<IActionResult> DataService([FromQuery] string call, [FromBody] Dictionary<string, object> jsonData)
        {
            
            string responseBody = null;


            string token  = Utilities.GetRsaToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jsonContent = JsonSerializer.Serialize(jsonData);

            try
            {
                // Set Base Address and Port
                _httpClient.BaseAddress = new Uri(BASE_URL);

                using (HttpResponseMessage response = await _httpClient.PostAsync($"api/data/dataService?call={call}",
                    new StringContent(jsonContent, Encoding.UTF8, "application/json")))
                {
                    response.EnsureSuccessStatusCode(); // Ensure the response is successful
                    responseBody = await response.Content.ReadAsStringAsync(); // Read the response body
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}"); // Return exception message if there is an error
            }

            return Ok(responseBody); // Return the response as JSON
        }


    }

}
