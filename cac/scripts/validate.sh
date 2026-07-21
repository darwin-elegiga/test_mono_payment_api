#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export PATH="$HOME/bin:$PATH"

cue eval . >/dev/null
cue export --inject tag=validate --inject deployment=dev --inject env=dev . >/dev/null
cue export --inject tag=validate --inject deployment=stage --inject env=stage . >/dev/null
cue export --inject tag=validate --inject deployment=prod --inject env=prod . >/dev/null

echo "CUE validation passed for dev, stage, and prod."
