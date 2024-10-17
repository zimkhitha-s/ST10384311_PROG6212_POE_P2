using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10384311PROG6212POE.Models.Entities
{
    public class Claims
    {
        // The Properties of the Claims
        [Key]
        public int ClaimId { get; set; }  // Primary Key

        [Required(ErrorMessage = "Lecturer name is required.")]
        public string LecturerName { get; set; }

        [Required(ErrorMessage = "Lecturer email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string LecturerEmail { get; set; }

        [Required(ErrorMessage = "Claim period is required.")]
        public string ClaimPeriod { get; set; }

        [Required(ErrorMessage = "Total hours worked is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total hours must be greater than zero.")]
        public int TotalHours { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public string? SupportingDocsUrl { get; set; }

        public string? Status { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//