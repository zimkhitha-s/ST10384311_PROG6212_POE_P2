using Microsoft.AspNetCore.Mvc;
using ST10384311PROG6212POE.Models;
using ST10384311PROG6212POE.Data;
using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models.Entities;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text;
using ClosedXML.Excel;


namespace ST10384311PROG6212POE.Controllers
{
    public class HomeController : Controller
    { 
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor to initialize the database context, sign-in manager, and user manager
        public HomeController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the different views
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SubmitClaims()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method for setting the user role (via GET request)
        // Role selection via a GET request
        [HttpGet]
        public IActionResult SetRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                _logger.LogError("No role selected in SetRole.");
                return RedirectToAction("Index");
            }

            TempData["SelectedRole"] = role; // Store the role temporarily
            _logger.LogInformation("Role selected: {Role}", role);
            return RedirectToAction("Login");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Handle role-specific redirection (called after login)
        [HttpGet]
        public IActionResult RedirectToRolePage()
        {
            var role = TempData["SelectedRole"]?.ToString();
            _logger.LogInformation("Redirecting based on role: {Role}", role);

            if (role == "Lecturer")
            {
                return RedirectToAction("SubmitClaims");
            }
            else if (role == "Administrator")
            {
                return RedirectToAction("ProcessClaims");
            }
            else if (role == "HR")
            {
                return RedirectToAction("HRDashboard");
            }

            _logger.LogError("Invalid role in RedirectToRolePage: {Role}", role);
            return RedirectToAction("Index");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to handle login (POST request)
        [HttpPost]
        public async Task<IActionResult> LoginUserIn(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var roles = await _userManager.GetRolesAsync(user);

                // Fetch the role from TempData (selected role by user before login)
                var selectedRole = TempData["SelectedRole"]?.ToString();

                if (selectedRole != null && roles.Contains(selectedRole))
                {
                    // Redirect based on role
                    if (selectedRole == "Lecturer")
                    {
                        return RedirectToAction("SubmitClaims");
                    }
                    else if (selectedRole == "Administrator")
                    {
                        return RedirectToAction("ProcessClaims");
                    }
                    else if (selectedRole == "HR")
                    {
                        return RedirectToAction("HRDashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "You do not have permission for the selected role.");
                    return View("Login");
                }
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View("Login");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to handle the submission of claims (POST request)
        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> SubmitClaims(Claims claim, IFormFile supportingDocs)
        {
            if (ModelState.IsValid)
            {
                claim.TotalAmount = claim.TotalHours * claim.HourlyRate;

                if (supportingDocs != null && supportingDocs.Length > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    var fileName = Path.GetFileName(supportingDocs.FileName);
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
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to update the status of a claim (POST request)
        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the ProcessClaims view with pending claims
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ProcessClaims()
        {
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .ToListAsync();
            return View(pendingClaims);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the ClaimsStatus view with all claims
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> ClaimsStatus()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method for HR dashboard
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> HRDashboard()
        {
            // Fetch approved claims from the database
            var approvedClaims = await _context.Claims
                .Where(c => c.Status == "Approved")
                .ToListAsync();

            // Pass the claims to the view
            return View(approvedClaims);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action to display the claims for HR to manage and process
        [Authorize(Roles = "HR")]
        public IActionResult GenerateReport()
        {
            var approvedClaims = _context.Claims.Where(c => c.Status == "Approved").ToList();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("LecturerName,LecturerEmail,ClaimPeriod,TotalHours,TotalAmount");

            foreach (var claim in approvedClaims)
            {
                csvBuilder.AppendLine($"{claim.LecturerName},{claim.LecturerEmail},{claim.ClaimPeriod},{claim.TotalHours},{claim.TotalAmount}");
            }

            return File(Encoding.UTF8.GetBytes(csvBuilder.ToString()), "text/csv", "ApprovedClaimsReport.csv");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action to generate and download the claims report as an Excel file
        [Authorize(Roles = "HR")]
        public IActionResult GenerateClaimsReport()
        {
            // Get approved claims from the database
            var claims = _context.Claims
                .Where(c => c.Status == "Approved") // Get only approved claims
                .ToList();

            // Create a new Excel workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet to the workbook
                var worksheet = workbook.Worksheets.Add("Approved Claims");
                var currentRow = 1;

                // Set headers for the report
                worksheet.Cell(currentRow, 1).Value = "Lecturer Name";
                worksheet.Cell(currentRow, 2).Value = "Lecturer Email";
                worksheet.Cell(currentRow, 3).Value = "Claim Period";
                worksheet.Cell(currentRow, 4).Value = "Total Hours";
                worksheet.Cell(currentRow, 5).Value = "Hourly Rate";
                worksheet.Cell(currentRow, 6).Value = "Total Amount";
                currentRow++;

                // Add data for each claim
                foreach (var claim in claims)
                {
                    worksheet.Cell(currentRow, 1).Value = claim.LecturerName;
                    worksheet.Cell(currentRow, 2).Value = claim.LecturerEmail;
                    worksheet.Cell(currentRow, 3).Value = claim.ClaimPeriod;
                    worksheet.Cell(currentRow, 4).Value = claim.TotalHours;
                    worksheet.Cell(currentRow, 5).Value = claim.HourlyRate;
                    worksheet.Cell(currentRow, 6).Value = claim.TotalAmount;
                    currentRow++;
                }

                // Format the columns for better readability
                worksheet.Columns().AdjustToContents();

                // Write the file to a memory stream
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0; 

                // Return the Excel file as a downloadable response
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ApprovedClaimsReport.xlsx");
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action to manage and update lecturer data
        [Authorize(Roles = "HR")]
        public IActionResult ManageLecturerData()
        {
            // Implement functionality to view/update lecturer data
            var lecturers = _context.Claims
                .Select(c => new { c.LecturerName, c.LecturerEmail })
                .Distinct()
                .ToList();
            return View(lecturers); // Display list of lecturers with data
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action to update lecturer data (could include name, email, etc.)
        [HttpPost]
        [Authorize(Roles = "HR")]
        public IActionResult UpdateLecturerData(string lecturerEmail, string newLecturerName)
        {
            var lecturer = _context.Claims.FirstOrDefault(c => c.LecturerEmail == lecturerEmail);
            if (lecturer != null)
            {
                lecturer.LecturerName = newLecturerName; // Update lecturer name
                _context.SaveChanges();
            }
            return RedirectToAction("ManageLecturerData");
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the Error view with error details
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//