package config

EnvConfigMap: {
  stage: {
    namespace:               "client-payments-stage"
    dotnetEnv:               "Staging"
    replicas:                1
    containerPort:           8080
    appsettingsConfigMapKey: "appsettings.staging.json"
    appsettingsMountPath:    "/app/appsettings.Staging.json"
    ingressHostname:         "stg-payapi.vpayusa.net"
    httpRouteName:           "payment-api-v1"

    helmAppLabel:      "payment-api-api-staging"
    helmInstanceLabel: "payment-api-api"
    helmClusterDomain: "stage.pks.vpayusa.net"
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
        remoteKey:       "secret/client-payments/payment-api/stage/db2"
      }
      httpRoute: {
        enabled: true
        parentRef: {
          name: "gateway"
        }
        hostnames: ["stg-payapi.vpayusa.net"]
      }
    }
  }
}

AppSettingsDataMap: {
  stage: {
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
          Host: "graylogsandbox.vpayusa.net"
          Port: 5555
          LogLevel: {
            Default:                                    "Information"
            Microsoft:                                  "Warning"
            "VPay.Payment.Api.Auth.VPayAuthenticationHandler": "Information"
            "VPay.Payment.AuthService": "Information"
          }
        }
      }
      PaymentSettings: {
        TradingPostIp: "10.140.104.25"
      }
      Db2: {
        Dsn:      "AS400"
        Hostname: "assadb01.vpayusa.net"
        IsLocal:  false
      }
      FaxmanApi: {
        BaseAddress: "https://faxman-api.stg.pks.vpayusa.net/"
      }
    }
  }
}
