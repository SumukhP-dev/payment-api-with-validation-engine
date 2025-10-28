# Comprehensive API Test Suite
Write-Host "=== Comprehensive Payment API Test Suite ===" -ForegroundColor Green
Write-Host "Testing all API endpoints and scenarios" -ForegroundColor Cyan

$baseUrl = "http://localhost:5255"
$testResults = @()

# Helper function to test API calls
function Test-ApiCall {
    param(
        [string]$Method,
        [string]$Endpoint,
        [string]$Description,
        [object]$Body = $null,
        [int]$ExpectedStatus = 200,
        [string]$ContentType = "application/json"
    )
    
    Write-Host "`n--- $Description ---" -ForegroundColor Yellow
    Write-Host "Method: $Method | Endpoint: $Endpoint" -ForegroundColor Cyan
    
    try {
        $params = @{
            Uri = "$baseUrl$Endpoint"
            Method = $Method
        }
        
        if ($Body) {
            $params.Body = $Body
            $params.ContentType = $ContentType
        }
        
        $response = Invoke-WebRequest @params
        $actualStatus = $response.StatusCode
        
        if ($actualStatus -eq $ExpectedStatus) {
            Write-Host "✅ SUCCESS - Status: $actualStatus" -ForegroundColor Green
            if ($response.Content) {
                Write-Host "Response: $($response.Content)" -ForegroundColor White
            }
            $testResults += @{ Test = $Description; Status = "PASS"; Details = "Status: $actualStatus" }
        } else {
            Write-Host "❌ FAILED - Expected: $ExpectedStatus, Got: $actualStatus" -ForegroundColor Red
            $testResults += @{ Test = $Description; Status = "FAIL"; Details = "Expected: $ExpectedStatus, Got: $actualStatus" }
        }
    } catch {
        $actualStatus = $_.Exception.Response.StatusCode.value__
        if ($actualStatus -eq $ExpectedStatus) {
            Write-Host "✅ SUCCESS - Status: $actualStatus (Expected Error)" -ForegroundColor Green
            Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Cyan
            $testResults += @{ Test = $Description; Status = "PASS"; Details = "Status: $actualStatus (Expected Error)" }
        } else {
            Write-Host "❌ FAILED - Expected: $ExpectedStatus, Got: $actualStatus" -ForegroundColor Red
            Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
            $testResults += @{ Test = $Description; Status = "FAIL"; Details = "Expected: $ExpectedStatus, Got: $actualStatus" }
        }
    }
}

# Test 1: Health Check
Test-ApiCall -Method "GET" -Endpoint "/" -Description "Health Check" -ExpectedStatus 200

# Test 2: Swagger Documentation
Test-ApiCall -Method "GET" -Endpoint "/swagger/v1/swagger.json" -Description "Swagger Documentation" -ExpectedStatus 200

# Test 3: Create Valid Payment
$validPayment = @{
    customerName = "John Doe"
    amount = 100.50
    currency = "USD"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Valid Payment" -Body $validPayment -ExpectedStatus 201

# Test 4: Create Payment with Zero Amount (Should Fail)
$zeroAmountPayment = @{
    customerName = "Jane Smith"
    amount = 0
    currency = "USD"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Zero Amount (Should Fail)" -Body $zeroAmountPayment -ExpectedStatus 400

# Test 5: Create Payment with Negative Amount (Should Fail)
$negativeAmountPayment = @{
    customerName = "Bob Wilson"
    amount = -50.00
    currency = "USD"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Negative Amount (Should Fail)" -Body $negativeAmountPayment -ExpectedStatus 400

# Test 6: Create Payment with Empty Customer Name (Should Fail)
$emptyNamePayment = @{
    customerName = ""
    amount = 75.00
    currency = "USD"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Empty Customer Name (Should Fail)" -Body $emptyNamePayment -ExpectedStatus 400

# Test 7: Create Payment with Invalid Currency (Should Fail)
$invalidCurrencyPayment = @{
    customerName = "Alice Brown"
    amount = 200.00
    currency = "GBP"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Invalid Currency (Should Fail)" -Body $invalidCurrencyPayment -ExpectedStatus 400

# Test 8: Create Valid EUR Payment
$validEurPayment = @{
    customerName = "Charlie Davis"
    amount = 150.75
    currency = "EUR"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Valid EUR Payment" -Body $validEurPayment -ExpectedStatus 201

# Test 9: Get All Payments
Test-ApiCall -Method "GET" -Endpoint "/api/payments" -Description "Get All Payments" -ExpectedStatus 200

# Test 10: Get Specific Payment (using a test ID)
Test-ApiCall -Method "GET" -Endpoint "/api/payment/00000000-0000-0000-0000-000000000000" -Description "Get Non-existent Payment (Should Fail)" -ExpectedStatus 404

# Test 11: Debug Status
Test-ApiCall -Method "GET" -Endpoint "/api/payment/debug/status" -Description "Get Debug Status" -ExpectedStatus 200

# Test 12: Memory Leak Detection
Test-ApiCall -Method "POST" -Endpoint "/api/payment/debug/dump-memory-leaks" -Description "Memory Leak Detection" -ExpectedStatus 200

# Test 13: Disable Debug Mode
Test-ApiCall -Method "POST" -Endpoint "/api/payment/debug/disable" -Description "Disable Debug Mode" -ExpectedStatus 200

# Test 14: Edge Cases - Very Small Amount
$smallAmountPayment = @{
    customerName = "Diana Lee"
    amount = 0.01
    currency = "USD"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Very Small Amount" -Body $smallAmountPayment -ExpectedStatus 201

# Test 15: Edge Cases - Large Amount
$largeAmountPayment = @{
    customerName = "Eve Johnson"
    amount = 999999.99
    currency = "USD"
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Large Amount" -Body $largeAmountPayment -ExpectedStatus 201

# Test 16: Invalid JSON (Should Fail)
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Invalid JSON (Should Fail)" -Body "{ invalid json }" -ExpectedStatus 400

# Test 17: Missing Required Fields (Should Fail)
$incompletePayment = @{
    customerName = "Frank Miller"
    # Missing amount and currency
} | ConvertTo-Json
Test-ApiCall -Method "POST" -Endpoint "/api/payment" -Description "Create Payment with Missing Required Fields (Should Fail)" -Body $incompletePayment -ExpectedStatus 400

# Test Summary
Write-Host "`n=== Test Summary ===" -ForegroundColor Green
$passedTests = ($testResults | Where-Object { $_.Status -eq "PASS" }).Count
$failedTests = ($testResults | Where-Object { $_.Status -eq "FAIL" }).Count
$totalTests = $testResults.Count

Write-Host "Total Tests: $totalTests" -ForegroundColor Cyan
Write-Host "Passed: $passedTests" -ForegroundColor Green
Write-Host "Failed: $failedTests" -ForegroundColor Red

if ($failedTests -eq 0) {
    Write-Host "`n🎉 ALL TESTS PASSED! API is working correctly." -ForegroundColor Green
} else {
    Write-Host "`n⚠️  Some tests failed. See details above." -ForegroundColor Yellow
}

Write-Host "`n=== Detailed Results ===" -ForegroundColor Green
$testResults | ForEach-Object {
    $color = if ($_.Status -eq "PASS") { "Green" } else { "Red" }
    Write-Host "$($_.Status): $($_.Test) - $($_.Details)" -ForegroundColor $color
}

Write-Host "`n=== API Endpoints Summary ===" -ForegroundColor Green
Write-Host "✅ POST   /api/payment              - Create payment" -ForegroundColor White
Write-Host "✅ GET    /api/payment/{id}         - Get specific payment" -ForegroundColor White
Write-Host "✅ GET    /api/payments             - Get all payments" -ForegroundColor White
Write-Host "✅ GET    /api/payment/debug/status - Debug status" -ForegroundColor White
Write-Host "✅ POST   /api/payment/debug/dump-memory-leaks - Memory leak detection" -ForegroundColor White
Write-Host "✅ POST   /api/payment/debug/disable - Disable debug mode" -ForegroundColor White
Write-Host "✅ GET    /swagger/v1/swagger.json  - API documentation" -ForegroundColor White
Write-Host "✅ GET    /                        - Health check" -ForegroundColor White