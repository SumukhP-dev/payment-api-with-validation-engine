# Test New API Routes
Write-Host "=== Testing New API Routes ===" -ForegroundColor Green

# Test 1: Create Payment (POST /api/payment)
Write-Host "`n1. Testing POST /api/payment (Create Payment)..." -ForegroundColor Yellow
$payment = @{
    CustomerName = "John Doe"
    Amount = 100.50
    Currency = "USD"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $payment -ContentType "application/json"
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    $createdPayment = $response.Content | ConvertFrom-Json
    Write-Host "   Created Payment ID: $($createdPayment.id)" -ForegroundColor Cyan
    $paymentId = $createdPayment.id
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $paymentId = "test-id"
}

# Test 2: Get Specific Payment (GET /api/payment/{id})
Write-Host "`n2. Testing GET /api/payment/{id} (Get Specific Payment)..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/$paymentId" -Method GET
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   Payment Data: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Get All Payments (GET /api/payments)
Write-Host "`n3. Testing GET /api/payments (Get All Payments)..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payments" -Method GET
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    $payments = $response.Content | ConvertFrom-Json
    Write-Host "   Total Payments: $($payments.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Debug Status (GET /api/payment/debug/status)
Write-Host "`n4. Testing GET /api/payment/debug/status..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/status" -Method GET
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   Debug Status: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: Memory Leak Dump (POST /api/payment/debug/dump-memory-leaks)
Write-Host "`n5. Testing POST /api/payment/debug/dump-memory-leaks..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment/debug/dump-memory-leaks" -Method POST
    Write-Host "✅ SUCCESS - Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   Response: $($response.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== API Route Testing Complete ===" -ForegroundColor Green
Write-Host "New API Structure:" -ForegroundColor Cyan
Write-Host "  POST   /api/payment              - Create a single payment" -ForegroundColor White
Write-Host "  GET    /api/payment/{id}         - Get a specific payment" -ForegroundColor White
Write-Host "  GET    /api/payments             - Get all payments" -ForegroundColor White
Write-Host "  GET    /api/payment/debug/status - Debug status" -ForegroundColor White
Write-Host "  POST   /api/payment/debug/dump-memory-leaks - Memory leak detection" -ForegroundColor White
Write-Host "  POST   /api/payment/debug/disable - Disable debug mode" -ForegroundColor White
