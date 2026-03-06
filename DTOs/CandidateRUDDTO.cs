
using Assignment.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{

    public class CandidateRUDDTO
    {
        public string? Id { get; set; }

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

        [Required]
        public string? PhoneNumber { get; set; }


    }
}
