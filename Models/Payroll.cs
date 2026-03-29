using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class Payroll
    {
        [Key] public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public string Month { get; set; } = string.Empty; // e.g., 2026-03
        public decimal BasicSalary { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string PaymentStatus { get; set; } = "PENDING";
    }
}
