using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class Evaluation
    {
        [Key] public int EvaluationId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateOnly EvaluationDate { get; set; }
        public decimal Score { get; set; }
        public string? Remarks { get; set; }
    }
}
