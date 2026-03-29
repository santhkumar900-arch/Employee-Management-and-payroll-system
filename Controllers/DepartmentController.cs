using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,HR Officer")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationContext _ctx;
        public DepartmentController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Department
        public IActionResult Index()
        {
            var list = _ctx.Departments.ToList();
            return View(list);
        }

        // GET: /Department/Create
        [HttpGet]
        public IActionResult Create() => View();

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department dept)
        {
            if (!ModelState.IsValid) return View(dept);
            _ctx.Departments.Add(dept);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
