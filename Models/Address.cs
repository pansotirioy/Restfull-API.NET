
namespace Assignment.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }

        public string? State { get; set; }

        public int PostalCode { get; set; }

        public string? Country { get; set; }

        public int LandlineNumber { get; set; }

        public string CandidateId { get; set; } = null!;
        //nagigational property
        public Candidate? Candidate { get; set; }
    }
}