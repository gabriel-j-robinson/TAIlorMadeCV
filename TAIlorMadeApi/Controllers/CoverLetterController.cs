using Microsoft.AspNetCore.Mvc;
using TAIlorMadeApi.Models;

namespace TAIlorMadeApi.Controllers
{
    [ApiController]
    [Route("api/coverletters")]
    public class CoverLetterController
    {
        private readonly ResumeRequestContext _context;

        public CoverLetterController(ResumeRequestContext context)
        {
            _context = context;
        }
    }
}
