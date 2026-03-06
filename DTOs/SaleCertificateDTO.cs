using Assignment.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class SaleCertificateDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }

        [Required]
        public string? AssessmentTestCode { get; set; }

        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }
       
    }
}
