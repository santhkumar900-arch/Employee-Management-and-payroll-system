using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class Appraisal
    {
        [Key] public int AppraisalId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateOnly AppraisalDate { get; set; }
        public decimal NewSalary { get; set; }
    }
}