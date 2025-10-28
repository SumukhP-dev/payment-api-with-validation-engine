# Test script for Payment API Debug functionality
Write-Host "Testing Payment API Debug Functionality" -ForegroundColor Green

# Test 1: Check debug status
Write-Host "`n1. Checking debug status..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/status" -Method GET
    Write-Host "Debug Status Response:" -ForegroundColor Cyan
    Write-Host $response.Content
} catch {
    Write-Host "Error checking debug status: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Create a valid payment
Write-Host "`n2. Creating a valid payment..." -ForegroundColor Yellow
$validPayment = @{
    CustomerName = "John Doe"
    Amount = 100.50
    Currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $validPayment -ContentType "application/json"
    Write-Host "Valid Payment Response:" -ForegroundColor Cyan
    Write-Host $response.Content
} catch {
    Write-Host "Error creating valid payment: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Create an invalid payment (negative amount)
Write-Host "`n3. Creating an invalid payment (negative amount)..." -ForegroundColor Yellow
$invalidPayment = @{
    CustomerName = "Jane Doe"
    Amount = -50.00
    Currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $invalidPayment -ContentType "application/json"
    Write-Host "Invalid Payment Response:" -ForegroundColor Cyan
    Write-Host $response.Content
} catch {
    Write-Host "Expected error for invalid payment: $($_.Exception.Message)" -ForegroundColor Yellow
}

# Test 4: Get all payments
Write-Host "`n4. Getting all payments..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method GET
    Write-Host "All Payments Response:" -ForegroundColor Cyan
    Write-Host $response.Content
} catch {
    Write-Host "Error getting all payments: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: Dump memory leaks
Write-Host "`n5. Dumping memory leaks..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/dump-memory-leaks" -Method POST
    Write-Host "Memory Leak Dump Response:" -ForegroundColor Cyan
    Write-Host $response.Content
} catch {
    Write-Host "Error dumping memory leaks: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nDebug API testing completed!" -ForegroundColor Green
Write-Host "Check the console output and validation_debug_*.log files for detailed debug information." -ForegroundColor Cyan
