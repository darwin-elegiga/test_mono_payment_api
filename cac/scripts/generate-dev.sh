#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export PATH="$HOME/bin:$PATH"
mkdir -p generated/dev
rm -f generated/dev/*.yaml
cue cmd --inject tag=${1:-latest} --inject deployment=dev --inject env=dev gen .
echo "Generated files in cac/generated/dev:"
find generated/dev -maxdepth 1 -type f | sort
