# Test Currency Validation Specifically
Write-Host "=== Currency Validation Test ===" -ForegroundColor Green

# Test GBP payment (should fail)
Write-Host "`nTesting GBP payment (should be rejected)..." -ForegroundColor Yellow
$gbpPayment = @{ CustomerName = "Test GBP"; Amount = 100.00; Currency = "GBP" } | ConvertTo-Json
Write-Host "Payment data: $gbpPayment" -ForegroundColor Cyan

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $gbpPayment -ContentType "application/json"
    Write-Host "❌ BUG: GBP payment was accepted when it should be rejected!" -ForegroundColor Red
    Write-Host "Response: $($response.Content)" -ForegroundColor White
} catch {
    Write-Host "✅ GBP payment correctly rejected: $($_.Exception.Message)" -ForegroundColor Green
}

# Test USD payment (should succeed)
Write-Host "`nTesting USD payment (should be accepted)..." -ForegroundColor Yellow
$usdPayment = @{ CustomerName = "Test USD"; Amount = 100.00; Currency = "USD" } | ConvertTo-Json
Write-Host "Payment data: $usdPayment" -ForegroundColor Cyan

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5255/api/payment" -Method POST -Body $usdPayment -ContentType "application/json"
    Write-Host "✅ USD payment correctly accepted" -ForegroundColor Green
} catch {
    Write-Host "❌ USD payment incorrectly rejected: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Currency Validation Test Complete ===" -ForegroundColor Green
