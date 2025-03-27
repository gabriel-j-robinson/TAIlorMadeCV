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

                var promptBuilder = new StringBuilder();

                promptBuilder.AppendLine("You are an expert AI assistant that writes professional, tailored cover letters for job applications.");
                promptBuilder.AppendLine();
                promptBuilder.AppendLine("Please analyze the job description below and extract any context that is explicitly available, such as:");
                promptBuilder.AppendLine("- Company name");
                promptBuilder.AppendLine("- Job title");
                promptBuilder.AppendLine("- Technologies or skills mentioned");
                promptBuilder.AppendLine();
                promptBuilder.AppendLine("Then write a professional, persuasive, and personalized cover letter for the job, using the candidate’s background.");
                promptBuilder.AppendLine();
                promptBuilder.AppendLine("**Important rules:**");
                promptBuilder.AppendLine("- If the job description does NOT mention the company name, recipient name, or address, DO NOT use placeholders like [Company Name] or [Hiring Manager’s Name].");
                promptBuilder.AppendLine("- Simply omit those sections or write naturally around them.");
                promptBuilder.AppendLine("- Do NOT add a letterhead with the applicant’s contact info — only generate the body of the letter and closing.");
                promptBuilder.AppendLine("- Do NOT repeat the job description verbatim.");
                promptBuilder.AppendLine("- Use a warm, confident, and professional tone.");
                promptBuilder.AppendLine();
                promptBuilder.AppendLine("Job Description:");
                promptBuilder.AppendLine(jobDescription);
                promptBuilder.AppendLine();
                promptBuilder.AppendLine("Candidate Background:");
                promptBuilder.AppendLine(userBackground);
                promptBuilder.AppendLine();
                promptBuilder.AppendLine("Date: " + DateTime.Today.ToString("MMMM d, yyyy"));

                if (!string.IsNullOrWhiteSpace(githubUrl))
                    promptBuilder.AppendLine("GitHub Profile: " + githubUrl);

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
                            content = promptBuilder.ToString()
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
