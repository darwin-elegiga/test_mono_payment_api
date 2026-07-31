package app

import (
  "encoding/yaml"
  "tool/file"
)

command: gen: {
  if _cfg.resources.deployment.enabled {
    "write:deployment": file.Create & {
      filename: "generated/\(_deployment)/deployment.yaml"
      contents: yaml.Marshal(Deployment)
    }
  }

  if _cfg.resources.service.enabled {
    "write:service": file.Create & {
      filename: "generated/\(_deployment)/service.yaml"
      contents: yaml.Marshal(Service)
    }
  }

  if _cfg.resources.configMap.enabled {
    "write:configmap": file.Create & {
      filename: "generated/\(_deployment)/configmap-api.yaml"
      contents: yaml.Marshal(ConfigMap)
    }
  }

  if _cfg.resources.externalSecret.enabled {
    "write:externalsecret": file.Create & {
      filename: "generated/\(_deployment)/externalsecret.yaml"
      contents: yaml.Marshal(ExternalSecret)
    }
  }

  if _cfg.resources.httpRoute.enabled {
    "write:httproute": file.Create & {
      filename: "generated/\(_deployment)/httproute.yaml"
      contents: yaml.Marshal(HTTPRoute)
    }
  }
}
