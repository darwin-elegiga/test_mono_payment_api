#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export PATH="$HOME/bin:$PATH"
mkdir -p generated/stage
rm -f generated/stage/*.yaml
cue cmd --inject tag=${1:-latest} --inject deployment=stage --inject env=stage gen .
echo "Generated files in cac/generated/stage:"
find generated/stage -maxdepth 1 -type f | sort
