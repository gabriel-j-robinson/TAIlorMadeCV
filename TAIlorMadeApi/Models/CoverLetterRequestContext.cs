using Microsoft.EntityFrameworkCore;

namespace TAIlorMadeApi.Models
{
    public class CoverLetterRequestContext : DbContext
    {
        public CoverLetterRequestContext(DbContextOptions<CoverLetterRequestContext> options) : base(options) { }

        public DbSet<CoverLetterRequest> CoverLetterRequests { get; set; }
    }
}
