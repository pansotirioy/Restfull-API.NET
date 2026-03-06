using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }

        [Required]
        public string? question { get; set; }

        [Required]
        public string? answer1 { get; set; }

        [Required]
        public string? answer2 { get; set; }

        [Required]
        public string? answer3 { get; set; }

        [Required]
        public string? answer4 { get; set; }

        [Required]
        public int? correct { get; set; }

        public int CandidatesAnalyticsId { get; set; }
    }
}
