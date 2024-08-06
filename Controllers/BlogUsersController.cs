using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogUsersController : ControllerBase
    {
        private readonly BlogAPIContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public BlogUsersController(BlogAPIContext context, UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //GET: api/Members oturum açmış kullanıcı bilgilerini döner
        [Authorize(Roles = "BlogUser")]
        [HttpGet("Self")]
        public async Task<ActionResult<BlogUser>> GetEmployeeSelf()
        {
            var loggedBlogUserName = User.FindFirstValue(ClaimTypes.Name);
            if (loggedBlogUserName == null)
            {
                return NotFound();
            }



            var blogUser = await _context.blogUsers.FirstOrDefaultAsync(e => e.UserName == loggedBlogUserName);


            if (blogUser == null)
            {
                return NotFound();
            }

            return blogUser;
        }

        // GET: api/BlogUsers
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogUser>>> GetblogUsers()
        {
            if (_context.blogUsers == null)
            {
                return NotFound();
            }
            return await _context.blogUsers.ToListAsync();
        }

        // GET: api/BlogUsers/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogUser>> GetBlogUser(string id)
        {
            if (_context.blogUsers == null)
            {
                return NotFound();
            }
            var blogUser = await _context.blogUsers.FindAsync(id);

            if (blogUser == null)
            {
                return NotFound();
            }

            return blogUser;
        }

        // PUT: api/BlogUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogUser(string id, BlogUser blogUser, string? currentPassword = null)
        {

            if (id != blogUser.Id)
            {
                return BadRequest();
            }


            if (currentPassword != null)
            {
                await _userManager.ChangePasswordAsync(blogUser, currentPassword, "NewPassword"); // Yeni şifreyi geçin
            }

            _context.Entry(blogUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogUserExists(id))
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

        // POST: api/BlogUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BlogUser>> PostBlogUser(BlogUser blogUser, string password)
        {
            if (_context.blogUsers == null)
            {
                return Problem("Entity set 'BlogAPIContext.blogUsers'  is null.");
            }

            await _userManager.CreateAsync(blogUser, password);
            await _userManager.AddToRoleAsync(blogUser, "BlogUser");



            _context.blogUsers.Add(blogUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BlogUserExists(blogUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBlogUser", new { id = blogUser.Id }, blogUser);
        }

        // DELETE: api/BlogUsers/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogUser(string id)
        {
            if (_context.blogUsers == null)
            {
                return NotFound();
            }
            var blogUser = await _context.blogUsers.FindAsync(id);
            if (blogUser == null)
            {
                return NotFound();
            }

            _context.blogUsers.Remove(blogUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogUserExists(string id)
        {
            return (_context.blogUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
