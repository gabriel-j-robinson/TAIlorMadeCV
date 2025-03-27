namespace TAIlorMadeApi.Models
{
    public class CoverLetterRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ResumeId { get; set; }
        public string JobDescription { get; set; }
        public string? CoverLetter { get; set; }
        public string Status { get; set; } = "Processing"; // Or "Completed", "Failed"
    }
}
