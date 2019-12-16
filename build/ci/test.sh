#!/usr/bin/env bash
cd $(dirname $0)

set -e

docker run --rm \
    --workdir /dotnet/VPay.Payment.Tests \
    payment-api/build-sdk:${GIT_COMMIT_SHORT_HASH:-docker} \
    dotnet test -c Debug
