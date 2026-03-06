
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public enum PhoteId
    {
        Natiaonal_Card,
        Passport,
        Driving_License
    }
    public class PhotoIdDTO
    {
        public int Id { get; set; }

        [Required]
        public PhoteId PhotoIdImage { get; set; }

        [Required]
        public int PhotoIdNumber { get; set; }

        [Required]
        public DateOnly DateOfIssue { get; set; }

        [Required]
        public string? CandidateId { get; set; }

    }
}
