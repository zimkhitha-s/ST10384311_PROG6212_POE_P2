using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10384311PROG6212POE.Models.Entities
{
    public class Claims
    {
        // The Properties of the Claims Model
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public string LecturerName { get; set; }


        [Required]
        [EmailAddress]
        public string LecturerEmail { get; set; }

        [Required]
        public string ClaimPeriod { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalHours { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string? SupportingDocsUrl { get; set; }
        public string? Status { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//