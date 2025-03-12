using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TAILorMadeLib.Model;
using System.Text.Json;

namespace TAILorMadeLib
{
    public class CoverLetterGenerator
    {
        private static readonly string apiKey = Environment.GetEnvironmentVariable("MY_CHATGPT_KEY");
        private static readonly string endpoint = "https://api.openai.com/v1/chat/completions";

        public static async Task<string> GenerateCoverLetter(string jobDescription, string userBackground)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new Request()
                {
                    model = "gpt-4o",
                    messages = new Message[]
                    {
                        new Message() { role = "system", content = "You are an AI specialized in writing professional cover letters." },
                        new Message() { role = "user", content = $"Write a personalized cover letter for a job with the following description: {jobDescription}. The candidate has the following background: {userBackground}" }
                    }
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = "";
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        result = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    }
                    return result;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorResponse);
                    return "Error generating cover letter.";
                }
            }
        }
    }
}
