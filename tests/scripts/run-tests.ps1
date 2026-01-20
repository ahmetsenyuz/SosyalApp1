# PowerShell script to run all tests
param(
    [string]$Configuration = "Release"
)

Write-Host "Starting test execution..." -ForegroundColor Green

try {
    # Restore NuGet packages
    Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
    dotnet restore
    
    # Build the solution
    Write-Host "Building solution..." -ForegroundColor Yellow
    dotnet build --configuration $Configuration
    
    # Run unit tests
    Write-Host "Running unit tests..." -ForegroundColor Yellow
    dotnet test tests/unitTests --configuration $Configuration --verbosity normal
    
    # Run integration tests
    Write-Host "Running integration tests..." -ForegroundColor Yellow
    dotnet test tests/integrationTests --configuration $Configuration --verbosity normal
    
    # Run end-to-end tests
    Write-Host "Running end-to-end tests..." -ForegroundColor Yellow
    dotnet test tests/e2eTests --configuration $Configuration --verbosity normal
    
    # Generate coverage report
    Write-Host "Generating coverage report..." -ForegroundColor Yellow
    dotnet test tests/unitTests --configuration $Configuration --collect:"XPlat Code Coverage" --results-directory:"./TestResults"
    
    Write-Host "All tests completed successfully!" -ForegroundColor Green
}
catch {
    Write-Error "Test execution failed: $_"
    exit 1
}