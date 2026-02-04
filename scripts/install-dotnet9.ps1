# Install .NET 9 SDK using winget
# Run as: powershell -ExecutionPolicy Bypass -File scripts\install-dotnet9.ps1

$ErrorActionPreference = "Stop"

Write-Host "Installing .NET 9 SDK..." -ForegroundColor Cyan

# Check winget is available
if (-not (Get-Command winget -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: winget not found. Install it from the Microsoft Store (App Installer)." -ForegroundColor Red
    exit 1
}

# Install .NET 9 SDK
winget install Microsoft.DotNet.SDK.9 --accept-source-agreements --accept-package-agreements

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Installation failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "`nVerifying installation..." -ForegroundColor Cyan

# Refresh PATH for current session
$env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")

# Verify dotnet is available
if (Get-Command dotnet -ErrorAction SilentlyContinue) {
    dotnet --version
    Write-Host "`n.NET 9 SDK installed successfully." -ForegroundColor Green
} else {
    Write-Host "WARNING: dotnet not found in PATH. You may need to restart your terminal." -ForegroundColor Yellow
}
