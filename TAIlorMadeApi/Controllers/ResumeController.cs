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
        private readonly IWebHostEnvironment _environment;

        public ResumeController(ResumeRequestContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadDto uploadDto)
        {
            if (uploadDto.ResumeFile == null || uploadDto.ResumeFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Create uploads directory if not exists
            string uploadsPath = Path.Combine(_environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Generate unique file name
            string fileName = $"{Guid.NewGuid()}_{uploadDto.ResumeFile.FileName}";
            string filePath = Path.Combine(uploadsPath, fileName);

            // Save the file locally
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDto.ResumeFile.CopyToAsync(stream);
            }

            // Extract text
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

            // Store file info in DB
            var resumeRequest = new ResumeRequest
            {
                FileName = uploadDto.ResumeFile.FileName,
                FilePath = filePath,
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

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadResume(Guid id)
        {
            var request = await _context.ResumeRequests.FindAsync(id);
            if (request == null || !System.IO.File.Exists(request.FilePath))
            {
                return NotFound("File not found.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(request.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", request.FileName);
        }
    }

}
