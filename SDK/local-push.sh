#!/usr/bin/env bash
set -euo pipefail

# Usage: ./push.sh [SourceName] [SourcePath]
# Default SourceName: nugetrepo
# Default SourcePath: ~/nugetrepo

SOURCE_NAME="${1:-nugetrepo}"
SOURCE_PATH="${2:-$HOME/nugetrepo}"
ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
PACKAGES_DIR="${ROOT_DIR}/nupkgs"

echo "[push.sh] Checking NuGet source: $SOURCE_NAME"

# Normalize the source path for comparison (resolve ~ and get absolute path)
NORMALIZED_SOURCE_PATH=$(cd "$SOURCE_PATH" 2>/dev/null && pwd || echo "$SOURCE_PATH")

# Check if source path already exists in the source list
SOURCE_LIST=$(dotnet nuget list source 2>/dev/null)
if echo "$SOURCE_LIST" | grep -qF "$NORMALIZED_SOURCE_PATH"; then
  echo "[push.sh] Source path '$SOURCE_PATH' already exists"
  # Extract the existing source name from the path (format: "  1.  name [Enabled]")
  # Get the line before the path, extract the name (everything between number and [)
  EXISTING_SOURCE_NAME=$(echo "$SOURCE_LIST" | grep -B1 -F "$NORMALIZED_SOURCE_PATH" | grep -E "^[[:space:]]*[0-9]+\." | sed -E 's/^[[:space:]]*[0-9]+\.[[:space:]]+(.*)[[:space:]]+\[.*$/\1/' | sed 's/[[:space:]]*$//')
  if [ -n "$EXISTING_SOURCE_NAME" ]; then
    SOURCE_NAME="$EXISTING_SOURCE_NAME"
    echo "[push.sh] Using existing source name: $SOURCE_NAME"
  fi
elif echo "$SOURCE_LIST" | grep -qE "^[[:space:]]*[0-9]+\.[[:space:]]+$SOURCE_NAME[[:space:]]"; then
  echo "[push.sh] Source '$SOURCE_NAME' already exists"
else
  echo "[push.sh] Source '$SOURCE_NAME' not found, adding it..."
  
  # Create the directory if it doesn't exist
  if [ ! -d "$SOURCE_PATH" ]; then
    echo "[push.sh] Creating directory: $SOURCE_PATH"
    mkdir -p "$SOURCE_PATH"
  fi
  
  # Try to add the source (suppress error output and capture it)
  ADD_EXIT_CODE=0
  ADD_OUTPUT=$(dotnet nuget add source "$SOURCE_PATH" --name "$SOURCE_NAME" 2>&1) || ADD_EXIT_CODE=$?
  if [ "$ADD_EXIT_CODE" -ne 0 ]; then
    # If it fails because it already exists, find the actual name
    if echo "$ADD_OUTPUT" | grep -q "already been added"; then
      echo "[push.sh] Source already exists with a different name, finding it..."
      UPDATED_SOURCE_LIST=$(dotnet nuget list source 2>/dev/null)
      if echo "$UPDATED_SOURCE_LIST" | grep -qF "$NORMALIZED_SOURCE_PATH"; then
        EXISTING_SOURCE_NAME=$(echo "$UPDATED_SOURCE_LIST" | grep -B1 -F "$NORMALIZED_SOURCE_PATH" | grep -E "^[[:space:]]*[0-9]+\." | sed -E 's/^[[:space:]]*[0-9]+\.[[:space:]]+(.*)[[:space:]]+\[.*$/\1/' | sed 's/[[:space:]]*$//')
        if [ -n "$EXISTING_SOURCE_NAME" ]; then
          SOURCE_NAME="$EXISTING_SOURCE_NAME"
          echo "[push.sh] Using existing source name: $SOURCE_NAME"
        fi
      fi
    else
      echo "[push.sh] Error: Failed to add source: $ADD_OUTPUT"
      exit 1
    fi
  else
    echo "[push.sh] Source '$SOURCE_NAME' added successfully"
  fi
fi

# Push packages
if [ ! -d "$PACKAGES_DIR" ]; then
  echo "[push.sh] Error: Packages directory not found: $PACKAGES_DIR"
  echo "[push.sh] Please run ./pack.sh first to create packages"
  exit 1
fi

PACKAGE_COUNT=$(find "$PACKAGES_DIR" -name "*.nupkg" | wc -l | tr -d ' ')
if [ "$PACKAGE_COUNT" -eq 0 ]; then
  echo "[push.sh] Error: No .nupkg files found in $PACKAGES_DIR"
  echo "[push.sh] Please run ./pack.sh first to create packages"
  exit 1
fi

echo "[push.sh] Pushing $PACKAGE_COUNT package(s) to $SOURCE_NAME..."
for package in "$PACKAGES_DIR"/*.nupkg; do
  echo "[push.sh] Pushing $(basename "$package")..."
  dotnet nuget push "$package" --source "$SOURCE_NAME"
done

echo "[push.sh] Push complete."

