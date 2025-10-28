# Test Clean API
Write-Host "=== Testing Clean API ===" -ForegroundColor Green

$baseUrl = "http://localhost:5255"

# Test 1: Valid Payment
Write-Host "`n1. Testing Valid Payment..." -ForegroundColor Yellow
$validPayment = @{
    customerName = "John Doe"
    amount = 100.50
    currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/payment" -Method POST -Body $validPayment -ContentType "application/json"
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Invalid Currency (Should Fail)
Write-Host "`n2. Testing Invalid Currency Payment (Should Fail)..." -ForegroundColor Yellow
$invalidCurrencyPayment = @{
    customerName = "Alice Brown"
    amount = 200.00
    currency = "GBP"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/payment" -Method POST -Body $invalidCurrencyPayment -ContentType "application/json"
    Write-Host "❌ UNEXPECTED SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq 400) {
        Write-Host "✅ CORRECTLY REJECTED - Status: 400 Bad Request" -ForegroundColor Green
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Cyan
    } else {
        Write-Host "❌ WRONG ERROR TYPE - Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
    }
}

# Test 3: Get All Payments
Write-Host "`n3. Testing Get All Payments..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/payments" -Method GET
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Debug Status
Write-Host "`n4. Testing Debug Status..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/payment/debug/status" -Method GET
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Clean API Testing Complete ===" -ForegroundColor Green
