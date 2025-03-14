using Microsoft.EntityFrameworkCore;

namespace TAIlorMadeApi.Models
{
    public class ResumeRequestContext : DbContext
    {
        public ResumeRequestContext(DbContextOptions<ResumeRequestContext> options)
            : base(options) { }

        public DbSet<ResumeRequest> ResumeRequests { get; set; }
    }

}
