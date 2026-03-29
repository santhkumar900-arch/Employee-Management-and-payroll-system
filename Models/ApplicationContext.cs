using Microsoft.EntityFrameworkCore;

namespace EmployeeHrSystem.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
        public DbSet<Payroll> Payrolls => Set<Payroll>();
        public DbSet<Evaluation> Evaluations => Set<Evaluation>();
        public DbSet<Appraisal> Appraisals => Set<Appraisal>();
        public DbSet<HRReport> HRReports => Set<HRReport>();
        public DbSet<AppUser> Users => Set<AppUser>();
    }
}