using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeHrSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _ctx;
        private readonly IPasswordHasher<AppUser> _hasher;

        public AccountController(ApplicationContext ctx, IPasswordHasher<AppUser> hasher)
        {
            _ctx    = ctx;
            _hasher = hasher;
        }

        // ── GET /Account/Login ──────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login()
        {
            // Already authenticated → go to dashboard
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToDashboard(User.FindFirstValue(ClaimTypes.Role));

            return View();
        }

        // ── POST /Account/Login ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password)  ||
                string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            // Look up user by username AND role
            var user = _ctx.Users
                           .FirstOrDefault(u =>
                               u.Username == username.Trim() &&
                               u.Role     == role);

            if (user == null)
            {
                ViewBag.Error = "Invalid username, password, or role.";
                return View();
            }

            // Verify hashed password
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid username, password, or role.";
                return View();
            }

            // Build claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("EmployeeId",
                          user.EmployeeId.HasValue
                              ? user.EmployeeId.Value.ToString()
                              : string.Empty)
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = false });

            return RedirectToDashboard(user.Role);
        }

        // ── GET /Account/Logout ─────────────────────────────────────────────
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // ── GET /Account/AccessDenied ───────────────────────────────────────
        public IActionResult AccessDenied() => View();

        // ── Helper ──────────────────────────────────────────────────────────
        private IActionResult RedirectToDashboard(string? role)
        {
            return role switch
            {
                "Admin"           => RedirectToAction("Admin",    "Dashboard"),
                "HR Officer"      => RedirectToAction("HR",       "Dashboard"),
                "Payroll Officer" => RedirectToAction("Payroll",  "Dashboard"),
                "Manager"         => RedirectToAction("Manager",  "Dashboard"),
                "Employee"        => RedirectToAction("Employee", "Dashboard"),
                _                 => RedirectToAction("Login",    "Account")
            };
        }
    }
}
