/*The Javascript Code for the Submit Claims Page*/

// Choosing between full-time or part-time lecturer
document.addEventListener("DOMContentLoaded", function () {
    const lecturerType = document.getElementById('lecturerType');
    const claimOptionsFullTime = document.getElementById('claimOptionsFullTime');
    const claimOptionsPartTime = document.getElementById('claimOptionsPartTime');

    lecturerType.addEventListener('change', function () {
        if (lecturerType.value === 'full-time') {
            claimOptionsFullTime.style.display = 'block';
            claimOptionsPartTime.style.display = 'none';
        } else if (lecturerType.value === 'part-time') {
            claimOptionsFullTime.style.display = 'none';
            claimOptionsPartTime.style.display = 'block';
        } else {
            claimOptionsFullTime.style.display = 'none';
            claimOptionsPartTime.style.display = 'none';
        }
    });
});

// Choosing between the different claim types
document.getElementById('claimTypePartTime').addEventListener('change', function () {
    var salaryFields = document.getElementById('salaryFields');

    // Check if the selected claim type is 'Lecture Hours'
    if (this.value === 'lecture-hours') {
        salaryFields.style.display = 'block';  // Show salary fields
    } else {
        salaryFields.style.display = 'none';   // Hide salary fields
    }
});


