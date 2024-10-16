using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Data;
using ST10384311PROG6212POE.Models;
using ST10384311PROG6212POE.Models.Entities;
using System.Diagnostics;

namespace ST10384311PROG6212POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;  // Inject the database context
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: SubmitClaims (Displays the form for submitting a claim)
        public IActionResult SubmitClaims()
        {
            return View();
        }

        // POST: SubmitClaims (Handles claim submission)
        [HttpPost]
        public async Task<IActionResult> SubmitClaims(Claims claim, IFormFile supportingDocs)
        {
            const decimal hourlyRate = 200;  // Fixed hourly rate for calculation

            if (ModelState.IsValid)
            {
                // Calculate the total amount based on the number of hours worked
                claim.TotalAmount = claim.TotalHours * hourlyRate;

                if (supportingDocs != null && supportingDocs.Length > 0)
                {
                    // Save the uploaded file to wwwroot/uploads
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var fileName = Path.GetFileName(supportingDocs.FileName);  // Get the original file name

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    var filePath = Path.Combine(uploadsPath, fileName);

                    // Save the file asynchronously
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await supportingDocs.CopyToAsync(stream);
                    }

                    // Store the file path in the database (relative URL)
                    claim.SupportingDocsUrl = "/uploads/" + fileName;
                }

                // Set the initial claim status to "Pending"
                claim.Status = "Pending";

                // Save the claim to the database
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                // Redirect to Claims Status after submitting
                return RedirectToAction("ClaimsStatus");
            }

            return View(claim); // Return the form with validation errors if the model is invalid
        }


        [HttpPost]
        public async Task<IActionResult> UpdateClaimStatus(int claimId, string newStatus)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = newStatus; // Set the new status based on the button clicked
                await _context.SaveChangesAsync();
                return RedirectToAction("ProcessClaims");
            }

            return NotFound();
        }



        // GET: ProcessClaims (Displays pending claims for administrators)
        public async Task<IActionResult> ProcessClaims()
        {
            // Fetch only claims that are still pending asynchronously
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .ToListAsync();
            return View(pendingClaims);  // Pass the list of claims to the view
        }

        // GET: ClaimsStatus (Displays claims submitted by the lecturer)
        public async Task<IActionResult> ClaimsStatus()
        {
            // Retrieve all claims asynchronously (in a real app, you'd filter claims by the logged-in lecturer's ID)
            var claims = await _context.Claims.ToListAsync();
            return View(claims);  // Pass the claims to the view
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
