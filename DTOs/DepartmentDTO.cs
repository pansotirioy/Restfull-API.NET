
using System.ComponentModel.DataAnnotations;

namespace Assignment.DTOs
{
    public class DepartmentDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;
        [Required]
        [MaxLength(13)]
        public string Phone { get; set; } = default!;
    }
}
