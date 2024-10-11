namespace ST10384311PROG6212POE.Models
{
    public class LecturerModel : EmployeeModel
    {
        // The Properties of the LecturerModel
        public int LecturerId { get; set; }
        public string LecturerType { get; set; }
        public int LecturerHoursWorked { get; set; }
        public int LecturerHourlyRate{ get; set; }
        public string LecturerSupportingDocs { get; set; }
    }
}
