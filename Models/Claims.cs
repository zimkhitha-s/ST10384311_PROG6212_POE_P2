namespace ST10384311PROG6212POE.Models
{
    public class Claims
    {
        // The Properties of the Claims
        public int ClaimId { get; set; }  // Primary Key
        public string LecturerName { get; set; }  // For now, store lecturer name directly
        public string LecturerEmail { get; set; }  // Similarly, store the lecturer's email

        public string ClaimPeriod { get; set; }
        public int TotalHours { get; set; }
        public decimal TotalAmount { get; set; }
        public string SupportingDocsUrl { get; set; }
        public string Status { get; set; }  // e.g., Pending, Approved
    }
}
