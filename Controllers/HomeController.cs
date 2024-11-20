using Microsoft.AspNetCore.Mvc;
using ST10384311PROG6212POE.Models;
using ST10384311PROG6212POE.Data;
using Microsoft.EntityFrameworkCore;
using ST10384311PROG6212POE.Models.Entities;
using System.Diagnostics;

namespace ST10384311PROG6212POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        // Constructor to initialize the database context and logger
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the Index view
        public IActionResult Index()
        {
            return View();
        }

        // Action method to return the SubmitClaims view (GET request)
        public IActionResult SubmitClaims()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to handle the submission of claims (POST request)
        [HttpPost]
        public async Task<IActionResult> SubmitClaims(Claims claim, IFormFile supportingDocs)
        {
            const decimal hourlyRate = 200;

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Calculate the total amount based on the total hours and hourly rate
                claim.TotalAmount = claim.TotalHours * hourlyRate;

                // Check if supporting documents are provided
                if (supportingDocs != null && supportingDocs.Length > 0)
                {
                    // Define the path to save the uploaded files
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var fileName = Path.GetFileName(supportingDocs.FileName);

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    var filePath = Path.Combine(uploadsPath, fileName);

                    // Save the uploaded file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await supportingDocs.CopyToAsync(stream);
                    }

                    // Set the URL of the supporting documents
                    claim.SupportingDocsUrl = "/uploads/" + fileName;
                }

                // Set the status of the claim to "Pending"
                claim.Status = "Pending";
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                // Redirect to the ClaimsStatus action
                return RedirectToAction("ClaimsStatus");
            }

            // Return the view with the claim model if the model state is not valid
            return View(claim);
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to update the status of a claim (POST request)
        [HttpPost]
        public async Task<IActionResult> UpdateClaimStatus(int claimId, string newStatus)
        {
            // Find the claim by its ID
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                // Update the status of the claim
                claim.Status = newStatus;
                await _context.SaveChangesAsync();
                return RedirectToAction("ProcessClaims");
            }

            // Return a 404 Not Found result if the claim is not found
            return NotFound();
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the ProcessClaims view with pending claims
        public async Task<IActionResult> ProcessClaims()
        {
            // Get the list of pending claims from the database
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .ToListAsync();
            return View(pendingClaims);
        }
//------------------------------------------------------------------------------------------------------------------------------------------------------//
        // Action method to return the ClaimsStatus view with all claims
        public async Task<IActionResult> ClaimsStatus()
        {
            // Get the list of all claims from the database
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
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