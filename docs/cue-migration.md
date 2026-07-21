# CUE Migration Notes

## Overview

This repository migrated Kubernetes manifest generation from Helm chart rendering to CUE-based generation under `cac/`.

Reference implementation used during migration:
- `vpay-report-service` branch `cue-central-registry`

## CUE Structure

- `cac/config/base.cue`: Shared config schema and resource toggle model.
- `cac/config/dev.cue`: Development environment overlay and appsettings data.
- `cac/config/stage.cue`: Stage environment overlay and appsettings data.
- `cac/config/prod.cue`: Production environment overlay and appsettings data.
- `cac/templates/*.cue`: Split template definitions per resource kind.
- `cac/main.cue`: Resource wiring and environment-driven composition.
- `cac/gen_tool.cue`: File generation commands into `cac/generated/<env>/`.
- `cac/scripts/*.sh`: Validation and generation scripts.

## Resource Mapping Decisions

- Helm `Deployment` -> CUE `Deployment`.
- Helm `Service` -> CUE `Service`.
- Helm custom ConfigMap flow -> CUE `ConfigMap` in `configmap.yaml`.
- Helm secret data -> CUE `ExternalSecret` in `externalsecret.yaml`.
- Helm ingress routing -> CUE `HTTPRoute` in `httproute.yaml`.

## AppSettings Conventions

- Application runtime config file remains `appsettings.json`.
- Environment file naming remains JSON-based (`appsettings.Development.json`, `appsettings.Staging.json`, `appsettings.Production.json`).
- CUE-generated ConfigMap preserves JSON key names:
  - `appsettings.json`
  - `appsettings.development.json`
  - `appsettings.staging.json`
  - `appsettings.production.json`
- Separate appsettings publication workflow is provided in `.github/workflows/publish-appsettings-configmap.yml` and copies only generated `configmap.yaml`.

## Generation and Validation

Run from repository root:

```bash
bash cac/scripts/validate.sh
bash cac/scripts/generate-dev.sh <image-tag>
bash cac/scripts/generate-stage.sh <image-tag>
bash cac/scripts/generate-prod.sh <image-tag>
```

Outputs are written to:

- `cac/generated/dev/`
- `cac/generated/stage/`
- `cac/generated/prod/`

## Workflow Alignment

Added migration workflows:

- `.github/workflows/cue-manifest-migration-dev.yml`
- `.github/workflows/cue-manifest-migration-stage.yml`
- `.github/workflows/cue-manifest-migration-prod.yml`

Each workflow is dispatch-driven and performs:

1. CUE generation.
2. Helm rendering for parity comparison.
3. Compare step (resource keys and deployment image references).
4. Publish of generated environment manifests.

Existing stage/prod deploy workflows are preserved to keep current deployment contract unchanged.

## Known Constraint

Local Helm parity can be blocked when chart dependency fetch requires authentication to the internal Helm repository (`401`). In that case:

- CUE generation and validation remain executable.
- Helm parity comparison is pending until repository auth is available.
