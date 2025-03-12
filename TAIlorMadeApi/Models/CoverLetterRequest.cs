namespace TAIlorMadeApi.Models
{
    public class CoverLetterRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string JobDescription { get; set; }
        public string ResumeText { get; set; }
        public string CoverLetter { get; set; } // Initially null
        public string Status { get; set; } = "Processing"; // "Processing", "Completed", "Failed"
    }
}
