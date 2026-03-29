using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,HR Officer,Manager,Employee")]
    public class LeaveController : Controller
    {
        private readonly ApplicationContext _ctx;
        public LeaveController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Leave
        public IActionResult Index()
        {
            var query = _ctx.LeaveRequests
                           .Include(r => r.Employee)
                           .AsQueryable();

            if (User.IsInRole("Employee"))
            {
                var empIdStr = User.FindFirstValue("EmployeeId");
                if (int.TryParse(empIdStr, out int empId))
                {
                    query = query.Where(r => r.EmployeeId == empId);
                }
            }

            var list = query.OrderByDescending(r => r.LeaveId).ToList();
            return View(list);
        }

        // GET: /Leave/Apply
        [HttpGet]
        public IActionResult Apply()
        {
            int? selectedId = null;
            if (User.IsInRole("Employee"))
            {
                var empIdStr = User.FindFirstValue("EmployeeId");
                if (int.TryParse(empIdStr, out int empId))
                {
                    selectedId = empId;
                }
            }
            
            LoadEmployeesForDropdown(selectedId);
            return View();
        }

        // POST: /Leave/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(LeaveRequest r)
        {
            if (User.IsInRole("Employee"))
            {
                var empIdStr = User.FindFirstValue("EmployeeId");
                if (int.TryParse(empIdStr, out int empId))
                {
                    r.EmployeeId = empId;
                }
                r.Status = "APPLIED"; // Enforce status for employee
            }

            // Defensive validation to prevent FK errors
            if (!_ctx.Employees.Any(e => e.Id == r.EmployeeId))
            {
                ModelState.AddModelError(nameof(r.EmployeeId), "Employee not found.");
            }

            if (!ModelState.IsValid)
            {
                LoadEmployeesForDropdown(r.EmployeeId);
                return View(r);
            }

            _ctx.LeaveRequests.Add(r);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,HR Officer,Manager")]
        public IActionResult Approve(int id)
        {
            var request = _ctx.LeaveRequests.Find(id);
            if (request != null)
            {
                request.Status = "APPROVED";
                _ctx.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,HR Officer,Manager")]
        public IActionResult Reject(int id)
        {
            var request = _ctx.LeaveRequests.Find(id);
            if (request != null)
            {
                request.Status = "REJECTED";
                _ctx.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Populates ViewBag.EmployeeItems with a Name-only dropdown (value = Id).
        /// </summary>
        private void LoadEmployeesForDropdown(int? selectedId = null)
        {
            var items = _ctx.Employees
                            .OrderBy(e => e.Name)
                            .Select(e => new { e.Id, e.Name })
                            .ToList();

            ViewBag.EmployeeItems = new SelectList(items, "Id", "Name", selectedId);
        }
    }
}