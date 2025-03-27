using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TAILorMadeLib.Model;

namespace TAILorMadeLib
{
    public class ResumeSummarizer
    {
        private static readonly string apiKey = Environment.GetEnvironmentVariable("MY_CHATGPT_KEY");
        private static readonly string endpoint = "https://api.openai.com/v1/chat/completions";

        public static async Task<string> SummarizeResume(string resumeText)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new Request
                {
                    model = "gpt-4o",
                    messages = new[]
                    {
                    new Message
                    {
                        role = "system",
                        content = "You are an expert AI that summarizes resumes into professional background blurbs for cover letters."
                    },
                    new Message
                    {
                        role = "user",
                        content = $"Summarize the following resume into a short professional paragraph suitable for use in a job application. Focus on the candidate's skills, experience, and accomplishments. Do not include section headers like 'Experience' or 'Education'. Avoid listing tools unless they are directly relevant. Make the tone professional but confident. Resume: {resumeText}"
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
                    return "Error summarizing resume.";
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(jsonResponse);
                return doc.RootElement
                          .GetProperty("choices")[0]
                          .GetProperty("message")
                          .GetProperty("content")
                          .GetString() ?? "No summary returned.";
            }
        }

        public static async Task<string> GetBackgroundForPrompt(string resumeText, bool summarize = true)
        {
            return summarize ? await SummarizeResume(resumeText) : resumeText;
        }
    }
}
