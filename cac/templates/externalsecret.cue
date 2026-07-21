package templates

#ExternalSecret: {
	apiVersion: "external-secrets.io/v1beta1"
	kind:       "ExternalSecret"
	metadata: {
		name:      string
		namespace: string
		labels?: {
			[string]: string
		}
	}
	spec: {
		refreshInterval: string
		secretStoreRef: {
			name: string
			kind: string
		}
		target: {
			name:           string
			creationPolicy: string
			deletionPolicy:  string
			template?: {
				engineVersion: string
				data: {
					[string]: string
				}
			}
		}
		data: [...{
			secretKey: string
			sourceRef?: {
				storeRef: {
					name: string
					kind: string
				}
			}
			remoteRef: {
				key:      string
				property: string
				decodingStrategy?: string
			}
		}]
	}
}