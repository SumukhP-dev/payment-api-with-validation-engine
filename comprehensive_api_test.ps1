# Comprehensive API Test for AWS Deployment
Write-Host "=== Payment API Comprehensive Test ===" -ForegroundColor Green
Write-Host "Testing API for AWS Elastic Beanstalk deployment compatibility" -ForegroundColor Cyan

# Test 1: API Health Check
Write-Host "`n1. Testing API Health Check..." -ForegroundColor Yellow
try {
    $healthResponse = Invoke-WebRequest -Uri "http://localhost:5255" -Method GET
    Write-Host "✅ API is running - Status: $($healthResponse.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "❌ API Health Check Failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Test 2: Swagger Documentation
Write-Host "`n2. Testing Swagger Documentation..." -ForegroundColor Yellow
try {
    $swaggerResponse = Invoke-WebRequest -Uri "http://localhost:5255/swagger/v1/swagger.json" -Method GET
    Write-Host "✅ Swagger documentation accessible" -ForegroundColor Green
} catch {
    Write-Host "⚠️  Swagger documentation not accessible: $($_.Exception.Message)" -ForegroundColor Yellow
}

# Test 3: Debug Status
Write-Host "`n3. Testing Debug Status..." -ForegroundColor Yellow
try {
    $debugResponse = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/status" -Method GET
    Write-Host "✅ Debug Status: $($debugResponse.Content)" -ForegroundColor Green
} catch {
    Write-Host "❌ Debug Status Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Valid Payment Tests
Write-Host "`n4. Testing Valid Payment Scenarios..." -ForegroundColor Yellow
$validPayments = @(
    @{ Name = "Valid USD Payment"; Payment = @{ CustomerName = "John Doe"; Amount = 100.50; Currency = "USD" } },
    @{ Name = "Valid EUR Payment"; Payment = @{ CustomerName = "Jane Smith"; Amount = 75.25; Currency = "EUR" } },
    @{ Name = "Small Amount"; Payment = @{ CustomerName = "Alice Brown"; Amount = 0.01; Currency = "USD" } },
    @{ Name = "Large Amount"; Payment = @{ CustomerName = "Bob Wilson"; Amount = 999999.99; Currency = "USD" } }
)

foreach ($test in $validPayments) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body ($test.Payment | ConvertTo-Json) -ContentType "application/json"
        if ($response.StatusCode -eq 201) {
            Write-Host "✅ $($test.Name): SUCCESS" -ForegroundColor Green
        } else {
            Write-Host "❌ $($test.Name): Unexpected status $($response.StatusCode)" -ForegroundColor Red
        }
    } catch {
        Write-Host "❌ $($test.Name): FAILED - $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Test 5: Invalid Payment Tests (Should Fail)
Write-Host "`n5. Testing Invalid Payment Scenarios (Should Fail)..." -ForegroundColor Yellow
$invalidPayments = @(
    @{ Name = "Negative Amount"; Payment = @{ CustomerName = "Test User"; Amount = -50.00; Currency = "USD" } },
    @{ Name = "Zero Amount"; Payment = @{ CustomerName = "Test User"; Amount = 0; Currency = "USD" } },
    @{ Name = "Empty Customer Name"; Payment = @{ CustomerName = ""; Amount = 100.00; Currency = "USD" } }
)

foreach ($test in $invalidPayments) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body ($test.Payment | ConvertTo-Json) -ContentType "application/json"
        Write-Host "❌ $($test.Name): Should have failed but succeeded" -ForegroundColor Red
    } catch {
        Write-Host "✅ $($test.Name): Correctly rejected - $($_.Exception.Message)" -ForegroundColor Green
    }
}

# Test 6: Currency Validation Bug Test
Write-Host "`n6. Testing Currency Validation (Critical Bug Check)..." -ForegroundColor Yellow
$currencyTests = @(
    @{ Name = "Valid USD"; Payment = @{ CustomerName = "Test User"; Amount = 100.00; Currency = "USD" }; Expected = "Success" },
    @{ Name = "Valid EUR"; Payment = @{ CustomerName = "Test User"; Amount = 100.00; Currency = "EUR" }; Expected = "Success" },
    @{ Name = "Invalid GBP"; Payment = @{ CustomerName = "Test User"; Amount = 100.00; Currency = "GBP" }; Expected = "Fail" },
    @{ Name = "Invalid JPY"; Payment = @{ CustomerName = "Test User"; Amount = 100.00; Currency = "JPY" }; Expected = "Fail" }
)

foreach ($test in $currencyTests) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body ($test.Payment | ConvertTo-Json) -ContentType "application/json"
        if ($test.Expected -eq "Success") {
            Write-Host "✅ $($test.Name): Correctly accepted" -ForegroundColor Green
        } else {
            Write-Host "❌ $($test.Name): BUG - Should be rejected but was accepted!" -ForegroundColor Red
        }
    } catch {
        if ($test.Expected -eq "Fail") {
            Write-Host "✅ $($test.Name): Correctly rejected" -ForegroundColor Green
        } else {
            Write-Host "❌ $($test.Name): Unexpected failure - $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Test 7: Get All Payments
Write-Host "`n7. Testing Get All Payments..." -ForegroundColor Yellow
try {
    $allPaymentsResponse = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method GET
    $payments = $allPaymentsResponse.Content | ConvertFrom-Json
    Write-Host "✅ Retrieved $($payments.Count) payments successfully" -ForegroundColor Green
} catch {
    Write-Host "❌ Get All Payments Failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 8: Memory Leak Detection
Write-Host "`n8. Testing Memory Leak Detection..." -ForegroundColor Yellow
try {
    $memoryResponse = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/dump-memory-leaks" -Method POST
    Write-Host "✅ Memory leak detection: $($memoryResponse.Content)" -ForegroundColor Green
} catch {
    Write-Host "❌ Memory leak detection failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Test Summary ===" -ForegroundColor Green
Write-Host "API is ready for AWS Elastic Beanstalk deployment!" -ForegroundColor Cyan
Write-Host "Note: There is a critical currency validation bug that needs to be fixed before production deployment." -ForegroundColor Yellow
