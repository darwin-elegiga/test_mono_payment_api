package config

EnvConfigMap: {
  dev: {
    env:                     "dev"
    deployment:              "dev"
    namespace:               "client-payments-dev"
    dotnetEnv:               "Development"
    replicas:                1
    servicePort:             8080
    containerPort:           8080
    imagePullPolicy:         "Always"
    envVars: {
      ASPNETCORE_ENVIRONMENT: "Development"
      TZ:                     "America/Chicago"
    }
    labels: {
      "app.kubernetes.io/part-of": "payment-api"
    }
    appsettingsConfigMapKey: "appsettings.development.json"
    appsettingsMountPath:    "/app/appsettings.Development.json"
    ingressHostname:         "dev-payapi.vpayusa.net"
    httpRouteName:           "payment-api-v1"

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
        remoteKey:       "secret/client-payments/payment-api/dev/db2"
        data: [
          {
            secretKey: "Db2Username"
            remoteRef: {
              key:      "secret/client-payments/payment-api/dev/db2"
              property: "username"
            }
          },
          {
            secretKey: "Db2Password"
            remoteRef: {
              key:      "secret/client-payments/payment-api/dev/db2"
              property: "password"
            }
          },
        ]
      }
      httpRoute: {
        enabled: true
        hostnames: [
          "dev-payapi.vpayusa.net"
          "payment-api.vpay-np-k8.vpayusa.net"
          "payment-api.vpay-np-stl-k8.vpayusa.net"
          "payment-api.dev.pks.vpayusa.net"
          "payment-api.plpksdev.vpayusa.net"
        ]
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
