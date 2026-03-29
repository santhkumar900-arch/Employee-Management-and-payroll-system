using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class Attendance
    {
        [Key] public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; } = "PRESENT"; // PRESENT, ABSENT, LEAVE
    }
}