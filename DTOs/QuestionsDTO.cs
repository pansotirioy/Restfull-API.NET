using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.DTOs
{
    public class Questions
    {
        public int Id { get; set; }

        [Required]
        public string? Question { get; set; }

        [Required]
        public string? answer1 { get; set; }

        [Required]
        public string? answer2 { get; set; }

        [Required]
        public string? answer3 { get; set; }

        [Required]
        public string? answer4 { get; set; }

        [Required]
        public int? right { get; set; }

        [Required]
        public int CandidatesAnalyticsId { get; set; }
    }
}
