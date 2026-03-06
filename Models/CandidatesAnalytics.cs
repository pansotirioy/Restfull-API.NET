using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class CandidatesAnalytics
    {
        public int Id { get; set; }

        public string? TopicDescription { get; set; }

        public int AwardedMarks { get; set; }

        public int PossibleMarks { get; set; }

        //navigational property
        public List<Certificate>? Certificate { get; set; }

        public ICollection<Question>? Questions { get; set; }
    }
}
