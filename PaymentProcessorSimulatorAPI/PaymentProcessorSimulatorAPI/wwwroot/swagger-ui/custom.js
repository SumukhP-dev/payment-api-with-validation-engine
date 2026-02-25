// Custom JavaScript to fix clear button functionality in Swagger UI
window.addEventListener('load', function() {
    // Wait for Swagger UI to fully load
    setTimeout(function() {
        // Find all clear buttons (multiple selectors for compatibility)
        var clearButtons = document.querySelectorAll('.btn.clear, .clear-btn, button[title="Clear"], .opblock-summary-control');
        
        // Add click event listeners to clear buttons
        clearButtons.forEach(function(button) {
            button.addEventListener('click', function(e) {
                // Check if this is actually a clear button
                if (button.textContent.toLowerCase().includes('clear') || button.title.toLowerCase().includes('clear')) {
                    e.preventDefault();
                    e.stopPropagation();
                    
                    // Clear all response sections
                    var responses = document.querySelectorAll('.responses-wrapper, .response, .response-col, .highlight');
                    responses.forEach(function(response) {
                        response.innerHTML = '';
                        response.style.display = 'none';
                    });
                    
                    // Clear all curl command sections
                    var curlSections = document.querySelectorAll('.curl-command, .highlight-code');
                    curlSections.forEach(function(curl) {
                        curl.innerHTML = '';
                    });
                    
                    // Clear all execute sections
                    var executeSections = document.querySelectorAll('.execute-wrapper');
                    executeSections.forEach(function(execute) {
                        var responses = execute.querySelectorAll('.response');
                        responses.forEach(function(response) {
                            response.innerHTML = '';
                        });
                    });
                    
                    // Reset all try-it-out forms
                    var tryItOutForms = document.querySelectorAll('.try-out-form, .opblock-body form');
                    tryItOutForms.forEach(function(form) {
                        form.reset();
                    });
                    
                    // Clear all response status badges
                    var statusBadges = document.querySelectorAll('.response-status, .status-col');
                    statusBadges.forEach(function(badge) {
                        badge.textContent = '';
                        badge.className = badge.className.replace(/status-\d+/g, '');
                    });
                    
                    // Hide all response sections
                    var opblocks = document.querySelectorAll('.opblock');
                    opblocks.forEach(function(opblock) {
                        var responses = opblock.querySelectorAll('.responses-wrapper');
                        responses.forEach(function(response) {
                            response.style.display = 'none';
                        });
                    });
                    
                    console.log('Swagger UI responses cleared!');
                    
                    // Show visual feedback
                    var feedback = document.createElement('div');
                    feedback.textContent = 'Responses cleared!';
                    feedback.style.cssText = 'position: fixed; top: 20px; right: 20px; background: #28a745; color: white; padding: 10px 20px; border-radius: 5px; z-index: 9999;';
                    document.body.appendChild(feedback);
                    
                    setTimeout(function() {
                        document.body.removeChild(feedback);
                    }, 2000);
                }
            });
        });
        
        // Also add a global clear button if none exists
        var existingClearButton = document.querySelector('.btn.clear, .clear-btn');
        if (!existingClearButton) {
            var globalClearButton = document.createElement('button');
            globalClearButton.textContent = 'Clear All Responses';
            globalClearButton.className = 'btn clear';
            globalClearButton.style.cssText = 'position: fixed; top: 10px; right: 10px; z-index: 9999; background: #ff6b6b; color: white; border: none; padding: 8px 16px; border-radius: 4px; cursor: pointer;';
            document.body.appendChild(globalClearButton);
            
            // Add click handler to global clear button
            globalClearButton.addEventListener('click', function() {
                // Trigger clear on all response sections
                var responses = document.querySelectorAll('.responses-wrapper, .response, .highlight');
                responses.forEach(function(response) {
                    response.innerHTML = '';
                    response.style.display = 'none';
                });
                
                console.log('Global clear button clicked!');
                
                // Show visual feedback
                var feedback = document.createElement('div');
                feedback.textContent = 'All responses cleared!';
                feedback.style.cssText = 'position: fixed; top: 50px; right: 10px; background: #28a745; color: white; padding: 10px 20px; border-radius: 5px; z-index: 9999;';
                document.body.appendChild(feedback);
                
                setTimeout(function() {
                    document.body.removeChild(feedback);
                }, 2000);
            });
        }
    }, 3000);
});
