using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10384311PROG6212POE.Models.Entities
{
    public class Claims
    {
        // The Primary Key of the Claims Model
        [Key]
        public int ClaimId { get; set; }

        // Foreign Key to link with the Lecturer table
        [Required]
        public int LecturerId { get; set; } // Foreign Key

        [ForeignKey("LecturerId")]
        public Lecturer Lecturer { get; set; } // Navigation Property

        // The Claim Period Variable for the Lecturer
        [Required(ErrorMessage = "Claim period is required.")]
        public string ClaimPeriod { get; set; }

        // The Total Hours Variable for the Lecturer
        [Required(ErrorMessage = "Total hours worked is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total hours must be greater than zero.")]
        public int TotalHours { get; set; }

        // The Total Amount Variable for the Lecturer's Claim Total Amount
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        // The Hourly Rate for the Lecturer
        public int HourlyRate { get; set; }

        // The Supporting Documents URL Variable for the Lecturer's Supporting Documents 
        public string? SupportingDocsUrl { get; set; }

        // The Status Variable for the Lecturer's Claim Status
        public string? Status { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//