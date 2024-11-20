namespace ST10384311PROG6212POE.Models.Entities
{
    public class Lecturer : Employee
    {
        // The Properties of the Lecturer
        public int LecturerHoursWorked { get; set; }
        public int LecturerHourlyRate { get; set; }
        public string? LecturerSupportingDocsPath { get; set; }

        // Navigation Property: A Lecturer can have multiple claims
        public ICollection<Claims> Claims { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//