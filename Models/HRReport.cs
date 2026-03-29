using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class HRReport
    {
        [Key] public int ReportId { get; set; }
        public DateOnly ReportDate { get; set; }
        public int TotalEmployees { get; set; }
        public decimal AverageAttendance { get; set; }
        public string PayrollSummary { get; set; } = string.Empty;
    }
}
