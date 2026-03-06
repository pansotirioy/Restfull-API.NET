

using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class CandidatesAnalyticsDTO
    {
        public int Id { get; set; }

        public string? TopicDescription { get; set; }

        public int AwardedMarks { get; set; }

        public int PossibleMarks { get; set; }

        [Required]
        public int CertificateId { get; set; }

    }
}
