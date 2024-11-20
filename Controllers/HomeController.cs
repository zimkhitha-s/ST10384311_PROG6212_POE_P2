using Microsoft.AspNetCore.Mvc;
using ST10384311PROG6212POE.Models;
using ST10384311PROG6212POE.Data;
using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models.Entities;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ST10384311PROG6212POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Constructor to initialize the database context, logger, and Identity services
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Index (home page)
        public IActionResult Index()
        {
            return View();
        }

        // GET: SubmitClaims (for submitting claims)
        [Authorize(Roles = "Lecturer")] // Only Lecturers can submit claims
        public IActionResult SubmitClaims()
        {
            return View();
        }

        // GET: CreateAccount (page to create an account)
        public IActionResult CreateAccount()
        {
            return View();
        }

        // POST: Handle submission of claims (Lecturer submits claims)
        [HttpPost]
        [Authorize(Roles = "Lecturer")] // Only Lecturers can submit claims
        public async Task<IActionResult> SubmitClaims(Claims claim, IFormFile supportingDocs)
        {
            const decimal hourlyRate = 200;

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Calculate the total amount based on the total hours and hourly rate
                claim.TotalAmount = claim.TotalHours * hourlyRate;

                // Handle supporting documents
                if (supportingDocs != null && supportingDocs.Length > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var fileName = Path.GetFileName(supportingDocs.FileName);

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    var filePath = Path.Combine(uploadsPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await supportingDocs.CopyToAsync(stream);
                    }

                    claim.SupportingDocsUrl = "/uploads/" + fileName;
                }

                claim.Status = "Pending";
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                return RedirectToAction("ClaimsStatus");
            }

            return View(claim);
        }

        // GET: ProcessClaims (for processing claims by Admins)
        [Authorize(Roles = "Administrator")] // Only Administrators can process claims
        public async Task<IActionResult> ProcessClaims()
        {
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .ToListAsync();
            return View(pendingClaims);
        }

        // POST: UpdateClaimStatus (for updating the status of a claim)
        [HttpPost]
        [Authorize(Roles = "Administrator")] // Only Administrators can update claim status
        public async Task<IActionResult> UpdateClaimStatus(int claimId, string newStatus)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = newStatus;
                await _context.SaveChangesAsync();
                return RedirectToAction("ProcessClaims");
            }

            return NotFound();
        }

        // GET: ClaimsStatus (for all claims, visible to all logged-in users)
        public async Task<IActionResult> ClaimsStatus()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }

        // Action to handle user registration (GET)
        public IActionResult Register()
        {
            return View();
        }

        // POST: Handle registration (creating new users)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmployeeRole = model.Role };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Add role after successful registration
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // Sign in the user after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index"); // Redirect to Home page
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Action for user login (GET)
        public IActionResult Login()
        {
            return View();
        }

        // POST: Handle user login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        // POST: Logout (user logs out)
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Redirect to Home after logout
        }

        // Action method to return the Error view
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//