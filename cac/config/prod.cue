package config

EnvConfigMap: {
  prod: {
    env:                     "prod"
    deployment:              "prod"
    namespace:               "client-payments-prod"
    dotnetEnv:               "Production"
    replicas:                3
    servicePort:             8080
    containerPort:           8080
    imagePullPolicy:         "Always"
    envVars: {
      ASPNETCORE_ENVIRONMENT: "Production"
      TZ:                     "America/Chicago"
    }
    labels: {
      "app.kubernetes.io/part-of": "payment-api"
    }
    appsettingsConfigMapKey: "appsettings.production.json"
    appsettingsMountPath:    "/app/appsettings.Production.json"
    ingressHostname:         "payapi.vpayusa.net"
    httpRouteName:           "payment-api-v1"

    helmAppLabel:      "payment-api-api-production"
    helmInstanceLabel: "payment-api-api"
    helmClusterDomain: "prod.pks.vpayusa.net"
    helmChartLabel:    "payment-api-0.0.1"

    resources: {
      deployment: {
        enabled: true
        requests: {
          cpu:    "10m"
          memory: "256Mi"
        }
        limits: {
          cpu:    "100m"
          memory: "512Mi"
        }
      }
      service: {
        enabled: true
      }
      configMap: {
        enabled: true
        name:    "payment-api-appsettings-json"
      }
      externalSecret: {
        enabled:         true
        storeName:       "vault-store-secret"
        storeKind:       "ClusterSecretStore"
        refreshInterval: "1h"
        creationPolicy:  "Owner"
        deletionPolicy:  "Retain"
        targetName:      "payment-api-appsettings-secure-json"
        remoteKey:       "secret/client-payments/payment-api/prod/db2"
        data: [
          {
            secretKey: "Db2Username"
            remoteRef: {
              key:      "secret/client-payments/payment-api/prod/db2"
              property: "username"
            }
          },
          {
            secretKey: "Db2Password"
            remoteRef: {
              key:      "secret/client-payments/payment-api/prod/db2"
              property: "password"
            }
          },
        ]
      }
      httpRoute: {
        enabled: true
        hostnames: ["payapi.vpayusa.net"]
      }
    }
  }
}

AppSettingsDataMap: {
  prod: {
    base: AppSettingsDataMap.dev.base
    env: {
      Logging: {
        IncludeScopes: true
        LogLevel: Default: "Information"
        Debug: LogLevel: Default: "Error"
        Console: {
          IncludeScopes: false
          LogLevel: {
            Default:   "Information"
            Microsoft: "Warning"
          }
        }
        GELF: {
          Host: "syslog.vpayusa.net"
          Port: 5555
          LogLevel: {
            Default:                                    "Information"
            Microsoft:                                  "Warning"
            "VPay.Payment.Api.Auth.VPayAuthenticationHandler": "Warning"
            "VPay.Payment.AuthService": "Error"
          }
        }
      }
      PaymentSettings: {
        TradingPostIp: "10.130.104.25"
      }
      Db2: {
        Dsn:      "AS400"
        Hostname: "vpay01.vpayusa.net"
        IsLocal:  false
      }
      FaxmanApi: {
        BaseAddress: "https://faxman-api.prod.pks.vpayusa.net/"
      }
    }
  }
}
