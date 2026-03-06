using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class Questions
    {
        public int Id { get; set; }

        public string? Question { get; set; }

        public string? answer1 { get; set; }

        public string? answer2 { get; set; }

        public string? answer3 { get; set; }

        public string? answer4 { get; set; }

        public int? right { get; set; }

        public int CandidatesAnalyticsId { get; set; }
        public CandidatesAnalytics? candidatesAnalytics { get; set; }
    }
}
