package templates

#PersistentVolume: {
	apiVersion: "v1"
	kind:       "PersistentVolume"
	metadata: {
		name: string
		labels?: {
			[string]: string
		}
	}
	spec: _
}
