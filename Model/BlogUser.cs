using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Model
{
	public class BlogUser : IdentityUser
    {
        public long IdNumber { get; set; }
        public string Name { get; set; } = "";
        public string? MiddleName { get; set; }
        public string? FamilyName { get; set; }
        public string Adddress { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public DateTime RegisterDate { get; set; }

        public string Title { get; set; } = "";

        [NotMapped]
        public string? Password { get; set; }
        [NotMapped]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }

        public ICollection<Post>? Posts { get; set; }

       
    }
	
}


