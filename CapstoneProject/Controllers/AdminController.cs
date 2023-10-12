using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CapstoneProject.Data;

namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // Add a user to the Data base
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] IdentityUser user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email;
                user.EmailConfirmed = true;
                user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, user.PasswordHash);

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(new { Errors = result.Errors });
                }
            }

            return BadRequest(ModelState);
        }

        // Show all the Users
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }
    }
}

