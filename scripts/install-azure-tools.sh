#!/bin/bash
# Install Azure Functions Core Tools v4
# Run as: bash scripts/install-azure-tools.sh

set -e

echo "Installing Azure Functions Core Tools v4..."

# Detect OS
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    # Linux (Ubuntu/Debian)
    echo "Detected Linux..."

    # Install Microsoft package repository
    curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
    sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
    sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-$(lsb_release -cs)-prod $(lsb_release -cs) main" > /etc/apt/sources.list.d/dotnetdev.list'

    sudo apt-get update
    sudo apt-get install -y azure-functions-core-tools-4

elif [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    echo "Detected macOS..."

    if ! command -v brew &> /dev/null; then
        echo "ERROR: Homebrew not found. Install it from https://brew.sh"
        exit 1
    fi

    brew tap azure/functions
    brew install azure-functions-core-tools@4

else
    echo "ERROR: Unsupported OS. Use npm: npm install -g azure-functions-core-tools@4"
    exit 1
fi

echo ""
echo "Verifying installation..."
func --version

echo ""
echo "Azure Functions Core Tools installed successfully."
