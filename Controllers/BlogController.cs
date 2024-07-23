using Microsoft.AspNetCore.Mvc;
using FortressTrialTask_JanJeffersonLam.Models;
using FortressTrialTask_JanJeffersonLam.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FortressTrialTask_JanJeffersonLam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPost([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.DatePosted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPost = await _context.Posts.CountAsync();

            var response = new
            {
                TotalPost = totalPost,
                Post = post
            };
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost (int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] BlogPost post)
        {
            if (post == null)
            {
                return BadRequest("Blog post data is null.");
            }

            if (post.UserId <= 0)
            {
                return BadRequest("UserId is required.");
            }

            var user = await _context.Users.FindAsync(post.UserId);
            if (user == null)
            {
                return BadRequest("Invalid UserId.");
            }

            post.DatePosted = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }





        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] BlogPost post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound(); 
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool PostExist(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        

    }


}
