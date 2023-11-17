using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CapstoneProject.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }

        // Basic user information
        public string UserName { get; set; }
        public string Email { get; set; }

        // Additional user details
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Roles assigned to the user
        public List<string> Roles { get; set; }

        // Add more user properties as needed
    }
}
