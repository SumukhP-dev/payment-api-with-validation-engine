// Custom JavaScript to fix clear button functionality in Swagger UI
window.addEventListener('load', function() {
    // Wait for Swagger UI to fully load
    setTimeout(function() {
        // Find all clear buttons
        var clearButtons = document.querySelectorAll('.btn.clear');
        
        // Add click event listeners to clear buttons
        clearButtons.forEach(function(button) {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                
                // Clear all response sections
                var responses = document.querySelectorAll('.response-col');
                responses.forEach(function(response) {
                    response.innerHTML = '';
                });
                
                // Clear all curl command sections
                var curlSections = document.querySelectorAll('.curl-command');
                curlSections.forEach(function(curl) {
                    curlSection.innerHTML = '';
                });
                
                // Clear all highlight sections
                var highlights = document.querySelectorAll('.highlight');
                highlights.forEach(function(highlight) {
                    highlight.innerHTML = '';
                });
                
                // Reset try-it-out forms
                var tryItOutForms = document.querySelectorAll('.try-out-form');
                tryItOutForms.forEach(function(form) {
                    form.reset();
                });
                
                // Clear all response status badges
                var statusBadges = document.querySelectorAll('.response-status');
                statusBadges.forEach(function(badge) {
                    badge.textContent = '';
                    badge.className = 'response-status';
                });
                
                console.log('Swagger UI responses cleared!');
            });
        });
    }, 2000);
});
