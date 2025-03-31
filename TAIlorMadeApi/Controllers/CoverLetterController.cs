using System.Net;
using Hangfire;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TAIlorMadeApi.Jobs;
using TAIlorMadeApi.Models;
using TAILorMadeLib;

namespace TAIlorMadeApi.Controllers
{
    [ApiController]
    [Route("api/coverletters")]
    public class CoverLetterController : ControllerBase
    {
        private readonly ResumeRequestContext _context;

        public CoverLetterController(ResumeRequestContext context)
        {
            _context = context;
        }

        [HttpPost("generate-summary")]
        public async Task<IActionResult> GenerateSummary([FromBody] GenerateCoverLetterDto dto)
        {
            var resume = await _context.ResumeRequests.FindAsync(dto.ResumeId);

            if (resume == null) return NotFound();

            var summary = await ResumeSummarizer.GetBackgroundForPrompt(resume.ResumeText);
            var request = new CoverLetterRequest
            {
                ResumeId = dto.ResumeId,
                JobDescription = dto.JobDescription,
                ResumeSummary = summary,
                Status = "PendingSummaryApproval"
            };

            _context.CoverLetterRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpPut("{id}/approve-summary")]
        public async Task<IActionResult> ApproveOrEditSummary(Guid id, [FromBody] string editedSummary)
        {
            var request = await _context.CoverLetterRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.ResumeSummary = editedSummary;
            request.SummaryEditedByUser = true;

            request.Status = "Pending";

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = await _context.CoverLetterRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPost("{id}/regenerate-summary")]
        public async Task<IActionResult> RegenerateSummary(Guid id)
        {
            var request = await _context.CoverLetterRequests.FindAsync(id);

            if (request == null) return NotFound();

            var resume = await _context.ResumeRequests.FindAsync(request.ResumeId);

            if (request == null || resume == null) return NotFound();

            var summary = await ResumeSummarizer.GetBackgroundForPrompt(resume.ResumeText);

            request.ResumeSummary = summary;
            request.SummaryEditedByUser = false;
            await _context.SaveChangesAsync();

            return Ok(summary);
        }


        [HttpPost("{id}/generate")]
        public async Task<IActionResult> EnqueueCoverLetterGeneration(Guid id, [FromServices] IBackgroundJobClient backgroundJobs)
        {
            var request = await _context.CoverLetterRequests.FindAsync(id);
            if (request == null) return NotFound();

            if (request.Status != "Pending")
                return BadRequest("Cover letter is not ready to be generated. Please approve the summary first.");

            backgroundJobs.Enqueue<CoverLetterJob>(job => job.GenerateAsync(request.Id));

            return Accepted(request);
        }
    }
}
