using EmployeeHrSystem.Models;

namespace EmployeeHrSystem.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationContext ctx)
        {
            ctx.Database.EnsureCreated();

            if (!ctx.Departments.Any())
            {
                ctx.Departments.AddRange(
                    new Department { DepartmentName = "Engineering" },
                    new Department { DepartmentName = "QA" },
                    new Department { DepartmentName = "HR" }
                );
                ctx.SaveChanges();
            }

            if (!ctx.Employees.Any())
            {
                var eng = ctx.Departments.First(d => d.DepartmentName == "Engineering");
                var qa = ctx.Departments.First(d => d.DepartmentName == "QA");
                ctx.Employees.AddRange(
                    new Employee { Name = "Anita", Designation = "Developer", DepartmentId = eng.DepartmentId, BasicSalary = 65000, ContactInfo = "anita@example.com" },
                    new Employee { Name = "Ravi", Designation = "Tester", DepartmentId = qa.DepartmentId, BasicSalary = 52000, ContactInfo = "ravi@example.com" }
                );
                ctx.SaveChanges();
            }
        }
    }
}