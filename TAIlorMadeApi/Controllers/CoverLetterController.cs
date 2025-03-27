using System.Net;
using Hangfire;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TAIlorMadeApi.Jobs;
using TAIlorMadeApi.Models;

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

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCoverLetter([FromServices] IBackgroundJobClient backgroundJobs, [FromBody] GenerateCoverLetterDto dto)
        {
            var resume = await _context.ResumeRequests.FindAsync(dto.ResumeId);
            if (resume == null)
            {
                return NotFound();
            }

            var request = new CoverLetterRequest
            {
                ResumeId = dto.ResumeId,
                JobDescription = dto.JobDescription,
                Status = "Pending"
            };

            _context.CoverLetterRequests.Add(request);
            await _context.SaveChangesAsync();

            backgroundJobs.Enqueue<CoverLetterJob>(job => job.GenerateAsync(request.Id));

            return AcceptedAtAction(nameof(Get), new { id = request.Id }, request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = await _context.CoverLetterRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }
    }
}
