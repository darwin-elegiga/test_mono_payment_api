package templates

#ConfigMap: {
	apiVersion: "v1"
	kind:       "ConfigMap"
	metadata: {
		name:      string
		namespace?: string
		labels?: {
			[string]: string
		}
	}
	data?: {
		[string]: string
	}
}