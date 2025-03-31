namespace TAIlorMadeApi.Models
{
    public class CoverLetterRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ResumeId { get; set; }
        public string? JobDescription { get; set; }
        public string? CoverLetter { get; set; }
        public string? ResumeSummary { get; set; } // Editable summary
        public bool SummaryEditedByUser { get; set; } = false;
        public string Status { get; set; } = "PendingSummaryApproval";
    }
}
