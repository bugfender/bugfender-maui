#!/usr/bin/env bash
set -euo pipefail

# Usage: ./build.sh [Configuration]
# Default Configuration: Release

CONFIG="${1:-Release}"
ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"

echo "[build.sh] Root: $ROOT_DIR"
echo "[build.sh] Configuration: $CONFIG"

echo "[build.sh] Building the project..."
dotnet build -c "$CONFIG"

echo "[build.sh] Build complete."
