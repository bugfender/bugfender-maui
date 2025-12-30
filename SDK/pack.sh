#!/usr/bin/env bash
set -euo pipefail

# Usage:
#  PACK_NO_BUILD=1 ./pack.sh [Configuration] [OutputDir]
#  ./pack.sh Release ./nupkgs
# By default PACK_NO_BUILD=1 (pack with --no-build). Set PACK_NO_BUILD=0 to build first.

CONFIG="${1:-Release}"
OUT_DIR="${2:-$(pwd)/nupkgs}"
ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
PACK_NO_BUILD="${PACK_NO_BUILD:-1}"

mkdir -p "$OUT_DIR"

# Always restore packages first
echo "[pack.sh] Restoring packages..."
dotnet restore "$ROOT_DIR/Bugfender.Sdk.csproj"

if [ "$PACK_NO_BUILD" = "1" ] || [ "$PACK_NO_BUILD" = "true" ]; then
  echo "[pack.sh] Packing without building (--no-build). Ensure platform DLLs are up-to-date."
  dotnet pack "$ROOT_DIR/Bugfender.Sdk.csproj" -c "$CONFIG" -o "$OUT_DIR" --no-build
else
  echo "[pack.sh] Building before pack..."
  "$ROOT_DIR/build.sh" "$CONFIG"
  dotnet pack "$ROOT_DIR/Bugfender.Sdk.csproj" -c "$CONFIG" -o "$OUT_DIR"
fi

echo "[pack.sh] Package output: $OUT_DIR"
