namespace TAIlorMadeApi.Models
{
    public class ResumeRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string ResumeText { get; set; }
    }
}
