using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace CapstoneProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        // Show all the Users
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> UserProfile()
        {
            // Get the currently logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                // Map user data to a UserProfile model
                var userProfile = new UserProfile
                {
                    UserId = currentUser.Id,
                    UserName = currentUser.UserName,
                    Email = currentUser.Email,
                    // Add other user-related properties as needed
                };

                // Pass the user profile model to the view
                return View("UserProfile", userProfile);
            }

            // If the user is not found, return an error or redirect to another page
            return NotFound();
        }


        // Retrieve user data by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Map user data to a UserProfileModel and return
                var userProfile = new UserProfile
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    // Add other user-related properties as needed
                };

                return Ok(userProfile);
            }

            return NotFound();
        }

        // Retrieve user activities, purchase history, etc. when needed
    }
}
