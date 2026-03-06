using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Candidate : IdentityUser
    {

        public string? FirstName { get; set; }

        public string MiddleName { get; set; } = string.Empty;

        public string? LastName { get; set; }

        public Gender Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? NativeLanguage { get; set; }


        //Navigational Property
        public Address? Address { get; set; }


        public PhotoId? PhotoId { get; set; }

       // public AppUser? AppUser { get; set; }

        public ICollection<Certificate>? Certificates { get; set; }

        public DateOnly? CreatedDate { get; set; }


    }
}