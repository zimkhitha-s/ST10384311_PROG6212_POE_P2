namespace ST10384311PROG6212POE.Models.Entities
{
    public class Lecturer : Employee
    {
        // The Properties of the Lecturer
        public int LecturerId { get; set; }
        public string LecturerType { get; set; }
        public int LecturerHoursWorked { get; set; }
        public int LecturerHourlyRate { get; set; }
        public IFormFile LecturerSupportingDocs { get; set; }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//