using System.ComponentModel.DataAnnotations;

namespace EmployeeHrSystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Designation { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string? ContactInfo { get; set; }
        public decimal? BasicSalary { get; set; }
    }
}