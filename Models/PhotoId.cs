using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public enum PhoteId
    {
        Natiaonal_Card,
        Passport,
        Driving_License
    }
    public class PhotoId
    {
        public int Id { get; set; }

        public PhoteId PhotoIdImage { get; set; }

        public int PhotoIdNumber { get; set; }

        public DateOnly DateOfIssue { get; set; }

        public string? CandidateId { get; set; }
        //Nagigational Property
        public Candidate? Candidate { get; set; }
    }
}