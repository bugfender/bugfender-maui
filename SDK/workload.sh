#!/usr/bin/env bash
set -euo pipefail

# Ensure required .NET MAUI workloads are present.
# Runs: sudo dotnet workload restore
# If restore fails, runs: sudo dotnet workload install maui-android maui-ios

echo "[workload-ensure] Running: sudo dotnet workload restore"
if sudo dotnet workload restore; then
  echo "[workload-ensure] workload restore succeeded"
  exit 0
else
  echo "[workload-ensure] workload restore failed — installing workloads: maui-android maui-ios"
  sudo dotnet workload install maui-android maui-ios
  EXIT_CODE=$?
  if [ $EXIT_CODE -ne 0 ]; then
    echo "[workload-ensure] workload install failed with exit code $EXIT_CODE" >&2
    exit $EXIT_CODE
  fi
  echo "[workload-ensure] workload install completed successfully"
fi
