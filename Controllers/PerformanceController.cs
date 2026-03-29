using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class PerformanceController : Controller
    {
        private readonly ApplicationContext _ctx;
        public PerformanceController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Performance
        public IActionResult Index()
        {
            var list = _ctx.Evaluations
                           .Include(e => e.Employee)
                           .OrderByDescending(e => e.EvaluationDate)
                           .ToList();
            return View(list);
        }

        // GET: /Performance/Record
        [HttpGet]
        public IActionResult Record()
        {
            ViewBag.EmployeeItems = new SelectList(
                _ctx.Employees.OrderBy(e => e.Name).Select(e => new { e.Id, e.Name }).ToList(),
                "Id", "Name"
            );
            return View();
        }

        // POST: /Performance/Record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Record(Evaluation model)
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

            _ctx.Evaluations.Add(model);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
