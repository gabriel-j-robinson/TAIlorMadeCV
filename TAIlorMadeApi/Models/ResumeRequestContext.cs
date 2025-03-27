using System.Security.Policy;
using Microsoft.EntityFrameworkCore;

namespace TAIlorMadeApi.Models
{
    public class ResumeRequestContext : DbContext
    {
        public ResumeRequestContext(DbContextOptions<ResumeRequestContext> options)
            : base(options) { }

        public DbSet<ResumeRequest> ResumeRequests { get; set; }

        public DbSet<CoverLetterRequest> CoverLetterRequests { get; set; }
    }

}
