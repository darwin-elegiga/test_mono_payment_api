package templates

#PersistentVolumeClaim: {
	apiVersion: "v1"
	kind:       "PersistentVolumeClaim"
	metadata: {
		name:      string
		namespace: string
		labels?: {
			[string]: string
		}
	}
	spec: {
		accessModes: [...string]
		resources: {
			requests: {
				storage: string
			}
		}
	}
}
