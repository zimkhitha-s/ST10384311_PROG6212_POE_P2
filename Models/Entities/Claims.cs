using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10384311PROG6212POE.Models.Entities
{
    public class Claims
    {
        // The Properties of the Claims

        // The Primary Key of the Claims Model
        [Key]
        public int ClaimId { get; set; }

        // The Name Variable for the Lecturer and the Required Error Message for Error Handling
        [Required(ErrorMessage = "Lecturer name is required.")]
        public string LecturerName { get; set; }

        // The Email Variable for the Lecturer and the Required Error Message for Error Handling
        [Required(ErrorMessage = "Lecturer email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string LecturerEmail { get; set; }

        // The Claim Period Variable for the Lecturer and the Required Error Message for Error Handling
        [Required(ErrorMessage = "Claim period is required.")]
        public string ClaimPeriod { get; set; }

        // The Total Hours Variable for the Lecturer and the Required Error Message for Error Handling
        [Required(ErrorMessage = "Total hours worked is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total hours must be greater than zero.")]
        public int TotalHours { get; set; }

        // The Total Amount Variable for the Lecturer's Claim Total Amount
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public int HourlyRate { get; set; }

        // The Supporting Documents URL Variable for the Lecturer's Supporting Documents 
        public string? SupportingDocsUrl { get; set; }

        // The Status Variable for the Lecturer's Claim Status
        public string? Status { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//