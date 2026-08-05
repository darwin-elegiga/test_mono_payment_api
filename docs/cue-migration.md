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
- ConfigMap-only publishing is handled by `.github/workflows/cue-configmap-deploy.yml`, which generates CUE manifests and publishes only `configmap*.yaml` files.

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

Only `.gitkeep` placeholders are tracked in `cac/generated/`; generated manifests are produced by scripts and workflows at runtime.

## Workflow Alignment

Aligned CUE workflows:

- `.github/workflows/cue-build-and-deploy-dev.yml`
- `.github/workflows/cue-deploy-stage.yml`
- `.github/workflows/cue-deploy-prod.yml`
- `.github/workflows/cue-configmap-deploy.yml`

Workflow behavior:

1. Generate manifests from CUE for the target environment.
2. Capture and inject `deployedBy` metadata for label traceability.
3. Publish generated manifests to the shared manifests repository.
4. For configmap deploy, publish only `configmap*.yaml` outputs.

Existing Helm deploy workflows remain available for non-CUE deployment paths.

## Known Constraint

Local Helm parity is no longer part of the CUE workflows. If chart dependency access is unavailable, Helm deploy workflows may still require internal repository authentication.
