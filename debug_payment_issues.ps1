# Comprehensive Payment Debug Test
Write-Host "=== Payment API Debug Analysis ===" -ForegroundColor Green

# Test various payment scenarios
$testCases = @(
    @{ Name = "Valid USD Payment"; Payment = @{ CustomerName = "John Doe"; Amount = 100.50; Currency = "USD" }; Expected = "Success" },
    @{ Name = "Valid EUR Payment"; Payment = @{ CustomerName = "Jane Smith"; Amount = 75.25; Currency = "EUR" }; Expected = "Success" },
    @{ Name = "Zero Amount"; Payment = @{ CustomerName = "Bob Wilson"; Amount = 0; Currency = "USD" }; Expected = "Fail" },
    @{ Name = "Negative Amount"; Payment = @{ CustomerName = "Alice Brown"; Amount = -25.00; Currency = "USD" }; Expected = "Fail" },
    @{ Name = "Empty Customer Name"; Payment = @{ CustomerName = ""; Amount = 50.00; Currency = "USD" }; Expected = "Fail" },
    @{ Name = "Invalid Currency"; Payment = @{ CustomerName = "Charlie Davis"; Amount = 100.00; Currency = "GBP" }; Expected = "Fail" },
    @{ Name = "Very Small Amount"; Payment = @{ CustomerName = "Diana Lee"; Amount = 0.01; Currency = "USD" }; Expected = "Success" },
    @{ Name = "Large Amount"; Payment = @{ CustomerName = "Eve Johnson"; Amount = 999999.99; Currency = "USD" }; Expected = "Success" }
)

foreach ($testCase in $testCases) {
    Write-Host "`n--- Testing: $($testCase.Name) ---" -ForegroundColor Yellow
    Write-Host "Payment Data: $($testCase.Payment | ConvertTo-Json)" -ForegroundColor Cyan
    
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body ($testCase.Payment | ConvertTo-Json) -ContentType "application/json"
        
        if ($response.StatusCode -eq 201) {
            Write-Host "✅ SUCCESS: Payment created" -ForegroundColor Green
            Write-Host "Response: $($response.Content)" -ForegroundColor White
        } else {
            Write-Host "⚠️  UNEXPECTED: Status $($response.StatusCode)" -ForegroundColor Yellow
            Write-Host "Response: $($response.Content)" -ForegroundColor White
        }
    } catch {
        if ($testCase.Expected -eq "Fail") {
            Write-Host "✅ EXPECTED FAILURE: $($_.Exception.Message)" -ForegroundColor Green
        } else {
            Write-Host "❌ UNEXPECTED FAILURE: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Check debug status
Write-Host "`n--- Debug Status ---" -ForegroundColor Yellow
try {
    $debugResponse = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/status" -Method GET
    Write-Host "Debug Status: $($debugResponse.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "Error checking debug status: $($_.Exception.Message)" -ForegroundColor Red
}

# Get all payments
Write-Host "`n--- All Payments ---" -ForegroundColor Yellow
try {
    $allPaymentsResponse = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method GET
    Write-Host "All Payments: $($allPaymentsResponse.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "Error getting all payments: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Debug Analysis Complete ===" -ForegroundColor Green
