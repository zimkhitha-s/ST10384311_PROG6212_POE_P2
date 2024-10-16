using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10384311PROG6212POE.Models.Entities
{
    public class Claims
    {
        // The Properties of the Claims
        [Key]
        public int ClaimId { get; set; }  // Primary Key
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }

        public string ClaimPeriod { get; set; }
        public int TotalHours { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public string SupportingDocsUrl { get; set; }
        public string Status { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//