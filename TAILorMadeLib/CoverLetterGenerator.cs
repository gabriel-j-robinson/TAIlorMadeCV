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

        public static async Task<string> GenerateCoverLetter(
            string jobDescription,
            string userBackground,
            string githubUrl = null)
        {
            string result;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                string date = DateTime.Today.ToString("MMMM d, yyyy");

                string prompt = PromptBuilder.BuildCoverLetterPrompt(jobDescription, userBackground, githubUrl);

                var requestBody = new Request()
                {
                    model = "gpt-4o",
                    messages = new[]
                    {
                        new Message
                        {
                            role = "system",
                            content = "You are an AI specialized in writing tailored and professional cover letters. Avoid using placeholders if any information is missing."
                        },
                        new Message
                        {
                            role = "user",
                            content = prompt
                    }
                    }
                };

                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(error);
                    return "Error generating cover letter.";
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(jsonResponse);
                result = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

            }

            return result ?? "No content returned.";
        }
    }
}
