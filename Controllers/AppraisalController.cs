using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,HR Officer,Manager")]
    public class AppraisalController : Controller
    {
        private readonly ApplicationContext _ctx;
        public AppraisalController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Appraisal
        public IActionResult Index()
        {
            var list = _ctx.Appraisals
                           .Include(a => a.Employee)
                           .OrderByDescending(a => a.AppraisalDate)
                           .ToList();
            return View(list);
        }

        // GET: /Appraisal/Update
        [HttpGet]
        public IActionResult Update()
        {
            ViewBag.EmployeeItems = new SelectList(
                _ctx.Employees.OrderBy(e => e.Name).Select(e => new { e.Id, e.Name }).ToList(),
                "Id", "Name"
            );
            return View();
        }

        // POST: /Appraisal/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Appraisal model)
        {
            if (!_ctx.Employees.Any(e => e.Id == model.EmployeeId))
                ModelState.AddModelError(nameof(model.EmployeeId), "Employee not found.");

            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeItems = new SelectList(
                    _ctx.Employees.OrderBy(e => e.Name).Select(e => new { e.Id, e.Name }).ToList(),
                    "Id", "Name"
                );
                return View(model);
            }

            _ctx.Appraisals.Add(model);

            // (Optional) Immediately update Employee.BasicSalary = NewSalary
            var emp = _ctx.Employees.First(e => e.Id == model.EmployeeId);
            emp.BasicSalary = model.NewSalary;

            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}