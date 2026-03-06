
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class AppUserDTO 
    {

        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set;} = string.Empty;

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = string.Empty;

        public int? CandidateId { get; set; } = null;

    }
}
