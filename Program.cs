using EmployeeHrSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Database ─────────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

// ── Authentication & Authorization ───────────────────────────────────────────
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath         = "/Account/Login";
        options.AccessDeniedPath  = "/Account/AccessDenied";
        options.ExpireTimeSpan    = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// PasswordHasher registered so we can inject it in AccountController
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── Seed default users ────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db      = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    var hasher  = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

    db.Database.Migrate();   // apply any pending migrations

    // Default seed accounts (Username / Password / Role)
    var seeds = new[]
    {
        ("admin",    "Admin@123",   "Admin"),
        ("hruser",   "Hr@123",      "HR Officer"),
        ("payroll",  "Pay@123",     "Payroll Officer"),
        ("manager",  "Mgr@123",     "Manager"),
        ("employee", "Emp@123",     "Employee"),
    };

    foreach (var (uname, pwd, role) in seeds)
    {
        if (!db.Users.Any(u => u.Username == uname))
        {
            var user = new AppUser { Username = uname, Role = role };
            user.PasswordHash = hasher.HashPassword(user, pwd);
            db.Users.Add(user);
        }
    }
    db.SaveChanges();
}

// ── HTTP Pipeline ─────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();   // ← must come BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();