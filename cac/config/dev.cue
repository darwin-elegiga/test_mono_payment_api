package config

EnvConfigMap: {
  dev: {
    namespace:               "client-payments-dev"
    dotnetEnv:               "Development"
    replicas:                1
    containerPort:           8080
    appsettingsConfigMapKey: "appsettings.development.json"
    appsettingsMountPath:    "/app/appsettings.Development.json"
    ingressHostname:         "dev-payapi.vpayusa.net"

    helmAppLabel:      "payment-api-api-development"
    helmInstanceLabel: "payment-api-api"
    helmClusterDomain: "dev.pks.vpayusa.net"
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
        storeName:       "vault-backend"
        storeKind:       "ClusterSecretStore"
        refreshInterval: "1h"
        creationPolicy:  "Owner"
        deletionPolicy:  "Retain"
        remoteKey:       "secret/client-payments/payment-api/dev/db2"
      }
      httpRoute: {
        enabled: true
        parentRef: {
          name: "gateway"
        }
        hostnames: ["dev-payapi.vpayusa.net"]
      }
    }
  }
}

AppSettingsDataMap: {
  dev: {
    base: {
      Logging: {
        IncludeScopes: false
        Debug: LogLevel: Default: "Warning"
        Console: LogLevel: Default: "Information"
        GELF: {
          Host:      "graylogsandbox.vpayusa.net"
          Port:      5555
          LogSource: "VPay.Payment.Api"
          LogLevel: {
            Default:                              "Warning"
            "Microsoft.AspNetCore.DataProtection": "Error"
          }
        }
      }
      PaymentSettings: {
        ValidateIP:   false
        TradingPostIp: "127.0.0.1"
      }
      Db2: {
        Dsn:              "AS400"
        Hostname:         "vpay.payment.db2"
        Username:         "db2inst1"
        Password:         "apassword"
        DefaultLibraries: ",VPAYPGM,VPAYFAX,VPAYBRDDTA"
        IsLocal:          "true"
      }
      HealthcheckSetting: {
        ApplyDB2HealthCheck: true
      }
      Db2_OBSOLETE: {
        Dsn:      "AS400"
        Hostname: "sesidev01.vpayusa.net"
        Username: "WSPAYAPI"
        Password: "yOEFgzf1DZ"
        DefaultLibraries: ",SEWCPS,VPAYDTA,VPAYPGM,VPAYFAX,VPAYSEC,SEWVPAY,SEWACH,SEWBAS,ADMPGM,SEWADM,SEWSEC,SESEC,VPAYBRDPGM,VPAYBRDDTA,QGPL,CRYPTO,PORT835DTA,PORT835PGM,RAMNGSPEED,GUARDRCPGM,GUARDRCDTA"
      }
      FaxmanApi: {
        BaseAddress: "http://localhost:42332"
      }
    }
    env: {
      Logging: {
        IncludeScopes: false
        Debug: LogLevel: Default: "Information"
        Console: {
          LogLevel: {
            Default:   "Information"
            Microsoft: "Warning"
          }
        }
        LogLevel: Default: "Debug"
        GELF: {
          LogLevel: {
            Default:                                    "Information"
            Microsoft:                                  "Warning"
            "VPay.Payment.Api.Auth.VPayAuthenticationHandler": "Information"
          }
        }
      }
      PaymentSettings: {
        ValidateIP:    false
        UseCheckEmail: true
        TradingPostIp: "127.0.0.1"
      }
      Db2: {
        Dsn:      "AS400"
        Hostname: "sesidev01.vpayusa.net"
        IsLocal:  false
      }
      FaxmanApi: {
        BaseAddress: "https://faxman-api.dev.pks.vpayusa.net/"
      }
    }
  }
}
