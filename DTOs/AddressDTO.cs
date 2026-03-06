
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Street { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        [Range(0, 99999)]
        public int PostalCode { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public int LandlineNumber { get; set; }

        [Required]
        public string? CandidateId { get; set; }
    }
}
