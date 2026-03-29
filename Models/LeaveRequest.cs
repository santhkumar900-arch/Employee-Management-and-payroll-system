using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class LeaveRequest
    {
        [Key] public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string LeaveType { get; set; } = "CASUAL";
        public string Status { get; set; } = "APPLIED";
    }
}
