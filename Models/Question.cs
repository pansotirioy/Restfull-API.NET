using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string? question { get; set; }

        public string? answer1 { get; set; }

        public string? answer2 { get; set; }

        public string? answer3 { get; set; }

        public string? answer4 { get; set; }

        public int? correct { get; set; }

        public int? CandidatesAnalyticsId { get; set; }
        public CandidatesAnalytics? candidatesAnalytics { get; set; }
    }
}
