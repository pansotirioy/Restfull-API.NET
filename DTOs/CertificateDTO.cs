using Assignment.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class CertificateDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }

        [Required]
        public string? AssessmentTestCode { get; set; }

        public string? Description { get; set; }

        public DateOnly ExaminationDate { get; set; }
        
        public DateOnly ScoreReportDate { get; set; }

        public int CandidateScore { get; set; }

        public int MaximumScore { get; set; }

        public int PercentageScore { get; set; }

        public bool AssessmentResultLabel { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string? CandidateId { get; set; }

    }
}
