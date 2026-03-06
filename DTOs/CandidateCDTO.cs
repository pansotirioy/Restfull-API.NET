
using Assignment.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public enum Gender
    {
        Male,
        Female
    }

    public class CandidateCDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(20)]
        public string? FirstName { get; set; }

        [MaxLength(20)]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string? LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string? NativeLanguage { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = string.Empty;


    }
}
