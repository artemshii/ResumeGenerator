using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using ResumeGenerator.Data.Models;
using System.Text;
using System.Text.Json;
using ResumeGenerator.Data.Interfaces;

namespace ResumeGenerator.Data
{
    public class OpenAiIntegration: IOpenAiIntegration
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public OpenAiIntegration(IHttpClientFactory httpClientFactory, IConfiguration config)
        {

            _client = httpClientFactory.CreateClient();
            _config = config;

            SetUp();
            
        }
        private void SetUp()
        {


            _client.BaseAddress = new Uri("https://api.openai.com/");
            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer",
            _config["OpenAiToken"]);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");


        }

        async public Task<CompleteData> AiDataGetter(KeyWordsForAi _data)
        {
            string input = $@"
    Please generate detailed professional Resume data (make job and education descriptions realistic and polished, and add relevant professional skills and languages. All descriptions should be about the work i have done in this companies(not what is a company about), experience i have gained, i expect 4-6 sentences) using these keywords:
    {JsonSerializer.Serialize(_data)}

    Respond ONLY with valid JSON matching this exact C# DTO:

    {{
        ""ProfileSummary"": ""string"",
        ""JobTitle"": ""string"",
        ""Name"": ""string"",
        ""LinkedIn"": ""string"",
        ""Email"": ""string"",
        ""PhoneNumber"": ""string"",
        ""ProffesionalExperience"": [
            {{
                ""CompanyName"": ""string"",
                ""Position"": ""string"",
                ""Location"": ""string"",
                ""YearStart"": ""string"",
                ""YearEnd"": ""string or null"",
                ""CompanyDescription"": ""string""
            }}
        ],
        ""Education"": [
            {{
                ""InstitutionName"": ""string"",
                ""Degree"": ""string"",
                ""Location"": ""string"",
                ""YearStart"": ""string"",
                ""YearEnd"": ""string or null"",
                ""Description"": ""string""
            }}
        ],
        ""Skills"": [""string""],
        ""Languages"": [""string""]
    }}
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                input = input
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/responses", content);
            response.EnsureSuccessStatusCode();

            var deserialized = await response.Content.ReadFromJsonAsync<OpenAiResponse>();

            if(deserialized == null)
            {
                throw new Exception("Error occurred");
            }

            
            string jsonText = deserialized.output[0].content[0].text;

            // Remove triple backticks or leading text
            jsonText = jsonText.Trim();
            if (jsonText.StartsWith("```"))
            {
                int firstNewline = jsonText.IndexOf('\n');
                jsonText = jsonText.Substring(firstNewline + 1);
            }
            if (jsonText.EndsWith("```"))
            {
                jsonText = jsonText.Substring(0, jsonText.Length - 3);
            }

            // Now safely deserialize
            
            CompleteData finalData = JsonSerializer.Deserialize<CompleteData>(jsonText) ?? new CompleteData { };
            return finalData;


           
        }

    }
}
