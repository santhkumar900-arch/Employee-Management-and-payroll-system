using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,HR Officer,Manager")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationContext _ctx;
        public AttendanceController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Attendance
        public IActionResult Index()
        {
            var items = _ctx.Attendances
                            .Include(a => a.Employee)            // so we can use a.Employee.Name
                            .OrderByDescending(a => a.AttendanceId)
                            .ToList();
            return View(items);
        }

        // GET: /Attendance/Mark
        [HttpGet]
        public IActionResult Mark()
        {
            ViewBag.EmployeeItems = new SelectList(
                _ctx.Employees
                   .OrderBy(e => e.Name)
                   .Select(e => new { e.Id, e.Name })
                   .ToList(),
                "Id", "Name"
            );
            return View();
        }

        // POST: /Attendance/Mark
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Mark(Attendance a)
        {
            if (!_ctx.Employees.Any(e => e.Id == a.EmployeeId))
                ModelState.AddModelError(nameof(a.EmployeeId), "Select a valid employee.");

            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeItems = new SelectList(
                    _ctx.Employees
                       .OrderBy(e => e.Name)
                       .Select(e => new { e.Id, e.Name })
                       .ToList(),
                    "Id", "Name"
                );
                return View(a);
            }

            _ctx.Attendances.Add(a);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
