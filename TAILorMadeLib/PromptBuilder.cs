using System;
using System.Text;

namespace TAILorMadeLib
{
    public class PromptBuilder
    {
        public static string BuildCoverLetterPrompt(
            string jobDescription,
            string userBackground,
            string githubUrl = null)
        {
            var builder = new StringBuilder();

            builder.AppendLine("You are an expert AI assistant that writes professional, tailored cover letters for job applications.");
            builder.AppendLine();
            builder.AppendLine("Please analyze the job description below and extract any context that is explicitly available, such as:");
            builder.AppendLine("- Company name");
            builder.AppendLine("- Job title");
            builder.AppendLine("- Technologies or skills mentioned");
            builder.AppendLine();
            builder.AppendLine("Then write a professional, persuasive, and personalized cover letter for the job, using the candidate’s background.");
            builder.AppendLine();
            builder.AppendLine("**Important rules:**");
            builder.AppendLine("- If the job description does NOT mention the company name, recipient name, or address, DO NOT use placeholders like [Company Name] or [Hiring Manager’s Name].");
            builder.AppendLine("- Simply omit those sections or write naturally around them.");
            builder.AppendLine("- Do NOT add a letterhead with the applicant’s contact info — only generate the body of the letter and closing.");
            builder.AppendLine("- Do NOT repeat the job description verbatim.");
            builder.AppendLine("- Use a warm, confident, and professional tone.");
            builder.AppendLine();
            builder.AppendLine("Job Description:");
            builder.AppendLine(jobDescription);
            builder.AppendLine();
            builder.AppendLine("Candidate Background:");
            builder.AppendLine(userBackground);
            builder.AppendLine();
            builder.AppendLine("Date: " + DateTime.Today.ToString("MMMM d, yyyy"));

            if (!string.IsNullOrWhiteSpace(githubUrl))
            {
                builder.AppendLine("GitHub Profile: " + githubUrl);
            }

            return builder.ToString();
        }
    }
}
