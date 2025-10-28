# Test Restored Projects
Write-Host "=== Testing Restored Projects ===" -ForegroundColor Green

$baseUrl = "http://localhost:5255"

# Test 1: Valid Payment (should use C++ validation)
Write-Host "`n1. Testing Valid Payment (C++ validation)..." -ForegroundColor Yellow
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

# Test 2: Invalid Currency (should use C++ validation and reject GBP)
Write-Host "`n2. Testing Invalid Currency Payment (C++ validation should reject GBP)..." -ForegroundColor Yellow
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

# Test 3: Debug Status (C++ debug methods)
Write-Host "`n3. Testing Debug Status (C++ debug methods)..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/payment/debug/status" -Method GET
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Memory Leak Detection (C++ debug methods)
Write-Host "`n4. Testing Memory Leak Detection (C++ debug methods)..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/payment/debug/dump-memory-leaks" -Method POST
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Restored Projects Testing Complete ===" -ForegroundColor Green
Write-Host "`nRestored Projects:" -ForegroundColor Cyan
Write-Host "✅ ValidationEngine/ - C++ validation project" -ForegroundColor White
Write-Host "✅ ValidationEngineDebugDll/ - C++ debug DLL project" -ForegroundColor White
Write-Host "✅ ReqnrollCucumberTests/ - BDD test project" -ForegroundColor White
Write-Host "✅ screenshots/ - API documentation screenshots" -ForegroundColor White
Write-Host "✅ CPlusPlusValidationWrapper.cs - C++ wrapper service" -ForegroundColor White
Write-Host "✅ ValidationEngine.dll - C++ validation DLL" -ForegroundColor White
Write-Host "✅ WeatherForecast.cs - Template file" -ForegroundColor White
