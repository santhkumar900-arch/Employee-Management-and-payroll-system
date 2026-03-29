using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHrSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Admin() => View();

        [Authorize(Roles = "Admin,HR Officer")]
        public IActionResult HR() => View();

        [Authorize(Roles = "Admin,Payroll Officer")]
        public IActionResult Payroll() => View();

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Manager() => View();

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Employee() => View();
    }
}
