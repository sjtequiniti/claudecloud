# Install Azure Functions Core Tools v4 using winget
# Run as: powershell -ExecutionPolicy Bypass -File scripts\install-azure-tools.ps1

$ErrorActionPreference = "Stop"

Write-Host "Installing Azure Functions Core Tools v4..." -ForegroundColor Cyan

# Check winget is available
if (-not (Get-Command winget -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: winget not found. Install it from the Microsoft Store (App Installer)." -ForegroundColor Red
    exit 1
}

# Install Azure Functions Core Tools
winget install Microsoft.Azure.FunctionsCoreTools --accept-source-agreements --accept-package-agreements

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Installation failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "`nVerifying installation..." -ForegroundColor Cyan

# Refresh PATH for current session
$env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")

# Verify func is available
if (Get-Command func -ErrorAction SilentlyContinue) {
    func --version
    Write-Host "`nAzure Functions Core Tools installed successfully." -ForegroundColor Green
} else {
    Write-Host "WARNING: func not found in PATH. You may need to restart your terminal." -ForegroundColor Yellow
}
