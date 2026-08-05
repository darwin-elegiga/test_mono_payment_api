package config

#ExternalSecretStoreName: "vault-store-devops" | "vault-store-dba" | "vault-store-engineering" | "vault-store-secret"

#ExternalSecretStoreRef: {
  name: #ExternalSecretStoreName
  kind: *"ClusterSecretStore" | string
}

#Config: {
  appName:   string
  imageTag:  string
  imageRepo: string
  env:       "dev" | "stage" | "prod"
  deployment: "dev" | "stage" | "prod"

  namespace:               string
  dotnetEnv:               string
  replicas:                int
  servicePort:             *80 | int
  containerPort:           *80 | int
  imagePullPolicy:         *"Always" | string
  envVars: {
    ASPNETCORE_ENVIRONMENT: string
    TZ:                     string
  }
  labels: [string]: string
  appsettingsConfigMapKey: string
  appsettingsMountPath:    string
  ingressHostname:         string
  httpRouteName:           string

  helmAppLabel:      string
  helmInstanceLabel: string
  helmClusterDomain: string
  helmChartLabel:    string

  resources: {
    deployment: {
      enabled: bool
      requests: {
        cpu:    string
        memory: string
      }
      limits: {
        cpu:    string
        memory: string
      }
    }
    service: {
      enabled: bool
      type:    *"ClusterIP" | string
    }
    configMap: {
      enabled: bool
      name:    string
      fileName?: string
      content?:  string
    }
    externalSecret: {
      enabled:         bool
      storeName:       *"vault-store-secret" | #ExternalSecretStoreName
      storeKind:       *"ClusterSecretStore" | string
      refreshInterval: string
      creationPolicy:  string
      deletionPolicy:  string
      targetName:      string
      targetTemplate?: string
      remoteKey?:      string
      data?: [...{
        secretKey: string
        sourceRef?: {
          storeRef: #ExternalSecretStoreRef
        }
        remoteRef: {
          key:      string
          property: string
        }
      }]
    }
    httpRoute: {
      enabled: bool
      parentRef?: {
        name:        string
        namespace?:  string
        sectionName?: string
      }
      parentRefs?: [...{
        name: string
        namespace?: string
        sectionName?: string
      }]
      hostnames: [...string]
    }
  }
}

EnvConfigMap: [string]: #Config
AppSettingsDataMap: [string]: {
  base: _
  env:  _
}
