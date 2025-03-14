namespace TAIlorMadeApi.Models
{
    public class ResumeRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } // Original file name
        public string FilePath { get; set; } // Location where it's stored
        public string? ResumeText { get; set; } // Optional extracted text
    }
}
