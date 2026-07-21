package config

#Config: {
  appName:   string
  imageTag:  string
  imageRepo: string
  env:       string
  deployment: "dev" | "stage" | "prod"

  namespace:               string
  dotnetEnv:               string
  replicas:                int
  containerPort:           int
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
    }
    externalSecret: {
      enabled:         bool
      storeName:       string
      storeKind:       string
      refreshInterval: string
      creationPolicy:  string
      deletionPolicy:  string
      remoteKey:       string
    }
    httpRoute: {
      enabled: bool
      parentRef: {
        name:        string
        namespace?:  string
        sectionName?: string
      }
      hostnames: [...string]
    }
  }
}

EnvConfigMap: [string]: #Config
AppSettingsDataMap: [string]: {
  base: _
  env:  _
}
