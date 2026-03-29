using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,HR Officer")]
    public class ReportController : Controller
    {
        private readonly ApplicationContext _ctx;
        public ReportController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Report/HR
        public IActionResult HR()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var startOfMonth = new DateOnly(today.Year, today.Month, 1);

            // Number of days elapsed in the current month
            int daysSoFar =
                (today.ToDateTime(TimeOnly.MinValue)
                - startOfMonth.ToDateTime(TimeOnly.MinValue)).Days + 1;

            int totalEmployees = _ctx.Employees.Count();

            int presentCount = _ctx.Attendances
                .Where(a =>
                    a.Date >= startOfMonth &&
                    a.Date <= today &&
                    a.Status == "PRESENT")
                .Count();

            decimal averageAttendance = 0m;
            if (totalEmployees > 0 && daysSoFar > 0)
            {
                averageAttendance = Math.Round(
                    (decimal)presentCount /
                    (totalEmployees * daysSoFar) * 100m,
                    2
                );
            }

            var report = new HRReport
            {
                ReportDate = today,
                TotalEmployees = totalEmployees,
                AverageAttendance = averageAttendance,
                PayrollSummary = $"Total payroll records: {_ctx.Payrolls.Count()}"
            };

            return View(report);
        }
    }
}