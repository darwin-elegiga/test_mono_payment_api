package config

EnvConfigMap: {
  prod: {
    namespace:               "client-payments-prod"
    dotnetEnv:               "Production"
    replicas:                3
    containerPort:           8080
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
      }
      externalSecret: {
        enabled:         true
        storeName:       "vault-store-secret"
        storeKind:       "ClusterSecretStore"
        refreshInterval: "1h"
        creationPolicy:  "Owner"
        deletionPolicy:  "Retain"
        remoteKey:       "secret/client-payments/payment-api/prod/db2"
      }
      httpRoute: {
        enabled: true
        parentRef: {
          name: "gateway"
        }
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
