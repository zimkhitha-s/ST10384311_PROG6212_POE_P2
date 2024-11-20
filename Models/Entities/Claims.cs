using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10384311PROG6212POE.Models.Entities
{
    public class Claims
    {
        [Key]
        public int ClaimId { get; set; }

        // Foreign Key to link with Lecturer (inherits EmployeeId as primary key)
        [Required]
        public int LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public Lecturer Lecturer { get; set; } // Navigation Property

        [Required(ErrorMessage = "Claim period is required.")]
        public string ClaimPeriod { get; set; }

        [Required(ErrorMessage = "Total hours worked is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total hours must be greater than zero.")]
        public int TotalHours { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public int HourlyRate { get; set; }

        public string? SupportingDocsUrl { get; set; }

        public string? Status { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//