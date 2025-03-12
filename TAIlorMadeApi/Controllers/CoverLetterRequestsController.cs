using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAIlorMadeApi.Models;
using TAILorMadeLib;

namespace TAIlorMadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverLetterRequestsController : ControllerBase
    {
        private readonly CoverLetterRequestContext _dbContext;

        public CoverLetterRequestsController(CoverLetterRequestContext context)
        {
            _dbContext = context;
        }

        [HttpPost("generate-async")]
        public IActionResult GenerateCoverLetterAsync([FromForm] IFormFile resumePdf, [FromForm] string jobDescription)
        {
            if (resumePdf == null || resumePdf.Length == 0)
                return BadRequest("Resume file is required.");

            string extractedText = TextExtractor.ExtractTextFromPdf(resumePdf);

            var request = new CoverLetterRequest
            {
                JobDescription = jobDescription,
                ResumeText = extractedText,
            };

            _dbContext.CoverLetterRequests.Add(request);
            _dbContext.SaveChanges();

            // Schedule background job
            Hangfire.BackgroundJob.Enqueue(() => ProcessCoverLetter(request.Id));

            return Ok(new { jobId = request.Id });
        }

        public async Task ProcessCoverLetter(Guid requestId)
        {
            var request = _dbContext.CoverLetterRequests.Find(requestId);
            if (request == null) return;

            try
            {
                request.CoverLetter = await CoverLetterGenerator.GenerateCoverLetter(request.JobDescription, request.ResumeText);
                request.Status = "Completed";
            }
            catch
            {
                request.Status = "Failed";
            }

            _dbContext.SaveChanges();
        }

        [HttpGet("status/{jobId}")]
        public IActionResult GetCoverLetterStatus(Guid jobId)
        {
            var request = _dbContext.CoverLetterRequests.Find(jobId);
            if (request == null)
                return NotFound(new { message = "Job not found" });

            return Ok(new { status = request.Status, coverLetter = request.CoverLetter });
        }
    }
}
