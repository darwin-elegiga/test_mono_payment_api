#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export PATH="$HOME/bin:$PATH"
mkdir -p generated/prod
rm -f generated/prod/*.yaml
cue cmd --inject tag=${1:-latest} --inject deployment=prod --inject env=prod gen .
echo "Generated files in cac/generated/prod:"
find generated/prod -maxdepth 1 -type f | sort
