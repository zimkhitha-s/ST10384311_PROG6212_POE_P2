using Microsoft.AspNetCore.Mvc;
using ST10384311PROG6212POE.Models;

namespace ST10384311PROG6212POE.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly IClaimService _claimService;

        // Injecting the service that handles claim-related logic
        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        // Display the claim submission form
        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View(new ClaimModel());
        }

        // Submit a claim (handled by lecturers)
        [HttpPost]
        public IActionResult SubmitClaim(Claims claim, Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                // Manually associate the lecturer's details with the claim for now (without a foreign key)
                claim.LecturerName = lecturer.EmployeeName;
                claim.LecturerEmail = lecturer.EmployeeEmail;

                // Add logic to calculate the total amount
                claim.TotalAmount = claim.TotalHours * lecturer.LecturerHourlyRate;

                // Call the service to handle the claim submission
                _claimService.SubmitClaim(claim);

                // Redirect to the claim tracking page for the lecturer
                return RedirectToAction("TrackClaim", new { lecturerId = lecturer.EmployeeId });
            }
            return View(claim);
        }

        // Track claim status (handled by lecturers)
        [HttpGet]
        public IActionResult TrackClaim(int lecturerId)
        {
            // Retrieve the claims submitted by the lecturer using their ID
            var claims = _claimService.GetClaimsByLecturer(lecturerId);

            // Pass the claims to the view for display
            return View(claims);
        }
    }
}
