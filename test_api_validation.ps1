# Test API Validation and Error Handling
Write-Host "=== Testing API Validation and Error Handling ===" -ForegroundColor Green

# Test 1: Valid Payment
Write-Host "`n1. Testing Valid Payment..." -ForegroundColor Yellow
$validPayment = @{
    customerName = "John Doe"
    amount = 100.50
    currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $validPayment -ContentType "application/json"
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Zero Amount (Should return 400 Bad Request)
Write-Host "`n2. Testing Zero Amount Payment (Should return 400 Bad Request)..." -ForegroundColor Yellow
$zeroAmountPayment = @{
    customerName = "anon"
    amount = 0
    currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $zeroAmountPayment -ContentType "application/json"
    Write-Host "❌ UNEXPECTED SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq 400) {
        Write-Host "✅ CORRECTLY REJECTED - Status: 400 Bad Request" -ForegroundColor Green
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Cyan
    } else {
        Write-Host "❌ WRONG ERROR TYPE - Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
    }
}

# Test 3: Negative Amount (Should return 400 Bad Request)
Write-Host "`n3. Testing Negative Amount Payment (Should return 400 Bad Request)..." -ForegroundColor Yellow
$negativeAmountPayment = @{
    customerName = "anon"
    amount = -50.00
    currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $negativeAmountPayment -ContentType "application/json"
    Write-Host "❌ UNEXPECTED SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq 400) {
        Write-Host "✅ CORRECTLY REJECTED - Status: 400 Bad Request" -ForegroundColor Green
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Cyan
    } else {
        Write-Host "❌ WRONG ERROR TYPE - Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
    }
}

# Test 4: Empty Customer Name (Should return 400 Bad Request)
Write-Host "`n4. Testing Empty Customer Name (Should return 400 Bad Request)..." -ForegroundColor Yellow
$emptyNamePayment = @{
    customerName = ""
    amount = 100.00
    currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $emptyNamePayment -ContentType "application/json"
    Write-Host "❌ UNEXPECTED SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq 400) {
        Write-Host "✅ CORRECTLY REJECTED - Status: 400 Bad Request" -ForegroundColor Green
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Cyan
    } else {
        Write-Host "❌ WRONG ERROR TYPE - Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
    }
}

Write-Host "`n=== API Validation Testing Complete ===" -ForegroundColor Green
