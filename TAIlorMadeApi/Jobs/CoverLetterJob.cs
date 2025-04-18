﻿using TAIlorMadeApi.Models;
using TAILorMadeLib;

namespace TAIlorMadeApi.Jobs
{
    public class CoverLetterJob
    {
        private readonly ResumeRequestContext _context;

        public CoverLetterJob(ResumeRequestContext context)
        {
            _context = context;
        }

        public async Task GenerateAsync(Guid requestId)
        {
            var request = await _context.CoverLetterRequests.FindAsync(requestId);
            if (request == null) return;

            if (request.Status != "Pending") return; 

            var resume = await _context.ResumeRequests.FindAsync(request.ResumeId);
            if (resume == null)
            {
                request.Status = "Failed";
                await _context.SaveChangesAsync();
                return;
            }

            request.Status = "Processing";
            await _context.SaveChangesAsync();

            try
            {
                string userBackground = request.ResumeSummary ?? "";

                var coverLetter = await CoverLetterGenerator.GenerateCoverLetter(
                    request.JobDescription,
                    userBackground
                );

                request.CoverLetter = coverLetter;
                request.Status = "Completed";
            }
            catch
            {
                request.Status = "Failed";
            }

            await _context.SaveChangesAsync();
        }
    }
}
