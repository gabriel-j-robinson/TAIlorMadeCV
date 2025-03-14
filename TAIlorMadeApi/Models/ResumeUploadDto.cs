using System.ComponentModel.DataAnnotations;

namespace TAIlorMadeApi.Models
{
    public class ResumeUploadDto
    {
        [Required]
        public required IFormFile ResumeFile { get; set; }
    }
}
