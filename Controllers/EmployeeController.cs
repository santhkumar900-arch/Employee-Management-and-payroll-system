using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHrSystem.Controllers
{
    [Authorize(Roles = "Admin,HR Officer")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext _ctx;
        public EmployeeController(ApplicationContext ctx) => _ctx = ctx;

        // GET: /Employee
        public IActionResult Index()
        {
            var list = _ctx.Employees.Include(e => e.Department).ToList();
            return View(list);
        }

        // GET: /Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_ctx.Departments.ToList(), "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_ctx.Departments.ToList(), "DepartmentId", "DepartmentName");
                return View(employee);
            }
            _ctx.Employees.Add(employee);
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Edit/5
        public IActionResult Edit(int id)
        {
            var e = _ctx.Employees.Find(id);
            if (e == null) return NotFound();
            ViewBag.Departments = new SelectList(_ctx.Departments.ToList(), "DepartmentId", "DepartmentName", e.DepartmentId);
            return View(e);
        }

        // POST: /Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee e)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_ctx.Departments.ToList(), "DepartmentId", "DepartmentName", e.DepartmentId);
                return View(e);
            }
            _ctx.Entry(e).State = EntityState.Modified;
            _ctx.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Delete/5
        public IActionResult Delete(int id)
        {
            var e = _ctx.Employees.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
            if (e == null) return NotFound();
            return View(e);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var e = _ctx.Employees.Find(id);
            if (e != null)
            {
                _ctx.Employees.Remove(e);
                _ctx.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}