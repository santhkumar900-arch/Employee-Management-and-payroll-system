using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,Payroll Officer")]
    public class PayrollController : Controller
    {
        private readonly ApplicationContext _ctx;
        public PayrollController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Payroll
        [HttpGet]
        public IActionResult Index()
        {
            LoadEmployees(); // build Name-only dropdown (value = Id)
            var list = _ctx.Payrolls
                           .Include(p => p.Employee)          // <-- so we can show Employee.Name in the view
                           .OrderByDescending(p => p.PayrollId)
                           .ToList();
            return View(list);
        }

        // POST: /Payroll/Process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Process([FromForm] int employeeId, [FromForm] string month)
        {
            // Minimal validation
            if (string.IsNullOrWhiteSpace(month))
                ModelState.AddModelError("month", "Month is required (YYYY-MM).");

            var emp = _ctx.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (emp == null)
                ModelState.AddModelError("employeeId", "Employee not found.");

            if (!ModelState.IsValid)
            {
                LoadEmployees();
                var list = _ctx.Payrolls
                               .Include(p => p.Employee)
                               .OrderByDescending(p => p.PayrollId)
                               .ToList();
                return View("Index", list);
            }

            var p = new Payroll
            {
                EmployeeId = emp!.Id,
                Month = month.Trim(),
                BasicSalary = emp.BasicSalary ?? 0,
                Deductions = 0,
                NetSalary = emp.BasicSalary ?? 0,
                PaymentStatus = "PENDING"
            };

            _ctx.Payrolls.Add(p);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private void LoadEmployees()
        {
            // Name-only dropdown text; value remains Id
            var items = _ctx.Employees
                            .OrderBy(e => e.Name)
                            .Select(e => new { e.Id, e.Name })
                            .ToList();
            ViewBag.EmployeeItems = new SelectList(items, "Id", "Name");
        }
    }
}