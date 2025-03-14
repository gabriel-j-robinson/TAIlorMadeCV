using Microsoft.AspNetCore.Mvc;
using TAIlorMadeApi.Models;
using TAILorMadeLib;

namespace TAIlorMadeApi.Controllers
{
    [ApiController]
    [Route("api/resumes")]
    public class ResumeController : ControllerBase
    {
        private readonly ResumeRequestContext _context;

        public ResumeController(ResumeRequestContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadDto uploadDto)
        {
            if (uploadDto.ResumeFile == null || uploadDto.ResumeFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Extract text from the file
            string extractedText;

            try
            {
                extractedText = TextExtractor.ExtractTextFromPdf(uploadDto.ResumeFile);
            }
            catch (UnsupportedFileException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadFileException ex) 
            {
                return BadRequest(ex.Message);
            }

            // Save to DB
            var resumeRequest = new ResumeRequest
            {
                ResumeText = extractedText
            };

            _context.ResumeRequests.Add(resumeRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResumeRequest), new { id = resumeRequest.Id }, resumeRequest);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResumeRequest(Guid id)
        {
            var request = await _context.ResumeRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
    }
}
