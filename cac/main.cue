package app

import (
  json "encoding/json"
  "github.com/optum-financial/vpay-payment-api/cac/config"
  templates "github.com/optum-financial/vpay-payment-api/cac/templates"
)

_appName:    *"payment-api" | string @tag(name)
_imageTag:   *"latest" | string      @tag(tag)
_imageRepo:  *"centraluhg.jfrog.io/commpay-vpay-docker-vir/docker/payment-api/payment-web-api" | string @tag(image)
_env:        *"stage" | string       @tag(env)
_deployment: *"stage" | string       @tag(deployment)

_envCfg: config.EnvConfigMap[_deployment]

_cfg: config.#Config & _envCfg & {
  appName:    _appName
  imageTag:   _imageTag
  imageRepo:  _imageRepo
  env:        _env
  deployment: _deployment
}

_helmLabels: {
  app: _cfg.helmAppLabel
  "app.kubernetes.io/cluster-domain": _cfg.helmClusterDomain
  "app.kubernetes.io/instance": _cfg.helmInstanceLabel
  "app.kubernetes.io/managed-by": "Helm"
  "app.kubernetes.io/name": _cfg.appName
  "app.kubernetes.io/part-of": _cfg.appName
  "helm.sh/chart": _cfg.helmChartLabel
}

_appsettingsData: config.AppSettingsDataMap[_deployment]

if _cfg.resources.deployment.enabled {
  Deployment: templates.#Deployment & {
    metadata: {
      name:      _cfg.appName
      namespace: _cfg.namespace
      labels:    _helmLabels
    }
    spec: {
      replicas: _cfg.replicas
      revisionHistoryLimit: 4
      strategy: {
        type: "RollingUpdate"
        rollingUpdate: {
          maxSurge:       "100%"
          maxUnavailable: "0%"
        }
      }
      selector: matchLabels: {
        "app.kubernetes.io/instance": _cfg.helmInstanceLabel
        "app.kubernetes.io/name":     _cfg.appName
      }
      template: {
        metadata: {
          labels: _helmLabels
        }
        spec: {
          containers: [{
            name:            _cfg.appName
            image:           "\(_cfg.imageRepo):\(_cfg.imageTag)"
            imagePullPolicy: "Always"
            ports: [{
              name:          "http"
              containerPort: _cfg.containerPort
            }]
            env: [
              {
                name: "ASPNETCORE_ENVIRONMENT"
                value: _cfg.dotnetEnv
              },
              {
                name: "DOTNET_ENVIRONMENT"
                value: _cfg.dotnetEnv
              },
              {
                name: "K8S_NODE_NAME"
                valueFrom: fieldRef: fieldPath: "spec.nodeName"
              },
              {
                name: "K8S_POD_NAME"
                valueFrom: fieldRef: fieldPath: "metadata.name"
              },
            ]
            volumeMounts: [
              {
                name:      "\(_cfg.appName)-appsettings-secure-json"
                mountPath: "/app/appsettings.secure.json"
                subPath:   "appsettings.secure.json"
                readOnly:  true
              },
              {
                name:      "\(_cfg.appName)-appsettings-json"
                mountPath: "/app/appsettings.json"
                subPath:   "appsettings.json"
                readOnly:  true
              },
              {
                name:      "\(_cfg.appName)-appsettings-json"
                mountPath: _cfg.appsettingsMountPath
                subPath:   _cfg.appsettingsConfigMapKey
                readOnly:  true
              },
            ]
            readinessProbe: {
              httpGet: {
                path: "/api/about"
                port: _cfg.containerPort
              }
            }
            livenessProbe: {
              httpGet: {
                path: "/api/about"
                port: _cfg.containerPort
              }
            }
            resources: {
              requests: _cfg.resources.deployment.requests
              limits:   _cfg.resources.deployment.limits
            }
          }]
          volumes: [
            {
              name: "\(_cfg.appName)-appsettings-secure-json"
              secret: {
                secretName: "\(_cfg.appName)-appsettings-secure-json"
              }
            },
            {
              name: "\(_cfg.appName)-appsettings-json"
              configMap: {
                name: "\(_cfg.appName)-appsettings-json"
              }
            },
          ]
        }
      }
    }
  }
}

if _cfg.resources.service.enabled {
  Service: templates.#Service & {
    metadata: {
      name:      _cfg.appName
      namespace: _cfg.namespace
      labels:    _helmLabels
    }
    spec: {
      type: _cfg.resources.service.type
      selector: {
        app: _cfg.appName
      }
      ports: [{
        name:       "http"
        port:       _cfg.containerPort
        targetPort: _cfg.containerPort
      }]
    }
  }
}

if _cfg.resources.configMap.enabled {
  ConfigMap: templates.#ConfigMap & {
    metadata: {
      name:      "\(_cfg.appName)-appsettings-json"
      namespace: _cfg.namespace
      labels:    _helmLabels
    }
    data: {
      "appsettings.json":                      json.Marshal(_appsettingsData.base)
      "\(_cfg.appsettingsConfigMapKey)": json.Marshal(_appsettingsData.env)
    }
  }
}

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
            "appsettings.secure.json": "{\n  \"Db2\": {\n    \"Username\": \"{{ .Db2Username }}\",\n    \"Password\": \"{{ .Db2Password }}\"\n  }\n}"
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
        },
        {
          secretKey: "Db2Password"
          remoteRef: {
            key:      _cfg.resources.externalSecret.remoteKey
            property: "password"
          }
        },
      ]
    }
  }
}

if _cfg.resources.httpRoute.enabled {
  HTTPRoute: templates.#HTTPRoute & {
    metadata: {
      name:      _cfg.appName
      namespace: _cfg.namespace
      labels:    _helmLabels
    }
    spec: {
      parentRefs: [{
        name: _cfg.resources.httpRoute.parentRef.name
      }]
      hostnames: _cfg.resources.httpRoute.hostnames
      rules: [{
        backendRefs: [{
          name: _cfg.appName
          port: _cfg.containerPort
        }]
      }]
    }
  }
}
