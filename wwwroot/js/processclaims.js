/*The Javascript Code for the Process Claims Page*/


<script>
    function refreshStatus() {
        $.ajax({
            url: '@Url.Action("ClaimsStatus")',
            type: 'GET',
            success: function (result) {
                $('#claims-status-container').html(result);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching claim status:", error);
            }
        });
    }

    // Refresh every 10 seconds
    setInterval(refreshStatus, 10000);
</script>

