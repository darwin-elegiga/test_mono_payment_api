# ExternalSecret Per-Item Store Override Examples

**Reference Implementation Guide for vpay-payment-api**

This document shows practical examples for using per-item store overrides in ExternalSecret configurations.

---

## Current Implementation (Top-Level Store Only)

### Configuration (main.cue)
```cue
if _cfg.resources.externalSecret.enabled {
  ExternalSecret: templates.#ExternalSecret & {
    metadata: {
      name:      "\(_cfg.appName)-appsettings-secure-json"
      namespace: _cfg.namespace
      labels:    _helmLabels
    }
    spec: {
      refreshInterval: _cfg.resources.externalSecret.refreshInterval
      secretStoreRef: {
        name: _cfg.resources.externalSecret.storeName
        kind: _cfg.resources.externalSecret.storeKind
      }
      target: {
        name:           "\(_cfg.appName)-appsettings-secure-json"
        creationPolicy: _cfg.resources.externalSecret.creationPolicy
        deletionPolicy: _cfg.resources.externalSecret.deletionPolicy
        template: {
          engineVersion: "v2"
          data: {
            "appsettings.secure.json": "..."
          }
        }
      }
      data: [
        {
          secretKey: "Db2Username"
          remoteRef: {
            key:      _cfg.resources.externalSecret.remoteKey
            property: "username"
          }
          // Uses top-level secretStoreRef (vault-store-dba)
        },
        {
          secretKey: "Db2Password"
          remoteRef: {
            key:      _cfg.resources.externalSecret.remoteKey
            property: "password"
          }
          // Uses top-level secretStoreRef (vault-store-dba)
        },
      ]
    }
  }
}
```

### Rendered YAML Output
```yaml
apiVersion: external-secrets.io/v1
kind: ExternalSecret
metadata:
  name: payment-api-appsettings-secure-json
  namespace: client-payments-dev
spec:
  refreshInterval: 1h
  secretStoreRef:
    name: vault-store-dba
    kind: ClusterSecretStore
  target:
    name: payment-api-appsettings-secure-json
    creationPolicy: Owner
    deletionPolicy: Retain
    template:
      engineVersion: v2
      data:
        appsettings.secure.json: |-
          {
            "Db2": {
              "Username": "{{ .Db2Username }}",
              "Password": "{{ .Db2Password }}"
            }
          }
  data:
    - secretKey: Db2Username
      remoteRef:
        key: secret/client-payments/payment-api/dev/db2
        property: username
      # No sourceRef.storeRef → uses top-level store
    - secretKey: Db2Password
      remoteRef:
        key: secret/client-payments/payment-api/dev/db2
        property: password
      # No sourceRef.storeRef → uses top-level store
```

---

## Extended Implementation (Mixed Stores)

### Use Case
Payment API needs credentials from two different sources:
- Database (DB2) credentials from `vault-store-dba`
- RabbitMQ credentials from `vault-store-engineering`

### Configuration Enhancement (main.cue)

```cue
if _cfg.resources.externalSecret.enabled {
  ExternalSecret: templates.#ExternalSecret & {
    metadata: {
      name:      "\(_cfg.appName)-appsettings-secure-json"
      namespace: _cfg.namespace
      labels:    _helmLabels
    }
    spec: {
      refreshInterval: _cfg.resources.externalSecret.refreshInterval
      secretStoreRef: {
        name: _cfg.resources.externalSecret.storeName      // Default: vault-store-dba
        kind: _cfg.resources.externalSecret.storeKind      // Default: ClusterSecretStore
      }
      target: {
        name:           "\(_cfg.appName)-appsettings-secure-json"
        creationPolicy: _cfg.resources.externalSecret.creationPolicy
        deletionPolicy: _cfg.resources.externalSecret.deletionPolicy
        template: {
          engineVersion: "v2"
          data: {
            "appsettings.secure.json": """
            {
              "Db2": {
                "Username": "{{ .Db2Username }}",
                "Password": "{{ .Db2Password }}"
              },
              "RabbitMq": {
                "Username": "{{ .RabbitMqUsername }}",
                "Password": "{{ .RabbitMqPassword }}"
              }
            }
            """
          }
        }
      }
      data: [
        // Database credentials - uses top-level secretStoreRef
        {
          secretKey: "Db2Username"
          remoteRef: {
            key:      _cfg.resources.externalSecret.remoteKey
            property: "username"
          }
        },
        {
          secretKey: "Db2Password"
          remoteRef: {
            key:      _cfg.resources.externalSecret.remoteKey
            property: "password"
          }
        },
        
        // RabbitMQ credentials - override with vault-store-engineering
        {
          secretKey: "RabbitMqUsername"
          sourceRef: {
            storeRef: {
              name: "vault-store-engineering"
              kind: "ClusterSecretStore"
            }
          }
          remoteRef: {
            key:      "secret/client-payments/payment-api/dev/rabbitmq"
            property: "username"
          }
        },
        {
          secretKey: "RabbitMqPassword"
          sourceRef: {
            storeRef: {
              name: "vault-store-engineering"
              kind: "ClusterSecretStore"
            }
          }
          remoteRef: {
            key:      "secret/client-payments/payment-api/dev/rabbitmq"
            property: "password"
          }
        },
      ]
    }
  }
}
```

### Rendered YAML Output

```yaml
apiVersion: external-secrets.io/v1
kind: ExternalSecret
metadata:
  name: payment-api-appsettings-secure-json
  namespace: client-payments-dev
  labels:
    app: payment-api-api-development
    app.kubernetes.io/cluster-domain: dev.pks.vpayusa.net
    app.kubernetes.io/instance: payment-api-api
    app.kubernetes.io/managed-by: Helm
    app.kubernetes.io/name: payment-api
    app.kubernetes.io/part-of: payment-api
    helm.sh/chart: payment-api-0.0.1
spec:
  refreshInterval: 1h
  # Top-level store for database credentials
  secretStoreRef:
    name: vault-store-dba
    kind: ClusterSecretStore
  target:
    name: payment-api-appsettings-secure-json
    creationPolicy: Owner
    deletionPolicy: Retain
    template:
      engineVersion: v2
      data:
        appsettings.secure.json: |-
          {
            "Db2": {
              "Username": "{{ .Db2Username }}",
              "Password": "{{ .Db2Password }}"
            },
            "RabbitMq": {
              "Username": "{{ .RabbitMqUsername }}",
              "Password": "{{ .RabbitMqPassword }}"
            }
          }
  data:
    # Database credentials - uses top-level store (vault-store-dba)
    - secretKey: Db2Username
      remoteRef:
        key: secret/client-payments/payment-api/dev/db2
        property: username
      # No sourceRef.storeRef → uses top-level spec.secretStoreRef

    - secretKey: Db2Password
      remoteRef:
        key: secret/client-payments/payment-api/dev/db2
        property: password
      # No sourceRef.storeRef → uses top-level spec.secretStoreRef

    # RabbitMQ credentials - overrides with vault-store-engineering
    - secretKey: RabbitMqUsername
      sourceRef:
        storeRef:
          name: vault-store-engineering
          kind: ClusterSecretStore
      remoteRef:
        key: secret/client-payments/payment-api/dev/rabbitmq
        property: username
      # sourceRef.storeRef overrides top-level store for this item

    - secretKey: RabbitMqPassword
      sourceRef:
        storeRef:
          name: vault-store-engineering
          kind: ClusterSecretStore
      remoteRef:
        key: secret/client-payments/payment-api/dev/rabbitmq
        property: password
      # sourceRef.storeRef overrides top-level store for this item
```

---

## How It Works

### Precedence Rules

1. **For data items WITH `sourceRef.storeRef`**
   - Uses the specified store in `sourceRef.storeRef.name`
   - Example: RabbitMQ items use `vault-store-engineering`

2. **For data items WITHOUT `sourceRef.storeRef`**
   - Falls back to top-level `spec.secretStoreRef.name`
   - Example: Database items use `vault-store-dba`

### Secret Rendering

The template payload is rendered identically regardless of which store provides each secret:

```json
{
  "Db2": {
    "Username": "db2user",        // from vault-store-dba
    "Password": "db2pass"          // from vault-store-dba
  },
  "RabbitMq": {
    "Username": "rmquser",         // from vault-store-engineering
    "Password": "rmqpass"          // from vault-store-engineering
  }
}
```

---

## Configuration Patterns

### Pattern 1: Single Store (Current)
All credentials from one store (simplest):
```yaml
spec:
  secretStoreRef:
    name: vault-store-dba
  data:
    - secretKey: User1    # Uses vault-store-dba
    - secretKey: Pass1    # Uses vault-store-dba
```

### Pattern 2: Primary + Override
Most credentials from primary store, some from override:
```yaml
spec:
  secretStoreRef:
    name: vault-store-dba  # Default
  data:
    - secretKey: Db2User      # Uses vault-store-dba (default)
    - secretKey: RabbitMqUser # Uses vault-store-engineering (override)
      sourceRef:
        storeRef:
          name: vault-store-engineering
```

### Pattern 3: Multiple Stores
Credentials distributed across multiple stores:
```yaml
spec:
  secretStoreRef:
    name: vault-store-dba  # Fallback
  data:
    - secretKey: Db2User           # Uses vault-store-dba
    - secretKey: RabbitMqUser      # Uses vault-store-engineering
      sourceRef:
        storeRef:
          name: vault-store-engineering
    - secretKey: ServiceAccountKey # Uses vault-store-devops
      sourceRef:
        storeRef:
          name: vault-store-devops
```

---

## Implementation Checklist

- [ ] **Template Compatible** - externalsecret.cue already supports sourceRef.storeRef
- [ ] **API Version** - v1 schema includes sourceRef field
- [ ] **Default Store Set** - Top-level spec.secretStoreRef in place
- [ ] **Per-Item Overrides** - Add sourceRef.storeRef only when needed
- [ ] **Backward Compatible** - Items without sourceRef use default store
- [ ] **Tested** - Verify K8s applies and External Secrets fetches correctly

---

## Testing

### Dry Run
```bash
cd vpay-payment-api/cac
bash scripts/generate-dev.sh
kubectl apply --dry-run=client -f generated/dev/externalsecret.yaml
```

### Verify Schema
```bash
kubectl apply -f generated/dev/externalsecret.yaml --validate=true
```

### Check Status
```bash
kubectl get externalsecret -n client-payments-dev
kubectl describe externalsecret payment-api-appsettings-secure-json -n client-payments-dev
```

---

## Troubleshooting

### Issue: "Store not found"
- Verify ClusterSecretStore exists: `kubectl get clustersecretstores`
- Check store name spelling in config
- Verify RBAC permissions for the store

### Issue: "Secrets not synced"
- Check External Secrets operator logs: `kubectl logs -n external-secrets-system`
- Verify Vault connectivity from operator pod
- Check Vault auth token/policy

### Issue: "Template rendering failed"
- Verify all referenced keys exist in fetched secrets
- Check template syntax in appsettings.secure.json
- Ensure remoteRef.key and property match Vault structure

---

## References

- [External Secrets Operator - Per-Item Store](https://external-secrets.io/latest/api/externalsecret/#sourcereferencestore)
- [External Secrets - Store Selector](https://external-secrets.io/latest/guides/multi-store-setup/)
- [CUE Documentation](https://cuelang.org/docs/)

