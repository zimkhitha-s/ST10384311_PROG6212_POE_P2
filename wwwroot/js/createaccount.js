/*The Javascript Code for the Create Account Page*/
function updateRoleSpecificFields() {
    const role = document.getElementById("employeeRole").value;
    const container = document.getElementById("role-specific-fields");

    // Clear any existing fields
    container.innerHTML = "";

    if (role === "lecturer") {
        container.innerHTML += `
            <div>
                <label for="lecturerHoursWorked">Hours Worked:</label>
                <input type="number" id="lecturerHoursWorked" name="lecturerHoursWorked" required />
            </div>
            <div>
                <label for="lecturerHourlyRate">Hourly Rate:</label>
                <input type="number" id="lecturerHourlyRate" name="lecturerHourlyRate" step="0.01" required />
            </div>
            <div>
                <label for="lecturerSupportingDocs">Upload Supporting Document:</label>
                <input type="file" id="lecturerSupportingDocs" name="lecturerSupportingDocs" />
            </div>
        `;
    }
}
//-------------------------------------------------------------------------------------------End Of File--------------------------------------------------------------------//