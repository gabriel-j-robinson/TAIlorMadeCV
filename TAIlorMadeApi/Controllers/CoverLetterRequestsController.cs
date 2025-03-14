//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TAIlorMadeApi.Models;

//namespace TAIlorMadeApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CoverLetterRequestsController : ControllerBase
//    {
//        private readonly CoverLetterRequestContext _context;

//        public CoverLetterRequestsController(CoverLetterRequestContext context)
//        {
//            _context = context;
//        }

//        // GET: api/CoverLetterRequests
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<CoverLetterRequest>>> GetCoverLetterRequests()
//        {
//            return await _context.CoverLetterRequests.ToListAsync();
//        }

//        // GET: api/CoverLetterRequests/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<CoverLetterRequest>> GetCoverLetterRequest(Guid id)
//        {
//            var coverLetterRequest = await _context.CoverLetterRequests.FindAsync(id);

//            if (coverLetterRequest == null)
//            {
//                return NotFound();
//            }

//            return coverLetterRequest;
//        }

//        // PUT: api/CoverLetterRequests/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutCoverLetterRequest(Guid id, CoverLetterRequest coverLetterRequest)
//        {
//            if (id != coverLetterRequest.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(coverLetterRequest).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!CoverLetterRequestExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/CoverLetterRequests
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<CoverLetterRequest>> PostCoverLetterRequest(CoverLetterRequest coverLetterRequest)
//        {
//            _context.CoverLetterRequests.Add(coverLetterRequest);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetCoverLetterRequest", new { id = coverLetterRequest.Id }, coverLetterRequest);
//        }

//        // DELETE: api/CoverLetterRequests/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCoverLetterRequest(Guid id)
//        {
//            var coverLetterRequest = await _context.CoverLetterRequests.FindAsync(id);
//            if (coverLetterRequest == null)
//            {
//                return NotFound();
//            }

//            _context.CoverLetterRequests.Remove(coverLetterRequest);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool CoverLetterRequestExists(Guid id)
//        {
//            return _context.CoverLetterRequests.Any(e => e.Id == id);
//        }
//    }
//}
